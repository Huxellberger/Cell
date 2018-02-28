// Copyright (C) Threetee Gang All Rights Reserved

#if UNITY_EDITOR

using System.Collections.Generic;
using Assets.Scripts.Input;
using NSubstitute;
using NUnit.Framework;

namespace Assets.Editor.UnitTests.Input
{
    [TestFixture]
    public class DefaultInputMappingProviderTestFixture {

        [Test]
        public void DefaultInputMappingProviderCreated_RawInputsAreOnesProvided()
        {
            var mockTranslatedInputRepository = Substitute.For<ITranslatedInputRepositoryInterface>();
            var expectedRawInputs = new List<RawInput>
            {
              new RawInput("Test", EInputType.Button),
              new RawInput("Test2", EInputType.Analog)
            };

            var defaultInputMappingProvider = new DefaultInputMappingProvider(expectedRawInputs, mockTranslatedInputRepository);
            Assert.AreSame(expectedRawInputs, defaultInputMappingProvider.GetRawInputs());
        }

        [Test]
        public void DefaultInputMappingProviderCreated_UsesProvidedRawInputsToRetrieveMapping()
        {
            var mockTranslatedInputRepository = Substitute.For<ITranslatedInputRepositoryInterface>();
            var expectedRawInputs = new List<RawInput>
            {
                new RawInput("Test", EInputType.Button),
                new RawInput("Test2", EInputType.Analog)
            };

            var defaultInputMappingProvider = new DefaultInputMappingProvider(expectedRawInputs, mockTranslatedInputRepository);
            mockTranslatedInputRepository.Received().RetrieveMappingsForRawInputs(Arg.Is(expectedRawInputs));
        }

        [Test]
        public void DefaultInputMappingProvider_GetTranslatedInput_ReturnsCorrectlyMappedTranslatedInput()
        {
            var mockTranslatedInputRepository = Substitute.For<ITranslatedInputRepositoryInterface>();
            var validRawInput = new RawInput("TestInput", EInputType.Button);

            var expectedTranslation = new TranslatedInput(EInputKey.JumpButton, EInputType.Analog);

            var expectedRawInputs = new List<RawInput>
            {
                validRawInput,
            };

            var expectedTranslatedInputs = new Dictionary<RawInput, TranslatedInput>
            {
                { validRawInput, expectedTranslation }
            };

            mockTranslatedInputRepository.RetrieveMappingsForRawInputs(Arg.Is(expectedRawInputs)).Returns(expectedTranslatedInputs);

            var defaultInputMappingProvider = new DefaultInputMappingProvider(expectedRawInputs, mockTranslatedInputRepository);
            Assert.AreSame(expectedTranslation, defaultInputMappingProvider.GetTranslatedInput(validRawInput));
        }

        [Test]
        public void DefaultInputMappingProvider_GetTranslatedInputWithNone_ThrowsException()
        {
            var mockTranslatedInputRepository = Substitute.For<ITranslatedInputRepositoryInterface>();
            var validRawInput = new RawInput("TestInput", EInputType.Button);
            var invalidRawInput = new RawInput("InvalidInput", EInputType.Analog); 

            var expectedRawInputs = new List<RawInput>
            {
                validRawInput
            };

            var defaultInputMappingProvider = new DefaultInputMappingProvider(expectedRawInputs, mockTranslatedInputRepository);
            Assert.Throws<UnassignedInputMappingException>(() => defaultInputMappingProvider.GetTranslatedInput(invalidRawInput));
        }
    }
}

#endif

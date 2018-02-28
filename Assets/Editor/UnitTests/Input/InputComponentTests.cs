// Copyright (C) Threetee Gang All Rights Reserved

#if UNITY_EDITOR

using System.Collections.Generic;
using Assets.Scripts.Input;
using Assets.Scripts.Test.Input;
using Assets.Scripts.UnityLayer.Input;
using NSubstitute;
using NUnit.Framework;
using UnityEngine;

namespace Assets.Editor.UnitTests.Input
{
    [TestFixture]
    public class InputComponentTestFixture
    {
        [SetUp]
        public void BeforeTest()
        {
            _inputComponent = new GameObject().AddComponent<TestInputComponent>();
            _inputComponent.TestAwake();
        }

        [TearDown]
        public void AfterTest()
        {
            _inputComponent = null;
        }

        [Test]
        public void InputComponent_Update_GetsRawInputsFromMappingProvider()
        {
            var mockInputMappingProviderInterface = Substitute.For<IInputMappingProviderInterface>();
            var mockUnityInputInterface = Substitute.For<IUnityInputInterface>();
            _inputComponent.SetInputMappingProvider(mockInputMappingProviderInterface);
            _inputComponent.SetUnityInputInterface(mockUnityInputInterface);

            _inputComponent.TestUpdate();

            mockInputMappingProviderInterface.ReceivedWithAnyArgs().GetRawInputs();
        }

        [Test]
        public void InputComponent_Update_GetsTranslatedInputWithRawInput()
        {
            var mockInputMappingProviderInterface = Substitute.For<IInputMappingProviderInterface>();
            var mockUnityInputInterface = Substitute.For<IUnityInputInterface>();
            var rawInputs = new List<RawInput>
            {
                new RawInput("TestInput", EInputType.Button),
                new RawInput("OtherTestInput", EInputType.Analog)
            };

            mockInputMappingProviderInterface.GetRawInputs().Returns(rawInputs);
            _inputComponent.SetInputMappingProvider(mockInputMappingProviderInterface);
            _inputComponent.SetUnityInputInterface(mockUnityInputInterface);

            _inputComponent.TestUpdate();

            foreach (var rawInput in rawInputs)
            {
                mockInputMappingProviderInterface.Received().GetTranslatedInput(Arg.Is(rawInput));
            }
        }

        [Test]
        public void InputComponent_UpdateNoChange_AnalogInputCallsAnalogInputEvent()
        {
            var analogEventCalled = false;
            
            var expectedtranslatedInput = new TranslatedInput(EInputKey.Interact, EInputType.Analog);

            var mockInputMappingProviderInterface = Substitute.For<IInputMappingProviderInterface>();
            var mockUnityInputInterface = Substitute.For<IUnityInputInterface>();
            mockUnityInputInterface.GetAxis(Arg.Any<string>()).ReturnsForAnyArgs(expectedtranslatedInput.AxisValue);
            
            var rawInputs = new List<RawInput>
            {
                new RawInput("TestInput", EInputType.Analog)
            };

            mockInputMappingProviderInterface.GetRawInputs().Returns(rawInputs);
            mockInputMappingProviderInterface.GetTranslatedInput(Arg.Any<RawInput>()).Returns(expectedtranslatedInput);
            _inputComponent.SetInputMappingProvider(mockInputMappingProviderInterface);
            _inputComponent.SetUnityInputInterface(mockUnityInputInterface);

            _inputComponent.OnAnalogInputEvent += (key, value) =>
            {
                analogEventCalled = true;
            };

            _inputComponent.TestUpdate();

            Assert.IsTrue(analogEventCalled);
        }

        [Test]
        public void InputComponent_Update_AnalogInputCallsAnalogInputEvent()
        {
            var analogEventCalled = false;
            var actualKey = EInputKey.HorizontalAnalog;

            var expectedtranslatedInput = new TranslatedInput(EInputKey.Interact, EInputType.Analog);

            var mockInputMappingProviderInterface = Substitute.For<IInputMappingProviderInterface>();
            var mockUnityInputInterface = Substitute.For<IUnityInputInterface>();
           
            mockUnityInputInterface.GetAxis(Arg.Any<string>()).ReturnsForAnyArgs(expectedtranslatedInput.AxisValue + 10.0f);
            var rawInputs = new List<RawInput>
            {
                new RawInput("TestInput", EInputType.Analog)
            };

            mockInputMappingProviderInterface.GetRawInputs().Returns(rawInputs);
            mockInputMappingProviderInterface.GetTranslatedInput(Arg.Any<RawInput>()).Returns(expectedtranslatedInput);
            _inputComponent.SetInputMappingProvider(mockInputMappingProviderInterface);
            _inputComponent.SetUnityInputInterface(mockUnityInputInterface);

            _inputComponent.OnAnalogInputEvent += (key, value) =>
            {
                analogEventCalled = true;
                actualKey = key;
            };

            _inputComponent.TestUpdate();

            Assert.IsTrue(analogEventCalled);
            Assert.AreEqual(actualKey, expectedtranslatedInput.InputKey);
        }

        [Test]
        public void InputComponent_UpdateNoChange_ButtonInputDoesNotCallButtonInputEvent()
        {
            var buttonEventCalled = false;

            var expectedtranslatedInput = new TranslatedInput(EInputKey.Interact, EInputType.Button);

            var mockInputMappingProviderInterface = Substitute.For<IInputMappingProviderInterface>();
            var mockUnityInputInterface = Substitute.For<IUnityInputInterface>();
            mockUnityInputInterface.GetButton(Arg.Any<string>()).ReturnsForAnyArgs(expectedtranslatedInput.Pressed);
            var rawInputs = new List<RawInput>
            {
                new RawInput("TestInput", EInputType.Button)
            };

            mockInputMappingProviderInterface.GetRawInputs().Returns(rawInputs);
            mockInputMappingProviderInterface.GetTranslatedInput(Arg.Any<RawInput>()).Returns(expectedtranslatedInput);
            _inputComponent.SetInputMappingProvider(mockInputMappingProviderInterface);
            _inputComponent.SetUnityInputInterface(mockUnityInputInterface);

            _inputComponent.OnButtonInputEvent += (key, pressed) =>
            {
                buttonEventCalled = true;
            };

            _inputComponent.TestUpdate();

            Assert.IsFalse(buttonEventCalled);
        }

        [Test]
        public void InputComponent_Update_ButtonInputCallsButtonInputEvent()
        {
            var buttonEventCalled = false;
            var actualKey = EInputKey.HorizontalAnalog;

            var expectedtranslatedInput = new TranslatedInput(EInputKey.Interact, EInputType.Button);

            var mockInputMappingProviderInterface = Substitute.For<IInputMappingProviderInterface>();
            var mockUnityInputInterface = Substitute.For<IUnityInputInterface>();
            mockUnityInputInterface.GetButton(Arg.Any<string>()).ReturnsForAnyArgs(!expectedtranslatedInput.Pressed);
            var rawInputs = new List<RawInput>
            {
                new RawInput("TestInput", EInputType.Button)
            };

            mockInputMappingProviderInterface.GetRawInputs().Returns(rawInputs);
            mockInputMappingProviderInterface.GetTranslatedInput(Arg.Any<RawInput>()).Returns(expectedtranslatedInput);
           
            _inputComponent.SetInputMappingProvider(mockInputMappingProviderInterface);
            _inputComponent.SetUnityInputInterface(mockUnityInputInterface);

            _inputComponent.OnButtonInputEvent += (key, pressed) =>
            {
                buttonEventCalled = true;
                actualKey = key;
            };

            _inputComponent.TestUpdate();

            Assert.IsTrue(buttonEventCalled);
            Assert.AreEqual(actualKey, expectedtranslatedInput.InputKey);
        }

        [Test]
        public void InputComponent_UpdateNoChange_MouseInputDoesNotCallMouseInputEvent()
        {
            var mouseEventCalled = false;

            var expectedtranslatedInput = new TranslatedInput(EInputKey.Interact, EInputType.Mouse);

            var mockInputMappingProviderInterface = Substitute.For<IInputMappingProviderInterface>();
            var mockUnityInputInterface = Substitute.For<IUnityInputInterface>();
            mockUnityInputInterface.GetMousePosition().ReturnsForAnyArgs(expectedtranslatedInput.Coordinate);
            var rawInputs = new List<RawInput>
            {
                new RawInput("TestInput", EInputType.Button)
            };

            mockInputMappingProviderInterface.GetRawInputs().Returns(rawInputs);
            mockInputMappingProviderInterface.GetTranslatedInput(Arg.Any<RawInput>()).Returns(expectedtranslatedInput);
            _inputComponent.SetInputMappingProvider(mockInputMappingProviderInterface);
            _inputComponent.SetUnityInputInterface(mockUnityInputInterface);

            _inputComponent.OnMouseInputEvent += (key, newMousePos) =>
            {
                mouseEventCalled = true;
            };

            _inputComponent.TestUpdate();

            Assert.IsFalse(mouseEventCalled);
        }

        [Test]
        public void InputComponent_Update_MouseInputCallsMouseInputEvent()
        {
            var mouseEventCalled = false;
            var actualKey = EInputKey.HorizontalAnalog;

            var expectedtranslatedInput = new TranslatedInput(EInputKey.Interact, EInputType.Mouse);

            var mockInputMappingProviderInterface = Substitute.For<IInputMappingProviderInterface>();
            var mockUnityInputInterface = Substitute.For<IUnityInputInterface>();
            mockUnityInputInterface.GetMousePosition().ReturnsForAnyArgs(new Vector3(23.0f, 7.0f, 3.0f));
            var rawInputs = new List<RawInput>
            {
                new RawInput("TestInput", EInputType.Mouse)
            };

            mockInputMappingProviderInterface.GetRawInputs().Returns(rawInputs);
            mockInputMappingProviderInterface.GetTranslatedInput(Arg.Any<RawInput>()).Returns(expectedtranslatedInput);
            _inputComponent.SetInputMappingProvider(mockInputMappingProviderInterface);
            _inputComponent.SetUnityInputInterface(mockUnityInputInterface);

            _inputComponent.OnMouseInputEvent += (key, newMousePos) =>
            {
                mouseEventCalled = true;
                actualKey = key;
            };

            _inputComponent.TestUpdate();

            Assert.IsTrue(mouseEventCalled);
            Assert.AreEqual(actualKey, expectedtranslatedInput.InputKey);
        }

        private TestInputComponent _inputComponent;
    }
}

#endif

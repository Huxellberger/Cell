// Copyright (C) Threetee Gang All Rights Reserved

using Assets.Scripts.Components.Character;
using Assets.Scripts.Components.Species;
using Assets.Scripts.Input;
using Assets.Scripts.Test.Components.Species;
using NUnit.Framework;
using UnityEngine;

namespace Assets.Editor.UnitTests.Components.Character
{
    [TestFixture]
    public class AnimalInputHandlerTestFixture
    {
        private MockSpeciesComponent _species;

        [SetUp]
        public void BeforeTest()
        {
            _species = new GameObject().AddComponent<MockSpeciesComponent>();
        }

        [TearDown]
        public void AfterTest()
        {
            _species = null;
        }

        [Test]
        public void HandlePositiveAnimalCry_NoSpeciesInterface_Unhandled()
        {
            var handler = new AnimalInputHandler(null);

            Assert.AreEqual(EInputHandlerResult.Unhandled, handler.HandleButtonInput(EInputKey.PositiveAnimalCry, true));
        }

        [Test]
        public void HandlePositiveAnimalCry_SpeciesInterface_Pressed_Handled()
        {
            var handler = new AnimalInputHandler(_species);

            Assert.AreEqual(EInputHandlerResult.Handled, handler.HandleButtonInput(EInputKey.PositiveAnimalCry, true));
        }

        [Test]
        public void HandlePositiveAnimalCry_SpeciesInterface_Pressed_PositiveAnimalCry()
        {
            var handler = new AnimalInputHandler(_species);

            handler.HandleButtonInput(EInputKey.PositiveAnimalCry, true);

            Assert.IsTrue(_species.SpeciesCryCalled);
            Assert.AreEqual(ECryType.Positive, _species.SpeciesCryTypeInput);
        }

        [Test]
        public void HandlePositiveAnimalCry_SpeciesInterface_Unpressed_Handled()
        {
            var handler = new AnimalInputHandler(_species);

            Assert.AreEqual(EInputHandlerResult.Handled, handler.HandleButtonInput(EInputKey.PositiveAnimalCry, false));
        }

        [Test]
        public void HandlePositiveAnimalCry_SpeciesInterface_Unpressed_NoPositiveAnimalCry()
        {
            var handler = new AnimalInputHandler(_species);

            handler.HandleButtonInput(EInputKey.PositiveAnimalCry, false);

            Assert.IsFalse(_species.SpeciesCryCalled);
        }

        [Test]
        public void HandleNegativeAnimalCry_NoSpeciesInterface_Unhandled()
        {
            var handler = new AnimalInputHandler(null);

            Assert.AreEqual(EInputHandlerResult.Unhandled, handler.HandleButtonInput(EInputKey.NegativeAnimalCry, true));
        }

        [Test]
        public void HandleNegativeAnimalCry_SpeciesInterface_Pressed_Handled()
        {
            var handler = new AnimalInputHandler(_species);

            Assert.AreEqual(EInputHandlerResult.Handled, handler.HandleButtonInput(EInputKey.NegativeAnimalCry, true));
        }

        [Test]
        public void HandleNegativeAnimalCry_SpeciesInterface_Pressed_NegativeAnimalCry()
        {
            var handler = new AnimalInputHandler(_species);

            handler.HandleButtonInput(EInputKey.NegativeAnimalCry, true);

            Assert.IsTrue(_species.SpeciesCryCalled);
            Assert.AreEqual(ECryType.Negative, _species.SpeciesCryTypeInput);
        }

        [Test]
        public void HandleNegativeAnimalCry_SpeciesInterface_Unpressed_Handled()
        {
            var handler = new AnimalInputHandler(_species);

            Assert.AreEqual(EInputHandlerResult.Handled, handler.HandleButtonInput(EInputKey.NegativeAnimalCry, false));
        }

        [Test]
        public void HandleNegativeAnimalCry_SpeciesInterface_Unpressed_NoNegativeAnimalCry()
        {
            var handler = new AnimalInputHandler(_species);

            handler.HandleButtonInput(EInputKey.NegativeAnimalCry, false);

            Assert.IsFalse(_species.SpeciesCryCalled);
        }
    }
}

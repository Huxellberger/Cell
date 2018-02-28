// Copyright (C) Threetee Gang All Rights Reserved 

using Assets.Scripts.Components.ActionStateMachine.States.OpenMenuUI;
using Assets.Scripts.Input;
using NUnit.Framework;

namespace Assets.Editor.UnitTests.Components.ActionStateMachine.States.OpenMenuUI
{
    [TestFixture]
    public class InGameMenuInputHandlerTestFixture
    {
        private InGameMenuInputHandler _handler;

        [SetUp]
        public void BeforeTest()
        {
            _handler = new InGameMenuInputHandler();
        }

        [TearDown]
        public void AfterTest()
        {
            _handler = null;
        }

        [Test]
        public void HandleButtonInput_SprintButton_Pressed_Handled()
        {
            Assert.AreEqual(EInputHandlerResult.Handled, _handler.HandleButtonInput(EInputKey.SprintButton, true));
        }

        [Test]
        public void HandleButtonInput_SprintButton_Unpressed_Handled()
        {
            Assert.AreEqual(EInputHandlerResult.Handled, _handler.HandleButtonInput(EInputKey.SprintButton, false));
        }

        [Test]
        public void HandleButtonInput_CameraZoomReset_Pressed_Handled()
        {
            Assert.AreEqual(EInputHandlerResult.Handled, _handler.HandleButtonInput(EInputKey.CameraZoomReset, true));
        }

        [Test]
        public void HandleButtonInput_CameraZoomReset_Unpressed_Handled()
        {
            Assert.AreEqual(EInputHandlerResult.Handled, _handler.HandleButtonInput(EInputKey.CameraZoomReset, false));
        }

        [Test]
        public void HandleButtonInput_Interact_Pressed_Handled()
        {
            Assert.AreEqual(EInputHandlerResult.Handled, _handler.HandleButtonInput(EInputKey.Interact, true));
        }

        [Test]
        public void HandleButtonInput_Interact_Unpressed_Handled()
        {
            Assert.AreEqual(EInputHandlerResult.Handled, _handler.HandleButtonInput(EInputKey.Interact, false));
        }

        [Test]
        public void HandleButtonInput_PositiveAnimalCry_Pressed_Handled()
        {
            Assert.AreEqual(EInputHandlerResult.Handled, _handler.HandleButtonInput(EInputKey.PositiveAnimalCry, true));
        }

        [Test]
        public void HandleButtonInput_PositiveAnimalCry_Unpressed_Handled()
        {
            Assert.AreEqual(EInputHandlerResult.Handled, _handler.HandleButtonInput(EInputKey.PositiveAnimalCry, false));
        }

        [Test]
        public void HandleButtonInput_NegativeAnimalCry_Pressed_Handled()
        {
            Assert.AreEqual(EInputHandlerResult.Handled, _handler.HandleButtonInput(EInputKey.NegativeAnimalCry, true));
        }

        [Test]
        public void HandleButtonInput_NegativeAnimalCry_Unpressed_Handled()
        {
            Assert.AreEqual(EInputHandlerResult.Handled, _handler.HandleButtonInput(EInputKey.NegativeAnimalCry, false));
        }

        [Test]
        public void HandleButtonInput_PrimaryHeldAction_Pressed_Handled()
        {
            Assert.AreEqual(EInputHandlerResult.Handled, _handler.HandleButtonInput(EInputKey.PrimaryHeldAction, true));
        }

        [Test]
        public void HandleButtonInput_PrimaryHeldAction_Unpressed_Handled()
        {
            Assert.AreEqual(EInputHandlerResult.Handled, _handler.HandleButtonInput(EInputKey.PrimaryHeldAction, false));
        }

        [Test]
        public void HandleButtonInput_SecondaryHeldAction_Pressed_Handled()
        {
            Assert.AreEqual(EInputHandlerResult.Handled, _handler.HandleButtonInput(EInputKey.SecondaryHeldAction, true));
        }

        [Test]
        public void HandleButtonInput_SecondaryHeldAction_Unpressed_Handled()
        {
            Assert.AreEqual(EInputHandlerResult.Handled, _handler.HandleButtonInput(EInputKey.SecondaryHeldAction, false));
        }

        [Test]
        public void HandleButtonInput_DropHeldItem_Pressed_Handled()
        {
            Assert.AreEqual(EInputHandlerResult.Handled, _handler.HandleButtonInput(EInputKey.DropHeldItem, true));
        }

        [Test]
        public void HandleButtonInput_DropHeldItem_Unpressed_Handled()
        {
            Assert.AreEqual(EInputHandlerResult.Handled, _handler.HandleButtonInput(EInputKey.DropHeldItem, false));
        }

        [Test]
        public void HandleButtonInput_CameraToggle__Pressed_Handled()
        {
            Assert.AreEqual(EInputHandlerResult.Handled, _handler.HandleButtonInput(EInputKey.CameraToggle, true));
        }

        [Test]
        public void HandleButtonInput_CameraToggle__Unpressed_Handled()
        {
            Assert.AreEqual(EInputHandlerResult.Handled, _handler.HandleButtonInput(EInputKey.CameraToggle, false));
        }

        [Test]
        public void HandleAnalogInput_CameraHorizontal_Handled()
        {
            Assert.AreEqual(EInputHandlerResult.Handled, _handler.HandleAnalogInput(EInputKey.CameraHorizontal, 1.0f));
        }

        [Test]
        public void HandleAnalogInput_CameraVertical_Handled()
        {
            Assert.AreEqual(EInputHandlerResult.Handled, _handler.HandleAnalogInput(EInputKey.CameraVertical, 1.0f));
        }

        [Test]
        public void HandleAnalogInput_CameraZoom_Handled()
        {
            Assert.AreEqual(EInputHandlerResult.Handled, _handler.HandleAnalogInput(EInputKey.CameraZoom, 1.0f));
        }

        
    }
}

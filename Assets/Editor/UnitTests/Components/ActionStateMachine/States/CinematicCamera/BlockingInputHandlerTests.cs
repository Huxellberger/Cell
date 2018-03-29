// Copyright (C) Threetee Gang All Rights Reserved

using Assets.Scripts.Components.ActionStateMachine.States.CinematicCamera;
using Assets.Scripts.Input;
using NUnit.Framework;

namespace Assets.Editor.UnitTests.Components.ActionStateMachine.States.CinematicCamera
{
    [TestFixture]
    public class BlockingInputHandlerTestFixture 
    {
        private BlockingInputHandler _handler;

        [SetUp]
        public void BeforeTest()
        {
            _handler = new BlockingInputHandler();
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
        public void HandleButtonInput_PrimaryPower_Pressed_Handled()
        {
            Assert.AreEqual(EInputHandlerResult.Handled, _handler.HandleButtonInput(EInputKey.PrimaryPower, true));
        }

        [Test]
        public void HandleButtonInput_PrimaryPower_Unpressed_Handled()
        {
            Assert.AreEqual(EInputHandlerResult.Handled, _handler.HandleButtonInput(EInputKey.PrimaryPower, false));
        }

        [Test]
        public void HandleButtonInput_SecondaryPower_Pressed_Handled()
        {
            Assert.AreEqual(EInputHandlerResult.Handled, _handler.HandleButtonInput(EInputKey.SecondaryPower, true));
        }

        [Test]
        public void HandleButtonInput_SecondaryPower_Unpressed_Handled()
        {
            Assert.AreEqual(EInputHandlerResult.Handled, _handler.HandleButtonInput(EInputKey.SecondaryPower, false));
        }

        [Test]
        public void HandleButtonInput_UseActiveGadget_Pressed_Handled()
        {
            Assert.AreEqual(EInputHandlerResult.Handled, _handler.HandleButtonInput(EInputKey.UseActiveGadget, true));
        }

        [Test]
        public void HandleButtonInput_UseActiveGadget_Unpressed_Handled()
        {
            Assert.AreEqual(EInputHandlerResult.Handled, _handler.HandleButtonInput(EInputKey.UseActiveGadget, false));
        }

        [Test]
        public void HandleButtonInput_CycleGadgetPositive_Pressed_Handled()
        {
            Assert.AreEqual(EInputHandlerResult.Handled, _handler.HandleButtonInput(EInputKey.CycleGadgetPositive, true));
        }

        [Test]
        public void HandleButtonInput_CycleGadgetPositive_Unpressed_Handled()
        {
            Assert.AreEqual(EInputHandlerResult.Handled, _handler.HandleButtonInput(EInputKey.CycleGadgetPositive, false));
        }

        [Test]
        public void HandleButtonInput_CycleGadgetNegative_Pressed_Handled()
        {
            Assert.AreEqual(EInputHandlerResult.Handled, _handler.HandleButtonInput(EInputKey.CycleGadgetNegative, true));
        }

        [Test]
        public void HandleButtonInput_CycleGadgetNegative_Unpressed_Handled()
        {
            Assert.AreEqual(EInputHandlerResult.Handled, _handler.HandleButtonInput(EInputKey.CycleGadgetNegative, false));
        }

        [Test]
        public void HandleAnalogInput_CameraHorizontal_Handled()
        {
            Assert.AreEqual(EInputHandlerResult.Handled, _handler.HandleAnalogInput(EInputKey.CameraHorizontal, 1.0f));
        }

        [Test]
        public void HandleAnalogInput_CameraZoom_Handled()
        {
            Assert.AreEqual(EInputHandlerResult.Handled, _handler.HandleAnalogInput(EInputKey.CameraZoom, 1.0f));
        }

        [Test]
        public void HandleAnalogInput_VerticalAnalog_Handled()
        {
            Assert.AreEqual(EInputHandlerResult.Handled, _handler.HandleAnalogInput(EInputKey.VerticalAnalog, 1.0f));
        }

        [Test]
        public void HandleAnalogInput_HorizontalAnalog_Handled()
        {
            Assert.AreEqual(EInputHandlerResult.Handled, _handler.HandleAnalogInput(EInputKey.HorizontalAnalog, 1.0f));
        }
    }
}

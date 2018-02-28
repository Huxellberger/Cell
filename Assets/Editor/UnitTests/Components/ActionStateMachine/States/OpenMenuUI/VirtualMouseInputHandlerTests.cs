// Copyright (C) Threetee Gang All Rights Reserved

using Assets.Scripts.Components.ActionStateMachine.States.OpenMenuUI;
using Assets.Scripts.Input;
using Assets.Scripts.Test.UI.VirtualMouse;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Editor.UnitTests.Components.ActionStateMachine.States.OpenMenuUI
{
    [TestFixture]
    public class VirtualMouseInputHandlerTestFixture
    {
        private MockVirtualMouseComponent _virtualMouse;

        [SetUp]
        public void BeforeTest()
        {
            _virtualMouse = new GameObject().AddComponent<MockVirtualMouseComponent>();
        }

        [TearDown]
        public void AfterTest()
        {
            _virtualMouse = null;
        }

        [Test]
        public void HorizontalAnalog_NoVirtualMouseInterface_Unhandled()
        {
            var handler = new VirtualMouseInputHandler(null);

            Assert.AreEqual(EInputHandlerResult.Unhandled, handler.HandleAnalogInput(EInputKey.HorizontalAnalog, 1.0f));
        }

        [Test]
        public void HorizontalAnalog_VirtualMouseInterface_Handled()
        {
            var handler = new VirtualMouseInputHandler(_virtualMouse);

            Assert.AreEqual(EInputHandlerResult.Handled, handler.HandleAnalogInput(EInputKey.HorizontalAnalog, 1.0f));
        }

        [Test]
        public void HorizontalAnalog_VirtualMouseInterface_SetsHorizontalMovement()
        {
            var handler = new VirtualMouseInputHandler(_virtualMouse);
            const float expectedValue = 0.6f;

           handler.HandleAnalogInput(EInputKey.HorizontalAnalog, expectedValue);

           Assert.AreEqual(expectedValue, _virtualMouse.ApplyHorizontalMovementResult);
        }

        [Test]
        public void VerticalAnalog_NoVirtualMouseInterface_Unhandled()
        {
            var handler = new VirtualMouseInputHandler(null);

            Assert.AreEqual(EInputHandlerResult.Unhandled, handler.HandleAnalogInput(EInputKey.VerticalAnalog, 1.0f));
        }

        [Test]
        public void VerticalAnalog_VirtualMouseInterface_Handled()
        {
            var handler = new VirtualMouseInputHandler(_virtualMouse);

            Assert.AreEqual(EInputHandlerResult.Handled, handler.HandleAnalogInput(EInputKey.VerticalAnalog, 1.0f));
        }

        [Test]
        public void VerticalAnalog_VirtualMouseInterface_SetsVerticalMovement()
        {
            var handler = new VirtualMouseInputHandler(_virtualMouse);
            const float expectedValue = 0.6f;

            handler.HandleAnalogInput(EInputKey.VerticalAnalog, expectedValue);

            Assert.AreEqual(expectedValue, _virtualMouse.ApplyVerticalMovementResult);
        }

        [Test]
        public void VirtualLeftClick_NoVirtualMouseInterface_Unhandled()
        {
            var handler = new VirtualMouseInputHandler(null);

            Assert.AreEqual(EInputHandlerResult.Unhandled, handler.HandleAnalogInput(EInputKey.VirtualLeftClick, 1.0f));
        }

        [Test]
        public void VirtualLeftClick_VirtualMouseInterface_Handled()
        {
            var handler = new VirtualMouseInputHandler(_virtualMouse);

            Assert.AreEqual(EInputHandlerResult.Handled, handler.HandleButtonInput(EInputKey.VirtualLeftClick, false));
        }

        [Test]
        public void VirtualLeftClick_VirtualMouseInterface_SetsLeftButton()
        {
            var handler = new VirtualMouseInputHandler(_virtualMouse);
            const bool expectedValue = true;

            handler.HandleButtonInput(EInputKey.VirtualLeftClick, expectedValue);

            Assert.AreEqual(PointerEventData.InputButton.Left, _virtualMouse.SetButtonStateButtonResult);
            Assert.AreEqual(expectedValue, _virtualMouse.SetButtonStatePressedResult);
        }

        [Test]
        public void VirtualRightClick_NoVirtualMouseInterface_Unhandled()
        {
            var handler = new VirtualMouseInputHandler(null);

            Assert.AreEqual(EInputHandlerResult.Unhandled, handler.HandleAnalogInput(EInputKey.VirtualRightClick, 1.0f));
        }

        [Test]
        public void VirtualRightClick_VirtualMouseInterface_Handled()
        {
            var handler = new VirtualMouseInputHandler(_virtualMouse);

            Assert.AreEqual(EInputHandlerResult.Handled, handler.HandleButtonInput(EInputKey.VirtualRightClick, false));
        }

        [Test]
        public void VirtualRightClick_VirtualMouseInterface_SetsRightButton()
        {
            var handler = new VirtualMouseInputHandler(_virtualMouse);
            const bool expectedValue = true;

            handler.HandleButtonInput(EInputKey.VirtualRightClick, expectedValue);

            Assert.AreEqual(PointerEventData.InputButton.Right, _virtualMouse.SetButtonStateButtonResult);
            Assert.AreEqual(expectedValue, _virtualMouse.SetButtonStatePressedResult);
        }

        [Test]
        public void VirtualMiddleClick_NoVirtualMouseInterface_Unhandled()
        {
            var handler = new VirtualMouseInputHandler(null);

            Assert.AreEqual(EInputHandlerResult.Unhandled, handler.HandleAnalogInput(EInputKey.VirtualMiddleClick, 1.0f));
        }

        [Test]
        public void VirtualMiddleClick_VirtualMouseInterface_Handled()
        {
            var handler = new VirtualMouseInputHandler(_virtualMouse);

            Assert.AreEqual(EInputHandlerResult.Handled, handler.HandleButtonInput(EInputKey.VirtualMiddleClick, false));
        }

        [Test]
        public void VirtualMiddleClick_VirtualMouseInterface_SetsMiddleButton()
        {
            var handler = new VirtualMouseInputHandler(_virtualMouse);
            const bool expectedValue = true;

            handler.HandleButtonInput(EInputKey.VirtualMiddleClick, expectedValue);

            Assert.AreEqual(PointerEventData.InputButton.Middle, _virtualMouse.SetButtonStateButtonResult);
            Assert.AreEqual(expectedValue, _virtualMouse.SetButtonStatePressedResult);
        }
    }
}

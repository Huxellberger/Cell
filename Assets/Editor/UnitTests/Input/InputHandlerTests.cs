// Copyright (C) Threetee Gang All Rights Reserved

#if UNITY_EDITOR

using Assets.Scripts.Input;
using NUnit.Framework;
using UnityEngine;

namespace Assets.Editor.UnitTests.Input
{
    [TestFixture]
    public class InputHandlerTestFixture {

        [Test]
        public void Button_NoBinding_Unhandled()
        {
            var inputHandler = new TestInputHandler();

            Assert.AreEqual(inputHandler.HandleButtonInput(EInputKey.HorizontalAnalog, false), EInputHandlerResult.Unhandled);
        }

        [Test]
        public void Button_WrongBinding_Unhandled()
        {
            var inputHandler = new TestInputHandler();

            inputHandler.AddButtonResponse
            (
                EInputKey.JumpButton, pressed => EInputHandlerResult.Handled
            );

            Assert.AreEqual(inputHandler.HandleButtonInput(EInputKey.HorizontalAnalog, false), EInputHandlerResult.Unhandled);
            inputHandler.ClearResponses();
        }

        [Test]
        public void Button_BindingHandles_Handled()
        {
            var inputHandler = new TestInputHandler();
            const EInputKey expectedInputKey = EInputKey.JumpButton;

            inputHandler.AddButtonResponse
            (
                expectedInputKey, pressed => EInputHandlerResult.Handled
            );

            Assert.AreEqual(inputHandler.HandleButtonInput(expectedInputKey, false), EInputHandlerResult.Handled);
            inputHandler.ClearResponses();
        }

        [Test]
        public void Button_BindingDoesNotHandle_Unhandled()
        {
            var inputHandler = new TestInputHandler();
            const EInputKey expectedInputKey = EInputKey.JumpButton;

            inputHandler.AddButtonResponse
            (
                expectedInputKey, pressed => EInputHandlerResult.Unhandled
            );

            Assert.AreEqual(inputHandler.HandleButtonInput(expectedInputKey, false), EInputHandlerResult.Unhandled);
            inputHandler.ClearResponses();
        }

        [Test]
        public void Analog_NoBinding_Unhandled()
        {
            var inputHandler = new TestInputHandler();

            Assert.AreEqual(inputHandler.HandleAnalogInput(EInputKey.HorizontalAnalog, 0.0f), EInputHandlerResult.Unhandled);
        }

        [Test]
        public void Analog_WrongBinding_Unhandled()
        {
            var inputHandler = new TestInputHandler();

            inputHandler.AddAnalogResponse
            (
                EInputKey.JumpButton, analogValue => EInputHandlerResult.Handled
            );

            Assert.AreEqual(inputHandler.HandleAnalogInput(EInputKey.HorizontalAnalog, 0.0f), EInputHandlerResult.Unhandled);
            inputHandler.ClearResponses();
        }

        [Test]
        public void Analog_BindingHandles_Handled()
        {
            var inputHandler = new TestInputHandler();
            const EInputKey expectedInputKey = EInputKey.JumpButton;

            inputHandler.AddAnalogResponse
            (
                expectedInputKey, analogValue => EInputHandlerResult.Handled
            );

            Assert.AreEqual(inputHandler.HandleAnalogInput(expectedInputKey, 0.0f), EInputHandlerResult.Handled);
            inputHandler.ClearResponses();
        }

        [Test]
        public void Analog_BindingDoesNotHandle_Unhandled()
        {
            var inputHandler = new TestInputHandler();
            const EInputKey expectedInputKey = EInputKey.JumpButton;

            inputHandler.AddAnalogResponse
            (
                expectedInputKey, analogValue => EInputHandlerResult.Unhandled
            );

            Assert.AreEqual(inputHandler.HandleAnalogInput(expectedInputKey, 0.0f), EInputHandlerResult.Unhandled);
            inputHandler.ClearResponses();
        }

        [Test]
        public void Mouse_NoBinding_Unhandled()
        {
            var inputHandler = new TestInputHandler();

            Assert.AreEqual(inputHandler.HandleMouseInput(EInputKey.HorizontalAnalog, new Vector3()), EInputHandlerResult.Unhandled);
        }

        [Test]
        public void Mouse_WrongBinding_Unhandled()
        {
            var inputHandler = new TestInputHandler();

            inputHandler.AddMouseResponse
            (
                EInputKey.JumpButton, mousePosition => EInputHandlerResult.Handled
            );

            Assert.AreEqual(inputHandler.HandleMouseInput(EInputKey.HorizontalAnalog, new Vector3()), EInputHandlerResult.Unhandled);
            inputHandler.ClearResponses();
        }

        [Test]
        public void Mouse_BindingHandles_Handled()
        {
            var inputHandler = new TestInputHandler();
            const EInputKey expectedInputKey = EInputKey.JumpButton;

            inputHandler.AddMouseResponse
            (
                expectedInputKey, mousePosition => EInputHandlerResult.Handled
            );

            Assert.AreEqual(inputHandler.HandleMouseInput(expectedInputKey, new Vector3()), EInputHandlerResult.Handled);
            inputHandler.ClearResponses();
        }

        [Test]
        public void Mouse_BindingDoesNotHandle_Unhandled()
        {
            var inputHandler = new TestInputHandler();
            const EInputKey expectedInputKey = EInputKey.JumpButton;

            inputHandler.AddMouseResponse
            (
                expectedInputKey, mousePosition => EInputHandlerResult.Unhandled
            );

            Assert.AreEqual(inputHandler.HandleMouseInput(expectedInputKey, new Vector3()), EInputHandlerResult.Unhandled);
            inputHandler.ClearResponses();
        }
    }
}

#endif

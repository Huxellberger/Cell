// Copyright (C) Threetee Gang All Rights Reserved

#if UNITY_EDITOR

using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Input;
using Assets.Scripts.Input.Handlers;
using NUnit.Framework;
using UnityEngine;

namespace Assets.Editor.UnitTests.Input.Handlers
{
    [TestFixture]
    public class CustomInputHandlerTestFixture
    {
        private bool _callbackInvoked;
        private readonly List<EInputKey> _expectedInputs = new List<EInputKey> {EInputKey.Interact, EInputKey.HorizontalAnalog};
        private EInputKey _unexpectedInput = EInputKey.VerticalAnalog;

        [SetUp]
        public void BeforeTest()
        {
            _callbackInvoked = false;
        }

        [Test]
        public void ConstructWithButtonInputs_PressingButtonInvokesDelegate()
        {
            var handler = new CustomInputHandler(_expectedInputs, OnPressButton);

            handler.HandleButtonInput(_expectedInputs.First(), true);

            Assert.IsTrue(_callbackInvoked);
        }

        [Test]
        public void ConstructWithButtonInputs_PressingWrongButtonInvokesNothing()
        {
            var handler = new CustomInputHandler(_expectedInputs, OnPressButton);

            handler.HandleButtonInput(_unexpectedInput, true);

            Assert.IsFalse(_callbackInvoked);
        }

        [Test]
        public void ConstructWithAnalogInputs_PressingAnalogInvokesDelegate()
        {
            var handler = new CustomInputHandler(_expectedInputs, OnPressAnalog);

            handler.HandleAnalogInput(_expectedInputs.First(), 1.0f);

            Assert.IsTrue(_callbackInvoked);
        }

        [Test]
        public void ConstructWithAnalogInputs_PressingWrongAnalogInvokesNothing()
        {
            var handler = new CustomInputHandler(_expectedInputs, OnPressAnalog);

            handler.HandleAnalogInput(_unexpectedInput, 1.0f);

            Assert.IsFalse(_callbackInvoked);
        }

        [Test]
        public void ConstructWithMouseInputs_PressingMouseInvokesDelegate()
        {
            var handler = new CustomInputHandler(_expectedInputs, OnPressMouse);

            handler.HandleMouseInput(_expectedInputs.First(), Vector3.back);

;           Assert.IsTrue(_callbackInvoked);
        }

        [Test]
        public void ConstructWithMouseInputs_PressingWrongMouseInvokesNothing()
        {
            var handler = new CustomInputHandler(_expectedInputs, OnPressMouse);

            handler.HandleMouseInput(_unexpectedInput, Vector3.back);

            Assert.IsFalse(_callbackInvoked);
        }

        private EInputHandlerResult OnPressButton(bool isPressed)
        {
            _callbackInvoked = true;
            return EInputHandlerResult.Handled;
        }

        private EInputHandlerResult OnPressAnalog(float inValue)
        {
            _callbackInvoked = true;
            return EInputHandlerResult.Handled;
        }

        private EInputHandlerResult OnPressMouse(Vector3 inCoord)
        {
            _callbackInvoked = true;
            return EInputHandlerResult.Handled;
        }
    }
}

#endif

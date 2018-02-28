// Copyright (C) Threetee Gang All Rights Reserved

#if UNITY_EDITOR

using System;
using Assets.Scripts.Input;
using Assets.Scripts.Test.Input;
using NUnit.Framework;
using UnityEngine;

namespace Assets.Editor.UnitTests.Input
{
    public class InputBinderTestInputHandler
        : InputHandler
    {
        public InputBinderTestInputHandler(EInputHandlerResult inInputHandlerResult)
            :base()
        {
            ButtonResponses.Add
            (
                InputKeyToUse, pressed =>
                {
                    ReceivedResponse = true;
                    return inInputHandlerResult;
                }
            );

            AnalogResponses.Add
            (
                InputKeyToUse, analogValue =>
                {
                    ReceivedResponse = true;
                    return inInputHandlerResult;
                }
            );

            MouseResponses.Add
            (
                InputKeyToUse, mousePosition =>
                {
                    ReceivedResponse = true;
                    return inInputHandlerResult;
                }
            );
        }

        public bool ReceivedResponse { get; set; }
        public static readonly EInputKey InputKeyToUse = EInputKey.JumpButton;
    };

    [TestFixture]
    public class InputBinderComponentTests
    {

        [SetUp]
        public void SetupTest()
        {
            _inputBinderComponent = new GameObject().AddComponent<TestInputBinderComponent>();
            _inputBinderComponent.TestAwake();
            _inputInterface = new MockInputInterface();
        }

        [TearDown]
        public void TearDown()
        {
            _inputInterface = null;
            _inputBinderComponent = null;
        }

        [Test]
        public void ReceiveButtonEvent_ExecutesThroughHandler()
        {
            var firstHandler = new InputBinderTestInputHandler(EInputHandlerResult.Handled);

            _inputBinderComponent.RegisterInputHandler(firstHandler);

            _inputBinderComponent.SetInputInterface(_inputInterface);

            _inputInterface.TestActivateButtonEvent(InputBinderTestInputHandler.InputKeyToUse, false);

            Assert.IsTrue(firstHandler.ReceivedResponse);
        }

        [Test]
        public void ReceiveButtonEvent_ExecutesUntilResponseHandled()
        {
            var firstHandler = new InputBinderTestInputHandler(EInputHandlerResult.Unhandled);
            var secondHandler = new InputBinderTestInputHandler(EInputHandlerResult.Handled);
            var thirdHandler = new InputBinderTestInputHandler(EInputHandlerResult.Unhandled);

            _inputBinderComponent.RegisterInputHandler(firstHandler);
            _inputBinderComponent.RegisterInputHandler(secondHandler);
            _inputBinderComponent.RegisterInputHandler(thirdHandler);

            _inputBinderComponent.SetInputInterface(_inputInterface);

            _inputInterface.TestActivateButtonEvent(InputBinderTestInputHandler.InputKeyToUse, false);

            Assert.IsTrue(thirdHandler.ReceivedResponse);
            Assert.IsTrue(secondHandler.ReceivedResponse);
            Assert.IsFalse(firstHandler.ReceivedResponse);
        }

        [Test]
        public void ReceiveAnalogEvent_ExecutesThroughHandler()
        {
            var firstHandler = new InputBinderTestInputHandler(EInputHandlerResult.Handled);

            _inputBinderComponent.RegisterInputHandler(firstHandler);

            _inputBinderComponent.SetInputInterface(_inputInterface);

            _inputInterface.TestActivateAnalogEvent(InputBinderTestInputHandler.InputKeyToUse, 1.0f);

            Assert.IsTrue(firstHandler.ReceivedResponse);
        }

        [Test]
        public void ReceiveAnalogEvent_ExecutesUntilResponseHandled()
        {
            var firstHandler = new InputBinderTestInputHandler(EInputHandlerResult.Unhandled);
            var secondHandler = new InputBinderTestInputHandler(EInputHandlerResult.Handled);
            var thirdHandler = new InputBinderTestInputHandler(EInputHandlerResult.Unhandled);

            _inputBinderComponent.RegisterInputHandler(firstHandler);
            _inputBinderComponent.RegisterInputHandler(secondHandler);
            _inputBinderComponent.RegisterInputHandler(thirdHandler);

            _inputBinderComponent.SetInputInterface(_inputInterface);

            _inputInterface.TestActivateAnalogEvent(InputBinderTestInputHandler.InputKeyToUse, 0.0f);

            Assert.IsTrue(thirdHandler.ReceivedResponse);
            Assert.IsTrue(secondHandler.ReceivedResponse);
            Assert.IsFalse(firstHandler.ReceivedResponse);
        }

        [Test]
        public void ReceiveMouseEvent_ExecutesThroughHandler()
        {
            var firstHandler = new InputBinderTestInputHandler(EInputHandlerResult.Handled);

            _inputBinderComponent.RegisterInputHandler(firstHandler);

            _inputBinderComponent.SetInputInterface(_inputInterface);

            _inputInterface.TestActivateMouseEvent(InputBinderTestInputHandler.InputKeyToUse, new Vector3());

            Assert.IsTrue(firstHandler.ReceivedResponse);
        }

        [Test]
        public void ReceiveMouseEvent_ExecutesUntilResponseHandled()
        {
            var firstHandler = new InputBinderTestInputHandler(EInputHandlerResult.Unhandled);
            var secondHandler = new InputBinderTestInputHandler(EInputHandlerResult.Handled);
            var thirdHandler = new InputBinderTestInputHandler(EInputHandlerResult.Unhandled);

            _inputBinderComponent.RegisterInputHandler(firstHandler);
            _inputBinderComponent.RegisterInputHandler(secondHandler);
            _inputBinderComponent.RegisterInputHandler(thirdHandler);

            _inputBinderComponent.SetInputInterface(_inputInterface);

            _inputInterface.TestActivateMouseEvent(InputBinderTestInputHandler.InputKeyToUse, new Vector3());

            Assert.IsTrue(thirdHandler.ReceivedResponse);
            Assert.IsTrue(secondHandler.ReceivedResponse);
            Assert.IsFalse(firstHandler.ReceivedResponse);
        }

        [Test]
        public void UnregisterHandler_DoesNotReceiveResponse()
        {
            var handler = new InputBinderTestInputHandler(EInputHandlerResult.Handled);

            _inputBinderComponent.RegisterInputHandler(handler);

            _inputBinderComponent.SetInputInterface(_inputInterface);

            _inputBinderComponent.UnregisterInputHandler(handler);

            _inputInterface.TestActivateMouseEvent(InputBinderTestInputHandler.InputKeyToUse, new Vector3());

            Assert.IsFalse(handler.ReceivedResponse);
        }

        [Test]
        public void SetNullInputInterface_ThrowsException()
        {
            IInputInterface inputInterface = null;
            Assert.Throws<ArgumentNullException>(() => _inputBinderComponent.SetInputInterface(inputInterface));
        }

        [Test]
        public void UnregisterHandlerNotRegistered_ThrowsException()
        {
            Assert.Throws<InvalidInputHandlerException>(() =>_inputBinderComponent.UnregisterInputHandler(new TestInputHandler()));
        }

        [Test]
        public void RegisterHandlerAlreadyRegistered_ThrowsException()
        {
            var repeatHandler = new TestInputHandler();

            _inputBinderComponent.RegisterInputHandler(repeatHandler);
            Assert.Throws<InvalidInputHandlerException>(() => _inputBinderComponent.RegisterInputHandler(repeatHandler));
        }

        [Test]
        public void RegisterNullHandler_ThrowsException()
        {
            TestInputHandler nullHandler = null;
            Assert.Throws<InvalidInputHandlerException>(() => _inputBinderComponent.RegisterInputHandler(nullHandler));
        }

        private TestInputBinderComponent _inputBinderComponent;
        private MockInputInterface _inputInterface;
    }
}

#endif

// Copyright (C) Threetee Gang All Rights Reserved

using Assets.Editor.UnitTests.Messaging;
using Assets.Scripts.Components.ActionStateMachine;
using Assets.Scripts.Components.ActionStateMachine.States.CinematicCamera;
using Assets.Scripts.Components.Health;
using Assets.Scripts.Messaging;
using Assets.Scripts.Test.Components.ActionStateMachine;
using Assets.Scripts.Test.Components.Character;
using Assets.Scripts.Test.Components.Controller;
using Assets.Scripts.Test.Components.Health;
using Assets.Scripts.Test.Input;
using Assets.Scripts.Test.Messaging;
using NUnit.Framework;
using UnityEngine;

namespace Assets.Editor.UnitTests.Components.ActionStateMachine.States.CinematicCamera
{
    [TestFixture]
    public class CinematicCameraActionStateTestFixture
    {
        private CinematicCameraActionState _actionState;
        private Camera _inCamera;
        private MockActionStateMachineComponent _actionStateMachine;
        private MockInputBinderComponent _inputBinder;
        private MockHealthComponent _health;
        private TestCharacterComponent _character;
        private TestControllerComponent _controller;
        private Camera _originalCamera;
        private float _cameraTime;

        [SetUp]
        public void BeforeTest()
        {
            _inCamera = new GameObject().AddComponent<Camera>();

            _actionStateMachine = new GameObject().AddComponent<MockActionStateMachineComponent>();
            _inputBinder = _actionStateMachine.gameObject.AddComponent<MockInputBinderComponent>();
            _health = _actionStateMachine.gameObject.AddComponent<MockHealthComponent>();
            _character = _inputBinder.gameObject.AddComponent<TestCharacterComponent>();
            _character.gameObject.AddComponent<TestUnityMessageEventDispatcherComponent>().TestAwake();

            _controller = new GameObject().AddComponent<TestControllerComponent>();
            _originalCamera = _controller.gameObject.AddComponent<Camera>();
            _character.ActiveController = _controller;

            _cameraTime = 2.0f;

            _actionState = new CinematicCameraActionState(new CinematicCameraActionStateInfo(_character.gameObject, _inCamera, _cameraTime));
        }

        [TearDown]
        public void AfterTest()
        {
            _actionState = null;

            _originalCamera = null;
            _controller = null;

            _character = null;
            _health = null;
            _inputBinder = null;
            _actionStateMachine = null;

            _inCamera = null;
        }

        [Test]
        public void Creation_CinematicCameraId()
        {
            Assert.AreEqual(EActionStateId.CinematicCamera, _actionState.ActionStateId);
        }

        [Test]
        public void Start_RegistersMenuInputHandler()
        {
            _actionState.Start();

            Assert.IsTrue(_inputBinder.IsHandlerOfTypeRegistered<BlockingInputHandler>());
        }

        [Test]
        public void Start_SwapsActiveStateOfCameras()
        {
            var initialState = _inCamera.enabled;
            var otherInitialState = _originalCamera.enabled;

            _actionState.Start();

            Assert.AreNotEqual(initialState, _inCamera.enabled);
            Assert.AreNotEqual(otherInitialState, _originalCamera.enabled);
        }

        [Test]
        public void Start_SendsEnterCinematicActionStateMessage()
        {
            var messageSpy = new UnityTestMessageHandleResponseObject<EnterCinematicCameraActionStateMessage>();

            var handle = UnityMessageEventFunctions.RegisterActionWithDispatcher<EnterCinematicCameraActionStateMessage>(
                _character.gameObject, messageSpy.OnResponse);

            _actionState.Start();

            Assert.IsTrue(messageSpy.ActionCalled);

            UnityMessageEventFunctions.UnregisterActionWithDispatcher(_character.gameObject, handle);
        }

        [Test]
        public void Start_BlocksHealthChanges()
        {
            _actionState.Start();

            Assert.IsFalse(_health.SetHealthChangeEnabledResult);
            Assert.AreEqual(EHealthLockReason.Cinematic, _health.SetHealthChangeEnabledLockReason);
        }

        [Test]
        public void End_UnregistersMenuInputHandler()
        {
            _actionState.Start();
            _actionState.End();

            Assert.IsTrue(_inputBinder.IsHandlerOfTypeUnregistered<BlockingInputHandler>());
        }

        [Test]
        public void End_SwapsActiveStateOfCameras()
        {
            _actionState.Start();

            var initialState = _inCamera.enabled;
            var otherInitialState = _originalCamera.enabled;

            _actionState.End();

            Assert.AreNotEqual(initialState, _inCamera.enabled);
            Assert.AreNotEqual(otherInitialState, _originalCamera.enabled);
        }

        [Test]
        public void End_SendsExitCinematicActionStateMessage()
        {
            var messageSpy = new UnityTestMessageHandleResponseObject<ExitCinematicCameraActionStateMessage>();

            _actionState.Start();

            var handle = UnityMessageEventFunctions.RegisterActionWithDispatcher<ExitCinematicCameraActionStateMessage>(
                _character.gameObject, messageSpy.OnResponse);

            _actionState.End();

            Assert.IsTrue(messageSpy.ActionCalled);

            UnityMessageEventFunctions.UnregisterActionWithDispatcher(_character.gameObject, handle);
        }

        [Test]
        public void End_EnablesHealthChanges()
        {
            _actionState.Start();
            _actionState.End();

            Assert.IsTrue(_health.SetHealthChangeEnabledResult);
            Assert.AreEqual(EHealthLockReason.Cinematic, _health.SetHealthChangeEnabledLockReason);
        }

        [Test]
        public void Update_LessThanDuration_NoTransition()
        {
            _actionState.Start();

            _actionState.Update(_cameraTime - 0.1f);

            Assert.IsNull(_actionStateMachine.RequestedId);
        }

        [Test]
        public void Update_Duration_TransitionsIntoNullState()
        {
            _actionState.Start();

            _actionState.Update(_cameraTime + 0.1f);

            Assert.AreEqual(EActionStateId.Null, _actionStateMachine.RequestedId);
            Assert.AreEqual(EActionStateMachineTrack.Cinematic, _actionStateMachine.RequestedTrack);
        }
    }
}

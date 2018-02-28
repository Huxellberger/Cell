// Copyright (C) Threetee Gang All Rights Reserved

using Assets.Editor.UnitTests.Helpers;
using Assets.Editor.UnitTests.Messaging;
using Assets.Scripts.Components.ActionStateMachine;
using Assets.Scripts.Components.ActionStateMachine.States.Spawning;
using Assets.Scripts.Components.Character;
using Assets.Scripts.Components.Controller;
using Assets.Scripts.Messaging;
using Assets.Scripts.Test.Components.ActionStateMachine;
using Assets.Scripts.Test.Components.Character;
using Assets.Scripts.Test.Input;
using Assets.Scripts.Test.Messaging;
using NUnit.Framework;
using UnityEngine;

namespace Assets.Editor.UnitTests.Components.ActionStateMachine.States.Spawning
{
    [TestFixture]
    public class SpawningActionStateTestFixture
    {
        private MockInputBinderComponent _inputBinder;
        private MockPlayerCameraComponent _playerCamera;
        private CharacterComponent _characterComponent;
        private MockActionStateMachineComponent _actionStateMachine;

        [SetUp]
        public void BeforeTest()
        {
            _inputBinder = new GameObject().AddComponent<MockInputBinderComponent>();
            _actionStateMachine = _inputBinder.gameObject.AddComponent<MockActionStateMachineComponent>();
            _characterComponent = _inputBinder.gameObject.AddComponent<CharacterComponent>();
            _inputBinder.gameObject.AddComponent<TestUnityMessageEventDispatcherComponent>().TestAwake();

            _characterComponent.ActiveController = _inputBinder.gameObject.AddComponent<ControllerComponent>();
            _playerCamera = _characterComponent.ActiveController.gameObject.AddComponent<MockPlayerCameraComponent>();
        }

        [TearDown]
        public void AfterTest()
        {
            _playerCamera = null;

            _characterComponent = null;
            _actionStateMachine = null;
            _inputBinder = null;
        }

        [Test]
        public void Created_HasSpawningActionStateID()
        {
            var actionState = new SpawningActionState(new ActionStateInfo(new GameObject()), new SpawningActionStateParams());

            Assert.AreEqual(EActionStateId.Spawning, actionState.ActionStateId);
        }

        [Test]
        public void Start_SetsCameraPositionBasedOnParams()
        {
            var spawningParams = new SpawningActionStateParams { InitialCameraLocation = new Vector3(10.0f, 11.0f, 30.0f), InitialCameraRotation = new Vector3(1.0f, 0.5f, 2.0f) };
            var spawning = new SpawningActionState(new ActionStateInfo(_inputBinder.gameObject), spawningParams);

            spawning.Start();

            ExtendedAssertions.AssertVectorsNearlyEqual(spawningParams.InitialCameraLocation, _playerCamera.SetLocation.Value);
            ExtendedAssertions.AssertVectorsNearlyEqual(spawningParams.InitialCameraRotation, _playerCamera.SetRotation.Value);
        }

        [Test]
        public void Start_RequestLocomotion()
        {
            var spawning = new SpawningActionState(new ActionStateInfo(_inputBinder.gameObject), new SpawningActionStateParams());

            spawning.Start();

            Assert.AreEqual(EActionStateMachineTrack.Locomotion, _actionStateMachine.RequestedTrack);
            Assert.AreEqual(EActionStateId.Locomotion, _actionStateMachine.RequestedId);
            Assert.AreSame(_inputBinder.gameObject, _actionStateMachine.RequestedInfo.Owner);
        }

        [Test]
        public void Start_EnterSpawningActionStateMessageSent()
        {
            var spawning = new SpawningActionState(new ActionStateInfo(_inputBinder.gameObject), new SpawningActionStateParams());

            var messageSpy = new UnityTestMessageHandleResponseObject<EnterSpawningActionStateMessage>();

            var handle =
                UnityMessageEventFunctions.RegisterActionWithDispatcher<EnterSpawningActionStateMessage>(
                    _inputBinder.gameObject, messageSpy.OnResponse);

            spawning.Start();

            Assert.IsTrue(messageSpy.ActionCalled);

            UnityMessageEventFunctions.UnregisterActionWithDispatcher(_inputBinder.gameObject, handle);
        }

        [Test]
        public void End_LeftSpawningActionStateMessageSent()
        {
            var spawning = new SpawningActionState(new ActionStateInfo(_inputBinder.gameObject), new SpawningActionStateParams());

            spawning.Start();

            var messageSpy = new UnityTestMessageHandleResponseObject<LeftSpawningActionStateMessage>();

            var handle =
                UnityMessageEventFunctions.RegisterActionWithDispatcher<LeftSpawningActionStateMessage>(
                    _inputBinder.gameObject, messageSpy.OnResponse);

            spawning.End();

            Assert.IsTrue(messageSpy.ActionCalled);

            UnityMessageEventFunctions.UnregisterActionWithDispatcher(_inputBinder.gameObject, handle);
        }
    }
}

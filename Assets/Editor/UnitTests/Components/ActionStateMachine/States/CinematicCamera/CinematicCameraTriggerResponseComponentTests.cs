// Copyright (C) Threetee Gang All Rights Reserved

using Assets.Scripts.Components.ActionStateMachine;
using Assets.Scripts.Components.ActionStateMachine.States.CinematicCamera;
using Assets.Scripts.Components.Controller;
using Assets.Scripts.Components.Trigger;
using Assets.Scripts.Messaging;
using Assets.Scripts.Mode;
using Assets.Scripts.Test.Components.ActionStateMachine;
using Assets.Scripts.Test.Components.ActionStateMachine.States.CinematicCamera;
using Assets.Scripts.Test.Messaging;
using Assets.Scripts.Test.Mode;
using NUnit.Framework;
using UnityEngine;

namespace Assets.Editor.UnitTests.Components.ActionStateMachine.States.CinematicCamera
{
    [TestFixture]
    public class CinematicCameraTriggerResponseComponentTestFixture
    {
        private TestCinematicCameraTriggerResponseComponent _cameraTrigger;
        private Camera _camera;

        private MockActionStateMachineComponent _actionStateMachine;

        [SetUp]
        public void BeforeTest()
        {
            var gameObject = new GameObject();
            _camera = gameObject.AddComponent<Camera>();
            _cameraTrigger = gameObject.AddComponent<TestCinematicCameraTriggerResponseComponent>();

            _cameraTrigger.TriggerObject = new GameObject();
            _cameraTrigger.TriggerObject.AddComponent<TestUnityMessageEventDispatcherComponent>().TestAwake();

            _cameraTrigger.TestStart();

            _actionStateMachine = new GameObject().AddComponent<MockActionStateMachineComponent>();

            var controller = new GameObject().AddComponent<ControllerComponent>();
            controller.SetPawn(_actionStateMachine.gameObject);

            var gameMode = new GameObject().AddComponent<TestGameModeComponent>();
            GameModeComponent.RegisteredGameMode = gameMode;

            gameMode.SetActiveController(controller);
        }

        [TearDown]
        public void AfterTest()
        {
            GameModeComponent.RegisteredGameMode = null;

            _actionStateMachine = null;

            _cameraTrigger.TestDestroy();

            _cameraTrigger = null;
            _camera = null;
        }

        [Test]
        public void Start_CameraDisabled()
        {
            Assert.IsFalse(_camera.enabled);
        }

        [Test]
        public void ReceiveTrigger_PushIntoCinematicCameraActionState()
        {
            TestTrigger();

            Assert.AreEqual(EActionStateMachineTrack.Cinematic, _actionStateMachine.RequestedTrack);
            Assert.AreEqual(EActionStateId.CinematicCamera, _actionStateMachine.RequestedId);

            var info = (CinematicCameraActionStateInfo)_actionStateMachine.RequestedInfo;

            Assert.AreSame(_actionStateMachine.gameObject, info.Owner);
            Assert.AreSame(_camera, info.SwappedCamera);
            Assert.AreEqual(_cameraTrigger.CinematicTime, info.CameraTime);
        }

        private void TestTrigger()
        {
            UnityMessageEventFunctions.InvokeMessageEventWithDispatcher(_cameraTrigger.TriggerObject, new TriggerMessage(_actionStateMachine.gameObject));
        }
    }
}

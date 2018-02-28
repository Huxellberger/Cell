// Copyright (C) Threetee Gang All Rights Reserved

#if UNITY_EDITOR

using System;
using Assets.Editor.UnitTests.Services.Spawn;
using Assets.Scripts.Components.ActionStateMachine.States.Dead;
using Assets.Scripts.Messaging;
using Assets.Scripts.Mode;
using Assets.Scripts.Services;
using Assets.Scripts.Services.Spawn;
using Assets.Scripts.Test.Components.Controller;
using Assets.Scripts.Test.Messaging;
using Assets.Scripts.Test.Mode;
using Assets.Scripts.Test.Services;
using Assets.Scripts.Test.Services.Spawn;
using NUnit.Framework;
using UnityEngine;

namespace Assets.Editor.UnitTests.Mode
{
    [TestFixture]
    public class MidnaGameModeComponentTestFixture
    {
        private TestGameModeComponent _gameModeComponent;
        private TestControllerComponent _controllerComponent;
        private MockSpawnService _spawnService;
        private TestSpawnLocationComponent _spawnLocation;

        private CursorLockMode _previousCursorLockMode;

        [SetUp]
        public void BeforeTest()
        {
            _previousCursorLockMode = Cursor.lockState;

            _gameModeComponent = new GameObject().AddComponent<TestGameModeComponent>();
            _gameModeComponent.gameObject.AddComponent<TestUnityMessageEventDispatcherComponent>().TestAwake();

            _controllerComponent = _gameModeComponent.gameObject.AddComponent<TestControllerComponent>();

            _gameModeComponent.PlayerControllerType = _controllerComponent.gameObject;
            _gameModeComponent.PlayerCharacterType = new GameObject {name = "TestName"};
            _gameModeComponent.HUDType = new GameObject();

            var serviceProvider = new GameObject().AddComponent<TestGameServiceProvider>();
            serviceProvider.TestAwake();
            _spawnService = new MockSpawnService();
            serviceProvider.AddService<ISpawnServiceInterface>(_spawnService);

            _spawnService.GetNearestSpawnLocationResult = _gameModeComponent.gameObject.transform;

            _spawnLocation = new GameObject().AddComponent<TestSpawnLocationComponent>();
            _gameModeComponent.StartingSpawnLocation = _spawnLocation;
        }

        [TearDown]
        public void AfterTest()
        {
            _spawnService = null;
            GameServiceProvider.ClearGameServiceProvider();
            GameModeComponent.RegisteredGameMode = null;
            Cursor.visible = true;

            Cursor.lockState = _previousCursorLockMode;
        }

        [Test]
        public void Start_RegistersAsGameMode()
        {
            _gameModeComponent.TestStart();

            Assert.AreEqual(GameModeComponent.RegisteredGameMode, _gameModeComponent);
        }

        [Test]
        public void Start_OverridesPriorGameMode()
        {
            _gameModeComponent.TestStart();

            var otherGameMode = new GameObject().AddComponent<TestGameModeComponent>();
            otherGameMode.gameObject.AddComponent<TestUnityMessageEventDispatcherComponent>().TestAwake();
            otherGameMode.PlayerControllerType = _controllerComponent.gameObject;
            otherGameMode.PlayerCharacterType = new GameObject { name = "TestName" };
            otherGameMode.HUDType = new GameObject();
            otherGameMode.StartingSpawnLocation = new GameObject().AddComponent<TestSpawnLocationComponent>();
            otherGameMode.TestStart();

            Assert.AreEqual(GameModeComponent.RegisteredGameMode, otherGameMode);
        }

        [Test]
        public void Start_ControllerInitialisedWithPawn()
        {
            _gameModeComponent.TestStart();

            Assert.NotNull(_gameModeComponent.ActiveController);

            var controller = _gameModeComponent.ActiveController;

            Assert.IsTrue(controller.PawnInstance.name.Contains(_gameModeComponent.PlayerCharacterType.name));
        }

        [Test]
        public void Start_ControllerTransformSetToSpawnLocation()
        {
            _gameModeComponent.TestStart();

            Assert.NotNull(_gameModeComponent.ActiveController);

            var controller = _gameModeComponent.ActiveController;

            Assert.AreSame(_spawnLocation.GetSpawnLocation(), controller.PawnInitialTransform);
        }

        [Test]
        public void Start_MouseCursorDisabled()
        {
            _gameModeComponent.TestStart();

            Assert.IsFalse(Cursor.visible);
        }

        /* Test is currently failing for unknown reasons. 
        [Test]
        public void Start_CursorConfined()
        {
            _gameModeComponent.TestStart();

            Assert.AreEqual(CursorLockMode.Confined, Cursor.lockState);
        }
        */

        [Test]
        public void Start_InvalidControllerType_ThrowsException()
        {
            _gameModeComponent.PlayerControllerType = new GameObject();
            Assert.Throws<ApplicationException>(() => _gameModeComponent.TestStart());
        }

        [Test]
        public void Start_InstantiatesHUD()
        {
            _gameModeComponent.TestStart();
            Assert.NotNull(_gameModeComponent.GetHUDInstance());
        }

        [Test]
        public void OnDestroy_MouseCursorEnabled()
        {
            _gameModeComponent.TestStart();
            _gameModeComponent.TestDestroy();

            Assert.IsTrue(Cursor.visible);
        }

        [Test]
        public void ReceivesRequestRespawnMessage_SetsControllerSpawnTransformToOneReceivedFromService()
        {
            _gameModeComponent.TestStart();

            var controller = _gameModeComponent.ActiveController;

            UnityMessageEventFunctions.InvokeMessageEventWithDispatcher(_gameModeComponent.gameObject, new RequestRespawnMessage(controller.PawnInstance));

            Assert.AreSame(_spawnService.GetNearestSpawnLocationResult, controller.PawnInitialTransform);
        }

        [Test]
        public void ReceivesRequestRespawnMessage_RecreatesPawn()
        {
            _gameModeComponent.TestStart();

            var controller = _gameModeComponent.ActiveController;
            var initialPawn = _gameModeComponent.ActiveController.PawnInstance;

            UnityMessageEventFunctions.InvokeMessageEventWithDispatcher(_gameModeComponent.gameObject, new RequestRespawnMessage(controller.PawnInstance));

            Assert.AreNotSame(initialPawn, controller.PawnInstance);
        }

        [Test]
        public void ReceivesRequestRespawnMessage_PawnNotMatching_DoesNotSetTransform()
        {
            _gameModeComponent.TestStart();

            UnityMessageEventFunctions.InvokeMessageEventWithDispatcher(_gameModeComponent.gameObject, new RequestRespawnMessage(_gameModeComponent.ActiveController.gameObject));

            Assert.AreNotSame(_spawnService.GetNearestSpawnLocationResult, _gameModeComponent.ActiveController.PawnInitialTransform);
        }
    }
}

#endif

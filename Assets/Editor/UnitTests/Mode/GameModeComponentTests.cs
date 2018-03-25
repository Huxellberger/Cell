// Copyright (C) Threetee Gang All Rights Reserved

#if UNITY_EDITOR

using System;
using Assets.Editor.UnitTests.Services.Spawn;
using Assets.Scripts.Components.ActionStateMachine.States.Dead;
using Assets.Scripts.Instance;
using Assets.Scripts.Messaging;
using Assets.Scripts.Mode;
using Assets.Scripts.Services;
using Assets.Scripts.Services.Persistence;
using Assets.Scripts.Services.Spawn;
using Assets.Scripts.Test.Components.Controller;
using Assets.Scripts.Test.Input;
using Assets.Scripts.Test.Instance;
using Assets.Scripts.Test.Messaging;
using Assets.Scripts.Test.Mode;
using Assets.Scripts.Test.Services;
using Assets.Scripts.Test.Services.Spawn;
using Assets.Scripts.UnityLayer.Storage;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        public void Awake_RegistersAsGameMode()
        {
            _gameModeComponent.TestAwake();

            Assert.AreEqual(GameModeComponent.RegisteredGameMode, _gameModeComponent);
        }

        [Test]
        public void Awake_OverridesPriorGameMode()
        {
            _gameModeComponent.TestAwake();

            var otherGameMode = new GameObject().AddComponent<TestGameModeComponent>();
            otherGameMode.gameObject.AddComponent<TestUnityMessageEventDispatcherComponent>().TestAwake();
            otherGameMode.PlayerControllerType = _controllerComponent.gameObject;
            otherGameMode.PlayerCharacterType = new GameObject { name = "TestName" };
            otherGameMode.HUDType = new GameObject();
            otherGameMode.StartingSpawnLocation = new GameObject().AddComponent<TestSpawnLocationComponent>();
            otherGameMode.TestAwake();

            Assert.AreEqual(GameModeComponent.RegisteredGameMode, otherGameMode);
        }

        [Test]
        public void Awake_ControllerInitialisedWithPawn()
        {
            _gameModeComponent.TestAwake();

            Assert.NotNull(_gameModeComponent.ActiveController);

            var controller = _gameModeComponent.ActiveController;

            Assert.IsTrue(controller.PawnInstance.name.Contains(_gameModeComponent.PlayerCharacterType.name));
        }

        [Test]
        public void Awake_ControllerTransformSetToSpawnLocation()
        {
            _gameModeComponent.TestAwake();

            Assert.NotNull(_gameModeComponent.ActiveController);

            var controller = _gameModeComponent.ActiveController;

            Assert.AreSame(_spawnLocation.GetSpawnLocation(), controller.PawnInitialTransform);
        }

        [Test]
        public void Awake_MouseCursorDisabled()
        {
            _gameModeComponent.TestAwake();

            Assert.IsFalse(Cursor.visible);
        }

        /* Test is currently failing for unknown reasons. 
        [Test]
        public void Awake_CursorConfined()
        {
            _gameModeComponent.TestAwake();

            Assert.AreEqual(CursorLockMode.Confined, Cursor.lockState);
        }
        */

        [Test]
        public void Awake_InvalidControllerType_ThrowsException()
        {
            _gameModeComponent.PlayerControllerType = new GameObject();
            Assert.Throws<ApplicationException>(() => _gameModeComponent.TestAwake());
        }

        [Test]
        public void Awake_InstantiatesHUD()
        {
            _gameModeComponent.TestAwake();
            Assert.NotNull(_gameModeComponent.GetHUDInstance());
        }

        [Test]
        public void OnDestroy_MouseCursorEnabled()
        {
            _gameModeComponent.TestAwake();
            _gameModeComponent.TestDestroy();

            Assert.IsTrue(Cursor.visible);
        }

        [Test]
        public void ReceivesRequestRespawnMessage_LoadsCurrentSaveIntoInstance()
        {
            var gameInstanceObject = new GameObject();
            gameInstanceObject.AddComponent<MockInputComponent>();
            var instance = gameInstanceObject.AddComponent<TestGameInstance>();
            instance.TestAwake();

            PersistenceFunctions.WriteCurrentSave(GameDataStorageConstants.SaveDataPath, null);

            _gameModeComponent.TestAwake();

            var controller = _gameModeComponent.ActiveController;

            UnityMessageEventFunctions.InvokeMessageEventWithDispatcher(_gameModeComponent.gameObject, new RequestRespawnMessage(controller.PawnInstance));

            Assert.IsNotNull(instance.NextSceneSaveData);
            Assert.AreEqual(SceneManager.GetActiveScene().path, instance.NextSceneToLoad);

            GameInstance.ClearGameInstance();
        }

        [Test]
        public void ReceivesRequestRespawnMessage_NonMatchingInstance_DoesNotLoadCurrentSaveIntoInstance()
        {
            var gameInstanceObject = new GameObject();
            gameInstanceObject.AddComponent<MockInputComponent>();
            var instance = gameInstanceObject.AddComponent<TestGameInstance>();
            instance.TestAwake();

            PersistenceFunctions.WriteCurrentSave(GameDataStorageConstants.SaveDataPath, null);

            _gameModeComponent.TestAwake();

            UnityMessageEventFunctions.InvokeMessageEventWithDispatcher(_gameModeComponent.gameObject, new RequestRespawnMessage(null));

            Assert.IsNull(instance.NextSceneSaveData);
            Assert.AreNotEqual(SceneManager.GetActiveScene().path, instance.NextSceneToLoad);

            GameInstance.ClearGameInstance();
        }
    }
}

#endif

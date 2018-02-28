// Copyright (C) Threetee Gang All Rights Reserved

using Assets.Scripts.Instance;
using Assets.Scripts.Test.Input;
using Assets.Scripts.Test.Instance;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Assets.Editor.UnitTests.Instance
{
    [TestFixture]
    public class GameInstanceTestFixture
    {
        private MockInputComponent _inputComponent;
        private TestGameInstance _gameInstance;

        [SetUp]
        public void BeforeTest()
        {
            _inputComponent = new GameObject().AddComponent<MockInputComponent>();
            _gameInstance = _inputComponent.gameObject.AddComponent<TestGameInstance>();
        }

        [TearDown]
        public void AfterTest()
        {
            GameInstance.ClearGameInstance();

            _gameInstance = null;
            _inputComponent = null;
        }

        [Test]
        public void Awake_SetAsGameInstance()
        {
            _gameInstance.TestAwake();

            Assert.AreSame(GameInstance.CurrentInstance, _gameInstance);
        }

        [Test]
        public void Awake_InstanceAlreadySet_Error()
        {
            LogAssert.Expect(LogType.Error, "Found existing GameInstance!");

           _gameInstance.TestAwake();

            var otherTestInstance = _inputComponent.gameObject.AddComponent<TestGameInstance>();
            otherTestInstance.TestAwake();

            Assert.AreSame(GameInstance.CurrentInstance, _gameInstance);
        }

        [Test]
        public void Awake_CanBeRemovedWithClear()
        {
            _gameInstance.TestAwake();

            GameInstance.ClearGameInstance();

            Assert.IsNull(GameInstance.CurrentInstance);
        }

        [Test]
        public void Awake_InitialisesInputComponent()
        {
            _gameInstance.TestAwake();

            Assert.NotNull(_inputComponent.InputMappingProvider);
            Assert.NotNull(_inputComponent.UnityInputInterface);
        }

        [Test]
        public void GetUIDispatcher_ReturnsValidDispatcher()
        {
            _gameInstance.TestAwake();

            Assert.IsNotNull(GameInstance.CurrentInstance.GetUIMessageDispatcher());
        }

        [Test]
        public void LoadLevel_SetsNextSceneToLoad()
        {
            _gameInstance.TestAwake();

            const string nextScene = "Test";

            _gameInstance.LoadLevel(nextScene);

            Assert.IsTrue(GameInstance.CurrentInstance.NextSceneToLoad.Equals(nextScene));
        }
    }
}

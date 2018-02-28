// Copyright (C) Threetee Gang All Rights Reserved

using Assets.Scripts.Instance;
using Assets.Scripts.Test.Input;
using Assets.Scripts.Test.Instance;
using Assets.Scripts.Test.UI.MainMenu;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Editor.UnitTests.UI.MainMenu
{
    [TestFixture]
    public class LoadSceneButtonComponentTestFixture
    {
        [Test]
        public void OnClick_SetsNextSceneToLoadForInstance()
        {
            var instanceObject = new GameObject();
            instanceObject.AddComponent<MockInputComponent>();

            instanceObject.AddComponent<TestGameInstance>().TestAwake();

            var button = new GameObject().AddComponent<Button>();
            var sceneLoader = button.gameObject.AddComponent<TestLoadSceneButtonComponent>();

            sceneLoader.LevelToLoad = "NOT AN EMPTY STRING";

            sceneLoader.TestClick();

            Assert.IsTrue(GameInstance.CurrentInstance.NextSceneToLoad.Equals(sceneLoader.LevelToLoad));

            GameInstance.ClearGameInstance();
        }
    }
}

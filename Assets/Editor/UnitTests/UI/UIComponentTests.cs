// Copyright (C) Threetee Gang All Rights Reserved

using Assets.Scripts.Instance;
using Assets.Scripts.Messaging;
using Assets.Scripts.Test.Input;
using Assets.Scripts.Test.Instance;
using Assets.Scripts.Test.UI;
using Assets.Scripts.UI;
using NUnit.Framework;
using UnityEngine;

namespace Assets.Editor.UnitTests.UI
{
    [TestFixture]
    public class UIComponentTestFixture
    {
        private TestUIComponent _ui;

        [SetUp]
        public void BeforeTest()
        {
            var instanceObject = new GameObject();
            instanceObject.AddComponent<MockInputComponent>();
            instanceObject.AddComponent<TestGameInstance>().TestAwake();

            _ui = new GameObject().AddComponent<TestUIComponent>();
        }

        [TearDown]
        public void AfterTest()
        {
            _ui = null;

            GameInstance.ClearGameInstance();
        }

        [Test]
        public void Start_OnStartCalled()
        {
            Assert.IsFalse(_ui.OnStartCalled);

            _ui.TestStart();

            Assert.IsTrue(_ui.OnStartCalled);
        }

        [Test]
        public void Start_UIEnabledMessage_Disabled_ElementDisabled()
        {
            _ui.TestStart();

            GameInstance.CurrentInstance.GetUIMessageDispatcher().InvokeMessageEvent(new UpdateUIEnabledMessage(false));

            Assert.IsFalse(_ui.gameObject.activeSelf);
        }

        [Test]
        public void Start_UIEnabledMessage_DisabledThenEnabled_ReturnsToEnabled()
        {
            _ui.TestStart();

            GameInstance.CurrentInstance.GetUIMessageDispatcher().InvokeMessageEvent(new UpdateUIEnabledMessage(false));
            GameInstance.CurrentInstance.GetUIMessageDispatcher().InvokeMessageEvent(new UpdateUIEnabledMessage(true));

            Assert.IsTrue(_ui.gameObject.activeSelf);
        }

        [Test]
        public void Start_UIEnabledMessage_Enabled_StaysEnabled()
        {
            _ui.TestStart();

            GameInstance.CurrentInstance.GetUIMessageDispatcher().InvokeMessageEvent(new UpdateUIEnabledMessage(true));

            Assert.IsTrue(_ui.gameObject.activeSelf);
        }

        [Test]
        public void Start_UIEnabledMessage_DisabledWhenDisabled_StaysDisabled()
        {
            _ui.TestStart();

            _ui.gameObject.SetActive(false);

            GameInstance.CurrentInstance.GetUIMessageDispatcher().InvokeMessageEvent(new UpdateUIEnabledMessage(false));

            Assert.IsFalse(_ui.gameObject.activeSelf);
        }

        [Test]
        public void Start_UIEnabledMessage_DisabledAndEnabledWhenDisabled_StaysDisabled()
        {
            _ui.TestStart();

            _ui.gameObject.SetActive(false);

            GameInstance.CurrentInstance.GetUIMessageDispatcher().InvokeMessageEvent(new UpdateUIEnabledMessage(false));
            GameInstance.CurrentInstance.GetUIMessageDispatcher().InvokeMessageEvent(new UpdateUIEnabledMessage(true));

            Assert.IsFalse(_ui.gameObject.activeSelf);
        }

        [Test]
        public void Start_UIEnabledMessage_SetToIgnore_NoEffect()
        {
            _ui.TestStart();

            _ui.DisabledOnNotification = false;

            GameInstance.CurrentInstance.GetUIMessageDispatcher().InvokeMessageEvent(new UpdateUIEnabledMessage(false));

            Assert.IsTrue(_ui.gameObject.activeSelf);
        }

        [Test]
        public void Start_GameInstanceIsUsedAsDispatcher()
        {
            _ui.TestStart();

            Assert.AreSame(GameInstance.CurrentInstance.GetUIMessageDispatcher(), _ui.GetDispatcher());
        }

        [Test]
        public void OnDestroy_OnEndCalled()
        {
            _ui.TestStart();

            Assert.IsFalse(_ui.OnEndCalled);

            _ui.TestDestroy();

            Assert.IsTrue(_ui.OnEndCalled);
        }
    }
}

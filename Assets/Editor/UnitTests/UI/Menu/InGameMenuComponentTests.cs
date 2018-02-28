// Copyright (C) Threetee Gang All Rights Reserved 

using Assets.Scripts.Test.UI.Menu;
using Assets.Scripts.UI.Menu;
using NUnit.Framework;
using UnityEngine;

namespace Assets.Editor.UnitTests.UI.Menu
{
    [TestFixture]
    public class InGameMenuComponentTestFixture
    {
        private TestInGameMenuComponent _menu;

        [SetUp]
        public void BeforeTest()
        {
            _menu = new GameObject().AddComponent<TestInGameMenuComponent>();
            _menu.TestStart();
        }

        [TearDown]
        public void AfterTest()
        {
            _menu.TestDestroy();
            _menu = null;
        }

        [Test]
        public void Start_DeactivatesObject()
        {
            Assert.IsFalse(_menu.gameObject.activeSelf);
        }

        [Test]
        public void RequestInGameMenuActivation_ActivatesObject()
        {
            _menu.TestDispatcher.InvokeMessageEvent(new RequestInGameMenuActivationMessage());

            Assert.IsTrue(_menu.gameObject.activeSelf);
        }

        [Test]
        public void RequestInGameMenuDeactivation_DeactivatesObject()
        {
            _menu.TestDispatcher.InvokeMessageEvent(new RequestInGameMenuActivationMessage());
            _menu.TestDispatcher.InvokeMessageEvent(new RequestInGameMenuDeactivationMessage());

            Assert.IsFalse(_menu.gameObject.activeSelf);
        }
    }
}

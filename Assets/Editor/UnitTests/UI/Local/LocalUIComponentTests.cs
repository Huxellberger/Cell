// Copyright (C) Threetee Gang All Rights Reserved

using Assets.Scripts.Messaging;
using Assets.Scripts.Test.Messaging;
using Assets.Scripts.Test.UI.Local;
using NUnit.Framework;
using UnityEngine;

namespace Assets.Editor.UnitTests.UI.Local
{
    [TestFixture]
    public class LocalUIComponentTestFixture 
    {
        private TestLocalUIComponent _localUi;
        private TestLocalUIElementComponent _firstElement;
        private TestLocalUIElementComponent _secondElement;
	
        [SetUp]
        public void BeforeTest()
        {
            var gameObject = new GameObject();
            gameObject.AddComponent<TestUnityMessageEventDispatcherComponent>().TestAwake();

            _localUi = gameObject.AddComponent<TestLocalUIComponent>();

            _firstElement = gameObject.AddComponent<TestLocalUIElementComponent>();
            _secondElement = new GameObject().AddComponent<TestLocalUIElementComponent>();

            _secondElement.gameObject.transform.parent = _localUi.gameObject.transform;
        }
	
        [TearDown]
        public void AfterTest()
        {
            _secondElement = null;
            _firstElement = null;

            _localUi = null;
        }
	
        [Test]
        public void Awake_SetsDispatcherInChildElements() 
        {
            _localUi.TestAwake();

            var dispatcher = _localUi.GetComponent<IUnityMessageEventInterface>().GetUnityMessageEventDispatcher();

            Assert.AreSame(dispatcher, _firstElement.GetDispatcher());
            Assert.AreSame(dispatcher, _secondElement.GetDispatcher());
        }

        [Test]
        public void OnDestroy_SetsDispatcherToNullInChildElements()
        {
            _localUi.TestAwake();

            var dispatcher = _localUi.GetComponent<IUnityMessageEventInterface>().GetUnityMessageEventDispatcher();

            _localUi.TestDestroy();

            Assert.IsNull(_firstElement.GetDispatcher());
            Assert.IsNull(_secondElement.GetDispatcher());
        }

        [Test]
        public void Awake_ChildrenHaveOnInitCalled()
        {
            _localUi.TestAwake();

            Assert.IsTrue(_firstElement.OnElementInitialisedCalled);
            Assert.IsTrue(_secondElement.OnElementInitialisedCalled);
        }

        [Test]
        public void OnDestroy_ChildrenHaveOnUninitCalled()
        {
            _localUi.TestAwake();

            _localUi.TestDestroy();

            Assert.IsTrue(_firstElement.OnElementUninitialisedCalled);
            Assert.IsTrue(_secondElement.OnElementUninitialisedCalled);
        }
    }
}

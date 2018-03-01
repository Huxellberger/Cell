// Copyright (C) Threetee Gang All Rights Reserved

using Assets.Scripts.Messaging;
using Assets.Scripts.Test.UI.Local;
using NUnit.Framework;
using UnityEngine;

namespace Assets.Editor.UnitTests.UI.Local
{
    [TestFixture]
    public class LocalUIElementComponentTestFixture 
    {
        private TestLocalUIElementComponent _local;
        private UnityMessageEventDispatcher _dispatcher;
	
        [SetUp]
        public void BeforeTest()
        {
            _local = new GameObject().AddComponent<TestLocalUIElementComponent>();
            _dispatcher = new UnityMessageEventDispatcher();
        }
	
        [TearDown]
        public void AfterTest()
        {
            _dispatcher = null;
            _local = null;
        }

        [Test]
        public void OnInit_OnInitImplCalled()
        {
            _local.OnElementInitialised(null);

            Assert.IsTrue(_local.OnElementInitialisedCalled);
        }

        [Test]
        public void OnUninit_OnUninitImplCalled()
        {
            _local.OnElementInitialised(null);
            _local.OnElementUninitialised();

            Assert.IsTrue(_local.OnElementUninitialisedCalled);
        }

        [Test]
        public void OnInit_DispatcherUpdated()
        {
            _local.OnElementInitialised(_dispatcher);

            Assert.AreSame(_dispatcher, _local.GetDispatcher());
        }

        [Test]
        public void OnUninit_DispatcherSetToNull()
        {
            _local.OnElementInitialised(_dispatcher);
            _local.OnElementUninitialised();

            Assert.IsNull(_local.GetDispatcher());
        }
    }
}

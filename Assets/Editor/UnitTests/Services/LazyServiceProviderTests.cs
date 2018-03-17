// Copyright (C) Threetee Gang All Rights Reserved

using Assets.Editor.UnitTests.Services.Time;
using Assets.Scripts.Services;
using Assets.Scripts.Services.Time;
using Assets.Scripts.Test.Services;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Assets.Editor.UnitTests.Services
{
    [TestFixture]
    public class LazyServiceProviderTestFixture
    {
        private LazyServiceProvider<ITimeServiceInterface> _lazy;

        [SetUp]
        public void BeforeTest()
        {
            _lazy = new LazyServiceProvider<ITimeServiceInterface>();
        }
	
        [TearDown]
        public void AfterTest()
        {
            _lazy = null;
        }
	
        [Test]
        public void Get_NoServiceProvider_ReturnsNull() 
        {
            Assert.IsNull(_lazy.Get());
        }

        [Test]
        public void Get_NoServiceRegistered_ReturnsNull()
        {
            new GameObject().AddComponent<TestGameServiceProvider>().TestAwake();
            var exampleType = typeof(ITimeServiceInterface);
            LogAssert.Expect(LogType.Error, "Could not find service of type" + exampleType);
            Assert.IsNull(_lazy.Get());

            GameServiceProvider.ClearGameServiceProvider();
        }

        [Test]
        public void Get_ServiceRegistered_ReturnsRegisteredService()
        {
            var registeredService = new MockTimeService();
            new GameObject().AddComponent<TestGameServiceProvider>().TestAwake();
            GameServiceProvider.CurrentInstance.AddService<ITimeServiceInterface>(registeredService);

            Assert.AreSame(registeredService, _lazy.Get());

            GameServiceProvider.ClearGameServiceProvider();
        }
    }
}

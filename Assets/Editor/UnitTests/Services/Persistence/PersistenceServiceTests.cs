// Copyright (C) Threetee Gang All Rights Reserved

using Assets.Scripts.Services.Persistence;
using Assets.Scripts.Test.Services.Persistence;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Assets.Editor.UnitTests.Services.Persistence
{
    [TestFixture]
    public class PersistenceServiceTestFixture
    {
        private PersistenceService _service;
        private MockPersistentEntityComponent _entity;

        [SetUp]
        public void BeforeTest()
        {
            _service = new PersistenceService();

            _entity = new GameObject().AddComponent<MockPersistentEntityComponent>();
        }
	
        [TearDown]
        public void AfterTest()
        {
            _entity = null;

            _service = null;
        }
	
        [Test]
        public void Register_ReturnedWithGet()
        {
            const string key = "Key";
            _service.RegisterPersistentEntity(key, _entity);

            Assert.AreSame(_entity, _service.GetEntity(key));
        }

        [Test]
        public void Register_Null_NotReturnedWithGet()
        {
            const string key = "Key";
            _service.RegisterPersistentEntity(key, null);

            LogAssert.Expect(LogType.Error, "Failed to find entity for " + key);

            Assert.IsNull(_service.GetEntity(key));
        }

        [Test]
        public void Register_NotRegistered_ErrorAndNullReturned()
        {
            const string key = "Key";

            LogAssert.Expect(LogType.Error, "Failed to find entity for " + key);

            Assert.IsNull(_service.GetEntity(key));
        }

        [Test]
        public void Unregister_NoLongerReturned()
        {
            const string key = "Key";

            _service.RegisterPersistentEntity(key, _entity);
            _service.UnregisterPersistentEntity(key);

            LogAssert.Expect(LogType.Error, "Failed to find entity for " + key);

            _service.GetEntity(key);
        }
    }
}

// Copyright (C) Threetee Gang All Rights Reserved

using Assets.Scripts.Services.Persistence;
using Assets.Scripts.Test.Services.Persistence;
using NUnit.Framework;
using UnityEngine;

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

            Assert.AreSame(_entity, _service.GetEntities()[key]);
        }

        [Test]
        public void Register_NotRegistered_EntryNotEntered()
        {
            const string key = "Key";

            Assert.IsFalse(_service.GetEntities().ContainsKey(key));
        }

        [Test]
        public void Unregister_NullReturned()
        {
            const string key = "Key";

            _service.RegisterPersistentEntity(key, _entity);
            _service.UnregisterPersistentEntity(key);

            Assert.IsNull(_service.GetEntities()[key]);
        }
    }
}

// Copyright (C) Threetee Gang All Rights Reserved

using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Assets.Scripts.Services.Persistence;
using Assets.Scripts.Test.Services.Persistence;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Assets.Editor.UnitTests.Services.Persistence
{
    [TestFixture]
    public class PersistenceFunctionsTestFixture
    {
        private MockPersistenceService _service;

        [SetUp]
        public void BeforeTest()
        {
            _service = new MockPersistenceService
            {
                GetEntitiesResult = new Dictionary<string, IPersistentEntityInterface>
                {
                    {"TestKey", new GameObject().AddComponent<MockPersistentEntityComponent>()},
                    {"OtherTestKey", new GameObject().AddComponent<MockPersistentEntityComponent>()},
                    {"NullTestKey", null}
                }
            };
        }
	
        [TearDown]
        public void AfterTest()
        {
            _service = null;
        }
	
        [Test]
        public void SaveData_DictionarySerializedOutAsExpected()
        {
            var writeMemoryStream = new MemoryStream();

            PersistenceFunctions.SaveData(writeMemoryStream, _service);

            var binaryFormatter = new BinaryFormatter();
            var readMemoryStream = new MemoryStream(writeMemoryStream.ToArray());

            Assert.AreEqual(_service.GetEntitiesResult.Count, binaryFormatter.Deserialize(readMemoryStream));

            foreach (var entity in _service.GetEntitiesResult)
            {
                Assert.AreEqual(entity.Key, binaryFormatter.Deserialize(readMemoryStream));
                Assert.AreEqual(entity.Value == null, binaryFormatter.Deserialize(readMemoryStream));
                if (entity.Value != null)
                {
                    Assert.AreSame(((MockPersistentEntityComponent)entity.Value).WriteDataStream, writeMemoryStream);
                }
            }
        }

        [Test]
        public void LoadData_DictionaryLoadedAsExpected()
        {
            var writeMemoryStream = new MemoryStream();

            PersistenceFunctions.SaveData(writeMemoryStream, _service);
            var readMemoryStream = new MemoryStream(writeMemoryStream.ToArray());
            PersistenceFunctions.LoadData(readMemoryStream, _service);

            foreach (var entity in _service.GetEntitiesResult)
            {
                if (entity.Value != null)
                {
                    var mockEntity = (MockPersistentEntityComponent) entity.Value;
                    Assert.AreSame(mockEntity.ReadDataStream, readMemoryStream);
                    Assert.IsFalse(mockEntity.PreviouslyDestroyedResult);
                }
            }
        }

        [Test]
        public void LoadData_EntityWasNull_NowRecognisedAsDestroyed()
        {
            const string exampleKey = "WhoopsImDead";
            var exampleEntity = new GameObject().AddComponent<MockPersistentEntityComponent>();
            _service.GetEntitiesResult.Add(exampleKey, null);

            var writeMemoryStream = new MemoryStream();
            PersistenceFunctions.SaveData(writeMemoryStream, _service);

            _service.GetEntitiesResult[exampleKey] = exampleEntity;
            var readMemoryStream = new MemoryStream(writeMemoryStream.ToArray());
            PersistenceFunctions.LoadData(readMemoryStream, _service);

            Assert.IsTrue(exampleEntity.PreviouslyDestroyedResult);
        }

        [Test]
        public void EntityMissing_ThrowsErrorOnLoad()
        {
            const string exampleKey = "WhoopsImDead";
            _service.GetEntitiesResult.Add(exampleKey, null);

            var writeMemoryStream = new MemoryStream();
            PersistenceFunctions.SaveData(writeMemoryStream, _service);

            _service.GetEntitiesResult.Remove(exampleKey);
            LogAssert.Expect(LogType.Error, "Failed to find entry for key " + exampleKey);

            var readMemoryStream = new MemoryStream(writeMemoryStream.ToArray());
            PersistenceFunctions.LoadData(readMemoryStream, _service);
        }
    }
}

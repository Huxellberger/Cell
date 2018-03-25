// Copyright (C) Threetee Gang All Rights Reserved

using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Assets.Editor.UnitTests.Helpers;
using Assets.Scripts.Core;
using Assets.Scripts.Services;
using Assets.Scripts.Services.Persistence;
using Assets.Scripts.Test.Services;
using Assets.Scripts.Test.Services.Persistence;
using NUnit.Framework;
using UnityEngine;

namespace Assets.Editor.UnitTests.Services.Persistence
{
    [TestFixture]
    public class PersistentEntityComponentTestFixture
    {
        private TestPersistentEntityComponent _entity;
        private MockPersistentBehaviourComponent _behaviour;
        private MockPersistentBehaviourComponent _otherBehaviour;
        private MockPersistenceService _service;

        [SetUp]
        public void BeforeTest()
        {
            _entity = new GameObject().AddComponent<TestPersistentEntityComponent>();
            _entity.gameObject.transform.position = new Vector3(12.0f, 44.0f, 33.0f);
            _entity.gameObject.transform.eulerAngles = new Vector3(33.0f, -12.0f, 2.0f);

            _behaviour = _entity.gameObject.AddComponent<MockPersistentBehaviourComponent>();
            _otherBehaviour = _entity.gameObject.AddComponent<MockPersistentBehaviourComponent>();

            _service = new MockPersistenceService();

            new GameObject().AddComponent<TestGameServiceProvider>().TestAwake();

            GameServiceProvider.CurrentInstance.AddService<IPersistenceServiceInterface>(_service);
        }
	
        [TearDown]
        public void AfterTest()
        {
            GameServiceProvider.ClearGameServiceProvider();

            _service = null;
            _entity = null;
        }
	
        [Test]
        public void Awake_RegistersWithServiceUsingObjectName() 
        {
            _entity.TestAwake();

            Assert.AreEqual(_entity.gameObject.name, _service.RegisterPersistentEntityKey);
            Assert.AreSame(_entity, _service.RegisterPersistentEntityChosen);
        }

        [Test]
        public void Awake_UnregistersWithServiceUsingObjectName()
        {
            _entity.TestAwake();
            _entity.TestDestroy();

            Assert.AreEqual(_entity.gameObject.name, _service.UnregisterPersistentEntityKey);
        }

        [Test]
        public void WriteData_WritesPositionAndTransformToStream()
        {
            var stream = new MemoryStream();
            _entity.WriteData(stream);

            var bf = new BinaryFormatter();

            stream = new MemoryStream(stream.ToArray());

            Assert.AreEqual(_entity.gameObject.activeSelf, (bool)bf.Deserialize(stream));
            Assert.AreEqual(_entity.gameObject.transform.position, ((Vector3Serializer)bf.Deserialize(stream)).AsVector);
            Assert.AreEqual(_entity.gameObject.transform.eulerAngles, ((Vector3Serializer)bf.Deserialize(stream)).AsVector);
        }

        [Test]
        public void WriteData_WritesAllBehaviours()
        {
            var stream = new MemoryStream();
            _entity.WriteData(stream);

            Assert.AreSame(_behaviour.WriteDataStream, stream);
            Assert.AreSame(_otherBehaviour.WriteDataStream, stream);
        }

        [Test]
        public void ReadData_ReadsInPositionsCorrectly()
        {
            var priorPosition = _entity.gameObject.transform.position;
            var priorRotation = _entity.gameObject.transform.eulerAngles;
            var initialActive = _entity.gameObject.activeSelf;

            var stream = new MemoryStream();
            _entity.WriteData(stream);

            _entity.gameObject.SetActive(!_entity.gameObject.activeSelf);
            _entity.gameObject.transform.position = Vector3.down;
            _entity.gameObject.transform.eulerAngles = Vector3.forward;
        
            _entity.ReadData(new MemoryStream(stream.ToArray()), false);

            Assert.AreEqual(initialActive, _entity.gameObject.activeSelf);
            ExtendedAssertions.AssertVectorsNearlyEqual(_entity.gameObject.transform.position, priorPosition);
            ExtendedAssertions.AssertVectorsNearlyEqual(_entity.gameObject.transform.eulerAngles, priorRotation);
        }

        [Test]
        public void ReadData_ReadsAllBehaviours()
        {
            var stream = new MemoryStream();
            _entity.WriteData(stream);

            var readStream = new MemoryStream(stream.ToArray());

            _entity.ReadData(readStream, false);

            Assert.AreSame(_behaviour.ReadDataStream, readStream);
            Assert.AreSame(_otherBehaviour.ReadDataStream, readStream);
        }

        [Test]
        public void ReadData_PreviouslyDestroyed_DoesNotReadValues()
        {
            _entity.gameObject.SetActive(!_entity.gameObject.activeSelf);
            _entity.gameObject.transform.position = Vector3.down;
            _entity.gameObject.transform.eulerAngles = Vector3.forward;

            _entity.ReadData(new MemoryStream(), true);
        }

        [Test]
        public void ReadData_PreviouslyDestroyed_DoesNotReadAllBehaviours()
        {
            var stream = new MemoryStream();
            _entity.WriteData(stream);

            var readStream = new MemoryStream(stream.ToArray());

            _entity.ReadData(readStream, true);

            Assert.IsNull(_behaviour.ReadDataStream);
            Assert.IsNull(_otherBehaviour.ReadDataStream);
        }
    }
}

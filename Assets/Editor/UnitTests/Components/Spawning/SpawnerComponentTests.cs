// Copyright (C) Threetee Gang All Rights Reserved

using Assets.Scripts.Test.Components.Spawning;
using NUnit.Framework;
using UnityEngine;

namespace Assets.Editor.UnitTests.Components.Spawning
{
    [TestFixture]
    public class SpawnerComponentTestFixture
    {
        private TestSpawnerComponent _spawner;
    
        [SetUp]
        public void BeforeTest()
        {
            _spawner = new GameObject().AddComponent<TestSpawnerComponent>();

            _spawner.SpawnablePrefab = new GameObject();
            _spawner.SpawnablePrefab.AddComponent<MockSpawnableComponent>();

            _spawner.MaxSpawnCount = 2;
            _spawner.SpawnPoint = new GameObject();
            _spawner.SpawnPoint.transform.position = new Vector3(1.0f, 3.0f, 20.0f);
            _spawner.SpawnPoint.transform.eulerAngles = new Vector3(20.0f, 30.0f, 12.0f);

            _spawner.TestStart();
        }

        [TearDown]
        public void AfterTest()
        {
            _spawner = null;
        }

        [Test]
        public void Start_NoSpawnedObjects()
        {
            Assert.AreEqual(0, _spawner.GetSpawnedGameObjects().Count);
            Assert.IsNull(_spawner.OnSpawnGameObject);
        }

        [Test]
        public void Spawn_SpawnsObject()
        {
            _spawner.Spawn();
            Assert.AreEqual(1, _spawner.GetSpawnedGameObjects().Count);
        }

        [Test]
        public void Spawn_SpawnsAtLocation()
        {
            _spawner.Spawn();
            Assert.AreEqual(_spawner.SpawnPoint.transform.position, _spawner.GetSpawnedGameObjects()[0].transform.position);
            Assert.AreEqual(_spawner.SpawnPoint.transform.rotation, _spawner.GetSpawnedGameObjects()[0].transform.rotation);
        }

        [Test]
        public void Spawn_SpawnsObjectsUpToMax()
        {
            for (var currentSpawnCount = 0; currentSpawnCount < _spawner.MaxSpawnCount + 3; currentSpawnCount++)
            {
                _spawner.Spawn();
            }

            Assert.AreEqual(_spawner.MaxSpawnCount, _spawner.GetSpawnedGameObjects().Count);
        }

        [Test]
        public void Spawn_SpawnerSet()
        {
            _spawner.Spawn();

            Assert.AreSame(_spawner, _spawner.GetSpawnedGameObjects()[0].GetComponent<MockSpawnableComponent>().SetSpawnerResult);
        }

        [Test]
        public void Spawn_OnSpawnCalled()
        {
            _spawner.Spawn();

            Assert.AreSame(_spawner.GetSpawnedGameObjects()[0], _spawner.OnSpawnGameObject);
        }

        [Test]
        public void Spawn_OnSpawnedCalled()
        {
            _spawner.Spawn();

            Assert.IsTrue(_spawner.GetSpawnedGameObjects()[0].GetComponent<MockSpawnableComponent>().OnSpawnedCalled);
        }

        [Test]
        public void RequestRespawn_NotSpawnable_NoEffect()
        {
            _spawner.Spawn();

            var nonSpawnedObject = new GameObject();

            _spawner.RequestRespawn(nonSpawnedObject);

            Assert.IsTrue(nonSpawnedObject.activeSelf);
            Assert.AreEqual(1, _spawner.GetSpawnedGameObjects().Count);
        }

        [Test]
        public void RequestRespawn_RespawnsAtLocation()
        {
            _spawner.Spawn();

            var spawnedObject = _spawner.GetSpawnedGameObjects()[0];

            spawnedObject.transform.position = new Vector3(12.0f, -10.0f, 30.0f);
            spawnedObject.transform.rotation = new Quaternion(1.0f, 2.0f, 3.0f, 1.0f);

            _spawner.RequestRespawn(spawnedObject);

            Assert.AreEqual(_spawner.SpawnPoint.transform.position, spawnedObject.transform.position);
            Assert.AreEqual(_spawner.SpawnPoint.transform.rotation, spawnedObject.transform.rotation);
        }

        [Test]
        public void RequestRespawn_Spawnable_RemovedAndDeactivated()
        {
            _spawner.Spawn();

            var spawnedObject = _spawner.GetSpawnedGameObjects()[0];

            _spawner.RequestRespawn(spawnedObject);

            Assert.IsFalse(spawnedObject.activeSelf);
            Assert.AreEqual(0, _spawner.GetSpawnedGameObjects().Count);
        }
    }
}

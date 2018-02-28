// Copyright (C) Threetee Gang All Rights Reserved

using Assets.Scripts.Test.Components.Spawning;
using NUnit.Framework;
using UnityEngine;

namespace Assets.Editor.UnitTests.Components.Spawning
{
    [TestFixture]
    public class TimeDelaySpawnerComponentTestFixture
    {
        private TestTimeDelaySpawnerComponent _spawner;

        [SetUp]
        public void BeforeTest()
        {
            _spawner = new GameObject().AddComponent<TestTimeDelaySpawnerComponent>();

            _spawner.SpawnablePrefab = new GameObject();
            _spawner.SpawnablePrefab.AddComponent<MockSpawnableComponent>();

            _spawner.MaxSpawnCount = 2;
            _spawner.SpawnDelta = 3.0f;
            _spawner.SpawnPoint = new GameObject();

            _spawner.TestStart();
        }

        [TearDown]
        public void AfterTest()
        {
            _spawner = null;
        }

        [Test]
        public void Update_NotSpawnDelay_NoSpawn()
        {
            _spawner.TestUpdate(_spawner.SpawnDelta * 0.5f);

            Assert.AreEqual(0, _spawner.GetSpawnedObjects().Count);
        }

        [Test]
        public void Update_SpawnDelay_Spawn()
        {
            _spawner.TestUpdate(_spawner.SpawnDelta + 0.1f);

            Assert.AreEqual(1, _spawner.GetSpawnedObjects().Count);
        }
    }
}

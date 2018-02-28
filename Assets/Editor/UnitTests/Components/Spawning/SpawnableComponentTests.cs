// Copyright (C) Threetee Gang All Rights Reserved

using Assets.Scripts.Test.Components.Spawning;
using NUnit.Framework;
using UnityEngine;

namespace Assets.Editor.UnitTests.Components.Spawning
{
    [TestFixture]
    public class SpawnableComponentTestFixture
    {
        private TestSpawnableComponent _spawnable;
        private MockSpawnerComponent _spawner;

        [SetUp]
        public void BeforeTest()
        {
            _spawnable = new GameObject().AddComponent<TestSpawnableComponent>();
            _spawner = new GameObject().AddComponent<MockSpawnerComponent>();
        }

        [TearDown]
        public void AfterTest()
        {
            _spawner = null;
            _spawnable = null;
        }

        [Test]
        public void SetSpawner_UsedForDespawn()
        {
            _spawnable.SetSpawner(_spawner);

            _spawnable.TestDespawn();

            Assert.AreSame(_spawnable.gameObject, _spawner.RequestRespawnGameObject);
        }

        [Test]
        public void OnSpawned_OnSpawnedImplCalled()
        {
            _spawnable.OnSpawned();

            Assert.IsTrue(_spawnable.OnSpawnedImplCalled);
        }
    }
}


// Copyright (C) Threetee Gang All Rights Reserved

using Assets.Scripts.Services;
using Assets.Scripts.Services.Spawn;
using Assets.Scripts.Test.Services;
using Assets.Scripts.Test.Services.Spawn;
using NUnit.Framework;
using UnityEngine;

namespace Assets.Editor.UnitTests.Services.Spawn
{
    [TestFixture]
    public class SpawnLocationComponentTestFixture
    {
        private MockSpawnService _spawnService;
        private TestSpawnLocationComponent _spawnLocation;

        [SetUp]
        public void BeforeTest()
        {
            var serviceProvider = new GameObject().AddComponent<TestGameServiceProvider>();
            serviceProvider.TestAwake();

            _spawnService = new MockSpawnService();
            GameServiceProvider.CurrentInstance.AddService<ISpawnServiceInterface>(_spawnService);

            _spawnLocation = new GameObject().AddComponent<TestSpawnLocationComponent>();
        }

        [TearDown]
        public void AfterTest()
        {
            _spawnLocation = null;
            GameServiceProvider.ClearGameServiceProvider();
        }

        [Test]
        public void Start_RegistersWithService()
        {
            _spawnLocation.TestStart();

            Assert.AreEqual(_spawnService.RegisteredSpawnLocation, _spawnLocation);
        }

        [Test]
        public void OnDestroy_UnregistersWithService()
        {
            _spawnLocation.TestStart();
            _spawnLocation.TestDestroy();

            Assert.AreEqual(_spawnService.UnregisteredSpawnLocation, _spawnLocation);
        }

        [Test]
        public void GetNearestSpawnLocation_ReturnsTransform()
        {
            _spawnLocation.TestStart();
            
            Assert.AreSame(_spawnLocation.gameObject.transform, _spawnLocation.GetSpawnLocation());
        }
    }
}

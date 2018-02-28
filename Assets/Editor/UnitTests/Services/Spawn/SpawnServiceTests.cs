// Copyright (C) Threetee Gang All Rights Reserved

using Assets.Scripts.Services.Spawn;
using Assets.Scripts.Test.Services.Spawn;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Assets.Editor.UnitTests.Services.Spawn
{
    [TestFixture]
    public class SpawnServiceTestFixture
    {
        private SpawnService _spawnService;
        private MockSpawnLocationComponent _spawnLocation;
        private MockSpawnLocationComponent _farSpawnLocation;

        [SetUp]
        public void BeforeTest()
        {
            _spawnService = new SpawnService();

            _spawnLocation = new GameObject().AddComponent<MockSpawnLocationComponent>();
            _spawnLocation.GetSpawnLocationResult = _spawnLocation.gameObject.transform;
            _spawnLocation.gameObject.transform.position = new Vector3(1.0f, 2.0f, 3.0f);

            _farSpawnLocation = new GameObject().AddComponent<MockSpawnLocationComponent>();
            _farSpawnLocation.GetSpawnLocationResult = _farSpawnLocation.gameObject.transform;
            _farSpawnLocation.gameObject.transform.position = new Vector3(1000.0f, 2000.0f, 3000.0f);
        }

        [TearDown]
        public void AfterTest()
        {
            _farSpawnLocation = null;
            _spawnLocation = null;

            _spawnService = null;
        }

        [Test]
        public void GetSpawnLocation_ReturnsClosestToPoint()
        {
            _spawnService.RegisterSpawnLocationWithService(_spawnLocation);
            _spawnService.RegisterSpawnLocationWithService(_farSpawnLocation);

            Assert.AreSame(_farSpawnLocation.GetSpawnLocationResult, _spawnService.GetNearestSpawnLocation(_farSpawnLocation.GetSpawnLocationResult.position));
        }

        [Test]
        public void GetSpawnLocation_NoLocations_ErrorThrownAndReturnsNull()
        {
            LogAssert.Expect(LogType.Error, "Tried to find a SpawnLocation when none were registered!");

            Assert.IsNull(_spawnService.GetNearestSpawnLocation(Vector3.zero));
        }

        [Test]
        public void RegisterLocation_AddedToLocations()
        {
            _spawnService.RegisterSpawnLocationWithService(_spawnLocation);

            Assert.AreSame(_spawnLocation.GetSpawnLocationResult, _spawnService.GetNearestSpawnLocation(_farSpawnLocation.GetSpawnLocationResult.position));
        }

        [Test]
        public void RegisterLocation_AlreadyRegistered_ErrorThrown()
        {
            LogAssert.Expect(LogType.Error, "Tried to register a spawn location that was already registered!");

            _spawnService.RegisterSpawnLocationWithService(_spawnLocation);
            _spawnService.RegisterSpawnLocationWithService(_spawnLocation);

            _spawnService.UnregisterSpawnLocationWithService(_spawnLocation);
        }

        [Test]
        public void UnregisterLocation_RemovedFromLocations()
        {
            _spawnService.RegisterSpawnLocationWithService(_spawnLocation);
            _spawnService.RegisterSpawnLocationWithService(_farSpawnLocation);

            _spawnService.UnregisterSpawnLocationWithService(_spawnLocation);

            Assert.AreSame(_farSpawnLocation.GetSpawnLocationResult, _spawnService.GetNearestSpawnLocation(_spawnLocation.GetSpawnLocationResult.position));
        
            _spawnService.UnregisterSpawnLocationWithService(_farSpawnLocation);
        }

        [Test]
        public void UnregisterLocation_NotRegistered_ErrorThrown()
        {
            LogAssert.Expect(LogType.Error, "Tried to unregister a spawn location that wasn't registered!");

            _spawnService.UnregisterSpawnLocationWithService(_spawnLocation);
        }
    }
}

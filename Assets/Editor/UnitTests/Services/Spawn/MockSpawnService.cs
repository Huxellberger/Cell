// Copyright (C) Threetee Gang All Rights Reserved

using Assets.Scripts.Services.Spawn;
using UnityEngine;

namespace Assets.Editor.UnitTests.Services.Spawn
{
    public class MockSpawnService 
        : ISpawnServiceInterface
    {
        public ISpawnLocationInterface RegisteredSpawnLocation = null;
        public ISpawnLocationInterface UnregisteredSpawnLocation = null;
        public Transform GetNearestSpawnLocationResult = null;

        public void RegisterSpawnLocationWithService(ISpawnLocationInterface inLocation)
        {
            RegisteredSpawnLocation = inLocation;
        }

        public void UnregisterSpawnLocationWithService(ISpawnLocationInterface inLocation)
        {
            UnregisteredSpawnLocation = inLocation;
        }

        public Transform GetNearestSpawnLocation(Vector3 inCurrentLocation)
        {
            return GetNearestSpawnLocationResult;
        }
    }
}

// Copyright (C) Threetee Gang All Rights Reserved

using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Services.Spawn
{
    public class SpawnService 
        : ISpawnServiceInterface
    {
        private readonly IList<ISpawnLocationInterface> _spawnLocations;

        public SpawnService()
        {
            _spawnLocations = new List<ISpawnLocationInterface>();
        }

        public void RegisterSpawnLocationWithService(ISpawnLocationInterface inLocation)
        {
            if (!_spawnLocations.Contains(inLocation))
            {
                _spawnLocations.Add(inLocation);
            }
            else
            {
                Debug.LogError("Tried to register a spawn location that was already registered!");
            }
        }

        public void UnregisterSpawnLocationWithService(ISpawnLocationInterface inLocation)
        {
            if (_spawnLocations.Contains(inLocation))
            {
                _spawnLocations.Remove(inLocation);
            }
            else
            {
                Debug.LogError("Tried to unregister a spawn location that wasn't registered!");
            }
        }

        public Transform GetNearestSpawnLocation(Vector3 inCurrentLocation)
        {
            if (_spawnLocations.Count == 0)
            {
                Debug.LogError("Tried to find a SpawnLocation when none were registered!");
            }

            Transform closestTransform = null;
            var closestDistance = float.PositiveInfinity;

            foreach (var spawnLocation in _spawnLocations)
            {
                var currentSpawnLocation = spawnLocation.GetSpawnLocation();
                var currentDistance = Vector3.Distance(currentSpawnLocation.position, inCurrentLocation);
                if (currentDistance < closestDistance)
                {
                    closestTransform = currentSpawnLocation;
                    closestDistance = currentDistance;
                }
            }

            return closestTransform;
        }
    }
}

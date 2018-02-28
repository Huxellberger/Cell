// Copyright (C) Threetee Gang All Rights Reserved

using UnityEngine;

namespace Assets.Scripts.Services.Spawn
{
    public interface ISpawnServiceInterface
    {
        void RegisterSpawnLocationWithService(ISpawnLocationInterface inLocation);
        void UnregisterSpawnLocationWithService(ISpawnLocationInterface inLocation);

        Transform GetNearestSpawnLocation(Vector3 inCurrentLocation);
    }
}

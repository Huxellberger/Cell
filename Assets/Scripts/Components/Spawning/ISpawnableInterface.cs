// Copyright (C) Threetee Gang All Rights Reserved

namespace Assets.Scripts.Components.Spawning
{
    public interface ISpawnableInterface
    {
        void SetSpawner(ISpawnerInterface inSpawner);
        void OnSpawned();
    }
}

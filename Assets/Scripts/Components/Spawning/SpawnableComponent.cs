// Copyright (C) Threetee Gang All Rights Reserved

using UnityEngine;

namespace Assets.Scripts.Components.Spawning
{
    public abstract class SpawnableComponent 
        : MonoBehaviour
        , ISpawnableInterface
    {
        protected ISpawnerInterface SpawnerInterface;

        // ISpawnableInterface
        public void SetSpawner(ISpawnerInterface inSpawner)
        {
            SpawnerInterface = inSpawner;
        }

        public void OnSpawned()
        {
            OnSpawnedImpl();
        }
        // ~ISpawnableInterface

        protected void Despawn()
        {
            if (SpawnerInterface != null)
            {
                SpawnerInterface.RequestRespawn(gameObject);
            }
        }

        protected abstract void OnSpawnedImpl();
    }
}

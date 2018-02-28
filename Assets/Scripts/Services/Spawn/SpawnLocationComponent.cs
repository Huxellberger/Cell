// Copyright (C) Threetee Gang All Rights Reserved

using UnityEngine;

namespace Assets.Scripts.Services.Spawn
{
    public class SpawnLocationComponent 
        : MonoBehaviour
        , ISpawnLocationInterface
    {
        private ISpawnServiceInterface _spawnServiceInterface;

        protected void Start()
        {
            _spawnServiceInterface = GameServiceProvider.CurrentInstance.GetService<ISpawnServiceInterface>();

            _spawnServiceInterface.RegisterSpawnLocationWithService(this);
        }

        protected void OnDestroy()
        {
            _spawnServiceInterface.UnregisterSpawnLocationWithService(this);
        }

        public Transform GetSpawnLocation()
        {
            return gameObject.transform;
        }
    }
}

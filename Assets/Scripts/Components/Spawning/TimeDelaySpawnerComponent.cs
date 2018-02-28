// Copyright (C) Threetee Gang All Rights Reserved

using UnityEngine;

namespace Assets.Scripts.Components.Spawning
{
    public class TimeDelaySpawnerComponent 
        : SpawnerComponent
    {
        public float SpawnDelta = 3.0f;

        private float _currentDelta = 0.0f;

        protected override void OnSpawn(GameObject inSpawnedGameObject)
        {
        }

        protected void Update()
        {
            var deltaTime = GetDeltaTime();
            _currentDelta += deltaTime;

            if (_currentDelta > SpawnDelta)
            {
                Spawn();

                _currentDelta = 0.0f;
            }
        }

        protected virtual float GetDeltaTime()
        {
            return Time.deltaTime;
        }
    }
}

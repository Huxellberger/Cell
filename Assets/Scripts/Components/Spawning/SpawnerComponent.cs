// Copyright (C) Threetee Gang All Rights Reserved

using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.UnityLayer.GameObjects;
using UnityEngine;

namespace Assets.Scripts.Components.Spawning
{
    public class SpawnerComponent 
        : MonoBehaviour
        , ISpawnerInterface
    {
        public int MaxSpawnCount = 1;
        public GameObject SpawnablePrefab;
        public GameObject SpawnPoint;

        protected List<GameObject> CurrentSpawnedObjects { get; set; }

        private List<GameObject> _waitingToSpawnObjects;

        protected void Start()
        {
            _waitingToSpawnObjects = new List<GameObject>(MaxSpawnCount);
            CurrentSpawnedObjects = new List<GameObject>(MaxSpawnCount);

            InitialiseSpawnables();
        }

        protected void OnDestroy()
        {
            foreach (var waitingToSpawnObject in _waitingToSpawnObjects)
            {
                DestructionFunctions.DestroyGameObject(waitingToSpawnObject);
            }

            _waitingToSpawnObjects.Clear();
        }

        private void InitialiseSpawnables()
        {
            for (var currentSpawnCount = 0; currentSpawnCount < MaxSpawnCount; currentSpawnCount++)
            {
                var currentSpawnable = Instantiate(SpawnablePrefab, SpawnPoint.transform.position,
                    SpawnPoint.transform.rotation);
                currentSpawnable.SetActive(false);

                var spawnableInterface = currentSpawnable.GetComponent<ISpawnableInterface>();
                if (spawnableInterface != null)
                {
                    spawnableInterface.SetSpawner(this);
                }

                _waitingToSpawnObjects.Add(currentSpawnable);
            }
        }

        // ISpawnerInterface
        public void Spawn()
        {
            if (_waitingToSpawnObjects.Count > 0)
            {
                var spawned = _waitingToSpawnObjects.Last();
                _waitingToSpawnObjects.Remove(spawned);
                CurrentSpawnedObjects.Add(spawned);

                spawned.SetActive(true);

                OnSpawn(spawned);

                var spawnableInterface = spawned.GetComponent<ISpawnableInterface>();
                if (spawnableInterface != null)
                {
                    spawnableInterface.OnSpawned();
                }
            }
        }

        protected virtual void OnSpawn(GameObject inSpawnedGameObject) { }

        
        public void RequestRespawn(GameObject inGameObject)
        {
            if (CurrentSpawnedObjects.Contains(inGameObject))
            {
                CurrentSpawnedObjects.Remove(inGameObject);

                inGameObject.SetActive(false);
                inGameObject.transform.position = SpawnPoint.transform.position;
                inGameObject.transform.rotation = SpawnPoint.transform.rotation;
                _waitingToSpawnObjects.Add(inGameObject);
            }
        }
        // ~ISpawnerInterface
    }
}

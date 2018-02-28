// Copyright (C) Threetee Gang All Rights Reserved

#if UNITY_EDITOR

using System.Collections.Generic;
using Assets.Scripts.Components.Spawning;
using UnityEngine;

namespace Assets.Scripts.Test.Components.Spawning
{
    public class TestSpawnerComponent 
        : SpawnerComponent
    {
        public GameObject OnSpawnGameObject { get; private set; }

        public void TestStart()
        {
            Start();
        }

        public void TestDestroy()
        {
            OnDestroy();
        }

        public List<GameObject> GetSpawnedGameObjects()
        {
            return CurrentSpawnedObjects;
        }

        protected override void OnSpawn(GameObject inSpawnedGameObject)
        {
            OnSpawnGameObject = inSpawnedGameObject;
        }
    }
}

#endif // UNITY_EDITOR

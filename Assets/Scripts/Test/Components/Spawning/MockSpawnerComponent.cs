// Copyright (C) Threetee Gang All Rights Reserved

#if UNITY_EDITOR

using Assets.Scripts.Components.Spawning;
using UnityEngine;

namespace Assets.Scripts.Test.Components.Spawning
{
    public class MockSpawnerComponent 
        : MonoBehaviour 
        , ISpawnerInterface
    {
        public bool SpawnCalled = false;
        public GameObject RequestRespawnGameObject { get; private set; }

        public void Spawn()
        {
            SpawnCalled = true;
        }

        public void RequestRespawn(GameObject inGameObject)
        {
            RequestRespawnGameObject = inGameObject;
        }
    }
}

#endif // UNITY_EDITOR

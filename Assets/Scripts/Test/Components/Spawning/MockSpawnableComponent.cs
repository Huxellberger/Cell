// Copyright (C) Threetee Gang All Rights Reserved

#if UNITY_EDITOR

using Assets.Scripts.Components.Spawning;
using UnityEngine;

namespace Assets.Scripts.Test.Components.Spawning
{
    public class MockSpawnableComponent 
        : MonoBehaviour 
        , ISpawnableInterface
    {
        public ISpawnerInterface SetSpawnerResult { get; private set; }
        public bool OnSpawnedCalled = false;

        public void SetSpawner(ISpawnerInterface inSpawner)
        {
            SetSpawnerResult = inSpawner;
        }

        public void OnSpawned()
        {
            OnSpawnedCalled = true;
        }
    }
}

#endif // UNITY_EDITOR

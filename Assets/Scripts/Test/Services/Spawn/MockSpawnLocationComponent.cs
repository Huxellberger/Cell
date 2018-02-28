// Copyright (C) Threetee Gang All Rights Reserved

#if UNITY_EDITOR

using Assets.Scripts.Services.Spawn;
using UnityEngine;

namespace Assets.Scripts.Test.Services.Spawn
{
    public class MockSpawnLocationComponent 
        : MonoBehaviour
        , ISpawnLocationInterface
    {
        public Transform GetSpawnLocationResult { get; set; }

        public Transform GetSpawnLocation()
        {
            return GetSpawnLocationResult;
        }
    }
}

#endif // UNITY_EDITOR 

// Copyright (C) Threetee Gang All Rights Reserved

#if UNITY_EDITOR

using System.Collections.Generic;
using Assets.Scripts.Components.Spawning;
using UnityEngine;

namespace Assets.Scripts.Test.Components.Spawning
{
    public class TestTimeDelaySpawnerComponent 
        : TimeDelaySpawnerComponent
    {
        private float _getDeltaTimeResult { get; set; }

        public void TestStart()
        {
            Start();
        }

        public void TestUpdate(float inDeltaTime)
        {
            _getDeltaTimeResult = inDeltaTime;
            Update();
        }

        public List<GameObject> GetSpawnedObjects()
        {
            return CurrentSpawnedObjects;
        }

        protected override float GetDeltaTime()
        {
            return _getDeltaTimeResult;
        }
    }
}

#endif // UNITY_EDITOR

// Copyright (C) Threetee Gang All Rights Reserved

#if UNITY_EDITOR

using Assets.Scripts.AI.Vision;
using UnityEngine;

namespace Assets.Scripts.Test.AI.Vision
{
    public class TestVisionComponent 
        : VisionComponent
    {
        public bool IsSuspiciousResult = false;
        private float _deltaTime;

        public void TestUpdate(float deltaTime)
        {
            _deltaTime = deltaTime;
            Update();
        }

        public void TestCollide(GameObject inObject)
        {
            OnGameObjectCollides(inObject);
        }

        public void TestStopColliding(GameObject inObject)
        {
            OnGameObjectStopsColliding(inObject);
        }

        protected override float GetDeltaTime()
        {
            return _deltaTime;
        }

        protected override bool IsSuspicious(GameObject inDetectedObject)
        {
            return IsSuspiciousResult;
        }
    }
}

#endif // UNITY_EDITOR

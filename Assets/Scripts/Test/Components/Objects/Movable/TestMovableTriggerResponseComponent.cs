// Copyright (C) Threetee Gang All Rights Reserved

#if UNITY_EDITOR

using Assets.Scripts.Components.Objects.Movable;

namespace Assets.Scripts.Test.Components.Objects.Movable
{
    public class TestMovableTriggerResponseComponent 
        : MovableTriggerResponseComponent
    {
        private float _getDeltaTimeResult = 0.0f;

        public void TestStart()
        {
            Start();
        }

        public void TestDestroy()
        {
            OnDestroy();
        }

        public void TestUpdate (float inDeltaTime)
        {
            _getDeltaTimeResult = inDeltaTime;
            FixedUpdate();
        }

        protected override float GetDeltaTime()
        {
            return _getDeltaTimeResult;
        }
    }
}

#endif // UNITY_EDITOR

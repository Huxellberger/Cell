// Copyright (C) Threetee Gang All Rights Reserved

#if UNITY_EDITOR

using Assets.Scripts.Components.Character;

namespace Assets.Scripts.Test.Components.Character
{
    public class TestPlayerCameraComponent 
        : PlayerCameraComponent
    {
        private float FixedDelta { get; set; }

        public void TestStart()
        {
            Start();
        }

        public void TestUpdate(float inDeltaTime)
        {
            FixedDelta = inDeltaTime;
            FixedUpdate();
        }

        protected override float GetDeltaTime()
        {
            return FixedDelta;
        }
    }
}

#endif // UNITY_EDITOR

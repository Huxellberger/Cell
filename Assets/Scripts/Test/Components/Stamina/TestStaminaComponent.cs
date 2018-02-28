// Copyright (C) Threetee Gang All Rights Reserved

#if UNITY_EDITOR

using Assets.Scripts.Components.Stamina;

namespace Assets.Scripts.Test.Components.Stamina
{
    public class TestStaminaComponent 
        : StaminaComponent
    {
        private float TimeDelta { get; set; }

        public void TestStart()
        {
            TimeDelta = 0.0f;
            Start();
        }

        public void TestUpdate(float inDeltaTime)
        {
            TimeDelta = inDeltaTime;
            Update();
        }

        protected override float GetDeltaTime()
        {
            return TimeDelta;
        }
    }
}

#endif // UNITY_EDITOR

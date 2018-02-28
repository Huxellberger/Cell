// Copyright (C) Threetee Gang All Rights Reserved

#if UNITY_EDITOR

using Assets.Scripts.Components.Movement;

namespace Assets.Scripts.Test.Components.Movement
{
    public class TestMovementComponent 
        : MovementComponent
    {
        private float TimeDelta { get; set; }

        public void  TestAwake ()
        {
            TimeDelta = 0.0f;

            Awake();
        }
	
        public void TestUpdate (float inTimeDelta)
        {
            TimeDelta = inTimeDelta;
            FixedUpdate();
        }

        protected override float GetDeltaTime()
        {
            return TimeDelta;
        }
    }
}

#endif // UNITY_EDITOR

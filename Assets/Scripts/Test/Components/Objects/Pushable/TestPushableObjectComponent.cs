// Copyright (C) Threetee Gang All Rights Reserved

#if UNITY_EDITOR

using Assets.Scripts.Components.Objects.Pushable;

namespace Assets.Scripts.Test.Components.Objects.Pushable
{
    public class TestPushableObjectComponent 
        : PushableObjectComponent
    {
        private float _getDeltaTimeResult = 0.0f;

        public void TestStart()
        {
		    Start();
        }
	
        public void TestUpdate(float inDeltaTime)
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

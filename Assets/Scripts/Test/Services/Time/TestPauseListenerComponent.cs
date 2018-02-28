// Copyright (C) Threetee Gang All Rights Reserved

#if UNITY_EDITOR

using Assets.Scripts.Services.Time;

namespace Assets.Scripts.Test.Services.Time
{
    public class TestPauseListenerComponent 
        : PauseListenerComponent
    {
        public void TestStart()
        {
		    Start();
        }
	
        public void TestDestroy()
        {
		    OnDestroy();
        }
    }
}

#endif // UNITY_EDITOR

// Copyright (C) Threetee Gang All Rights Reserved

#if UNITY_EDITOR

using Assets.Scripts.Components.Trigger;

namespace Assets.Scripts.Test.Components.Trigger
{
    public class TestEventOfInterestTriggerResponseComponent 
        : EventOfInterestTriggerResponseComponent 
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

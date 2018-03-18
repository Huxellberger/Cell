// Copyright (C) Threetee Gang All Rights Reserved

#if UNITY_EDITOR

using Assets.Scripts.AI.Chatter;

namespace Assets.Scripts.Test.AI.Chatter
{
    public class TestChatterComponent 
        : ChatterComponent
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

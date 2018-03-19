// Copyright (C) Threetee Gang All Rights Reserved

#if UNITY_EDITOR

using Assets.Scripts.AI.Companion;

namespace Assets.Scripts.Test.AI.Companion
{
    public class TestCompanionSetComponent 
        : CompanionSetComponent
    {
        public void TestStart() 
        {
            Start();
        }
	
        public void TestUpdate() 
        {
            Update();
        }
    }
}

#endif // UNITY_EDITOR

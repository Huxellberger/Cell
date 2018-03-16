// Copyright (C) Threetee Gang All Rights Reserved

#if UNITY_EDITOR

using Assets.Scripts.Components.Equipment.Holdables;

namespace Assets.Scripts.Test.Components.Equipment.Holdables
{
    public class TestHeldItemComponent 
        : HeldItemComponent
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

// Copyright (C) Threetee Gang All Rights Reserved

#if UNITY_EDITOR

using Assets.Scripts.Services.Persistence;

namespace Assets.Scripts.Test.Services.Persistence
{
    public class TestPersistentEntityComponent 
        : PersistentEntityComponent
    {
        public void TestAwake() 
        {
            Awake();
        }
	
        public void TestDestroy() 
        {
            OnDestroy();
        }
    }
}

#endif // UNITY_EDITOR

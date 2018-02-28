// Copyright (C) Threetee Gang All Rights Reserved

#if UNITY_EDITOR

using Assets.Scripts.Services;

namespace Assets.Scripts.Test.Services
{
    public class TestGameServiceProvider 
        : GameServiceProvider
    {
        public void TestAwake()
        {
            Awake();
        }
    }
}

#endif // UNITY_EDITOR

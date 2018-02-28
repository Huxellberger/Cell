// Copyright (C) Threetee Gang All Rights Reserved

#if UNITY_EDITOR

using Assets.Scripts.Instance;

namespace Assets.Scripts.Test.Instance
{
    public class TestGameInstance 
        : GameInstance
    {
        public void TestAwake()
        {
            Awake();
        }
    }
}

#endif // UNITY_EDITOR

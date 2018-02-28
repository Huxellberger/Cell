// Copyright (C) Threetee Gang All Rights Reserved

#if UNITY_EDITOR

using Assets.Scripts.Messaging;

namespace Assets.Scripts.Test.Messaging
{
    public class TestUnityMessageEventDispatcherComponent
        : UnityMessageEventDispatcherComponent
    {
        public void TestAwake()
        {
            Awake();
        }
    }
}

#endif

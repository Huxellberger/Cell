// Copyright (C) Threetee Gang All Rights Reserved

#if UNITY_EDITOR

using Assets.Scripts.Messaging;

namespace Assets.Editor.UnitTests.Messaging
{
    public class TestUnityMessageEventDispatcherInterface
        : IUnityMessageEventInterface
    {
        public TestUnityMessageEventDispatcherInterface()
        {
            Dispatcher = new UnityMessageEventDispatcher();
        }

        // IUnityMessageEventInterface
        public UnityMessageEventDispatcher GetUnityMessageEventDispatcher()
        {
            return Dispatcher;
        }
        // ~IUnityMessageEventInterface

        public UnityMessageEventDispatcher Dispatcher { get; set; }
    }
}

#endif

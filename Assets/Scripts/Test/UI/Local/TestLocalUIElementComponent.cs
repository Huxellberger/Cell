// Copyright (C) Threetee Gang All Rights Reserved

#if UNITY_EDITOR

using Assets.Scripts.Messaging;
using Assets.Scripts.UI.Local;

namespace Assets.Scripts.Test.UI.Local
{
    public class TestLocalUIElementComponent 
        : LocalUIElementComponent
    {
        public bool OnElementInitialisedCalled = false;
        public bool OnElementUninitialisedCalled = false;

        public UnityMessageEventDispatcher GetDispatcher()
        {
            return Dispatcher;
        }

        protected override void OnElementInitialisedImpl()
        {
            OnElementInitialisedCalled = true;
        }

        protected override void OnElementUninitialisedImpl()
        {
            OnElementUninitialisedCalled = true;
        }
    }
}

#endif // UNITY_EDITOR

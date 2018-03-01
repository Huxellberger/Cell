// Copyright (C) Threetee Gang All Rights Reserved

using Assets.Scripts.Messaging;
using UnityEngine;

namespace Assets.Scripts.UI.Local
{
    public abstract class LocalUIElementComponent 
        : MonoBehaviour
    {
        protected UnityMessageEventDispatcher Dispatcher;

        public void OnElementInitialised(UnityMessageEventDispatcher inDispatcher)
        {
            Dispatcher = inDispatcher;

            OnElementInitialisedImpl();
        }

        public void OnElementUninitialised()
        {
            OnElementUninitialisedImpl();

            Dispatcher = null;
        }

        protected abstract void OnElementInitialisedImpl();
        protected abstract void OnElementUninitialisedImpl();
    }
}

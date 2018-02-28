// Copyright (C) Threetee Gang All Rights Reserved

using UnityEngine;

namespace Assets.Scripts.Messaging
{
    public class UnityMessageEventDispatcherComponent 
        : MonoBehaviour
        , IUnityMessageEventInterface
    {
        private readonly UnityMessageEventDispatcher _dispatcher = new UnityMessageEventDispatcher();

        protected void Awake ()
        {
            
        }

        // IUnityMessageEventInterface
        public UnityMessageEventDispatcher GetUnityMessageEventDispatcher()
        {
            return _dispatcher;
        }
        // ~IUnityMessageEventInterface
    }
}

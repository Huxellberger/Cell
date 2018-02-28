// Copyright (C) Threetee Gang All Rights Reserved

using System;

namespace Assets.Scripts.Components.UnityEvent
{
    public class UnityMessageHandleException 
        : Exception
    {
        public UnityMessageHandleException(string message)
            : base(message)
        {
        }
    }
}

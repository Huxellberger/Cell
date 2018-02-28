// Copyright (C) Threetee Gang All Rights Reserved

using UnityEngine.Events;

namespace Assets.Scripts.Messaging
{
    [System.Serializable]
    public abstract class UnityMessagePayload
    {
    }

    [System.Serializable]
    public class UnityMessageEvent<TMessageType>
        : UnityEvent<TMessageType>
        where TMessageType : UnityMessagePayload
    {     
    }
}

// Copyright (C) Threetee Gang All Rights Reserved

using Assets.Scripts.Messaging;
using UnityEngine;

namespace Assets.Scripts.Components.Trigger
{
    public class TriggerMessage
        : UnityMessagePayload
    {
        public readonly GameObject TriggeringObject;

        public TriggerMessage(GameObject inTriggeringObject)
        {
            TriggeringObject = inTriggeringObject;
        }
    }

    public class CancelTriggerMessage
        : UnityMessagePayload
    {
        public readonly GameObject TriggeringObject;

        public CancelTriggerMessage(GameObject inTriggeringObject)
        {
            TriggeringObject = inTriggeringObject;
        }
    }
}

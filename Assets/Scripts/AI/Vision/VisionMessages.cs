// Copyright (C) Threetee Gang All Rights Reserved

using Assets.Scripts.Messaging;
using UnityEngine;

namespace Assets.Scripts.AI.Vision
{
    public class SuspiciousObjectSightedMessage 
        : UnityMessagePayload
    {
        public readonly GameObject SuspiciousGameObject;

        public SuspiciousObjectSightedMessage(GameObject inSuspiciousGameObject)
        {
            SuspiciousGameObject = inSuspiciousGameObject;
        }
    }

    public class SuspiciousObjectDetectedMessage
        : UnityMessagePayload
    {
        public readonly GameObject SuspiciousGameObject;

        public SuspiciousObjectDetectedMessage(GameObject inSuspiciousGameObject)
        {
            SuspiciousGameObject = inSuspiciousGameObject;
        }
    }
}

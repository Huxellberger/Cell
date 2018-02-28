// Copyright (C) Threetee Gang All Rights Reserved

using Assets.Scripts.Messaging;
using UnityEngine;

namespace Assets.Scripts.Components.ActionStateMachine.States.Dead
{
    public class EnterDeadActionStateMessage 
        : UnityMessagePayload
    {
    }

    public class LeftDeadActionStateMessage
        : UnityMessagePayload
    {
    }

    public class RequestRespawnMessage
        : UnityMessagePayload
    {
        public readonly GameObject RequestingPlayer;

        public RequestRespawnMessage(GameObject inRequestingPlayer)
            : base()
        {
            RequestingPlayer = inRequestingPlayer;
        }
    }
}

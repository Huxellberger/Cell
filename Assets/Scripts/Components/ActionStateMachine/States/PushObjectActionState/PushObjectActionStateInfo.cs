// Copyright (C) Threetee Gang All Rights Reserved

using UnityEngine;

namespace Assets.Scripts.Components.ActionStateMachine.States.PushObjectActionState
{
    public class PushObjectActionStateInfo 
        : ActionStateInfo
    {
        public readonly GameObject ObjectToPush;
        public readonly GameObject PushPointSocket;

        public PushObjectActionStateInfo()
            : this (null, null, null)
        {
        }

        public PushObjectActionStateInfo(GameObject inOwner, GameObject inObjectToPush, GameObject inPushPointSocket)
            : base(inOwner)
        {
            ObjectToPush = inObjectToPush;
            PushPointSocket = inPushPointSocket;
        }
    }
}

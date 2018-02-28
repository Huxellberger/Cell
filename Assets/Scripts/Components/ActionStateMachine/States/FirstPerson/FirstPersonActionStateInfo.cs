// Copyright (C) Threetee Gang All Rights Reserved

using UnityEngine;

namespace Assets.Scripts.Components.ActionStateMachine.States.FirstPerson
{
    public class FirstPersonActionStateInfo 
        : ActionStateInfo
    {
        public GameObject CameraObject;

        public FirstPersonActionStateInfo()
            : this(null, null)
        {
        }

        public FirstPersonActionStateInfo(GameObject inOwner, GameObject inCameraObject)
            : base(inOwner)
        {
            CameraObject = inCameraObject;
        }

    }
}

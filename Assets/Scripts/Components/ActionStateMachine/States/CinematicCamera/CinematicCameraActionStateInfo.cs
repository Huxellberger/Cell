// Copyright (C) Threetee Gang All Rights Reserved

using UnityEngine;

namespace Assets.Scripts.Components.ActionStateMachine.States.CinematicCamera
{
    public class CinematicCameraActionStateInfo 
        : ActionStateInfo
    {
        public readonly Camera SwappedCamera;
        public float CameraTime;

        public CinematicCameraActionStateInfo()
            : this (null, null, 0.0f)
        {
        }

        public CinematicCameraActionStateInfo(GameObject inOwner, Camera inSwappedCamera, float inCameraTime)
            : base(inOwner)
        {
            SwappedCamera = inSwappedCamera;
            CameraTime = inCameraTime;
        }
    }
}

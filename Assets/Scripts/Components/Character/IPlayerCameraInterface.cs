// Copyright (C) Threetee Gang All Rights Reserved

using UnityEngine;

namespace Assets.Scripts.Components.Character
{
    public interface IPlayerCameraInterface
    {
        void SetRelativeCameraPosition(Vector3 inStartLocation, Vector3 inStartRotation);
        void RotateHorizontal(float inRotation);
        void RotateVertical(float inRotation);
        void Zoom(float inZoom);
        void ResetZoom();
        void SetCameraMode(EPlayerCameraMode inMode);
    }
}

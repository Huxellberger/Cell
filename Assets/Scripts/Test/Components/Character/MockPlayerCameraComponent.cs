// Copyright (C) Threetee Gang All Rights Reserved

#if UNITY_EDITOR

using Assets.Scripts.Components.Character;
using UnityEngine;

namespace Assets.Scripts.Test.Components.Character
{
    public class MockPlayerCameraComponent 
        : MonoBehaviour
        , IPlayerCameraInterface
    {
        public Vector3 ? SetLocation { get; private set; }
        public Vector3 ? SetRotation { get; private set; }

        public float ? RotateHorizontalResult { get; private set; }
        public float ? RotateVerticalResult { get; private set; }
        public float ? ZoomResult { get; private set; }
        public bool ResetZoomCalled = false;
        public EPlayerCameraMode ? SetCameraModeResult { get; private set; }

        public void SetRelativeCameraPosition(Vector3 inStartLocation, Vector3 inStartRotation)
        {
            SetLocation = inStartLocation;
            SetRotation = inStartRotation;
        }

        public void RotateHorizontal(float inRotation)
        {
            RotateHorizontalResult = inRotation;
        }

        public void RotateVertical(float inRotation)
        {
            RotateVerticalResult = inRotation;
        }

        public void Zoom(float inZoom)
        {
            ZoomResult = inZoom;
        }

        public void ResetZoom()
        {
            ResetZoomCalled = true;
        }

        public void SetCameraMode(EPlayerCameraMode inMode)
        {
            SetCameraModeResult = inMode;
        }
    }
}

#endif // UNITY_EDITOR

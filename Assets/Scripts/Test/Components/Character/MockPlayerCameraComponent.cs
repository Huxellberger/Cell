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
        public float ? SetOrthographicSize { get; private set; }

        public float ? RotateHorizontalResult { get; private set; }
        public float ? ZoomResult { get; private set; }
        public bool ResetZoomCalled = false;

        public void SetRelativeCameraPosition(Vector3 inStartLocation, Vector3 inStartRotation, float inOrthographicSize)
        {
            SetLocation = inStartLocation;
            SetRotation = inStartRotation;
            SetOrthographicSize = inOrthographicSize;
        }

        public void RotateHorizontal(float inRotation)
        {
            RotateHorizontalResult = inRotation;
        }

        public void Zoom(float inZoom)
        {
            ZoomResult = inZoom;
        }

        public void ResetZoom()
        {
            ResetZoomCalled = true;
        }
    }
}

#endif // UNITY_EDITOR

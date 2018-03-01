// Copyright (C) Threetee Gang All Rights Reserved

using Assets.Scripts.Components.Controller;
using UnityEngine;

namespace Assets.Scripts.Components.Character
{
    public static class PlayerCameraConstants
    {
        public const float MaxZoomModifier = 0.2f;
        public const float MinZoomModifier = 2.0f;
    }

    // We should consider the camera being a singleton in the scene
    [RequireComponent(typeof(ControllerComponent), typeof(Camera))]
    public class PlayerCameraComponent 
        : MonoBehaviour
        , IPlayerCameraInterface
    {
        public float CameraHorizontalSpeed = 200.0f;
        public float ZoomSpeed = 2.0f;

        private ControllerComponent _controller;

        private Camera _camera;

        private Camera CurrentCamera
        {
            get
            {
                if (_camera == null)
                {
                    _camera = gameObject.GetComponent<Camera>();
                }

                return _camera;
            }
            set { _camera = value; }
        }

        private float InitialOrthographicSize;

        private float MaxZoom;
        private float MinZoom;

        private float HorizontalModifier { get; set; }
        private float VerticalModifier { get; set; }
        private float ZoomModifier { get; set; }
        private bool ResetZoomFlag { get; set; }

        protected void Start ()
        {
            _controller = gameObject.GetComponent<ControllerComponent>();
            CurrentCamera = gameObject.GetComponent<Camera>();

            gameObject.transform.parent = _controller.PawnInstance.transform;
        }

        protected void FixedUpdate()
        {
            var deltaTime = GetDeltaTime();

            transform.parent.Rotate(new Vector3(0.0f, 0.0f, HorizontalModifier * deltaTime * CameraHorizontalSpeed));

            if (ResetZoomFlag)
            {
                CurrentCamera.orthographicSize = InitialOrthographicSize;
            }
            else
            {
                CurrentCamera.orthographicSize =
                    Mathf.Clamp(_camera.orthographicSize - (ZoomModifier * ZoomSpeed * deltaTime), MaxZoom, MinZoom);
            }

            HorizontalModifier = 0.0f;
            VerticalModifier = 0.0f;
            ZoomModifier = 0.0f;
            ResetZoomFlag = false;
        }

        protected virtual float GetDeltaTime()
        {
            return Time.fixedDeltaTime;
        }

        // IPlayerCameraInterface
        public void SetRelativeCameraPosition(Vector3 inStartLocation, Vector3 inStartRotation, float inOrthographicSize)
        {
            InitialOrthographicSize = inOrthographicSize;
            MaxZoom = InitialOrthographicSize * PlayerCameraConstants.MaxZoomModifier;
            MinZoom = InitialOrthographicSize * PlayerCameraConstants.MinZoomModifier;
            CurrentCamera.orthographicSize = inOrthographicSize;

            gameObject.transform.localPosition = inStartLocation;
            gameObject.transform.eulerAngles = inStartRotation;
        }

        public void RotateHorizontal(float inRotation)
        {
            HorizontalModifier = Mathf.Clamp(inRotation, -1.0f, 1.0f);
        }

        public void Zoom(float inZoom)
        {
            ZoomModifier = Mathf.Clamp(inZoom, -1.0f, 1.0f);
        }

        public void ResetZoom()
        {
            ResetZoomFlag = true;
        }
        // ~IPlayerCameraInterface
    }
}

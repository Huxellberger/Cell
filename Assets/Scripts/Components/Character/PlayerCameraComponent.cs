// Copyright (C) Threetee Gang All Rights Reserved

using Assets.Scripts.Components.ActionStateMachine;
using Assets.Scripts.Components.ActionStateMachine.States.FirstPerson;
using Assets.Scripts.Components.Controller;
using UnityEngine;

namespace Assets.Scripts.Components.Character
{
    public static class PlayerCameraConstants
    {
        public const float MaxZoomModifier = 0.2f;
        public const float MinZoomModifier = 2.0f;
        public const float MaxXAxisAngle = 40.0f;
        public const float MinXAxisAngle = 5.0f;
    }

    // We should consider the camera being a singleton in the scene
    [RequireComponent(typeof(ControllerComponent), typeof(Camera))]
    public class PlayerCameraComponent 
        : MonoBehaviour
        , IPlayerCameraInterface
    {
        public Vector3 InitialLocation;
        public Vector3 InitialRotation;

        public float CameraHorizontalSpeed = 200.0f;
        public float CameraVerticalSpeed = 20.0f;
        public float ZoomSpeed = 2.0f;

        private ControllerComponent _controller;

        private Vector3 MaxZoom;
        private Vector3 MinZoom;

        private float HorizontalModifier { get; set; }
        private float VerticalModifier { get; set; }
        private float ZoomModifier { get; set; }
        private bool ResetZoomFlag { get; set; }

        protected void Start ()
        {
            _controller = gameObject.GetComponent<ControllerComponent>();

            gameObject.transform.parent = _controller.PawnInstance.transform;

            SetRelativeCameraPosition(InitialLocation, InitialRotation);
        }

        protected void FixedUpdate()
        {
            var deltaTime = GetDeltaTime();

            transform.parent.Rotate(new Vector3(0.0f, HorizontalModifier * deltaTime * CameraHorizontalSpeed));

            if (CanRotateVertically())
            {
                transform.RotateAround(gameObject.transform.parent.position, transform.right, VerticalModifier * deltaTime * CameraVerticalSpeed);
            }

            if (ResetZoomFlag)
            {
                transform.localPosition = InitialLocation;
                transform.eulerAngles = new Vector3(InitialRotation.x, transform.eulerAngles.y, transform.eulerAngles.z);;
            }
            else
            {
                transform.localPosition = new Vector3
                (
                    0.0f,
                    Mathf.Clamp(transform.localPosition.y - (InitialLocation.y * ZoomModifier * deltaTime * ZoomSpeed), MaxZoom.y, MinZoom.y),
                    Mathf.Clamp(transform.localPosition.z - (InitialLocation.z * ZoomModifier * deltaTime * ZoomSpeed), MinZoom.z, MaxZoom.z)
                );
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

        private bool CanRotateVertically()
        {
            if (transform.eulerAngles.x > PlayerCameraConstants.MaxXAxisAngle)
            {
                return VerticalModifier < 0f;
            }

            if (transform.eulerAngles.x < PlayerCameraConstants.MinXAxisAngle)
            {
                return VerticalModifier > 0f;
            }

            return true;
        }

        // IPlayerCameraInterface
        public void SetRelativeCameraPosition(Vector3 inStartLocation, Vector3 inStartRotation)
        {
            MaxZoom = inStartLocation * PlayerCameraConstants.MaxZoomModifier;
            MinZoom = inStartLocation * PlayerCameraConstants.MinZoomModifier;

            InitialLocation = inStartLocation;
            InitialRotation = inStartRotation;

            gameObject.transform.localPosition = InitialLocation;
            gameObject.transform.eulerAngles = InitialRotation;
        }

        public void RotateHorizontal(float inRotation)
        {
            HorizontalModifier = Mathf.Clamp(inRotation, -1.0f, 1.0f);
        }

        public void RotateVertical(float inRotation)
        {
            VerticalModifier = Mathf.Clamp(inRotation, -1.0f, 1.0f);
        }

        public void Zoom(float inZoom)
        {
            ZoomModifier = Mathf.Clamp(inZoom, -1.0f, 1.0f);
        }

        public void ResetZoom()
        {
            ResetZoomFlag = true;
        }

        public void SetCameraMode(EPlayerCameraMode inMode)
        {
            var actionStateMachine = _controller.PawnInstance.GetComponent<IActionStateMachineInterface>();
            if (actionStateMachine != null)
            {
                switch (inMode)
                {
                    case EPlayerCameraMode.FirstPerson:
                        actionStateMachine.RequestActionState
                        (
                            EActionStateMachineTrack.Locomotion, 
                            EActionStateId.FirstPerson, 
                            new FirstPersonActionStateInfo(_controller.PawnInstance, gameObject)
                        );
                        break;
                    case EPlayerCameraMode.ThirdPerson:
                        actionStateMachine.RequestActionState
                        (
                            EActionStateMachineTrack.Locomotion, 
                            EActionStateId.Locomotion, 
                            new ActionStateInfo(_controller.PawnInstance)
                        );
                        break;
                    default:
                        break;
                }
            }
        }
        // ~IPlayerCameraInterface
    }
}

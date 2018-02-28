// Copyright (C) Threetee Gang All Rights Reserved

using Assets.Scripts.Components.ActionStateMachine.States.Locomotion;
using Assets.Scripts.Components.Character;
using Assets.Scripts.Components.Interaction;
using Assets.Scripts.Components.Movement;
using Assets.Scripts.Input;
using UnityEngine;

namespace Assets.Scripts.Components.ActionStateMachine.States.SurfaceSticking
{
    public class SurfaceStickingActionState 
        : ActionState
    {
        private readonly SurfaceStickingActionStateInfo _surfaceInfo;
        private readonly SurfaceStickingActionStateParams _params;

        private SurfaceStickingInputHandler _surfaceInputHandler;
        private InteractionInputHandler _interactionInputHandler;

        private Vector3 _initialOwnerRotation;

        private Vector3 _initialCameraLocalPosition;
        private Vector3 _initialCameraRotation;

        public SurfaceStickingActionState(SurfaceStickingActionStateInfo inInfo, SurfaceStickingActionStateParams inParams) 
            : base(EActionStateId.SurfaceSticking, inInfo)
        {
            _surfaceInfo = inInfo;
            _params = inParams;
        }

        protected override void OnStart()
        {
            _initialOwnerRotation = Info.Owner.transform.eulerAngles;
            var initialPosition = Info.Owner.transform.position;
            Info.Owner.transform.parent = _surfaceInfo.Surface.transform;
            Info.Owner.transform.position = initialPosition;
            Info.Owner.transform.eulerAngles = _surfaceInfo.Surface.transform.eulerAngles;

            var rigidbody = Info.Owner.GetComponent<Rigidbody>();
            if (rigidbody != null)
            {
                rigidbody.useGravity = false;
            }

            SetupCamera();

            RegisterInputHandlers();
        }

        private void SetupCamera()
        {
            var characterComponent = Info.Owner.GetComponent<CharacterComponent>();
            if (characterComponent != null && characterComponent.ActiveController != null)
            {
                _initialCameraLocalPosition = characterComponent.ActiveController.gameObject.transform.localPosition;
                _initialCameraRotation = characterComponent.ActiveController.gameObject.transform.eulerAngles;

                characterComponent.ActiveController.gameObject.transform.localPosition = Info.Owner.transform.up * _params.CameraLocalPosition;
                characterComponent.ActiveController.gameObject.transform.eulerAngles = _params.CameraRotation;
            }
        }

        private void RegisterInputHandlers()
        {
            var inputBinder = Info.Owner.GetComponent<IInputBinderInterface>();
            if (inputBinder != null)
            {
                _surfaceInputHandler = new SurfaceStickingInputHandler(Info.Owner.GetComponent<IMovementInterface>());
                _interactionInputHandler = new InteractionInputHandler(Info.Owner.GetComponent<IInteractionInterface>());

                inputBinder.RegisterInputHandler(_surfaceInputHandler);
                inputBinder.RegisterInputHandler(_interactionInputHandler);
            }
        }

        protected override void OnUpdate(float deltaTime)
        {
        }

        protected override void OnEnd()
        {
            UnregisterInputHandlers();

            ResetCamera();

            var rigidbody = Info.Owner.GetComponent<Rigidbody>();
            if (rigidbody != null)
            {
                rigidbody.useGravity = true;
            }

            Info.Owner.transform.eulerAngles = _initialOwnerRotation;
            Info.Owner.transform.parent = null;
        }

        private void ResetCamera()
        {
            var characterComponent = Info.Owner.GetComponent<CharacterComponent>();
            if (characterComponent != null && characterComponent.ActiveController != null)
            {
                characterComponent.ActiveController.gameObject.transform.localPosition = _initialCameraLocalPosition;
                characterComponent.ActiveController.gameObject.transform.eulerAngles = _initialCameraRotation;
            }
        }

        private void UnregisterInputHandlers()
        {
            var inputBinder = Info.Owner.GetComponent<IInputBinderInterface>();
            if (inputBinder != null)
            {
                inputBinder.UnregisterInputHandler(_surfaceInputHandler);
                inputBinder.UnregisterInputHandler(_interactionInputHandler);
            }
        }
    }
}


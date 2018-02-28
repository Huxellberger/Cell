// Copyright (C) Threetee Gang All Rights Reserved

using Assets.Scripts.Components.Controller;
using Assets.Scripts.Components.Trigger;
using Assets.Scripts.Mode;
using UnityEngine;

namespace Assets.Scripts.Components.ActionStateMachine.States.CinematicCamera
{
    [RequireComponent(typeof(Camera))]
    public class CinematicCameraTriggerResponseComponent 
        : TriggerResponseComponent
    {
        public float CinematicTime = 3.0f;

        private Camera _cinematicCamera;

        private ControllerComponent _cachedController;

        protected override void Start()
        {
            base.Start();

            _cinematicCamera = gameObject.GetComponent<Camera>();
            _cinematicCamera.enabled = false;
        }

        protected override void OnTriggerImpl(TriggerMessage inMessage)
        {
            var controller = GetController();
            if (controller.PawnInstance != null)
            {
                var actionStateMachine = controller.PawnInstance.GetComponent<IActionStateMachineInterface>();
                if (actionStateMachine != null)
                {
                    actionStateMachine.RequestActionState
                    (
                        EActionStateMachineTrack.Cinematic,
                        EActionStateId.CinematicCamera,
                        new CinematicCameraActionStateInfo(controller.PawnInstance, _cinematicCamera, CinematicTime)
                    );
                }
            }
        }

        private ControllerComponent GetController()
        {
            if (_cachedController == null)
            {
                _cachedController = GameModeComponent.RegisteredGameMode.ActiveController;
            }

            return _cachedController;
        }

        protected override void OnCancelTriggerImpl(CancelTriggerMessage inMessage)
        {
        }
    }
}

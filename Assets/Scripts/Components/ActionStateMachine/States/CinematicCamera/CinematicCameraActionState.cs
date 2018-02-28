// Copyright (C) Threetee Gang All Rights Reserved

using Assets.Scripts.Components.ActionStateMachine.States.OpenMenuUI;
using Assets.Scripts.Components.Character;
using Assets.Scripts.Components.Health;
using Assets.Scripts.Input;
using Assets.Scripts.Messaging;
using UnityEngine;

namespace Assets.Scripts.Components.ActionStateMachine.States.CinematicCamera
{
    public class CinematicCameraActionState
        : ActionState
    {
        private readonly CinematicCameraActionStateInfo _cinematicInfo;

        private Camera _initialCamera;
        private Camera InitialCamera { get { return GetInitialCamera(); } set { _initialCamera = value; } }

        private InGameMenuInputHandler _blockingInputHandler;
        private float _elapsedTime;

        public CinematicCameraActionState(CinematicCameraActionStateInfo inInfo) 
            : base(EActionStateId.CinematicCamera, inInfo)
        {
            _cinematicInfo = inInfo;

            _elapsedTime = 0.0f;
        }

        protected override void OnStart()
        {
            SwapActiveCameras();
            RegisterInputHandlers();

            var healthInterface = Info.Owner.GetComponent<IHealthInterface>();
            if (healthInterface != null)
            {
                healthInterface.SetHealthChangedEnabled(false, EHealthLockReason.Cinematic);
            }

            UnityMessageEventFunctions.InvokeMessageEventWithDispatcher(Info.Owner, new EnterCinematicCameraActionStateMessage());
        }

        private void RegisterInputHandlers()
        {
            var inputBinder = Info.Owner.GetComponent<IInputBinderInterface>();
            if (inputBinder != null)
            {
                _blockingInputHandler = new InGameMenuInputHandler();
                inputBinder.RegisterInputHandler(_blockingInputHandler);
            }
        }

        protected override void OnUpdate(float deltaTime)
        {
            _elapsedTime += deltaTime;

            if (_elapsedTime > _cinematicInfo.CameraTime)
            {
                Info.Owner.GetComponent<IActionStateMachineInterface>().RequestActionState
                (
                    EActionStateMachineTrack.Cinematic, 
                    EActionStateId.Null, 
                    new ActionStateInfo(Info.Owner)
                );
            }
        }

        protected override void OnEnd()
        {
            UnityMessageEventFunctions.InvokeMessageEventWithDispatcher(Info.Owner, new ExitCinematicCameraActionStateMessage());

            var healthInterface = Info.Owner.GetComponent<IHealthInterface>();
            if (healthInterface != null)
            {
                healthInterface.SetHealthChangedEnabled(true, EHealthLockReason.Cinematic);
            }

            UnregisterInputHandlers();
            SwapActiveCameras();
        }

        private void UnregisterInputHandlers()
        {
            var inputBinder = Info.Owner.GetComponent<IInputBinderInterface>();
            if (inputBinder != null)
            {
                inputBinder.UnregisterInputHandler(_blockingInputHandler);
            }
        }

        private void SwapActiveCameras()
        {
            InitialCamera.enabled = !InitialCamera.enabled;

            _cinematicInfo.SwappedCamera.enabled = !_cinematicInfo.SwappedCamera.enabled;
        }

        private Camera GetInitialCamera()
        {
            if (_initialCamera != null)
            {
                return _initialCamera;
            }

            var character = Info.Owner.GetComponent<CharacterComponent>();
            if (character != null && character.ActiveController != null)
            {
                return character.ActiveController.GetComponent<Camera>();
            }

            return null;
        }
    }
}

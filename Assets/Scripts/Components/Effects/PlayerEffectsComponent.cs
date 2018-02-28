// Copyright (C) Threetee Gang All Rights Reserved

using Assets.Scripts.Components.ActionStateMachine.States.Dead;
using Assets.Scripts.Components.Character;
using Assets.Scripts.Messaging;
using Assets.Scripts.Services.Time;
using UnityEngine;

namespace Assets.Scripts.Components.Effects
{
    [RequireComponent(typeof(CharacterComponent))]
    public class PlayerEffectsComponent
        : MonoBehaviour
    {
        public float FadeInTime = 2.0f;
        public float FadeOutTime = 2.0f;

        public float PauseAlpha = 0.5f;

        private ICameraPostProcessingInterface _cameraPostProcessing;

        private UnityMessageEventHandle<EnterDeadActionStateMessage> _enterDeadActionStateHandle;
        private UnityMessageEventHandle<LeftDeadActionStateMessage> _leftDeadActionStateHandle;

        private UnityMessageEventHandle<PauseStatusChangedMessage> _pauseStatusChangedHandle;

        protected void Start()
        {
            var characterComponent = gameObject.GetComponent<CharacterComponent>();
            _cameraPostProcessing = characterComponent.ActiveController.gameObject
                .GetComponent<ICameraPostProcessingInterface>();

            RegisterForMessages();
        }

        protected void OnDestroy()
        {
            UnregisterForMessages();

            _cameraPostProcessing = null;
        }

        private void RegisterForMessages()
        {
            _enterDeadActionStateHandle =
                UnityMessageEventFunctions.RegisterActionWithDispatcher<EnterDeadActionStateMessage>(gameObject,
                    OnEnterDeadActionState);

            _leftDeadActionStateHandle =
                UnityMessageEventFunctions.RegisterActionWithDispatcher<LeftDeadActionStateMessage>(gameObject,
                    OnLeftDeadActionState);

            _pauseStatusChangedHandle =
                UnityMessageEventFunctions.RegisterActionWithDispatcher<PauseStatusChangedMessage>(gameObject,
                    OnPauseStatusChanged);
        }

        private void UnregisterForMessages()
        {
            UnityMessageEventFunctions.UnregisterActionWithDispatcher(gameObject, _pauseStatusChangedHandle);
            UnityMessageEventFunctions.UnregisterActionWithDispatcher(gameObject, _leftDeadActionStateHandle);
            UnityMessageEventFunctions.UnregisterActionWithDispatcher(gameObject, _enterDeadActionStateHandle);
        }

        private void OnEnterDeadActionState(EnterDeadActionStateMessage inMessage)
        {
            _cameraPostProcessing.RequestCameraFade(1.0f, FadeInTime);
        }

        private void OnLeftDeadActionState(LeftDeadActionStateMessage inMessage)
        {
            _cameraPostProcessing.RequestCameraFade(0.0f, FadeOutTime);
        }

        private void OnPauseStatusChanged(PauseStatusChangedMessage inMessage)
        {
            if (inMessage.NewPauseStatus == EPauseStatus.Paused)
            {
                _cameraPostProcessing.RequestCameraFade(PauseAlpha, 0.0f);
            }
            else
            {
                _cameraPostProcessing.RequestCameraFade(0.0f, 0.0f);
            }
        }
    }
}


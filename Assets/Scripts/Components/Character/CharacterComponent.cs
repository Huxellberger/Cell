// Copyright (C) Threetee Gang All Rights Reserved

using Assets.Scripts.Components.ActionStateMachine;
using Assets.Scripts.Components.Audio;
using Assets.Scripts.Components.Controller;
using Assets.Scripts.Input;
using Assets.Scripts.Instance;
using UnityEngine;

namespace Assets.Scripts.Components.Character
{
    [RequireComponent(typeof(IActionStateMachineInterface), typeof(IInputBinderInterface))]
    public class CharacterComponent 
        : MonoBehaviour
    {
        public EActionStateId StartingState = EActionStateId.Locomotion;

        private ControllerComponent _activeController;

        public ControllerComponent ActiveController
        {
            get { return _activeController; }
            set { OnNewControllerSet(value); }
        }

        protected virtual void Start()
        {
            InitialiseStartingState();
            InitialiseInputBinder();
        }

        private void InitialiseStartingState()
        {
            var actionStateMachineInterface = gameObject.GetComponent<IActionStateMachineInterface>();
            if (StartingState != EActionStateId.Null)
            {
                actionStateMachineInterface.RequestActionState(EActionStateMachineTrack.Locomotion, StartingState, new ActionStateInfo(gameObject));
            }
        }

        private void InitialiseInputBinder()
        {
            var inputBinderInterface = gameObject.GetComponent<IInputBinderInterface>();
            inputBinderInterface.SetInputInterface(GameInstance.CurrentInstance.gameObject.GetComponent<IInputInterface>());
        }

        private void OnNewControllerSet(ControllerComponent inNewController)
        {
            _activeController = inNewController;

            var musicComponent = gameObject.GetComponent<PlayerMusicComponent>();

            if (musicComponent != null)
            {
                if (ActiveController != null)
                {
                    musicComponent.MusicAudioSource = ActiveController.GetComponent<AudioSource>();
                }
            }
        }
    }
}

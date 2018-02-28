// Copyright (C) Threetee Gang All Rights Reserved

using Assets.Scripts.Components.ActionStateMachine.States.Locomotion;
using Assets.Scripts.Components.Species;
using Assets.Scripts.Input;
using Assets.Scripts.Messaging;
using UnityEngine;

namespace Assets.Scripts.Components.Character
{
    [RequireComponent(typeof(ISpeciesInterface))]
    public class AnimalCharacterComponent 
        : CharacterComponent
    {
        private UnityMessageEventHandle<EnterLocomotionStateMessage> _enterLocomotionHandle;
        private UnityMessageEventHandle<LeaveLocomotionStateMessage> _leaveLocomotionHandle;

        private AnimalInputHandler _animalInputHandler;

        protected override void Start()
        {
            base.Start();

            RegisterAnimalMessages();
        }

        protected void OnDestroy()
        {
            UnregisterAnimalMessages();
        }

        private void RegisterAnimalMessages()
        {
            _enterLocomotionHandle =
                UnityMessageEventFunctions.RegisterActionWithDispatcher<EnterLocomotionStateMessage>(gameObject,
                    OnEnterLocomotion);

            _leaveLocomotionHandle =
                UnityMessageEventFunctions.RegisterActionWithDispatcher<LeaveLocomotionStateMessage>(gameObject,
                    OnLeavingLocomotion);
        }

        private void UnregisterAnimalMessages()
        {
            UnityMessageEventFunctions.UnregisterActionWithDispatcher(gameObject, _leaveLocomotionHandle);
            UnityMessageEventFunctions.UnregisterActionWithDispatcher(gameObject, _enterLocomotionHandle);
        }

        private void OnEnterLocomotion(EnterLocomotionStateMessage inMessage)
        {
            var inputBinderInterface = gameObject.GetComponent<IInputBinderInterface>();
            _animalInputHandler = new AnimalInputHandler(gameObject.GetComponent<ISpeciesInterface>());

            inputBinderInterface.RegisterInputHandler(_animalInputHandler);
        }

        private void OnLeavingLocomotion(LeaveLocomotionStateMessage inMessage)
        {
            var inputBinderInterface = gameObject.GetComponent<IInputBinderInterface>();

            inputBinderInterface.UnregisterInputHandler(_animalInputHandler);
        }
    }
}

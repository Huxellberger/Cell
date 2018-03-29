// Copyright (C) Threetee Gang All Rights Reserved

using Assets.Scripts.AI.Companion;
using Assets.Scripts.Components.Character;
using Assets.Scripts.Components.Equipment.Holdables;
using Assets.Scripts.Components.Gadget;
using Assets.Scripts.Components.Interaction;
using Assets.Scripts.Components.Movement;
using Assets.Scripts.Input;
using Assets.Scripts.Input.Handlers;
using Assets.Scripts.Messaging;
using Assets.Scripts.Services;
using Assets.Scripts.Services.Time;

namespace Assets.Scripts.Components.ActionStateMachine.States.Locomotion
{
    public class LocomotionActionState 
        : ActionState
    {
        private LocomotionInputHandler _locomotionInputHandler;
        private PauseInputHandler _pauseInputHandler;
        private InteractionInputHandler _interactionInputHandler;
        private CompanionInputHandler _companionInputHandler;
        private GadgetInputHandler _gadgetInputHandler;

        private IInputBinderInterface _inputBinderInterface;

        public LocomotionActionState(ActionStateInfo inInfo) : base(EActionStateId.Locomotion, inInfo)
        {
        }

        protected override void OnStart()
        {
            var character = Info.Owner.GetComponent<CharacterComponent>();

            UnityMessageEventFunctions.InvokeMessageEventWithDispatcher(Info.Owner, new EnterLocomotionStateMessage());

            RegisterForInput(character);
        }

        private void RegisterForInput(CharacterComponent inCharacter)
        {
            _locomotionInputHandler = new LocomotionInputHandler(Info.Owner.GetComponent<IMovementInterface>(), inCharacter.ActiveController.GetComponent<IPlayerCameraInterface>(), Info.Owner.GetComponent<IHeldItemInterface>());
            _interactionInputHandler = new InteractionInputHandler(Info.Owner.GetComponent<IInteractionInterface>());
            _pauseInputHandler = new PauseInputHandler(GameServiceProvider.CurrentInstance.GetService<ITimeServiceInterface>());
            _companionInputHandler = new CompanionInputHandler(Info.Owner.GetComponent<ICompanionSetInterface>());
            _gadgetInputHandler = new GadgetInputHandler(Info.Owner.GetComponent<IGadgetSetInterface>());

            _inputBinderInterface = Info.Owner.GetComponent<IInputBinderInterface>();

            if (_inputBinderInterface != null)
            {
                _inputBinderInterface.RegisterInputHandler(_gadgetInputHandler);
                _inputBinderInterface.RegisterInputHandler(_companionInputHandler);
                _inputBinderInterface.RegisterInputHandler(_pauseInputHandler);
                _inputBinderInterface.RegisterInputHandler(_interactionInputHandler);
                _inputBinderInterface.RegisterInputHandler(_locomotionInputHandler);
            }
        }

        protected override void OnUpdate(float deltaTime)
        {
            
        }

        protected override void OnEnd()
        {
            UnregisterForInput();

            UnityMessageEventFunctions.InvokeMessageEventWithDispatcher(Info.Owner, new LeaveLocomotionStateMessage());
        }

        private void UnregisterForInput()
        {
            if (_inputBinderInterface != null)
            {
                _inputBinderInterface.UnregisterInputHandler(_gadgetInputHandler);
                _inputBinderInterface.UnregisterInputHandler(_companionInputHandler);
                _inputBinderInterface.UnregisterInputHandler(_pauseInputHandler);
                _inputBinderInterface.UnregisterInputHandler(_interactionInputHandler);
                _inputBinderInterface.UnregisterInputHandler(_locomotionInputHandler);
            }
        }
    }
}


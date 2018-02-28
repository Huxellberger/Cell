// Copyright (C) Threetee Gang All Rights Reserved

using Assets.Scripts.Components.Character;
using Assets.Scripts.Components.Controller;
using Assets.Scripts.Components.Equipment.Holdables;
using UnityEngine;

namespace Assets.Scripts.Components.ActionStateMachine.States.Transforming
{
    public class TransformingActionState 
        : ActionState
    {
        private readonly TransformingActionStateInfo _transformingInfo;

        private ControllerComponent _owningController;

        public TransformingActionState(TransformingActionStateInfo inInfo) 
            : base(EActionStateId.Transforming, inInfo)
        {
            _transformingInfo = inInfo;
        }

        protected override void OnStart()
        {
            var character = _transformingInfo.Owner.GetComponent<CharacterComponent>();
            if (character != null)
            {
                _owningController = character.ActiveController;

                DropHeldItem();
                CreateNewCharacter();
            }
        }

        private void DropHeldItem()
        {
            var heldItem = Info.Owner.GetComponent<IHeldItemInterface>();
            if (heldItem != null)
            {
                heldItem.DropHoldable();
            }
        }

        private void CreateNewCharacter()
        {
            var localPosition = _transformingInfo.Owner.transform.localPosition;
            var localRotation = _transformingInfo.Owner.transform.localRotation;

            _owningController.DestroyPawn();

            var newPlayer = Object.Instantiate(_transformingInfo.TransformTypePrefab,
                localPosition, localRotation);

            _owningController.SetPawn(newPlayer);
        }

        protected override void OnUpdate(float deltaTime)
        {
        }

        protected override void OnEnd()
        {
            var newPlayer = _owningController.PawnInstance;

            var actionStateMachine = newPlayer.gameObject.GetComponent<IActionStateMachineInterface>();

            actionStateMachine.RequestActionState(EActionStateMachineTrack.Locomotion, EActionStateId.Locomotion, new ActionStateInfo(newPlayer));
        }
    }
}

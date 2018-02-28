// Copyright (C) Threetee Gang All Rights Reserved

using Assets.Scripts.Components.ActionStateMachine;
using Assets.Scripts.Components.ActionStateMachine.States.Transforming;
using Assets.Scripts.Components.Interaction;
using Assets.Scripts.Components.Species;
using UnityEngine;

namespace Assets.Scripts.Components.Objects.Totem
{
    public class TotemComponent 
        : InteractableComponent
    {
        public GameObject TransformTypePrefab;
        private ESpeciesType ? _transformationSpeciesType;

        protected void Start()
        {
            var speciesInterface = TransformTypePrefab.GetComponent<ISpeciesInterface>();
            if (speciesInterface != null)
            {
                _transformationSpeciesType = speciesInterface.GetCurrentSpeciesType();
            }
        }

        protected override bool CanInteractImpl(GameObject inGameObject)
        {
            var actionStateMachine = inGameObject.GetComponent<IActionStateMachineInterface>();

            if (actionStateMachine != null &&
                actionStateMachine.IsActionStateActiveOnTrack(EActionStateMachineTrack.Locomotion,
                    EActionStateId.Locomotion))
            {
                var speciesInterface = inGameObject.GetComponent<ISpeciesInterface>();

                if (speciesInterface != null)
                {
                    return speciesInterface.GetCurrentSpeciesType() != _transformationSpeciesType;
                }
            }

            return false;
        }

        protected override void OnInteractImpl(GameObject inGameObject)
        {
            var actionStateMachineInterface = inGameObject.GetComponent<IActionStateMachineInterface>();

            if (TransformTypePrefab != null)
            {
                actionStateMachineInterface.RequestActionState(EActionStateMachineTrack.Locomotion, EActionStateId.Transforming, new TransformingActionStateInfo(inGameObject, TransformTypePrefab));
            }
            else
            {
                Debug.LogError("No Transform type assigned!");
            }
        }
    }
}

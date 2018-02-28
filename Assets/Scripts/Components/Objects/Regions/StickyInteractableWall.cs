// Copyright (C) Threetee Gang All Rights Reserved

using System.Collections.Generic;
using Assets.Scripts.Components.ActionStateMachine;
using Assets.Scripts.Components.ActionStateMachine.States.SurfaceSticking;
using Assets.Scripts.Components.Interaction;
using Assets.Scripts.Components.Species;
using UnityEngine;

namespace Assets.Scripts.Components.Objects.Regions
{
    public class StickyInteractableWall 
        : InteractableComponent
    {
        public List<ESpeciesType> StickySpeciesTypes = new List<ESpeciesType>();

        protected override bool CanInteractImpl(GameObject inGameObject)
        {
            if (inGameObject == null || inGameObject.GetComponent<IActionStateMachineInterface>() == null)
            {
                return false;
            }

            var species = inGameObject.GetComponent<ISpeciesInterface>();
            if (species != null)
            {
                return StickySpeciesTypes.Contains(species.GetCurrentSpeciesType());
            }

            return false;
        }

        protected override void OnInteractImpl(GameObject inGameObject)
        {
            var actionStateMachine = inGameObject.GetComponent<IActionStateMachineInterface>();
            if (actionStateMachine.IsActionStateActiveOnTrack(EActionStateMachineTrack.Locomotion,
                EActionStateId.SurfaceSticking))
            {
                actionStateMachine.RequestActionState
                (
                    EActionStateMachineTrack.Locomotion,
                    EActionStateId.Locomotion, 
                    new ActionStateInfo(inGameObject)
                );
            }
            else
            {
                actionStateMachine.RequestActionState
                (
                    EActionStateMachineTrack.Locomotion, 
                    EActionStateId.SurfaceSticking, 
                    new SurfaceStickingActionStateInfo(inGameObject, gameObject)
                );
            }
        }
    }
}

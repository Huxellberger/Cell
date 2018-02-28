// Copyright (C) Threetee Gang All Rights Reserved

using System.Collections.Generic;
using Assets.Scripts.Components.ActionStateMachine;
using Assets.Scripts.Components.ActionStateMachine.States.PushObjectActionState;
using Assets.Scripts.Components.Equipment.Holdables;
using Assets.Scripts.Components.Interaction;
using Assets.Scripts.Components.Species;
using UnityEngine;

namespace Assets.Scripts.Components.Objects.Pushable
{
    public class PushObjectPointComponent
        : InteractableComponent
    {
        public GameObject PushableObject;
        public List<ESpeciesType> PushableSpeciesTypes = new List<ESpeciesType>();

        private GameObject _owner;

        protected override bool CanInteractImpl(GameObject inGameObject)
        {
            if (_owner != null && inGameObject == _owner)
            {
                return true;
            }

            if (_owner != null || inGameObject == null || PushableObject == null)
            {
                return false;
            }

            return InLocomotion(inGameObject.GetComponent<IActionStateMachineInterface>()) &&
                   IsCorrectSpecies(inGameObject.GetComponent<ISpeciesInterface>()) &&
                   NotHoldingAnything(inGameObject.GetComponent<IHeldItemInterface>());
        }

        protected override void OnInteractImpl(GameObject inGameObject)
        {
            if (_owner == inGameObject)
            {
                inGameObject.GetComponent<IActionStateMachineInterface>().RequestActionState
                (
                    EActionStateMachineTrack.Locomotion,
                    EActionStateId.Locomotion,
                    new ActionStateInfo(inGameObject)
                );
                _owner = null;
            }
            else
            {
                _owner = inGameObject;
                inGameObject.GetComponent<IActionStateMachineInterface>().RequestActionState
                (
                    EActionStateMachineTrack.Locomotion,
                    EActionStateId.PushObject,
                    new PushObjectActionStateInfo(inGameObject, PushableObject, gameObject)
                );
            }
            
        }

        private bool InLocomotion(IActionStateMachineInterface inActionStateMachineInterface)
        {
            return inActionStateMachineInterface != null &&
                   inActionStateMachineInterface.IsActionStateActiveOnTrack(EActionStateMachineTrack.Locomotion,
                       EActionStateId.Locomotion);
        }

        private bool IsCorrectSpecies(ISpeciesInterface inSpeciesInterface)
        {
            return inSpeciesInterface != null &&
                   PushableSpeciesTypes.Contains(inSpeciesInterface.GetCurrentSpeciesType());
        }

        private bool NotHoldingAnything(IHeldItemInterface inHeldItemInterface)
        {
            return inHeldItemInterface == null || inHeldItemInterface.GetHeldItem() == null;
        }
    }
}

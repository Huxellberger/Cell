// Copyright (C) Threetee Gang All Rights Reserved

using Assets.Scripts.Components.ActionStateMachine;
using Assets.Scripts.Components.Character;
using Assets.Scripts.UnityLayer.GameObjects;
using UnityEngine;

namespace Assets.Scripts.Components.Controller
{
    public class ControllerComponent : MonoBehaviour
    {
        public GameObject PawnInstance { get; protected set; }
        public Transform PawnInitialTransform { get; set; }

        public void CreatePawnOfType(GameObject inPawnType)
        {
            if (PawnInitialTransform != null)
            {
                SetPawn(Instantiate(inPawnType, PawnInitialTransform.localPosition, PawnInitialTransform.localRotation));
            }
            else
            {
                SetPawn(Instantiate(inPawnType));
            }

            SetInitialActionState();
        }

        public void SetPawn(GameObject inPawnInstance)
        {
            if (PawnInstance != null)
            {
                var originalCharacterComponent = PawnInstance.GetComponent<CharacterComponent>();
                if (originalCharacterComponent != null)
                {
                    originalCharacterComponent.ActiveController = null;
                }
            }

            PawnInstance = inPawnInstance;

            if (PawnInstance != null)
            {
                var characterComponent = PawnInstance.GetComponent<CharacterComponent>();
                if (characterComponent != null)
                {
                    characterComponent.ActiveController = this;
                }

                UpdateTransformParent();
            }
        }

        public void DestroyPawn()
        {
            transform.parent = null;
            DestructionFunctions.DestroyGameObject(PawnInstance);
        }

        private void UpdateTransformParent()
        {
            transform.parent = PawnInstance.transform;
        }

        private void SetInitialActionState()
        {
            var stateMachine = PawnInstance.GetComponent<IActionStateMachineInterface>();

            if (stateMachine != null)
            {
                stateMachine.RequestActionState(EActionStateMachineTrack.Locomotion, EActionStateId.Spawning, new ActionStateInfo(PawnInstance));
            }
        }
    }
}

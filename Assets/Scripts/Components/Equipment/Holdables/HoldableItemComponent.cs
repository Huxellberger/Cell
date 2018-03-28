// Copyright (C) Threetee Gang All Rights Reserved

using Assets.Scripts.Components.Interaction;
using UnityEngine;

namespace Assets.Scripts.Components.Equipment.Holdables
{
    public abstract class HoldableItemComponent 
        : InteractableComponent 
        , IHoldableInterface
    {
        public GameObject InteractionZone;
        protected GameObject Owner;

        // InteractableComponent
        protected override bool CanInteractImpl(GameObject inGameObject)
        {
            return Owner == null && inGameObject != null && inGameObject.GetComponent<IHeldItemInterface>() != null;
        }

        protected override void OnInteractImpl(GameObject inGameObject)
        {
            var heldItemComponent = inGameObject.GetComponent<IHeldItemInterface>();

            if (heldItemComponent != null)
            {
                heldItemComponent.PickupHoldable(this);
            }
        }
        // ~InteractableComponent

        // IHoldableInterface
        public void UseHoldable(EHoldableAction inAction)
        {
            if (Owner != null)
            {
                UseHoldableImpl(inAction);
            }
        }

        public void OnHeld(GameObject inGameObject)
        {
            if (inGameObject != null && Owner == null)
            {
                var heldItemInterface = inGameObject.GetComponent<IHeldItemInterface>();
                if (heldItemInterface != null)
                {
                    var rigidbody = gameObject.GetComponent<Rigidbody>();
                    if (rigidbody != null)
                    {
                        rigidbody.constraints = RigidbodyConstraints.FreezeAll;
                    }

                    Owner = inGameObject;
                    InteractionZone.SetActive(false);
                    gameObject.transform.parent = heldItemInterface.GetHeldItemSocket();
                    gameObject.transform.position = gameObject.transform.parent.position;
                    gameObject.transform.eulerAngles = gameObject.transform.parent.eulerAngles;
                    OnHeldImpl();
                }
            }
        }

        public void OnDropped()
        {
            if (Owner != null)
            {
                OnDroppedImpl();
                gameObject.transform.parent = null;
                InteractionZone.SetActive(true);
                Owner = null;

                var rigidbody = gameObject.GetComponent<Rigidbody>();
                if (rigidbody != null)
                {
                    rigidbody.constraints = RigidbodyConstraints.None;
                }
            }
        }

        public GameObject GetHoldableObject()
        {
            return gameObject;
        }
        // ~IHoldableInterface

        protected abstract void UseHoldableImpl(EHoldableAction inAction);
        protected abstract void OnHeldImpl();
        protected abstract void OnDroppedImpl();
    }
}

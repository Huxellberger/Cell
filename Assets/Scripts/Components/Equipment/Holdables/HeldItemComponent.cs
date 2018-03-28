// Copyright (C) Threetee Gang All Rights Reserved

using UnityEngine;

namespace Assets.Scripts.Components.Equipment.Holdables
{
    public class HeldItemComponent 
        : MonoBehaviour 
        , IHeldItemInterface
    {
        public GameObject StartingHeldItem;
        public GameObject HeldItemSocketObject;

        private IHoldableInterface _currentHoldable;

        protected void Start()
        {
            if (StartingHeldItem != null)
            {
                PickupHoldable(Instantiate(StartingHeldItem).GetComponent<IHoldableInterface>());
            }
        }

        protected void OnDestroy()
        {
            DropHoldable();
        }

        public void UseCurrentHoldable(EHoldableAction inAction)
        {
            if (_currentHoldable != null)
            {
                _currentHoldable.UseHoldable(inAction);
            }
        }

        public void PickupHoldable(IHoldableInterface inHoldableInterface)
        {
            if (inHoldableInterface != null)
            {
                DropHoldable();

                _currentHoldable = inHoldableInterface;
                _currentHoldable.OnHeld(gameObject);
            }
        }

        public void DropHoldable()
        {
            if (_currentHoldable != null)
            {
                _currentHoldable.OnDropped();
                _currentHoldable = null;
            }
        }

        public IHoldableInterface GetHeldItem()
        {
            return _currentHoldable;
        }

        public Transform GetHeldItemSocket()
        {
            return HeldItemSocketObject.transform;
        }
    }
}

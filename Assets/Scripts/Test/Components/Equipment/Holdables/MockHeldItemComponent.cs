// Copyright (C) Threetee Gang All Rights Reserved

#if UNITY_EDITOR

using Assets.Scripts.Components.Equipment.Holdables;
using UnityEngine;

namespace Assets.Scripts.Test.Components.Equipment.Holdables
{
    public class MockHeldItemComponent 
        : MonoBehaviour
        , IHeldItemInterface
    {
        public EHoldableAction ? UseCurrentHoldableInput { get; private set; }
        public IHoldableInterface PickupHoldableInput { get; private set; }
        public bool DropHoldableCalled = false;
        public IHoldableInterface GetHeldItemResult { get; set; }
        public Transform GetHeldItemSocketResult { get; set; }

        public void UseCurrentHoldable(EHoldableAction inAction)
        {
            UseCurrentHoldableInput = inAction;
        }

        public void PickupHoldable(IHoldableInterface inHoldableInterface)
        {
            PickupHoldableInput = inHoldableInterface;
        }

        public void DropHoldable()
        {
            DropHoldableCalled = true;
        }

        public IHoldableInterface GetHeldItem()
        {
            return GetHeldItemResult;
        }

        public Transform GetHeldItemSocket()
        {
            return GetHeldItemSocketResult;
        }
    }
}

#endif // UNITY_EDITOR

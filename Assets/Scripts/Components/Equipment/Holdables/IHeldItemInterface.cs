// Copyright (C) Threetee Gang All Rights Reserved

using UnityEngine;

namespace Assets.Scripts.Components.Equipment.Holdables
{
    public interface IHeldItemInterface
    {
        void UseCurrentHoldable(EHoldableAction inAction);
        void PickupHoldable(IHoldableInterface inHoldableInterface);
        void DropHoldable();
        IHoldableInterface GetHeldItem();
        Transform GetHeldItemSocket();
    }
}

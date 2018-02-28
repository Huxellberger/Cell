// Copyright (C) Threetee Gang All Rights Reserved

using UnityEngine;

namespace Assets.Scripts.Components.Equipment.Holdables
{
    public interface IHoldableInterface
    {
        void UseHoldable(EHoldableAction inAction);
        void OnHeld(GameObject inGameObject);
        void OnDropped();
    }
}

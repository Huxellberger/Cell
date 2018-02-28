// Copyright (C) Threetee Gang All Rights Reserved

#if UNITY_EDITOR

using Assets.Scripts.Components.Equipment.Holdables;
using UnityEngine;

namespace Assets.Scripts.Test.Components.Equipment.Holdables
{
    public class TestHoldableItemComponent 
        : HoldableItemComponent
    {
        public EHoldableAction ? UseHoldableImplAction { get; private set; }

        public bool OnHeldImplCalled = false;
        public bool OnDroppedImplCalled = false;

        public GameObject GetOwner()
        {
            return Owner;
        }

        protected override void UseHoldableImpl(EHoldableAction inAction)
        {
            UseHoldableImplAction = inAction;
        }

        protected override void OnHeldImpl()
        {
            OnHeldImplCalled = true;
        }

        protected override void OnDroppedImpl()
        {
            OnDroppedImplCalled = true;
        }
    }
}

#endif // UNITY_EDITOR

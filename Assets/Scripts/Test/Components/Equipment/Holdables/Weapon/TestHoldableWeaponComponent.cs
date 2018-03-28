// Copyright (C) Threetee Gang All Rights Reserved

#if UNITY_EDITOR

using Assets.Scripts.Components.Equipment.Holdables;
using Assets.Scripts.Components.Equipment.Holdables.Weapon;
using UnityEngine;

namespace Assets.Scripts.Test.Components.Equipment.Holdables.Weapon
{
    public class TestHoldableWeaponComponent 
        : HoldableWeaponComponent 
    {
        public EHoldableAction ? UseHoldableImplAction { get; private set; }
        public bool OnHeldImplCalled = false;
        public bool OnDroppedImplCalled = false;

        public GameObject PrepareWeaponTarget { get; private set; }
        
        public GameObject CanUseWeaponImplTarget { get; private set; }
        public bool CanUseWeaponResult = true;

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

        protected override void PrepareWeapon(GameObject target)
        {
            PrepareWeaponTarget = target;
        }

        protected override bool CanUseWeaponImpl(GameObject target)
        {
            CanUseWeaponImplTarget = target;
            return CanUseWeaponResult;
        }

        public GameObject GetOwner()
        {
            return Owner;
        }
    }
}

#endif // UNITY_EDITOR

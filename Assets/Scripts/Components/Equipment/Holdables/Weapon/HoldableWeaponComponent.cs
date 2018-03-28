// Copyright (C) Threetee Gang All Rights Reserved

using Assets.Scripts.Core;
using UnityEngine;

namespace Assets.Scripts.Components.Equipment.Holdables.Weapon
{
    public abstract class HoldableWeaponComponent 
        : HoldableItemComponent
        , IWeaponInterface
    {
        public float AttackRadiusSquared = 100.0f;

        // IWeaponInterface
        public bool CanUseWeapon(GameObject target)
        {
            return target != null && IsInAttackRadius(target) && CanUseWeaponImpl(target);
        }

        public void UseWeapon(GameObject target)
        {
            if (CanUseWeapon(target))
            {
                PrepareWeapon(target);
                UseHoldable(EHoldableAction.Primary);
            }
        }
        // ~IWeaponInterface

        protected abstract void PrepareWeapon(GameObject target);
        protected abstract bool CanUseWeaponImpl(GameObject target);

        private bool IsInAttackRadius(GameObject target)
        {
            return VectorFunctions.DistanceSquared(gameObject.transform.position, target.transform.position) <
                   AttackRadiusSquared;
        }
    }
}

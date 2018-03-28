// Copyright (C) Threetee Gang All Rights Reserved

using Assets.Scripts.Components.Equipment.Holdables.Weapon;
using Assets.Scripts.Components.Spawning;
using UnityEngine;

namespace Assets.Scripts.Components.Equipment.Holdables.Spawner
{
    [RequireComponent(typeof(ISpawnerInterface))]
    public class HoldableSpawnerComponent 
        : HoldableWeaponComponent
    {
        public LayerMask BlockingMask;

        private ISpawnerInterface _spawner;

        protected void Start()
        {
            _spawner = gameObject.GetComponent<ISpawnerInterface>();
        }

        protected override void UseHoldableImpl(EHoldableAction inAction)
        {
            _spawner.Spawn();
        }

        protected override void OnHeldImpl()
        {
        }

        protected override void OnDroppedImpl()
        {
        }

        protected override void PrepareWeapon(GameObject target)
        {
            Owner.transform.up = (target.transform.position - Owner.transform.position).normalized;
        }

        protected override bool CanUseWeaponImpl(GameObject target)
        {
            return Owner != null && HasClearShot(target);
        }

        protected virtual bool HasClearShot(GameObject target)
        {
            var raycastDirection = (target.transform.position - gameObject.transform.position).normalized;
            var result = Physics2D.Raycast(gameObject.transform.position, raycastDirection, AttackRadiusSquared, BlockingMask);

            return result.transform == target.transform;
        }
    }
}

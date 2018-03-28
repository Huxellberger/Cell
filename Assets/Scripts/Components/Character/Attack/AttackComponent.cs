// Copyright (C) Threetee Gang All Rights Reserved

using Assets.Scripts.Components.Equipment.Holdables;
using Assets.Scripts.Components.Equipment.Holdables.Weapon;
using UnityEngine;

namespace Assets.Scripts.Components.Character.Attack
{
    [RequireComponent(typeof(IHeldItemInterface))]
    public class AttackComponent 
        : MonoBehaviour 
        , IAttackInterface
    {
        private IHeldItemInterface _heldItem;

        protected void Start()
        {
            _heldItem = gameObject.GetComponent<IHeldItemInterface>();
        }

        protected void OnDestroy()
        {
            _heldItem = null;
        }

        public bool CanAttack(GameObject inTarget)
        {
            if (inTarget != null)
            {
                var currentHoldable = _heldItem.GetHeldItem();
                if (currentHoldable != null)
                {
                    var weaponInterface = currentHoldable.GetHoldableObject().GetComponent<IWeaponInterface>();
                    if (weaponInterface != null)
                    {
                        return weaponInterface.CanUseWeapon(inTarget);
                    }
                }
            }

            return false;
        }

        public void Attack(GameObject inTarget)
        {
            if (CanAttack(inTarget))
            {
                _heldItem.GetHeldItem().GetHoldableObject().GetComponent<IWeaponInterface>().UseWeapon(inTarget);
            }
        }
    }
}

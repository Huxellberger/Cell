// Copyright (C) Threetee Gang All Rights Reserved

#if UNITY_EDITOR

using Assets.Scripts.Components.Equipment.Holdables.Weapon;
using UnityEngine;

namespace Assets.Scripts.Test.Components.Equipment.Holdables.Weapon
{
    public class MockWeaponComponent 
        : MonoBehaviour
        , IWeaponInterface
    {
        public GameObject CanUseWeaponTargetObject { get; private set; }
        public bool CanUseWeaponResult = true;
        public GameObject UseWeaponTargetObject { get; private set; }

        public bool CanUseWeapon(GameObject target)
        {
            CanUseWeaponTargetObject = target;

            return CanUseWeaponResult;
        }

        public void UseWeapon(GameObject target)
        {
            UseWeaponTargetObject = target;
        }
    }
}

#endif // UNITY_EDITOR

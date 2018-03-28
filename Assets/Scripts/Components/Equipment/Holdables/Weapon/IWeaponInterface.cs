// Copyright (C) Threetee Gang All Rights Reserved

using UnityEngine;

namespace Assets.Scripts.Components.Equipment.Holdables.Weapon
{
    public interface IWeaponInterface
    {
        bool CanUseWeapon(GameObject target);
        void UseWeapon(GameObject target);
    }
}

// Copyright (C) Threetee Gang All Rights Reserved

using UnityEngine;

namespace Assets.Scripts.Components.Character.Attack
{
    public interface IAttackInterface
    {
        bool CanAttack(GameObject inTarget);
        void Attack(GameObject inTarget);
    }
}

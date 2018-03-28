// Copyright (C) Threetee Gang All Rights Reserved

#if UNITY_EDITOR

using Assets.Scripts.Components.Character.Attack;
using UnityEngine;

namespace Assets.Scripts.Test.Components.Character.Attack
{
    public class MockAttackComponent 
        : MonoBehaviour 
        , IAttackInterface
    {
        public bool CanAttackResult = true;
        public bool AttackCalled = false;
        public GameObject AttackedGameObject { get; private set; }

        public bool CanAttack(GameObject inTarget)
        {
            return CanAttackResult;
        }

        public void Attack(GameObject inTarget)
        {
            AttackCalled = true;
            AttackedGameObject = inTarget;
        }
    }
}

#endif // UNITY_EDITOR

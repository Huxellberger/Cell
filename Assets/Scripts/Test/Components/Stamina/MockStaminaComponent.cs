// Copyright (C) Threetee Gang All Rights Reserved

#if UNITY_EDITOR

using Assets.Scripts.Components.Stamina;
using UnityEngine;

namespace Assets.Scripts.Test.Components.Stamina
{
    public class MockStaminaComponent 
        : MonoBehaviour 
        , IStaminaInterface
    {
        public int ? AlterStaminaResult { get; private set; }
        public bool RefreshStaminaCalled = false;

        public bool ? SetStaminaChangeEnabledResult { get; private set; }
        public ELockStaminaReason? SetStaminaChangeEnabledReason { get; private set; }

        public int GetCurrentStaminaResult = 0;

        public bool CanExpendStaminaResult = true;
        public bool IsStaminaDepletedResult = false;

        // IStaminaInterface
        public void AlterStamina(int inStaminaModification)
        {
            AlterStaminaResult = inStaminaModification;
        }

        public void RefreshStamina()
        {
            RefreshStaminaCalled = true;
        }

        public void SetStaminaChangeEnabled(bool isEnabled, ELockStaminaReason inReason)
        {
            SetStaminaChangeEnabledResult = isEnabled;
            SetStaminaChangeEnabledReason = inReason;
        }

        public int GetCurrentStamina()
        {
            return GetCurrentStaminaResult;
        }

        public bool CanExpendStamina(int inStaminaToExpend)
        {
            return CanExpendStaminaResult;
        }

        public bool IsStaminaDepleted()
        {
            return IsStaminaDepletedResult;
        }
        // ~IStaminaInterface
    }
}

#endif // UNITY_EDITOR


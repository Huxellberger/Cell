// Copyright (C) Threetee Gang All Rights Reserved

#if UNITY_EDITOR

using Assets.Scripts.Components.Health;
using UnityEngine;

namespace Assets.Scripts.Test.Components.Health
{
    public class MockHealthComponent 
        : MonoBehaviour
        , IHealthInterface
    {
        public HealthAdjustmentUnit AdjustHealthResult { get; private set; }
        public bool ? SetHealthChangeEnabledResult { get; private set; }
        public EHealthLockReason ? SetHealthChangeEnabledLockReason { get; private set; }

        public bool ReplenishHealthCalled = false;

        // IHealthInterface
        public void AdjustHealth(HealthAdjustmentUnit inAdjustment)
        {
            AdjustHealthResult = inAdjustment;
        }

        public void SetHealthChangedEnabled(bool isEnabled, EHealthLockReason inReason)
        {
            SetHealthChangeEnabledResult = isEnabled;
            SetHealthChangeEnabledLockReason = inReason;
        }

        public void ReplenishHealth()
        {
            ReplenishHealthCalled = true;
        }

        public int GetCurrentHealth()
        {
            return 1;
        }

        public int GetMaxHealth()
        {
            return 1;
        }
        // ~IHealthInterface
    }
}

#endif

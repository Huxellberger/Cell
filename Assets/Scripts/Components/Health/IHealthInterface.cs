// Copyright (C) Threetee Gang All Rights Reserved

namespace Assets.Scripts.Components.Health
{
    public interface IHealthInterface
    {
        void AdjustHealth(int inChange);
        void SetHealthChangedEnabled(bool isEnabled, EHealthLockReason inReason);
        void ReplenishHealth();
        int GetCurrentHealth();
        int GetMaxHealth();
    }
}

// Copyright (C) Threetee Gang All Rights Reserved

namespace Assets.Scripts.Components.Stamina
{
    public interface IStaminaInterface
    {
        void AlterStamina(int inStaminaModification);
        void RefreshStamina();
        void SetStaminaChangeEnabled(bool locking, ELockStaminaReason inReason);

        int GetCurrentStamina();

        bool CanExpendStamina(int inStaminaToExpend);
        bool IsStaminaDepleted();
    }
}

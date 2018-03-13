// Copyright (C) Threetee Gang All Rights Reserved

using UnityEngine;

namespace Assets.Scripts.Components.Power
{
    public interface IPowerInterface
    {
        void ActivatePower(GameObject inOwner);
        void OnPowerSet(GameObject inOwner);
        void OnPowerCleared();
        bool CanActivatePower(GameObject inOwner);
        float GetPowerCooldownPercentage();
    }
}

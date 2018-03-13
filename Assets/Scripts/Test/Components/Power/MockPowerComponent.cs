// Copyright (C) Threetee Gang All Rights Reserved

#if UNITY_EDITOR

using Assets.Scripts.Components.Power;
using UnityEngine;

namespace Assets.Scripts.Test.Components.Power
{
    public class MockPowerComponent 
        : MonoBehaviour 
        , IPowerInterface
    {
        public GameObject ActivatePowerGameObject { get; private set; }
        public GameObject OnPowerSetGameObject { get; private set; }
        public GameObject CanActivatePowerGameObject { get; private set; }

        public bool CanActivatePowerResult = true;
        public bool OnPowerClearedCalled = false;
        public float GetPowerCooldownPercentageResult = 0.0f;

        public void ActivatePower(GameObject inOwner)
        {
            ActivatePowerGameObject = inOwner;
        }

        public void OnPowerSet(GameObject inOwner)
        {
            OnPowerSetGameObject = inOwner;
        }

        public void OnPowerCleared()
        {
            OnPowerClearedCalled = true;
        }

        public bool CanActivatePower(GameObject inOwner)
        {
            CanActivatePowerGameObject = inOwner;

            return CanActivatePowerResult;
        }

        public float GetPowerCooldownPercentage()
        {
            return GetPowerCooldownPercentageResult;
        }
    }
}
#endif // UNITY_EDITOR

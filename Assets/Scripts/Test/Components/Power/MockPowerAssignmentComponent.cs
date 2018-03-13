// Copyright (C) Threetee Gang All Rights Reserved

#if UNITY_EDITOR

using Assets.Scripts.Components.Power;
using UnityEngine;

namespace Assets.Scripts.Test.Components.Power
{
    public class MockPowerAssignmentComponent 
        : MonoBehaviour 
        , IPowerAssignmentInterface
    {
        public IPowerInterface SetPowerInterface { get; private set; }
        public EPowerSetting ? SetPowerSetting { get; private set; }

        public EPowerSetting ? ActivatePowerSetting { get; private set; }

        public void SetPower(IPowerInterface inPower, EPowerSetting inPowerSetting)
        {
            SetPowerInterface = inPower;
            SetPowerSetting = inPowerSetting;
        }

        public void ActivatePower(EPowerSetting inPowerSetting)
        {
            ActivatePowerSetting = inPowerSetting;
        }
    }
}

#endif // UNITY_EDITOR

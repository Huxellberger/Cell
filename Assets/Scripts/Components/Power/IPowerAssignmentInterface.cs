// Copyright (C) Threetee Gang All Rights Reserved

namespace Assets.Scripts.Components.Power
{
    public interface IPowerAssignmentInterface
    {
        void SetPower(IPowerInterface inPower, EPowerSetting inPowerSetting);
        void ActivatePower(EPowerSetting inPowerSetting);
    }
}

// Copyright (C) Threetee Gang All Rights Reserved

using Assets.Scripts.Messaging;

namespace Assets.Scripts.Components.Power
{
    public class PowerSetMessage
        : UnityMessagePayload
    {
        public readonly IPowerInterface PowerInterface;
        public readonly EPowerSetting PowerSetting;

        public PowerSetMessage(IPowerInterface inPowerInterface, EPowerSetting inPowerSetting)
            : base()
        {
            PowerInterface = inPowerInterface;
            PowerSetting = inPowerSetting;
        }
    }

    public class PowerActivationAttemptMessage
        : UnityMessagePayload
    {
        public readonly EPowerSetting PowerSetting;
        public readonly bool Success;

        public PowerActivationAttemptMessage(EPowerSetting inPowerSetting, bool inSuccess)
            : base()
        {
            PowerSetting = inPowerSetting;
            Success = inSuccess;
        }
    }

    public class PowerUpdateMessage
        : UnityMessagePayload
    {
        public readonly EPowerSetting PowerSetting;
        public readonly bool Active;
        public readonly float UpdatePercentage;

        public PowerUpdateMessage(EPowerSetting inPowerSetting, bool inActive, float inUpdatePercentage)
        {
            PowerSetting = inPowerSetting;
            Active = inActive;
            UpdatePercentage = inUpdatePercentage;
        }
    }
}


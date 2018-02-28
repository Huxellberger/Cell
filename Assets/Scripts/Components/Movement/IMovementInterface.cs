// Copyright (C) Threetee Gang All Rights Reserved

namespace Assets.Scripts.Components.Movement
{
    public interface IMovementInterface
    {
        void ApplyForwardMotion(float inForwardMagnitude);
        void ApplySidewaysMotion(float inTurningMotion);
        void SetSprintEnabled(bool inSprintEnabled);
    }
}

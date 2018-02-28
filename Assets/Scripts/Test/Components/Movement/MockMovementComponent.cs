// Copyright (C) Threetee Gang All Rights Reserved

#if UNITY_EDITOR

using Assets.Scripts.Components.Movement;
using UnityEngine;

namespace Assets.Scripts.Test.Components.Movement
{
    public class MockMovementComponent
        : MonoBehaviour
        , IMovementInterface
    {
        public float? ApplyForwardMotionResult { get; private set; }
        public float? ApplySidewaysMotionResult { get; private set; }

        public bool? SetSprintEnabledResult { get; private set; }
        public bool RequestJumpCalled = false;

        public void ApplyForwardMotion(float inForwardMagnitude)
        {
            ApplyForwardMotionResult = inForwardMagnitude;
        }

        public void ApplySidewaysMotion(float inTurningMotion)
        {
            ApplySidewaysMotionResult = inTurningMotion;
        }

        public void SetSprintEnabled(bool isEnabled)
        {
            SetSprintEnabledResult = isEnabled;
        }

        public void RequestJump()
        {
            RequestJumpCalled = true;
        }
    }
}

#endif // UNITY_EDITOR

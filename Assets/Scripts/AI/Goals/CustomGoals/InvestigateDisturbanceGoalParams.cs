// Copyright (C) Threetee Gang All Rights Reserved

using UnityEngine;

namespace Assets.Scripts.AI.Goals.CustomGoals
{
    [System.Serializable]
    public struct InvestigateDisturbanceGoalParams
    {
        [Range(0.0f, 1.0f)]
        public float DesirabilityOnDetection;
        public float IdleDelay;
    }
}

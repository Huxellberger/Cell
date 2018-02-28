// Copyright (C) Threetee Gang All Rights Reserved

using UnityEngine;

namespace Assets.Scripts.AI.Goals.CustomGoals
{
    [System.Serializable]
    public struct RemainInRadiusGoalParams
    {
        [Range(0.0f, 1.0f)]
        public float LeftRadiusDesirability;

        public float MaxRadius;
    }
}

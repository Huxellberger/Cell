// Copyright (C) Threetee Gang All Rights Reserved

using UnityEngine;

namespace Assets.Scripts.AI.Goals.CustomGoals
{
    [System.Serializable]
    public class FollowTargetGoalParams
    {
        [Range(0.0f, 1.0f)]
        public float ValidTargetDesirability;

        public float FollowRadius;

        public float LoseFollowRadiusSquared;
    }
}

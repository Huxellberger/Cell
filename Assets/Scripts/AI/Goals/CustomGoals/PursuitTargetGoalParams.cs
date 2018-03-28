// Copyright (C) Threetee Gang All Rights Reserved

using System;
using UnityEngine;

namespace Assets.Scripts.AI.Goals.CustomGoals
{
    [Serializable]
    public class PursuitTargetGoalParams
    {
        [Range(0.0f, 1.0f)]
        public float TargetDetectedDesirability;
        public float DelayBeforePursuit;
        public float AbandonPursuitRadiusSquared;
    }
}

// Copyright (C) Threetee Gang All Rights Reserved

using System;
using UnityEngine;

namespace Assets.Scripts.AI.Goals.CustomGoals
{
    [Serializable]
    public class PatrolPointsGoalParams 
    {
        [Range(0.0f, 1.0f)]
        public float PatrolDesirability;
    }
}

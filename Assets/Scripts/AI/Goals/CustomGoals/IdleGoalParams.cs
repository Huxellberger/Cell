// Copyright (C) Threetee Gang All Rights Reserved

using UnityEngine;

namespace Assets.Scripts.AI.Goals.CustomGoals
{
    [System.Serializable]
    public class IdleGoalParams
    {
        [Range(0.0f, 1.0f)]
        public float IdleDesirability;
    }
}

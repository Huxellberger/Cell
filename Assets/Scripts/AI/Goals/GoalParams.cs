// Copyright (C) Threetee Gang All Rights Reserved

using Assets.Scripts.AI.Goals.CustomGoals;
using UnityEngine;

namespace Assets.Scripts.AI.Goals
{
    [CreateAssetMenu(fileName = "Params", menuName = "ProjectQ/AI/Goals", order = 1)]
    public class GoalParams 
        : ScriptableObject
    {
        public IdleGoalParams IdleGoalParameters;
        public FollowTargetGoalParams FollowTargetGoalParameters;
        public RemainInRadiusGoalParams RemainInRadiusGoalParameters;
    }
}

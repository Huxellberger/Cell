// Copyright (C) Threetee Gang All Rights Reserved

using System.Collections.Generic;
using Assets.Scripts.AI.Goals.CustomGoals;
using Assets.Scripts.Services;
using Assets.Scripts.Services.Wildlife;
using UnityEngine;

namespace Assets.Scripts.AI.Goals
{
    public class GoalBuilder 
        : IGoalBuilderInterface
    {
        public delegate Goal GoalCreatorDelegate();

        private GameObject Owner { get; set; }
        private GoalParams Params { get; set; }
        private IDictionary<EGoalID, GoalCreatorDelegate> GoalInstantiationFunctions { get; set; }

        public GoalBuilder(GameObject inOwner, GoalParams inParams)
        {
            Owner = inOwner;
            Params = inParams;

            GoalInstantiationFunctions = new Dictionary<EGoalID, GoalCreatorDelegate>();

            InitialiseGoalCreationFunctions();
        }

        public void InitialiseGoalCreationFunctions()
        {
            GoalInstantiationFunctions.Add(EGoalID.FollowTarget, () => new FollowTargetGoal(Owner, Params.FollowTargetGoalParameters, GameServiceProvider.CurrentInstance.GetService<IWildlifeServiceInterface>()));
            GoalInstantiationFunctions.Add(EGoalID.Idle, () => new IdleGoal(Owner, Params.IdleGoalParameters));
            GoalInstantiationFunctions.Add(EGoalID.RemainInRadius, () => new RemainInRadiusGoal(Owner, Params.RemainInRadiusGoalParameters));
            GoalInstantiationFunctions.Add(EGoalID.InvestigateDisturbance, () => new InvestigateDisturbanceGoal(Owner, Params.InvestigateDisturbanceGoalParameters));
        }

        public Goal CreateGoalForId(EGoalID inId)
        {
            if (GoalInstantiationFunctions.ContainsKey(inId))
            {
                return GoalInstantiationFunctions[inId]();
            }

            Debug.LogError("Failed to find goal with Id " + inId);
            return null;
        }
    }
}

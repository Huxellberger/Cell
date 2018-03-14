// Copyright (C) Threetee Gang All Rights Reserved

using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.AI.Goals
{
    public class GoalPlannerComponent 
        : MonoBehaviour
    {
        public List<EGoalID> PossibleGoalIds;
        public GoalParams GoalParameters;

        private IGoalBuilderInterface GoalBuilder { get; set; }

        private IList<Goal> PotentialGoals { get; set; }

        private Goal _activeGoal;

        private Goal ActiveGoal
        {
            get { return _activeGoal; }
            set
            {
                if (_activeGoal != null)
                {
                    _activeGoal.Terminate();
                }
                _activeGoal = value;

                if (_activeGoal != null)
                {
                    _activeGoal.Initialise();
                }
            }
        }

        protected void Start ()
        {
		    PotentialGoals = new List<Goal>();

            GoalBuilder = GetGoalBuilderInterface();

            foreach (var possibleGoalId in PossibleGoalIds)
            {
                var newGoal = GoalBuilder.CreateGoalForId(possibleGoalId);
                newGoal.RegisterGoal();
                PotentialGoals.Add(newGoal);
            }
        }

        protected void OnDestroy()
        {
            foreach (var potentialGoal in PotentialGoals)
            {
                potentialGoal.UnregisterGoal();
            }
        }
	
        protected void Update ()
        {
            if (ActiveGoal == null)
            {
                ActiveGoal = GetMostDesirableGoal();
                return;
            }

            var deltaTime = GetDeltaTime();

            var updateResult = ActiveGoal.Update(deltaTime);

            switch (updateResult)
            {
                case EGoalStatus.Completed:
                case EGoalStatus.Failed:
                    ActiveGoal.Terminate();
                    ActiveGoal = GetMostDesirableGoal();
                    ActiveGoal.Initialise();
                    break;

                case EGoalStatus.InProgress:
                    var mostDesirableGoal = GetMostDesirableGoal();
                    if (mostDesirableGoal != null && mostDesirableGoal != ActiveGoal)
                    {
                        ActiveGoal = mostDesirableGoal;
                    }
                    break;
                default:
                    break;
            }
        }

        private Goal GetMostDesirableGoal()
        {
            Goal currentMostDesirableGoal = null;
            var currentMostDesirableGoalValue = 0.0f;

            foreach (var potentialGoal in PotentialGoals)
            {
                var potentialGoalDesirability = potentialGoal.GetDesirability();

                if (potentialGoalDesirability > currentMostDesirableGoalValue)
                {
                    currentMostDesirableGoal = potentialGoal;
                    currentMostDesirableGoalValue = potentialGoalDesirability;
                }
            }

            return currentMostDesirableGoal;
        }

        protected virtual IGoalBuilderInterface GetGoalBuilderInterface()
        {
            return new GoalBuilder(gameObject, GoalParameters);
        }

        protected virtual float GetDeltaTime()
        {
            return Time.deltaTime;
        }
    }
}

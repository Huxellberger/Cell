// Copyright (C) Threetee Gang All Rights Reserved

using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.AI.Goals
{
    public abstract class CompositeGoal 
        : Goal
    {
        private readonly IList<Goal> _subGoals;
        private int _activeGoalIndex;

        protected CompositeGoal(GameObject inOwner) : base(inOwner)
        {
            _subGoals = new List<Goal>();
            _activeGoalIndex = -1;
        }

        protected abstract void OnInitialised();
        protected abstract void OnTerminated();

        public override void Initialise()
        {
            OnInitialised();

            if (_subGoals.Count == 0)
            {
                Debug.LogError("CompositeGoal has no SubGoals to complete on initialisation!");
                return;
            }

            _activeGoalIndex = _subGoals.Count - 1;

            InitialiseNewSubgoal();
        }

        public override EGoalStatus Update(float inDeltaTime)
        {
            if (_activeGoalIndex >= 0)
            {
                var goalStatus = _subGoals[_activeGoalIndex].Update(inDeltaTime);

                switch (goalStatus)
                {
                    case EGoalStatus.Completed:
                        return OnSubGoalCompleted();
                    case EGoalStatus.Failed:
                    case EGoalStatus.InProgress:
                        return goalStatus;
                    case EGoalStatus.Inactive:
                        Debug.LogError("Active SubGoal should not be inactive!");
                        return EGoalStatus.Failed;
                    default:
                        Debug.LogError("Invalid goal status found!");
                        break;
                }
            }

            return EGoalStatus.Inactive;
        }

        public override void Terminate()
        {
            if (_activeGoalIndex >= 0)
            {
                _subGoals[_activeGoalIndex].Terminate();
            }

            _activeGoalIndex = -1;
            _subGoals.Clear();

            OnTerminated();
        }

        protected void AddSubGoal(Goal inGoal)
        {
            _subGoals.Add(inGoal);
        }

        private EGoalStatus OnSubGoalCompleted()
        {
            _subGoals[_activeGoalIndex].Terminate();
            _activeGoalIndex--;

            if (_activeGoalIndex >= 0)
            {
                InitialiseNewSubgoal();
                return EGoalStatus.InProgress;
            }

            return EGoalStatus.Completed;
        }

        private void InitialiseNewSubgoal()
        {
            _subGoals[_activeGoalIndex].Initialise();
        }
    }
}

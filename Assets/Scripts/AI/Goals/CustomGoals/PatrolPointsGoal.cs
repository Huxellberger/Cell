// Copyright (C) Threetee Gang All Rights Reserved

using Assets.Scripts.AI.Pathfinding.Patrol;
using UnityEngine;

namespace Assets.Scripts.AI.Goals.CustomGoals
{
    public class PatrolPointsGoal 
        : AtomicGoal
    {
        private readonly PatrolPointsGoalParams _params;
        private readonly IPatrolInterface _patrol;

        public PatrolPointsGoal(GameObject inOwner, PatrolPointsGoalParams inParams) 
            : base(inOwner)
        {
            _params = inParams;
            _patrol = Owner.GetComponent<IPatrolInterface>();
        }

        public override void Initialise()
        {
            if (_patrol != null)
            {
                _patrol.StartPatrolling();
            }
        }

        public override EGoalStatus Update(float inDeltaTime)
        {
            if (_patrol == null)
            {
                return EGoalStatus.Failed;
            }

            return EGoalStatus.InProgress;
        }

        public override void Terminate()
        {
            if (_patrol != null)
            {
                _patrol.StopPatrolling();
            }
        }

        public override float CalculateDesirability()
        {
            return _params.PatrolDesirability;
        }
    }
}

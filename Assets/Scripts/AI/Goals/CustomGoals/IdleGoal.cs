// Copyright (C) Threetee Gang All Rights Reserved

using UnityEngine;

namespace Assets.Scripts.AI.Goals.CustomGoals
{
    public class IdleGoal 
        : Goal
    {
        private readonly IdleGoalParams _params;
        private EGoalStatus _currentStatus;

        public IdleGoal(GameObject inOwner, IdleGoalParams inParams) : base(inOwner)
        {
            _params = inParams;
            _currentStatus = EGoalStatus.Inactive;
        }

        public override void Initialise()
        {
            _currentStatus = EGoalStatus.InProgress;
        }

        public override EGoalStatus Update(float inDeltaTime)
        {
            return _currentStatus;
        }

        public override void Terminate()
        {
            _currentStatus = EGoalStatus.Inactive;
        }

        public override float CalculateDesirability()
        {
            return _params.IdleDesirability;
        }
    }
}

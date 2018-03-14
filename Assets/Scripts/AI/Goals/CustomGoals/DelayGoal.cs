// Copyright (C) Threetee Gang All Rights Reserved

using UnityEngine;

namespace Assets.Scripts.AI.Goals.CustomGoals
{
    public class DelayGoal 
        : AtomicGoal
    {
        private EGoalStatus _status;
        private readonly float _delay;
        private float _timePassed;

        public DelayGoal(GameObject inOwner, float inDelay) 
            : base(inOwner)
        {
            _status = EGoalStatus.Inactive;

            _delay = inDelay;
            _timePassed = 0.0f;
        }

        public override void Initialise()
        {
            _timePassed = 0.0f;
            _status = EGoalStatus.InProgress;
        }

        public override EGoalStatus Update(float inDeltaTime)
        {
            if (_status == EGoalStatus.InProgress)
            {
                _timePassed += inDeltaTime;

                if (_timePassed > _delay)
                {
                    _status = EGoalStatus.Completed;
                }
            }

            return _status;
        }

        public override void Terminate()
        {
            _status = EGoalStatus.Inactive;
        }
    }
}

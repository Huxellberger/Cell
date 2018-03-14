// Copyright (C) Threetee Gang All Rights Reserved

using Assets.Scripts.AI.Pathfinding;
using UnityEngine;

namespace Assets.Scripts.AI.Goals.CustomGoals
{
    public class MoveToTargetGoal 
        : AtomicGoal
    {
        private EGoalStatus _status;

        private readonly Vector3 _targetPosition;
        private IPathfindingInterface _pathfinding;

        public MoveToTargetGoal(GameObject inOwner, Vector3 inTargetPosition) 
            : base(inOwner)
        {
            _status = EGoalStatus.Inactive;
            _targetPosition = inTargetPosition;
        }

        public override void Initialise()
        {
            _pathfinding = Owner.GetComponent<IPathfindingInterface>();

            if (_pathfinding == null)
            {
                _status = EGoalStatus.Failed;
            }
            else
            {
                _status = EGoalStatus.InProgress;
                _pathfinding.SetTargetLocation(_targetPosition, OnReachedLocation);
            }
        }

        public override EGoalStatus Update(float inDeltaTime)
        {
            return _status;
        }

        public override void Terminate()
        {
            _pathfinding = null;
            _status = EGoalStatus.Inactive;
        }

        private void OnReachedLocation()
        {
            _status = EGoalStatus.Completed;
        }
    }
}

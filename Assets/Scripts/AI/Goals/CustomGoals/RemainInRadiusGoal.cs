// Copyright (C) Threetee Gang All Rights Reserved

using Assets.Scripts.AI.Pathfinding;
using Assets.Scripts.Core;
using UnityEngine;

namespace Assets.Scripts.AI.Goals.CustomGoals
{
    public class RemainInRadiusGoal 
        : AtomicGoal
    {
        private readonly RemainInRadiusGoalParams _params;
        private readonly IPathfindingInterface _pathfinding;
        private readonly Vector3 _returnPoint;
        private readonly float _maxDistanceSquared;

        private bool _returnInProgress;
        private EGoalStatus _currentStatus;

        public RemainInRadiusGoal(GameObject inOwner, RemainInRadiusGoalParams inParams) 
            : base(inOwner)
        {
            _params = inParams;
            _pathfinding = inOwner.GetComponent<IPathfindingInterface>();
            _returnPoint = inOwner.transform.position;
            _maxDistanceSquared = _params.MaxRadius * _params.MaxRadius;

            _returnInProgress = false;
            _currentStatus = EGoalStatus.Inactive;
        }

        public override void Initialise()
        {
            _currentStatus = EGoalStatus.InProgress;
            _pathfinding.SetTargetLocation(_returnPoint, OnReturningToLocation);
            _returnInProgress = true;
        }

        public override EGoalStatus Update(float inDeltaTime)
        {
            if (!_returnInProgress && _currentStatus == EGoalStatus.InProgress)
            {
                _currentStatus = EGoalStatus.Completed;
            }

            return _currentStatus;
        }

        public override void Terminate()
        {
            _currentStatus = EGoalStatus.Inactive;
        }

        public override float CalculateDesirability()
        {
            if (WithinRadius() && !_returnInProgress)
            {
                return 0.0f;
            }
            else
            {
                return _params.LeftRadiusDesirability;
            }
        }

        private void OnReturningToLocation()
        {
            _returnInProgress = false;
        }

        private bool WithinRadius()
        {
            return VectorFunctions.DistanceSquared(_returnPoint, Owner.transform.position) < _maxDistanceSquared;
        }
    }
}

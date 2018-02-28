// Copyright (C) Threetee Gang All Rights Reserved

using Assets.Scripts.AI.Pathfinding;
using Assets.Scripts.Components.Species;
using Assets.Scripts.Core;
using Assets.Scripts.Services.Wildlife;
using UnityEngine;

namespace Assets.Scripts.AI.Goals.CustomGoals
{
    public class FollowTargetGoal 
        : AtomicGoal
    {
        private readonly FollowTargetGoalParams _params;
        private readonly float _followRadiusSquared;
        private readonly IWildlifeServiceInterface _wildlifeServiceInterface;

        private readonly ISpeciesInterface _ownerSpeciesInterface;
        private readonly IPathfindingInterface _ownerPathfindingInterface;

        private GameObject _currentTarget;
        private ISpeciesInterface _currentTargetSpeciesInterface;

        private EGoalStatus _currentStatus;

        public FollowTargetGoal(GameObject inOwner, FollowTargetGoalParams inParams, IWildlifeServiceInterface inWildlifeServiceInterface) 
            : base(inOwner)
        {
            _params = inParams;
            _followRadiusSquared = Mathf.Pow(_params.FollowRadius, 2);
            _wildlifeServiceInterface = inWildlifeServiceInterface;

            _ownerSpeciesInterface = inOwner.GetComponent<ISpeciesInterface>();
            _ownerPathfindingInterface = inOwner.GetComponent<IPathfindingInterface>();

            _currentStatus = EGoalStatus.Inactive;
        }

        public override void Initialise()
        {
            _currentStatus = EGoalStatus.InProgress;
            _ownerPathfindingInterface.SetFollowTarget(_currentTarget);
        }

        public override EGoalStatus Update(float inDeltaTime)
        {
            if (_currentStatus == EGoalStatus.InProgress)
            {
                if (_currentTarget == null ||
                    VectorFunctions.DistanceSquared(Owner.transform.position, _currentTarget.transform.position) > _params.LoseFollowRadiusSquared ||
                    _currentTargetSpeciesInterface.IsSpeciesCryInProgress(ECryType.Negative))
                {
                    _currentTarget = null;
                    _currentTargetSpeciesInterface = null;

                    _currentStatus = EGoalStatus.Failed;
                }
            }

            return _currentStatus;
        }

        public override void Terminate()
        {
            _ownerPathfindingInterface.SetFollowTarget(null);
            _currentStatus = EGoalStatus.Inactive;
        }

        public override float CalculateDesirability()
        {
            if (_currentTarget == null)
            {
                var localWildlife = _wildlifeServiceInterface.GetWildlifeInRadius(Owner.transform.position, _params.FollowRadius);

                foreach (var localAnimal in localWildlife)
                {
                    if (IsMatchingAnimalCallingYou(localAnimal.Wildlife) && localAnimal.DistanceSquared < _followRadiusSquared)
                    {
                        _currentTarget = localAnimal.WildlifeGameObject;
                        _currentTargetSpeciesInterface = localAnimal.Wildlife;
                        break;
                    }
                }
            }

            if (_currentTarget == null)
            {
                return 0.0f;
            }

            return _params.ValidTargetDesirability;
        }

        private bool IsMatchingAnimalCallingYou(ISpeciesInterface species)
        {
            return species.IsSpeciesCryInProgress(ECryType.Positive) &&
                   species.GetCurrentSpeciesType() == _ownerSpeciesInterface.GetCurrentSpeciesType();
        }
    }
}

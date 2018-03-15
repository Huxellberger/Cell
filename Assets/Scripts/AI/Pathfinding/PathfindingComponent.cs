// Copyright (C) Threetee Gang All Rights Reserved

using Assets.Scripts.Core;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Tilemaps;

namespace Assets.Scripts.AI.Pathfinding
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class PathfindingComponent 
        : MonoBehaviour
        , IPathfindingInterface
    {
        public float FollowReplotDelay = 0.5f;
        public float FollowReplotRangeSquared = 200.0f;
        public float SpacingModifier = 3.0f;
        public float TargetCompleteRadiusSquared = 100.0f;

        private NavMeshAgent _navMeshAgent;
        private GameObject _followTarget;
        private OnPathfindingCompleteDelegate _delegate;
        private float _currentFollowDelay = 0.0f;

        protected void Start ()
        {
            _navMeshAgent = gameObject.GetComponent<NavMeshAgent>();
        }
	
        protected void Update ()
        {
            if (_followTarget != null)
            {
                UpdateFollowTarget();
            }

            if (_delegate != null)
            {
                UpdatePointTarget();
            }
        }

        private void UpdateFollowTarget()
        {
            var deltaTime = GetDeltaTime();

            _currentFollowDelay += deltaTime;

            if (_currentFollowDelay > FollowReplotDelay)
            {
                _currentFollowDelay = 0.0f;

                var newLocation = GetSurroundingFollowPoint(_followTarget);
                if (VectorFunctions.DistanceSquared(_navMeshAgent.destination, newLocation) > FollowReplotRangeSquared)
                {
                    PlotCourse(newLocation);
                }
            }
        }

        private void UpdatePointTarget()
        {
            if (VectorFunctions.DistanceSquared(_navMeshAgent.destination, gameObject.transform.position) < TargetCompleteRadiusSquared)
            {
                _delegate();
                PlotCourse(gameObject.transform.position);
                _delegate = null;
            }
        }

        protected virtual float GetDeltaTime()
        {
            return Time.deltaTime;
        }

        // IPathfindingInterface
        public void SetTargetLocation(Vector3 targetLocation, OnPathfindingCompleteDelegate inDelegate)
        {
            _followTarget = null;
            inDelegate = null;
            PlotCourse(targetLocation);   
        }

        public void SetFollowTarget(GameObject inTarget)
        {
            _followTarget = inTarget;

            if (_followTarget != null)
            {
                var destination = GetSurroundingFollowPoint(_followTarget);

                PlotCourse(destination);
            }
            else
            {
                PlotCourse(gameObject.transform.position);
            }
        }
        // ~IPathfindingInterface

        private void PlotCourse(Vector3 inDestination)
        {
            _navMeshAgent.SetDestination(inDestination);
        }

        private Vector3 GetSurroundingFollowPoint(GameObject inTarget)
        {
            var centralPosition = inTarget.transform.position;
            return centralPosition - (inTarget.transform.forward * SpacingModifier);
        }
    }
}

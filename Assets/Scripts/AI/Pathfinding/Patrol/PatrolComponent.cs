// Copyright (C) Threetee Gang All Rights Reserved

using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Core;
using UnityEngine;

namespace Assets.Scripts.AI.Pathfinding.Patrol
{
    [RequireComponent(typeof(IPathfindingInterface))]
    public class PatrolComponent 
        : MonoBehaviour 
        , IPatrolInterface
    {
        public List<Vector2> PatrolPoints = new List<Vector2>();
        public float IdleTimeBetweenPoints = 5.0f;
        public Color DebugDrawColor = Color.red;

        private IPathfindingInterface _pathfinding;
        private int _currentPatrolPoint = PatrolConstants.InvalidPatrolPoint;
        private bool _patrolling = false;

        protected void Start()
        {
            _pathfinding = gameObject.GetComponent<IPathfindingInterface>();
        }

        // IPatrolInterface
        public void StartPatrolling()
        {
            _currentPatrolPoint = GetNearestStartingPatrolPoint();

            if (_currentPatrolPoint != PatrolConstants.InvalidPatrolPoint)
            {
                _pathfinding.SetTargetLocation(PatrolPoints[_currentPatrolPoint], OnPointReached);
                _patrolling = true;
            }
            else
            {
                Debug.LogError("Tried to patrol without a valid patrol point available!");
            }
        }

        public void StopPatrolling()
        {
            if (_patrolling)
            {
                _pathfinding.CancelPathfinding();
                _patrolling = false;
            }
        }
        // ~IPatrolInterface

        public int GetNearestStartingPatrolPoint()
        {
            var bestPatrolPointIndex = PatrolConstants.InvalidPatrolPoint;
            var bestDistanceSquared = float.MaxValue;

            Vector2 patrollerLocation = gameObject.transform.position;

            for (var currentIndex = 0; currentIndex < PatrolPoints.Count; currentIndex++)
            {
                var distanceSquared = VectorFunctions.DistanceSquared(PatrolPoints[currentIndex], patrollerLocation);
                if (distanceSquared < bestDistanceSquared || bestPatrolPointIndex == PatrolConstants.InvalidPatrolPoint)
                {
                    bestPatrolPointIndex = currentIndex;
                    bestDistanceSquared = distanceSquared;
                }
            }

            return bestPatrolPointIndex;
        }

        private void OnPointReached()
        {
            StartCoroutine(IdleForDelay());
        }

        private IEnumerator IdleForDelay()
        {
            yield return new WaitForSeconds(IdleTimeBetweenPoints);

            if (_patrolling)
            {
                _currentPatrolPoint = (_currentPatrolPoint + 1) % PatrolPoints.Count;
                _pathfinding.SetTargetLocation(PatrolPoints[_currentPatrolPoint], OnPointReached);
            }
        }

        private void OnDrawGizmos()
        {
            if (PatrolPoints != null && PatrolPoints.Count > 1)
            {
                Gizmos.color = DebugDrawColor;
                for (var currentNodeIndex = 0; currentNodeIndex < PatrolPoints.Count - 1; currentNodeIndex++)
                {
                    Gizmos.DrawLine(PatrolPoints[currentNodeIndex], PatrolPoints[currentNodeIndex + 1]);
                }
            }
        }
    }
}

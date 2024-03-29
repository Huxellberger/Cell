﻿// Copyright (C) Threetee Gang All Rights Reserved

using System;
using System.Collections.Generic;
using Assets.Scripts.AI.Pathfinding.Heuristic;
using Assets.Scripts.AI.Pathfinding.Nav;
using Assets.Scripts.Components.Movement;
using Assets.Scripts.Core;
using Assets.Scripts.Services;
using Assets.Scripts.Services.Navigation;
using UnityEngine;

namespace Assets.Scripts.AI.Pathfinding
{
    [RequireComponent(typeof(IMovementInterface))]
    public class TilemapPathfindingComponent 
        : MonoBehaviour 
        , IPathfindingInterface
    {
        public float DistanceSquaredThreshold = 4.0f;
        public Color DebugDrawColour = Color.cyan;

        private IMovementInterface _movement;
        private INavigationServiceInterface _navigation;
        private HeuristicEvaluator _heuristicEvaluator;

        private OnPathfindingCompleteDelegate _currentTargetDelegate;
        private Vector2 _currentTargetLocation;
        private List<NavRegion> _pathRegions = new List<NavRegion>(8);
        private readonly List<NavNode> _pathNodes = new List<NavNode>(64);

        private GameObject _followTarget;

        protected void Start()
        {
            _movement = gameObject.GetComponent<IMovementInterface>();

            _navigation = GameServiceProvider.CurrentInstance.GetService<INavigationServiceInterface>();
            _heuristicEvaluator = new HeuristicEvaluator
            (
                new List<IBestFitHeuristicInterface>
                {
                    new ClosestNodeBestFitHeuristic(),
                    new FurthestFromStartBestFitHeuristic(),
                    new LowestCostBestFitHeuristic()
                }
            );
        }

        protected void Update()
        {
            if (_followTarget != null)
            {
                UpdateFollowTarget();
            }

            if (_currentTargetDelegate != null)
            {
                UpdateNodeStatus();

                if (_pathNodes.Count == 0)
                {
                   FinishPathfinding();
                }
            }
        }

        private void FinishPathfinding()
        {
            if (_currentTargetDelegate != null)
            {
                _currentTargetDelegate();
                _currentTargetDelegate = null;
            }
        }

        private void UpdateFollowTarget()
        {
            Vector2 followPosition = _followTarget.transform.position;
            if (VectorFunctions.DistanceSquared(followPosition, _currentTargetLocation) >
                DistanceSquaredThreshold)
            {
                SetTargetLocation(_followTarget.transform.position, () =>{});
            }
        }

        private void UpdateNodeStatus()
        {
            if (_pathNodes.Count == 0)
            {
                UpdateRegionPathStatus();

                if (_pathNodes.Count == 0)
                {
                    return;
                }
            }

            var targetNode = _pathNodes[0];

            var currentPosition = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y);

            if (VectorFunctions.DistanceSquared(currentPosition, targetNode.Position) <
                DistanceSquaredThreshold)
            {
                _pathNodes.RemoveAt(0);

                if (_pathNodes.Count == 0)
                {
                    UpdateRegionPathStatus();
                }
            }
            else
            {
                var deltaX =  targetNode.Position.x - gameObject.transform.position.x;
                var deltaY =  targetNode.Position.y - gameObject.transform.position.y;

                if (deltaX > 0.2f)
                {
                    _movement.ApplySidewaysMotion(1.0f);
                }
                else if (deltaX < -0.2f)
                {
                    _movement.ApplySidewaysMotion(-1.0f);
                }


                if (deltaY > 0.2f)
                {
                    _movement.ApplyForwardMotion(1.0f);
                }
                else if (deltaY < 0.2f)
                {
                    _movement.ApplyForwardMotion(-1.0f);
                }
            }
        }

        public void SetTargetLocation(Vector3 targetLocation, OnPathfindingCompleteDelegate inDelegate)
        {
            var regions = _navigation.GetRegionPath(gameObject.transform.position, targetLocation);

            if (regions != null)
            {
                _currentTargetDelegate = inDelegate;
                _currentTargetLocation = targetLocation;

                _pathRegions = regions;

               UpdateRegionPathStatus();
            }
            else
            {
                inDelegate();
            }
        }

        private void UpdateRegionPathStatus()
        {
            if (_pathRegions.Count == 0)
            {
                return;
            }

            if (_pathRegions.Count > 1)
            {
                PlotRegionPath(_pathRegions[0], _pathRegions[1], GetNearestNavNode(_pathRegions[0], gameObject.transform.position), _currentTargetLocation);
            }
            else
            {
                PlotRegionPath(_pathRegions[0], _pathRegions[0], GetNearestNavNode(_pathRegions[0], gameObject.transform.position), _currentTargetLocation);
            }

            _pathRegions.RemoveAt(0);
        }

        public void SetFollowTarget(GameObject inTarget)
        {
            _followTarget = inTarget;

            if (_followTarget != null)
            {
                SetTargetLocation(_followTarget.transform.position, () => {});
            }
            else
            {
                FinishPathfinding();
            }
        }

        public void CancelPathfinding()
        {
            _followTarget = null;
            _pathNodes.Clear();
            _currentTargetDelegate = null;
        }

        public NavNode GetNearestNavNode(NavRegion region, Vector2 position)
        {
            NavNode nearestNode = null;
            var nearestDistance = 100000.0f;

            foreach (var node in region.Nodes)
            {
                var distance = VectorFunctions.DistanceSquared(position, node.Position);
                if (distance < nearestDistance)
                {
                    nearestNode = node;
                    nearestDistance = distance;
                }
            }

            return nearestNode;
        }

        private void PlotRegionPath(NavRegion region, NavRegion targetRegion, NavNode startingLocation, Vector2 targetLocation)
        {
            _pathNodes.Clear();

            _pathNodes.Add(startingLocation);

            var lastAddedNode = startingLocation;

            var actualTargetLocation = targetLocation;
            var closestTargetNode = GetNearestNavNode(targetRegion, targetLocation);

            if (region != targetRegion)
            {
                // Calculate nearest point in region to current point
            }

            while (region.RegionBounds.Contains(lastAddedNode.Position) && !lastAddedNode.Position.Equals(closestTargetNode.Position))
            {
                lastAddedNode = _heuristicEvaluator.GetBestNode(lastAddedNode, startingLocation, closestTargetNode, _pathNodes);

                if (lastAddedNode != null)
                {
                    _pathNodes.Add(lastAddedNode);
                }
                else
                {
                    break;
                }
            }

            SmoothPath();
        }

        private void SmoothPath()
        {
            for (var i = 0; i < _pathNodes.Count - 2; i++)
            {
                foreach (var neighbour in _pathNodes[i].NeighbourRefs)
                {
                    if (neighbour != _pathNodes[i + 1] &&
                        Math.Abs
                        (
                            VectorFunctions.DistanceSquared(neighbour.Position, _pathNodes[i + 2].Position) -
                            VectorFunctions.DistanceSquared(_pathNodes[i + 1].Position, _pathNodes[i + 2].Position)
                        ) < 0.2f
                    )
                    {
                        _pathNodes.RemoveAt(i+1);
                        break;
                    }
                }
            }
        }

        private void OnDrawGizmos()
        {
            if (_pathNodes != null && _pathNodes.Count > 1)
            {
                Gizmos.color = DebugDrawColour;
                for (var currentNodeIndex = 0; currentNodeIndex < _pathNodes.Count - 1; currentNodeIndex++)
                {
                    Gizmos.DrawLine(_pathNodes[currentNodeIndex].Position, _pathNodes[currentNodeIndex + 1].Position);
                }
            }
        }
    }
}

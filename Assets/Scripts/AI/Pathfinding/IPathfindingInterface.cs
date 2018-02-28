// Copyright (C) Threetee Gang All Rights Reserved

using UnityEngine;

namespace Assets.Scripts.AI.Pathfinding
{
    public delegate void OnPathfindingCompleteDelegate();

    public interface IPathfindingInterface
    {
        void SetTargetLocation(Vector3 targetLocation, OnPathfindingCompleteDelegate inDelegate);
        void SetFollowTarget(GameObject inTarget);
    }
}

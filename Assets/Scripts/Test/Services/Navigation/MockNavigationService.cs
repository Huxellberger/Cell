// Copyright (C) Threetee Gang All Rights Reserved

#if UNITY_EDITOR

using System.Collections.Generic;
using Assets.Scripts.AI.Pathfinding.Nav;
using Assets.Scripts.Services.Navigation;
using UnityEngine;

namespace Assets.Scripts.Test.Services.Navigation
{
    public class MockNavigationService 
        : INavigationServiceInterface 
    {
        public Vector2 ? GetRegionPathStart { get; private set; }
        public Vector2 ? GetRegionPathDestination { get; private set; }

        public List<NavRegion> GetRegionPathResult { get; set; }

        public List<NavRegion> GetRegionPath(Vector2 start, Vector2 destination)
        {
            GetRegionPathStart = start;
            GetRegionPathDestination = destination;

            return GetRegionPathResult;
        }
    }
}

#endif // UNITY_EDITOR

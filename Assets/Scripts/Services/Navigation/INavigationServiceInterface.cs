// Copyright (C) Threetee Gang All Rights Reserved

using System.Collections.Generic;
using Assets.Scripts.AI.Pathfinding.Nav;
using UnityEngine;

namespace Assets.Scripts.Services.Navigation
{
    public interface INavigationServiceInterface
    {
        List<NavRegion> GetRegionPath(Vector2 start, Vector2 destination);
    }
}

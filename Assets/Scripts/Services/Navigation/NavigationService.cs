// Copyright (C) Threetee Gang All Rights Reserved

using System.Collections.Generic;
using Assets.Scripts.AI.Pathfinding.Nav;
using UnityEngine;

namespace Assets.Scripts.Services.Navigation
{
    public class NavigationService 
        : INavigationServiceInterface
    {
        private readonly NavTable _navigationTable;

        public NavigationService(TilemapNavData navData)
        {
            NavRegionGenerationFunctions.InitialiseNavRegionsFromData(navData);

            _navigationTable = navData.NavigationTable;
            _navigationTable.Initialise();
        }

        public List<NavRegion> GetRegionPath(Vector2 start, Vector2 destination)
        {
            return _navigationTable.GetRegionRouteForPoints(start, destination);
        }
    }
}

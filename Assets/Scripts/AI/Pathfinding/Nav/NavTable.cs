// Copyright (C) Threetee Gang All Rights Reserved

using System;
using System.Collections.Generic;

namespace Assets.Scripts.AI.Pathfinding.Nav
{
    [Serializable]
    public class NavTable
    {
        public class NavRegionQueueEntry
        {
            public readonly NavRegion CurrentRegion;
            public readonly NavRegion InitialRegion;

            public NavRegionQueueEntry(NavRegion inCurrentRegion, NavRegion inInitialRegion)
            {
                CurrentRegion = inCurrentRegion;
                InitialRegion = inInitialRegion;
            }
        }

        public List<NavRegion> Regions;

        private Dictionary<NavRegion, Dictionary<NavRegion, NavRegion>> _regionReachabilityTable;

        public NavTable(List<NavRegion> inRegions)
        {
            Regions = inRegions;
        }

        public void Initialise()
        {
            _regionReachabilityTable = new Dictionary<NavRegion, Dictionary<NavRegion, NavRegion>>();

            var adjacentReachabilityMappings = new Dictionary<NavRegion, List<NavRegion>>();

            foreach (var region in Regions)
            {
                adjacentReachabilityMappings.Add(region, new List<NavRegion>());
                _regionReachabilityTable.Add(region, new Dictionary<NavRegion, NavRegion>());

                foreach (var node in region.Nodes)
                {
                    if (node.NeighbourRefs != null)
                    {
                        foreach (var neighbour in node.NeighbourRefs)
                        {
                            var owningRegion = GetOwningNavRegion(Regions, neighbour);
                            if (owningRegion != region && owningRegion != null)
                            {
                                adjacentReachabilityMappings[region].Add(owningRegion);
                            }
                        }
                    }
                }
            }

            // BFS for fastest route
            var regionAdditionQueue = new List<NavRegionQueueEntry>();
            foreach (var adjacentPairing in adjacentReachabilityMappings)
            {
                _regionReachabilityTable[adjacentPairing.Key].Add(adjacentPairing.Key, adjacentPairing.Key);

                foreach (var adjacentRegion in adjacentPairing.Value)
                {
                    regionAdditionQueue.Add(new NavRegionQueueEntry(adjacentRegion, adjacentRegion));
                }

                while (regionAdditionQueue.Count > 0)
                {
                    var currentQueueEntry = regionAdditionQueue[0];

                    if (!_regionReachabilityTable[adjacentPairing.Key].ContainsKey(currentQueueEntry.CurrentRegion))
                    {
                        _regionReachabilityTable[adjacentPairing.Key].Add(currentQueueEntry.CurrentRegion, currentQueueEntry.InitialRegion);
                    }

                    foreach (var regionNeighbour in adjacentReachabilityMappings[currentQueueEntry.CurrentRegion])
                    {
                        if (!_regionReachabilityTable[adjacentPairing.Key].ContainsKey(regionNeighbour))
                        {
                            regionAdditionQueue.Add(new NavRegionQueueEntry(regionNeighbour, currentQueueEntry.InitialRegion));
                        }
                    }

                    regionAdditionQueue.RemoveAt(0);
                }
            }
        }

        public List<NavRegion> GetRegionRouteForPoints(NavNode start, NavNode destination)
        {
            if (_regionReachabilityTable == null)
            {
                return null;
            }

            var startRegion = GetOwningNavRegion(Regions, start);
            var endRegion = GetOwningNavRegion(Regions, destination);

            if (startRegion != null && endRegion != null)
            {
                var path = new List<NavRegion>();

                path.Add(startRegion);

                var currentRegion = startRegion;

                while (currentRegion != endRegion)
                {
                    currentRegion = _regionReachabilityTable[currentRegion][endRegion];
                    if (currentRegion == null)
                    {
                        return null;
                    }

                    path.Add(currentRegion);
                }

                return path;
            }

            return null;
        }

        private static NavRegion GetOwningNavRegion(List<NavRegion> regions, NavNode inNode)
        {
            foreach (var region in regions)
            {
                if (region.RegionBounds.Contains(inNode.Position))
                {
                    return region;
                }
            }

            return null;
        }
    }
}

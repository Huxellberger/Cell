// Copyright (C) Threetee Gang All Rights Reserved

using System.Collections.Generic;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace Assets.Scripts.AI.Pathfinding.Nav
{
    public static class NavRegionGenerationFunctions 
    {
        public static List<NavRegion> GenerateNavRegionsFromNodes(List<NavNode> nodes, int regionSize)
        {
            var assignedNodes = new HashSet<NavNode>();
            var assignedRegions = new List<NavRegion>();
            var nodesToAssign = new List<NavNode>(regionSize);

            var nodesToConsiderQueue = new List<NavNode>(64) {nodes[0]};
            var nodesForNextRegion = new List<NavNode>(regionSize);

            // Account for unconnected components
            while (assignedNodes.Count < nodes.Count)
            {
                nodesToConsiderQueue.Add(FindNextUnallocatedNode(nodes, assignedNodes));

                if (nodesToConsiderQueue[0] == null)
                {
                    Debug.LogWarning("Some nodes were duplicates!");
                    break;
                }

                // BFS to give nice even growth of region
                while (nodesToConsiderQueue.Count > 0)
                {
                    var currentNode = nodesToConsiderQueue[0];

                    if (!assignedNodes.Contains(currentNode))
                    {
                        nodesToAssign.Add(currentNode);
                        assignedNodes.Add(currentNode);

                        if (currentNode.NeighbourRefs != null)
                        {
                            foreach (var neighbourRef in currentNode.NeighbourRefs)
                            {
                                if (!assignedNodes.Contains(neighbourRef))
                                {
                                    nodesToConsiderQueue.Add(neighbourRef);
                                }
                            }
                        }
                    }

                    nodesToConsiderQueue.RemoveAt(0);
                }

                // Make sure we stop overlap in regions
                while (nodesToAssign.Count >= regionSize)
                {
                    for (var currentNodeIndex = 0; currentNodeIndex < regionSize; currentNodeIndex++)
                    {
                        nodesForNextRegion.Add(nodesToAssign[currentNodeIndex]);
                    }

                    nodesToAssign.RemoveRange(0, regionSize);

                    var regionBounds = CalculateBounds(nodesForNextRegion);

                    var conflictingNodes = new List<NavNode>();

                    foreach (var otherNodes in nodesToAssign)
                    {
                        if (regionBounds.Contains(otherNodes.Position))
                        {
                            conflictingNodes.Add(otherNodes);
                            nodesForNextRegion.Add(otherNodes);
                        }
                    }

                    assignedRegions.Add(new NavRegion(nodesForNextRegion.ToArray()));

                    nodesToAssign.RemoveAll((node) => conflictingNodes.Contains(node));

                    conflictingNodes.Clear();
                    nodesForNextRegion.Clear();
                }

                // make sure regions don't span unconnected components (and allocate last, potentially unfilled region)
                if (nodesToAssign.Count > 0)
                {
                    assignedRegions.Add(new NavRegion(nodesToAssign.ToArray()));
                    nodesToAssign.Clear();
                }
            }

            return assignedRegions;
        }

        private static Rect CalculateBounds(List<NavNode> nodes)
        {
            var minPoint = nodes[0].Position;
            var maxPoint = nodes[0].Position;

            for (var currentNodeIndex = 1; currentNodeIndex < nodes.Count; currentNodeIndex++)
            {
                if (NavRegion.IsSmallestPoint(nodes[currentNodeIndex].Position, minPoint))
                {
                    minPoint = nodes[currentNodeIndex].Position;
                }
                else if (NavRegion.IsLargestPoint(nodes[currentNodeIndex].Position, maxPoint))
                {
                    maxPoint = nodes[currentNodeIndex].Position;
                }
            }

            return Rect.MinMaxRect(minPoint.x, minPoint.y, maxPoint.x, maxPoint.y);
        }

        public static void InitialiseNavRegionsFromData(TilemapNavData data)
        {
            var currentNodeNeighbours = new List<NavNode>(4);

            foreach (var region in data.NavigationTable.Regions)
            {
                foreach (var node in region.Nodes)
                {
                    if (node.Neighbours != null)
                    {
                        foreach (var neighbourIndex in node.Neighbours)
                        {
                            currentNodeNeighbours.Add(data.NodeData[neighbourIndex]);
                        }

                        node.NeighbourRefs = currentNodeNeighbours.ToArray();
                    }

                    currentNodeNeighbours.Clear();
                }
            }
        }

        private static NavNode FindNextUnallocatedNode(List<NavNode> nodes, HashSet<NavNode> allocatedNodes)
        {
            foreach (var node in nodes)
            {
                if (!allocatedNodes.Contains(node))
                {
                    return node;
                }
            }

            return null;
        }
    }
}

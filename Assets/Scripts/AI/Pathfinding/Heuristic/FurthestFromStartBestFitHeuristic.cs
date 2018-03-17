// Copyright (C) Threetee Gang All Rights Reserved

using Assets.Scripts.AI.Pathfinding.Nav;
using Assets.Scripts.Core;

namespace Assets.Scripts.AI.Pathfinding.Heuristic
{
    public class FurthestFromStartBestFitHeuristic 
        : IBestFitHeuristicInterface
    {
        public NavNode GetBestNode(NavNode currentNode, NavNode startNode, NavNode destinationNode)
        {
            if (currentNode != null && startNode != null && currentNode.NeighbourRefs != null)
            {
                var foundNewNode = false;
                var furthestFromStart = currentNode;
                var furthestDistance = VectorFunctions.DistanceSquared(currentNode.Position, startNode.Position);

                foreach (var neighbour in currentNode.NeighbourRefs)
                {
                    var neighbourDistance =
                        VectorFunctions.DistanceSquared(neighbour.Position, startNode.Position);
                    if (neighbourDistance > furthestDistance)
                    {
                        furthestDistance = neighbourDistance;
                        furthestFromStart = neighbour;
                        foundNewNode = true;
                    }
                }

                if (foundNewNode)
                {
                    return furthestFromStart;
                }
            }

            return null;
        }
    }
}

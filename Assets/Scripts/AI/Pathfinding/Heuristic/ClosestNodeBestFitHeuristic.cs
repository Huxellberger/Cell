// Copyright (C) Threetee Gang All Rights Reserved

using Assets.Scripts.AI.Pathfinding.Nav;
using Assets.Scripts.Core;

namespace Assets.Scripts.AI.Pathfinding.Heuristic
{
    public class ClosestNodeBestFitHeuristic 
        : IBestFitHeuristicInterface
    {
        public NavNode GetBestNode(NavNode currentNode, NavNode startNode, NavNode destinationNode)
        {
            if (currentNode != null && destinationNode != null && currentNode.NeighbourRefs != null)
            {
                var foundNewNode = false;
                var closestToTarget = currentNode;
                var closestDistance = VectorFunctions.DistanceSquared(currentNode.Position, destinationNode.Position);

                foreach (var neighbour in currentNode.NeighbourRefs)
                {
                    var neighbourDistance =
                        VectorFunctions.DistanceSquared(neighbour.Position, destinationNode.Position);
                    if (neighbourDistance < closestDistance)
                    {
                        closestDistance = neighbourDistance;
                        closestToTarget = neighbour;
                        foundNewNode = true;
                    }
                }

                if (foundNewNode)
                {
                    return closestToTarget;
                }
            }

            return null;
        }
    }
}

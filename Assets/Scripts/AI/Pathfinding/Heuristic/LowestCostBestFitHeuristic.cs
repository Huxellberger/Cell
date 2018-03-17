// Copyright (C) Threetee Gang All Rights Reserved

using Assets.Scripts.AI.Pathfinding.Nav;

namespace Assets.Scripts.AI.Pathfinding.Heuristic
{
    public class LowestCostBestFitHeuristic 
        : IBestFitHeuristicInterface
    {
        public NavNode GetBestNode(NavNode currentNode, NavNode startNode, NavNode destinationNode)
        {
            if (currentNode != null && currentNode.NeighbourRefs != null)
            {
                NavNode bestNode = null;

                foreach (var neighbour in currentNode.NeighbourRefs)
                {
                    if (bestNode == null || neighbour.Weight < bestNode.Weight)
                    {
                        bestNode = neighbour;
                    }
                }

                return bestNode;
            }

            return null;
        }
    }
}

// Copyright (C) Threetee Gang All Rights Reserved

using System.Collections.Generic;
using Assets.Scripts.AI.Pathfinding.Nav;

namespace Assets.Scripts.AI.Pathfinding.Heuristic
{
    public class HeuristicEvaluator
    {
        private readonly List<IBestFitHeuristicInterface> _heuristics;

        public HeuristicEvaluator(List<IBestFitHeuristicInterface> inHeuristics)
        {
            _heuristics = inHeuristics;
        }

        public NavNode GetBestNode(NavNode currentNode, NavNode startNode, NavNode destinationNode)
        {
            foreach (var heuristic in _heuristics)
            {
               var bestNode = heuristic.GetBestNode(currentNode, startNode, destinationNode);

                if (bestNode != null)
                {
                    return bestNode;
                }
            }

            return null;
        }
    }
}

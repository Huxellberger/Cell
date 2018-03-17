// Copyright (C) Threetee Gang All Rights Reserved

#if UNITY_EDITOR

using Assets.Scripts.AI.Pathfinding.Heuristic;
using Assets.Scripts.AI.Pathfinding.Nav;

namespace Assets.Editor.UnitTests.AI.Pathfinding.Heuristic
{
    public class MockBestFitHeuristic 
        : IBestFitHeuristicInterface
    {
        public NavNode GetBestNodeResult { get; set; }
        public bool GetBestNodeCalled = false;

        public NavNode GetBestNode(NavNode currentNode, NavNode startNode, NavNode destinationNode)
        {
            GetBestNodeCalled = true;
            return GetBestNodeResult;
        }
    }
}

#endif // UNITY_EDITOR

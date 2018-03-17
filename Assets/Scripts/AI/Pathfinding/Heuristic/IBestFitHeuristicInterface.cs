// Copyright (C) Threetee Gang All Rights Reserved

using Assets.Scripts.AI.Pathfinding.Nav;

namespace Assets.Scripts.AI.Pathfinding.Heuristic
{
    public interface IBestFitHeuristicInterface
    {
        NavNode GetBestNode(NavNode currentNode, NavNode startNode, NavNode destinationNode);
    }
}

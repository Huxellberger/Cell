// Copyright (C) Threetee Gang All Rights Reserved

using Assets.Scripts.AI.Pathfinding.Heuristic;
using Assets.Scripts.AI.Pathfinding.Nav;
using NUnit.Framework;
using UnityEngine;

namespace Assets.Editor.UnitTests.AI.Pathfinding.Heuristic
{
    [TestFixture]
    public class LowestCostBestFitHeuristicTestFixture 
    {
        [Test]
        public void NoCurrent_ReturnsNull() 
        {
            Assert.IsNull(new LowestCostBestFitHeuristic().GetBestNode(null, null, null));
        }

        [Test]
        public void NullCurrentNeighbours_ReturnsNull()
        {
            var currentNode = new NavNode {Position = new Vector2(1.0f, 5.0f)};
            Assert.IsNull(new LowestCostBestFitHeuristic().GetBestNode(currentNode, null, null));
        }

        [Test]
        public void NoCurrentNeighbours_ReturnsNull()
        {
            var currentNode = new NavNode { Position = new Vector2(1.0f, 5.0f), NeighbourRefs = new NavNode[0]};
            Assert.IsNull(new LowestCostBestFitHeuristic().GetBestNode(currentNode, null, null));
        }

        [Test]
        public void CurrentNeighbours_ReturnsLowestWeightedNeighbour()
        {
            var lowestWeightNode = new NavNode{Weight = 1};
            var higherWeightNode = new NavNode{Weight = lowestWeightNode.Weight + 1};

            var currentNode = new NavNode { Position = new Vector2(1.0f, 5.0f), NeighbourRefs = new []{lowestWeightNode, higherWeightNode } };
            Assert.AreSame(lowestWeightNode, new LowestCostBestFitHeuristic().GetBestNode(currentNode, null, null));
        }
    }
}

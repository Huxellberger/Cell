// Copyright (C) Threetee Gang All Rights Reserved

using Assets.Scripts.AI.Pathfinding.Heuristic;
using Assets.Scripts.AI.Pathfinding.Nav;
using NUnit.Framework;
using UnityEngine;

namespace Assets.Editor.UnitTests.AI.Pathfinding.Heuristic
{
    [TestFixture]
    public class CloestNodeBestFitHeuristicTestFixture 
    {
        [Test]
        public void NullDestination_ReturnsNull()
        {
            var currentNode = new NavNode
            {
                Position = new Vector2(10.0f, 1.0f),
                NeighbourRefs = new[]
                {
                    new NavNode
                    {
                        Position = new Vector2(12.0f, 1.0f)
                    }
                }
            };

            Assert.IsNull(new ClosestNodeBestFitHeuristic().GetBestNode(currentNode, null, null));
        }

        [Test]
        public void NullCurrent_ReturnsNull()
        {
            var destinationNode = new NavNode { Position = new Vector2(12.0f, 1.0f) };

            Assert.IsNull(new ClosestNodeBestFitHeuristic().GetBestNode(null, null, destinationNode));
        }

        [Test]
        public void NullCurrentNeighbours_ReturnsNull()
        {
            var currentNode = new NavNode
            {
                Position = new Vector2(10.0f, 1.0f)
            };

            var destinationNode = new NavNode { Position = new Vector2(12.0f, 1.0f) };

            Assert.IsNull(new ClosestNodeBestFitHeuristic().GetBestNode(currentNode, null, destinationNode));
        }

        [Test]
        public void CurrentNeighboursFurtherThanCurrent_ReturnsNull()
        {
            var currentNode = new NavNode
            {
                Position = new Vector2(12.0f, 0.0f),
                NeighbourRefs = new[]
                {
                    new NavNode
                    {
                        Position = new Vector2(22.0f, 1.0f)
                    },
                    new NavNode
                    {
                        Position = new Vector2(32.0f, 1.0f)
                    }
                }
            };

            var destinationNode = new NavNode { Position = new Vector2(12.0f, 1.0f) };

            Assert.IsNull(new ClosestNodeBestFitHeuristic().GetBestNode(currentNode, null, destinationNode));
        }

        [Test]
        public void CurrentNeighboursCloserThanCurrent_ReturnsCloestNeighbourToDestination()
        {
            var closestNeighbour = new NavNode { Position = new Vector2(12.0f, 2.0f) };

            var currentNode = new NavNode
            {
                Position = new Vector2(22.0f, 0.0f),
                NeighbourRefs = new[]
                {
                    closestNeighbour,
                    new NavNode
                    {
                        Position = new Vector2(14.0f, 1.0f)
                    }
                }
            };

            var destinationNode = new NavNode { Position = new Vector2(12.0f, 1.0f) };

            Assert.AreSame(closestNeighbour, new ClosestNodeBestFitHeuristic().GetBestNode(currentNode, null, destinationNode));
        }
    }
}

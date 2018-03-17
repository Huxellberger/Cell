// Copyright (C) Threetee Gang All Rights Reserved

using Assets.Scripts.AI.Pathfinding.Heuristic;
using Assets.Scripts.AI.Pathfinding.Nav;
using NUnit.Framework;
using UnityEngine;

namespace Assets.Editor.UnitTests.AI.Pathfinding.Heuristic
{
    [TestFixture]
    public class FurthestFromStartBestFitHeuristicTestFixture 
    {
        [Test]
        public void NullStart_ReturnsNull()
        {
            var currentNode = new NavNode
            {
                Position = new Vector2(10.0f, 1.0f),
                NeighbourRefs = new []
                {
                    new NavNode
                    {
                        Position = new Vector2(12.0f, 1.0f)
                    }
                }
            };

            Assert.IsNull(new FurthestFromStartBestFitHeuristic().GetBestNode(currentNode, null, null));
        }

        [Test]
        public void NullCurrent_ReturnsNull()
        {
            var startNode = new NavNode{Position = new Vector2(12.0f, 1.0f)};

            Assert.IsNull(new FurthestFromStartBestFitHeuristic().GetBestNode(null, startNode, null));
        }

        [Test]
        public void NullCurrentNeighbours_ReturnsNull()
        {
            var currentNode = new NavNode
            {
                Position = new Vector2(10.0f, 1.0f)
            };

            var startNode = new NavNode { Position = new Vector2(12.0f, 1.0f) };

            Assert.IsNull(new FurthestFromStartBestFitHeuristic().GetBestNode(currentNode, startNode, null));
        }

        [Test]
        public void CurrentNeighboursCloserThanCurrent_ReturnsNull()
        {
            var currentNode = new NavNode
            {
                Position = new Vector2(42.0f, 0.0f),
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

            var startNode = new NavNode { Position = new Vector2(12.0f, 1.0f) };

            Assert.IsNull(new FurthestFromStartBestFitHeuristic().GetBestNode(currentNode, startNode, null));
        }

        [Test]
        public void CurrentNeighboursFurtherThanCurrent_ReturnsFurthestNeighbourFromStart()
        {
            var furthestNeighbour = new NavNode{Position = new Vector2(52.0f, 2.0f)};

            var currentNode = new NavNode
            {
                Position = new Vector2(22.0f, 0.0f),
                NeighbourRefs = new[]
                {
                    furthestNeighbour,
                    new NavNode
                    {
                        Position = new Vector2(32.0f, 1.0f)
                    }
                }
            };

            var startNode = new NavNode { Position = new Vector2(12.0f, 1.0f) };

            Assert.AreSame(furthestNeighbour, new FurthestFromStartBestFitHeuristic().GetBestNode(currentNode, startNode, null));
        }
    }
}

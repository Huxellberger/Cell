// Copyright (C) Threetee Gang All Rights Reserved

using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.AI.Pathfinding.Nav;
using NUnit.Framework;

namespace Assets.Editor.UnitTests.AI.Pathfinding.Nav
{
    [TestFixture]
    public class NavRegionGenerationFunctionsTestFixture 
    {
        
        [Test]
        public void GenerateNavRegionsFromNodes_DoesNotCreateRegionsAcrossUnconnectedComponents()
        {
            const int neighbourCount = 4;
            const int singleNodes = 30;
            const int expectedRegionCount = 5;

            var nodesToAllocate = new List<NavNode>();
            var neighbourNodes = new List<NavNode>(neighbourCount);

            for (int j = 0; j < expectedRegionCount; j++)
            {
                for (int i = 0; i < singleNodes; i++)
                {
                    var newNode = new NavNode();
                    
                    for (int k = 0; k < neighbourCount - 1; k++)
                    {
                        var newNeighbour = new NavNode();
                        neighbourNodes.Add(newNeighbour);
                    }

                    newNode.NeighbourRefs = neighbourNodes.ToArray();

                    nodesToAllocate.Add(newNode);

                    foreach (var neighbourNode in neighbourNodes)
                    {
                        nodesToAllocate.Add(neighbourNode);
                    }

                    neighbourNodes.Clear();
                }
            }

            Assert.AreEqual(expectedRegionCount * singleNodes, NavRegionGenerationFunctions.GenerateNavRegionsFromNodes(nodesToAllocate, singleNodes * neighbourCount).Count);
        }

        [Test]
        public void GenerateNavRegionsFromNodes_CreatesExpectedNumberOfRegions()
        {
            const int neighbourCount = 4;
            const int singleNodes = 30;
            const int expectedRegionCount = 5;

            var nodesToAllocate = new List<NavNode>();
            var neighbourNodes = new List<NavNode>(neighbourCount);
            NavNode priorNode = null;

            for (int j = 0; j < expectedRegionCount; j++)
            {
                for (int i = 0; i < singleNodes; i++)
                {
                    var newNode = new NavNode();

                    for (int k = 0; k < neighbourCount - 1; k++)
                    {
                        var newNeighbour = new NavNode();
                        neighbourNodes.Add(newNeighbour);
                    }

                    if (priorNode != null)
                    {
                        var updatedPriorNeighbours = priorNode.NeighbourRefs.ToList();
                        updatedPriorNeighbours.Add(newNode);

                        priorNode.NeighbourRefs = updatedPriorNeighbours.ToArray();
                        neighbourNodes.Add(priorNode);
                    }

                    newNode.NeighbourRefs = neighbourNodes.ToArray();

                    nodesToAllocate.Add(newNode);

                    foreach (var neighbourNode in neighbourNodes)
                    {
                        nodesToAllocate.Add(neighbourNode);
                    }

                    neighbourNodes.Clear();

                    priorNode = newNode;
                }
            }

            Assert.AreEqual(expectedRegionCount, NavRegionGenerationFunctions.GenerateNavRegionsFromNodes(nodesToAllocate, singleNodes * neighbourCount).Count);
        }

        [Test]
        public void GenerateNavRegionsFromNodes_DoesNotConsiderDuplicates()
        {
            const int singleNodes = 30;
            const int expectedRegionCount = 5;

            var nodesToAllocate = new List<NavNode>();

            var newNode = new NavNode();

            for (int j = 0; j < expectedRegionCount; j++)
            {
                for (int i = 0; i < singleNodes; i++)
                {
                    nodesToAllocate.Add(newNode);
                }
            }

            Assert.AreEqual(1, NavRegionGenerationFunctions.GenerateNavRegionsFromNodes(nodesToAllocate, singleNodes).Count);
        }
    }
}

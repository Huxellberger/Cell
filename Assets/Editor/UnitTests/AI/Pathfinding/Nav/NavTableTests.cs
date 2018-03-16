// Copyright (C) Threetee Gang All Rights Reserved

using System.Collections.Generic;
using Assets.Scripts.AI.Pathfinding.Nav;
using NUnit.Framework;
using UnityEngine;

namespace Assets.Editor.UnitTests.AI.Pathfinding.Nav
{
    [TestFixture]
    public class NavTableTestFixture
    {
        private NavRegion _regionA;
        private NavRegion _regionB;
        private NavRegion _regionC;

        private NavNode _nodeAMin;
        private NavNode _nodeAMax;
        private NavNode _nodeBMin;
        private NavNode _nodeBMax;
        private NavNode _nodeCMin;
        private NavNode _nodeCMax;

        [SetUp]
        public void BeforeTest()
        {
		    _nodeAMin = new NavNode{Position = new Vector2(-1.0f, -1.0f)};
            _nodeAMax = new NavNode{Position = new Vector2(0.0f, 0.0f)};

            _nodeBMin = new NavNode{Position = new Vector2(1.0f, 1.0f)};
            _nodeBMax = new NavNode{Position =  new Vector2(2.0f, 2.0f)};

            _nodeCMin = new NavNode{Position = new Vector2(3.0f, 3.0f)};
            _nodeCMax = new NavNode{Position = new Vector2(4.0f, 4.0f)};

            _regionA = new NavRegion(new []{_nodeAMin, _nodeAMax});
            _regionB = new NavRegion(new []{_nodeBMin, _nodeBMax});
            _regionC = new NavRegion(new []{_nodeCMin, _nodeCMax});
        }
	
        [TearDown]
        public void AfterTest()
        {
            _regionC = null;
            _regionB = null;
            _regionA = null;

            _nodeCMax = null;
            _nodeCMin = null;
            _nodeBMax = null;
            _nodeBMin = null;
            _nodeAMax = null;
            _nodeAMin = null;
        }
	
        [Test]
        public void NavTable_NotInitialised_NullLookup()
        {
            _nodeAMin.NeighbourRefs = new[] {_nodeBMin};

            var table = new NavTable(new List<NavRegion>{_regionA, _regionB});

            Assert.IsNull(table.GetRegionRouteForPoints(_nodeAMin.Position, _nodeBMin.Position));
        }

        [Test]
        public void NavTable_Initialised_CorrectAdjacencyLookup()
        {
            _nodeAMin.NeighbourRefs = new[] { _nodeBMin };

            var table = new NavTable(new List<NavRegion> { _regionA, _regionB });
            table.Initialise();

            var result = table.GetRegionRouteForPoints(_nodeAMin.Position, _nodeBMin.Position);
            Assert.AreEqual(2, result.Count);
            Assert.AreSame(_regionA, result[0]);
            Assert.AreSame(_regionB, result[1]);
        }

        [Test]
        public void NavTable_Initialised_CorrectLookupForSameRegionTraversal()
        {
            var table = new NavTable(new List<NavRegion> { _regionA, _regionB });
            table.Initialise();

            var result = table.GetRegionRouteForPoints(_nodeAMin.Position, _nodeAMax.Position);
            Assert.AreEqual(1, result.Count);
            Assert.AreSame(_regionA, result[0]);
        }

        [Test]
        public void NavTable_Initialised_CorrectlyNestedLookup()
        {
            _nodeAMin.NeighbourRefs = new[] { _nodeBMin };
            _nodeBMin.NeighbourRefs = new[] { _nodeCMin };

            var table = new NavTable(new List<NavRegion> { _regionA, _regionB, _regionC });
            table.Initialise();

            var result = table.GetRegionRouteForPoints(_nodeAMin.Position, _nodeCMin.Position);
            Assert.AreEqual(3, result.Count);
            Assert.AreSame(_regionA, result[0]);
            Assert.AreSame(_regionB, result[1]);
            Assert.AreSame(_regionC, result[2]);
        }

        [Test]
        public void NavTable_Initialised_FastestRoute()
        {
            _nodeAMin.NeighbourRefs = new[] { _nodeCMin, _nodeBMin };
            _nodeCMin.NeighbourRefs = new[] { _nodeBMin };
            _nodeBMin.NeighbourRefs = new[] { _nodeCMin };

            var table = new NavTable(new List<NavRegion> { _regionA, _regionB, _regionC });
            table.Initialise();

            var result = table.GetRegionRouteForPoints(_nodeAMin.Position, _nodeBMin.Position);
            Assert.AreEqual(2, result.Count);
            Assert.AreSame(_regionA, result[0]);
            Assert.AreSame(_regionB, result[1]);
        }

        [Test]
        public void NavTable_Initialised_NullForBadLookup()
        {
            _nodeAMin.NeighbourRefs = new[] { _nodeBMin };

            var table = new NavTable(new List<NavRegion> { _regionA, _regionB });
            table.Initialise();

            var result = table.GetRegionRouteForPoints(_nodeAMin.Position, _nodeBMin.Position);
            Assert.IsNull(table.GetRegionRouteForPoints(_nodeAMin.Position, _nodeCMin.Position));
        }
    }
}

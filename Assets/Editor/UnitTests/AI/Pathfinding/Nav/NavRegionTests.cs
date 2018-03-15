// Copyright (C) Threetee Gang All Rights Reserved

using System.Collections.Generic;
using Assets.Scripts.AI.Pathfinding.Nav;
using NUnit.Framework;
using UnityEngine;

namespace Assets.Editor.UnitTests.AI.Pathfinding.Nav
{
    [TestFixture]
    public class NavRegionTestFixture
    {
        [Test]
        public void ConstructedWithPoints_GeneratesCorrectBounds()
        {
            var expectedMin = new Vector2(-10.0f, -12.0f);
            var expectedMax = new Vector2(100.0f, 110.0f);

            var points = new List<NavNode>
            {
                new NavNode {Position = new Vector2(0.0f, 0.0f)},
                new NavNode {Position = new Vector2(1.0f, 2.0f)},
                new NavNode {Position = expectedMax},
                new NavNode {Position = new Vector2(100.0f, 101.0f)},
                new NavNode {Position = expectedMin}
            };

            var region = new NavRegion(points.ToArray());

            Assert.AreEqual(expectedMin, region.RegionBounds.min);
            Assert.AreEqual(expectedMax, region.RegionBounds.max);
        }
    }
}

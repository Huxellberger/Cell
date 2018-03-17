// Copyright (C) Threetee Gang All Rights Reserved

using System.Collections.Generic;
using Assets.Scripts.AI.Pathfinding.Heuristic;
using Assets.Scripts.AI.Pathfinding.Nav;
using NUnit.Framework;

namespace Assets.Editor.UnitTests.AI.Pathfinding.Heuristic
{
    [TestFixture]
    public class HeuristicEvaluatorTestFixture 
    {
        [Test]
        public void NoHeuristics_ReturnsNull()
        {
            Assert.IsNull(new HeuristicEvaluator(new List<IBestFitHeuristicInterface>()).GetBestNode(null, null, null));
        }

        [Test]
        public void NoHeuristicsFindBestNode_ReturnsNull() 
        {
            Assert.IsNull(new HeuristicEvaluator(new List<IBestFitHeuristicInterface>{ new MockBestFitHeuristic(), new MockBestFitHeuristic()}).GetBestNode(null, null, null));
        }

        [Test]
        public void NoHeuristicsFindBestNode_CallsAllHeuristics()
        {
            var firstHeuristic = new MockBestFitHeuristic();
            var secondHeuristic = new MockBestFitHeuristic();

            new HeuristicEvaluator(new List<IBestFitHeuristicInterface> { firstHeuristic, secondHeuristic }).GetBestNode(null, null, null);

            Assert.IsTrue(firstHeuristic.GetBestNodeCalled);
            Assert.IsTrue(secondHeuristic.GetBestNodeCalled);
        }

        [Test]
        public void HeuristicsFindBestNode_StopsCallingWhenFindsBestNode()
        {
            var firstHeuristic = new MockBestFitHeuristic();
            var secondHeuristic = new MockBestFitHeuristic{ GetBestNodeResult = new NavNode()};
            var thirdHeuristic = new MockBestFitHeuristic();

            new HeuristicEvaluator(new List<IBestFitHeuristicInterface> { firstHeuristic, secondHeuristic, thirdHeuristic }).GetBestNode(null, null, null);

            Assert.IsTrue(firstHeuristic.GetBestNodeCalled);
            Assert.IsTrue(secondHeuristic.GetBestNodeCalled);
            Assert.IsFalse(thirdHeuristic.GetBestNodeCalled);
        }
    }
}

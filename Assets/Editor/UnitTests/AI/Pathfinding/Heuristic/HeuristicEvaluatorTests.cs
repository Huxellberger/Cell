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
            Assert.IsNull(new HeuristicEvaluator(new List<IBestFitHeuristicInterface>()).GetBestNode(null, null, null, new List<NavNode>()));
        }

        [Test]
        public void NoHeuristicsFindBestNode_ReturnsNull() 
        {
            Assert.IsNull(new HeuristicEvaluator(new List<IBestFitHeuristicInterface>{ new MockBestFitHeuristic(), new MockBestFitHeuristic()}).GetBestNode(null, null, null, new List<NavNode>()));
        }

        [Test]
        public void NoPriorNodesFindBestNode_ReturnsNull()
        {
            Assert.IsNull(new HeuristicEvaluator(new List<IBestFitHeuristicInterface> { new MockBestFitHeuristic{GetBestNodeResult = new NavNode()}, new MockBestFitHeuristic() }).GetBestNode(null, null, null, null));
        }

        [Test]
        public void NoHeuristicsFindBestNode_CallsAllHeuristics()
        {
            var firstHeuristic = new MockBestFitHeuristic();
            var secondHeuristic = new MockBestFitHeuristic();

            new HeuristicEvaluator(new List<IBestFitHeuristicInterface> { firstHeuristic, secondHeuristic }).GetBestNode(null, null, null, new List<NavNode>());

            Assert.IsTrue(firstHeuristic.GetBestNodeCalled);
            Assert.IsTrue(secondHeuristic.GetBestNodeCalled);
        }

        [Test]
        public void HeuristicsFindBestNode_StopsCallingWhenFindsBestNode()
        {
            var firstHeuristic = new MockBestFitHeuristic();
            var secondHeuristic = new MockBestFitHeuristic{ GetBestNodeResult = new NavNode()};
            var thirdHeuristic = new MockBestFitHeuristic();

            new HeuristicEvaluator(new List<IBestFitHeuristicInterface> { firstHeuristic, secondHeuristic, thirdHeuristic }).GetBestNode(null, null, null, new List<NavNode>());

            Assert.IsTrue(firstHeuristic.GetBestNodeCalled);
            Assert.IsTrue(secondHeuristic.GetBestNodeCalled);
            Assert.IsFalse(thirdHeuristic.GetBestNodeCalled);
        }

        [Test]
        public void HeuristicsFindBestNode_ReturnsExpectedNode()
        {
            var firstHeuristic = new MockBestFitHeuristic();
            var secondHeuristic = new MockBestFitHeuristic { GetBestNodeResult = new NavNode() };
            var thirdHeuristic = new MockBestFitHeuristic();

            Assert.AreSame
            (
                secondHeuristic.GetBestNodeResult, 
                new HeuristicEvaluator
                (
                    new List<IBestFitHeuristicInterface> { firstHeuristic, secondHeuristic, thirdHeuristic }).
                    GetBestNode(null, null, null, new List<NavNode>()
                 )
             );
        }

        [Test]
        public void HeuristicsFindBestNode_StopsCallingWhenFindsBestNodeNotAlreadyFound()
        {
            var firstHeuristic = new MockBestFitHeuristic();
            var secondHeuristic = new MockBestFitHeuristic { GetBestNodeResult = new NavNode() };
            var thirdHeuristic = new MockBestFitHeuristic{GetBestNodeResult = new NavNode()};

            new HeuristicEvaluator(new List<IBestFitHeuristicInterface> { firstHeuristic, secondHeuristic, thirdHeuristic }).GetBestNode(null, null, null, new List<NavNode>{secondHeuristic.GetBestNodeResult});

            Assert.IsTrue(firstHeuristic.GetBestNodeCalled);
            Assert.IsTrue(secondHeuristic.GetBestNodeCalled);
            Assert.IsTrue(thirdHeuristic.GetBestNodeCalled);
        }

        [Test]
        public void HeuristicsFindBestNode_ReturnsFirstUniqueNode()
        {
            var firstHeuristic = new MockBestFitHeuristic();
            var secondHeuristic = new MockBestFitHeuristic { GetBestNodeResult = new NavNode() };
            var thirdHeuristic = new MockBestFitHeuristic { GetBestNodeResult = new NavNode() };

            Assert.AreSame
            (
                thirdHeuristic.GetBestNodeResult,
                new HeuristicEvaluator
                (
                    new List<IBestFitHeuristicInterface> { firstHeuristic, secondHeuristic, thirdHeuristic }).
                    GetBestNode(null, null, null, new List<NavNode> { secondHeuristic.GetBestNodeResult}
                )
            );
        }
    }
}

// Copyright (C) Threetee Gang All Rights Reserved 

using Assets.Scripts.Test.AI.Goals;
using NUnit.Framework;
using UnityEngine;

namespace Assets.Editor.UnitTests.AI.Goals
{
    [TestFixture]
    public class GoalTestFixture
    {
        [Test]
        public void GoalCreated_OwnerIsPassedGameObject()
        {
            var owner = new GameObject();

            var goal = new TestGoal(owner);

            Assert.AreSame(owner, goal.GetOwner());
        }

        [Test]
        public void GetDesirability_ReturnsZeroByDefault()
        {
            var owner = new GameObject();

            var goal = new TestGoal(owner);

            Assert.AreEqual(0.0f, goal.GetDesirability());
        }

        [Test]
        public void GetDesirability_ClampedBelowOne()
        {
            var owner = new GameObject();

            var goal = new TestGoal(owner)
            {
                OverrideDesirabilityFunction = true,
                CalculateDesirabilityOverride = 1.1f
            };

            Assert.AreEqual(1.0f, goal.GetDesirability());
        }

        [Test]
        public void GetDesirability_ClampedAboveZero()
        {
            var owner = new GameObject();

            var goal = new TestGoal(owner)
            {
                OverrideDesirabilityFunction = true,
                CalculateDesirabilityOverride = -0.1f
            };

            Assert.AreEqual(0.0f, goal.GetDesirability());
        }
    }
}

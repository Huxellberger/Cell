// Copyright (C) Threetee Gang All Rights Reserved

using Assets.Scripts.AI.Goals;
using Assets.Scripts.AI.Goals.CustomGoals;
using NUnit.Framework;
using UnityEngine;

namespace Assets.Editor.UnitTests.AI.Goals.CustomGoals
{
    [TestFixture]
    public class IdleGoalTestFixture
    {
        private IdleGoal _goal;
        private IdleGoalParams _params;

        [SetUp]
        public void BeforeTest()
        {
            _params = new IdleGoalParams{ IdleDesirability = 0.3f };
            _goal = new IdleGoal(new GameObject(), _params);
        }

        [TearDown]
        public void AfterTest()
        {
            _goal = null;
            _params = null;
        }

        [Test]
        public void Update_NotInitialised_StatusIsInactive()
        {
            Assert.AreEqual(EGoalStatus.Inactive, _goal.Update(1.0f));
        }

        [Test]
        public void Update_Initialised_StatusIsInProgress()
        {
            _goal.Initialise();
            Assert.AreEqual(EGoalStatus.InProgress, _goal.Update(1.0f));
        }

        [Test]
        public void Update_Terminated_StatusIsInactive()
        {
            _goal.Initialise();
            _goal.Terminate();
            Assert.AreEqual(EGoalStatus.Inactive, _goal.Update(1.0f));
        }

        [Test]
        public void CalculateDesirability_ReturnsParamValue()
        {
            Assert.AreEqual(_params.IdleDesirability, _goal.CalculateDesirability());
        }
    }
}

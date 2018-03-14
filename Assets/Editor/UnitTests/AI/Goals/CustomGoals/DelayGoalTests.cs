// Copyright (C) Threetee Gang All Rights Reserved

using Assets.Scripts.AI.Goals;
using Assets.Scripts.AI.Goals.CustomGoals;
using NUnit.Framework;

namespace Assets.Editor.UnitTests.AI.Goals.CustomGoals
{
    [TestFixture]
    public class DelayGoalTestFixture 
    {
        [Test]
        public void Update_Created_Inactive() 
        {
            Assert.AreEqual(EGoalStatus.Inactive, new DelayGoal(null, 1.0f).Update(2.0f));
        }

        [Test]
        public void Update_Initialised_InProgress()
        {
            var goal = new DelayGoal(null, 1.0f);
            goal.Initialise();
            Assert.AreEqual(EGoalStatus.InProgress, goal.Update(0.0f));
        }

        [Test]
        public void Update_InitialisedAndDelayPassed_Completed()
        {
            var goal = new DelayGoal(null, 1.0f);
            goal.Initialise();
            Assert.AreEqual(EGoalStatus.InProgress, goal.Update(0.0f));
        }

        [Test]
        public void Update_Terminated_Inactive()
        {
            var goal = new DelayGoal(null, 1.0f);
            goal.Initialise();
            goal.Terminate();
            Assert.AreEqual(EGoalStatus.Inactive, goal.Update(0.0f));
        }
    }
}

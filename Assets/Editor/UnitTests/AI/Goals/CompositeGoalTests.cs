// Copyright (C) Threetee Gang All Rights Reserved 

using Assets.Scripts.AI.Goals;
using Assets.Scripts.Test.AI.Goals;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Assets.Editor.UnitTests.AI.Goals
{
    [TestFixture]
    public class CompositeGoalTestFixture
    {
        private TestGoal _goal;
        private TestGoal _otherGoal;

        private TestCompositeGoal _compositeGoal;
        private GameObject _owner;

        [SetUp]
        public void BeforeTest()
        {
            _owner = new GameObject();

            _goal = new TestGoal(_owner);
            _otherGoal = new TestGoal(_owner);

            _compositeGoal = new TestCompositeGoal(_owner);
        }

        [TearDown]
        public void AfterTest()
        {
            _owner = null;

            _compositeGoal = null;

            _goal = null;
            _otherGoal = null;
        }

        [Test]
        public void Initialise_NoSubGoals_Errors()
        {
            LogAssert.Expect(LogType.Error, "CompositeGoal has no SubGoals to complete on initialisation!");

            _compositeGoal.Initialise();
        }

        [Test]
        public void Initialise_CallsInitialiseOnLastSubgoal()
        {
            _compositeGoal.TestAddSubGoal(_goal);
            _compositeGoal.TestAddSubGoal(_otherGoal);

            _compositeGoal.Initialise();

            Assert.IsFalse(_goal.Initialised);
            Assert.IsTrue(_otherGoal.Initialised);
        }

        [Test]
        public void Initialise_EmptiesSubGoalsOnTermination()
        {
            _compositeGoal.TestAddSubGoal(_goal);
            _compositeGoal.TestAddSubGoal(_otherGoal);

            _compositeGoal.Initialise();

            _otherGoal.UpdateResult = EGoalStatus.Completed;

            _compositeGoal.Update(1.0f);
            _compositeGoal.Terminate();

            LogAssert.Expect(LogType.Error, "CompositeGoal has no SubGoals to complete on initialisation!");
            _compositeGoal.Initialise();
        }

        [Test]
        public void Update_CallsUpdateOnLastSubGoal()
        {
            _compositeGoal.TestAddSubGoal(_goal);
            _compositeGoal.TestAddSubGoal(_otherGoal);

            _compositeGoal.Initialise();
            _compositeGoal.Update(1.0f);

            Assert.IsFalse(_goal.Updated);
            Assert.IsTrue(_otherGoal.Updated);
        }

        [Test]
        public void Update_Failed_DoesNotTerminateSubGoalAndReturnsFailure()
        {
            _compositeGoal.TestAddSubGoal(_goal);
            _compositeGoal.Initialise();

            _goal.UpdateResult = EGoalStatus.Failed;

            Assert.AreEqual(EGoalStatus.Failed, _compositeGoal.Update(0.0f));

            Assert.IsFalse(_goal.Terminated);
        }

        [Test]
        public void Update_Failed_RemainsFailingAfterFailure()
        {
            _compositeGoal.TestAddSubGoal(_goal);
            _compositeGoal.Initialise();

            _goal.UpdateResult = EGoalStatus.Failed;
            _compositeGoal.Update(0.0f);

            Assert.AreEqual(EGoalStatus.Failed, _compositeGoal.Update(1.0f));
        }

        [Test]
        public void Update_InProgress_ReturnsInProgress()
        {
            _compositeGoal.TestAddSubGoal(_goal);
            _compositeGoal.Initialise();

            _goal.UpdateResult = EGoalStatus.InProgress;
            Assert.AreEqual(EGoalStatus.InProgress, _compositeGoal.Update(1.0f));
        }

        [Test]
        public void Update_Inactive_ErrorsAndReturnsFailed()
        {
            LogAssert.Expect(LogType.Error, "Active SubGoal should not be inactive!");

            _compositeGoal.TestAddSubGoal(_goal);
            _compositeGoal.Initialise();

            _goal.UpdateResult = EGoalStatus.Inactive;
            Assert.AreEqual(EGoalStatus.Failed, _compositeGoal.Update(1.0f));
        }

        [Test]
        public void Update_Completed_ReturnsInProgressIfGoalsRemaining()
        {
            _compositeGoal.TestAddSubGoal(_goal);
            _compositeGoal.TestAddSubGoal(_otherGoal);
            _compositeGoal.Initialise();

            _otherGoal.UpdateResult = EGoalStatus.Completed;
            Assert.AreEqual(EGoalStatus.InProgress, _compositeGoal.Update(1.0f));
        }

        [Test]
        public void Update_Completed_TerminatesOldGoalAndInitialisesNewGoal()
        {
            _compositeGoal.TestAddSubGoal(_goal);
            _compositeGoal.TestAddSubGoal(_otherGoal);
            _compositeGoal.Initialise();

            _otherGoal.UpdateResult = EGoalStatus.Completed;
            _compositeGoal.Update(1.0f);
            
            Assert.IsTrue(_otherGoal.Terminated);
            Assert.IsTrue(_goal.Initialised);
        }

        [Test]
        public void Update_Completed_UpdatesNextGoal()
        {
            _compositeGoal.TestAddSubGoal(_goal);
            _compositeGoal.TestAddSubGoal(_otherGoal);
            _compositeGoal.Initialise();

            _otherGoal.UpdateResult = EGoalStatus.Completed;
            _compositeGoal.Update(1.0f);
            _compositeGoal.Update(1.0f);

            Assert.IsTrue(_goal.Updated);
        }

        [Test]
        public void Update_Completed_ReturnsNextGoalStatusOnUpdate()
        {
            _compositeGoal.TestAddSubGoal(_goal);
            _compositeGoal.TestAddSubGoal(_otherGoal);
            _compositeGoal.Initialise();

            _otherGoal.UpdateResult = EGoalStatus.Completed;
            _compositeGoal.Update(1.0f);

            Assert.AreEqual(EGoalStatus.InProgress, _compositeGoal.Update(1.0f));
        }

        [Test]
        public void Update_AllGoalsCompleted_ReturnsCompleted()
        {
            _compositeGoal.TestAddSubGoal(_goal);
            _compositeGoal.TestAddSubGoal(_otherGoal);
            _compositeGoal.Initialise();

            _otherGoal.UpdateResult = EGoalStatus.Completed;
            _compositeGoal.Update(1.0f);

            _goal.UpdateResult = EGoalStatus.Completed;
            Assert.AreEqual(EGoalStatus.Completed, _compositeGoal.Update(1.0f));
        }

        [Test]
        public void Update_AllGoalsCompleted_InactiveAfterReturnsCompleted()
        {
            _compositeGoal.TestAddSubGoal(_goal);
            _compositeGoal.TestAddSubGoal(_otherGoal);
            _compositeGoal.Initialise();

            _otherGoal.UpdateResult = EGoalStatus.Completed;
            _compositeGoal.Update(1.0f);

            _goal.UpdateResult = EGoalStatus.Completed;
            _compositeGoal.Update(1.0f);
            Assert.AreEqual(EGoalStatus.Inactive, _compositeGoal.Update(1.0f));
        }

        [Test]
        public void Terminate_TerminatesActiveGoal()
        {
            _compositeGoal.TestAddSubGoal(_goal);
            _compositeGoal.TestAddSubGoal(_otherGoal);
            _compositeGoal.Initialise();

            _compositeGoal.Terminate();

            Assert.IsTrue(_otherGoal.Terminated);
            Assert.IsFalse(_goal.Terminated);
        }
    }
}

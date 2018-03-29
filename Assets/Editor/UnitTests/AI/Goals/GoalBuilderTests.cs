// Copyright (C) Threetee Gang All Rights Reserved

using Assets.Scripts.AI.Goals;
using Assets.Scripts.AI.Goals.CustomGoals;
using Assets.Scripts.Services;
using Assets.Scripts.Services.Wildlife;
using Assets.Scripts.Test.Services;
using Assets.Scripts.Test.Services.Wildlife;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Assets.Editor.UnitTests.AI.Goals
{
    [TestFixture]
    public class GoalBuilderTestFixture
    {
        private GoalBuilder _goalBuilder;
        private GameObject _owner;

        [SetUp]
        public void BeforeTest()
        {
            new GameObject().AddComponent<TestGameServiceProvider>().TestAwake();
            GameServiceProvider.CurrentInstance.AddService<IWildlifeServiceInterface>(new MockWildlifeService());

            _owner = new GameObject();
            var goalParams = new GoalParams();

            goalParams.FollowTargetGoalParameters = new FollowTargetGoalParams();
            goalParams.IdleGoalParameters = new IdleGoalParams();

            _goalBuilder = new GoalBuilder(_owner, goalParams);
        }

        [TearDown]
        public void AfterTest()
        {
            _goalBuilder = null;
            _owner = null;

            GameServiceProvider.ClearGameServiceProvider();
        }

        [Test]
        public void CreateGoalWithId_Default_ErrorAndNullGoal()
        {
            LogAssert.Expect(LogType.Error, "Failed to find goal with Id " + EGoalID.Default);

            Assert.IsNull(_goalBuilder.CreateGoalForId(EGoalID.Default));
        }

        [Test]
        public void CreateGoalWithId_FollowTargetGoalId_CreatesFollowTargetGoal()
        {
            Assert.IsNotNull((FollowTargetGoal) _goalBuilder.CreateGoalForId(EGoalID.FollowTarget));
        }

        [Test]
        public void CreateGoalWithId_IdleGoalId_CreatesIdleGoal()
        {
            Assert.IsNotNull((IdleGoal)_goalBuilder.CreateGoalForId(EGoalID.Idle));
        }

        [Test]
        public void CreateGoalWithId_RemainInRadius_CreateRemainInRadiusGoal()
        {
            Assert.IsNotNull((RemainInRadiusGoal)_goalBuilder.CreateGoalForId(EGoalID.RemainInRadius));
        }

        [Test]
        public void CreateGoalWithId_InvestigateDisturbance_CreateInvestigateAudioDisturbanceGoal()
        {
            Assert.IsNotNull((InvestigateAudioDisturbanceGoal)_goalBuilder.CreateGoalForId(EGoalID.InvestigateAudioDisturbance));
        }

        [Test]
        public void CreateGoalWithId_InvestigateDisturbance_CreateInvestigateVisualDisturbanceGoal()
        {
            Assert.IsNotNull((InvestigateVisualDisturbanceGoal)_goalBuilder.CreateGoalForId(EGoalID.InvestigateVisualDisturbance));
        }

        [Test]
        public void CreateGoalWithId_PursuitTargetGoal_CreatePursuitTargetGoal()
        {
            Assert.IsNotNull((PursuitTargetGoal)_goalBuilder.CreateGoalForId(EGoalID.PursuitTarget));
        }

        [Test]
        public void CreateGoalWithId_PatrolPointsGoal_CreatePatrolPointsGoal()
        {
            Assert.IsNotNull((PatrolPointsGoal)_goalBuilder.CreateGoalForId(EGoalID.PatrolPoints));
        }
    }
}

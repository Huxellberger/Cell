// Copyright (C) Threetee Gang All Rights Reserved

using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.AI.Goals;
using Assets.Scripts.Test.AI.Goals;
using NUnit.Framework;
using UnityEngine;

namespace Assets.Editor.UnitTests.AI.Goals
{
    [TestFixture]
    public class GoalPlannerComponentTestFixture
    {
        private TestGoalPlannerComponent _planner;
        private MockGoalBuilder _builder;

        [SetUp]
        public void BeforeTest()
        {
            var owner = new GameObject();

            _builder = new MockGoalBuilder(owner);

            _planner = owner.AddComponent<TestGoalPlannerComponent>();

            _planner.TestGoalBuilderInterface = _builder;

            _planner.PossibleGoalIds = new List<EGoalID>();
            _planner.PossibleGoalIds.Add(EGoalID.Default);
            _planner.PossibleGoalIds.Add(EGoalID.Default);
            _planner.PossibleGoalIds.Add(EGoalID.Default);

            _planner.TestStart();
        }

        [TearDown]
        public void AfterTest()
        {
            _planner = null;
            _builder = null;
        }

        [Test]
        public void Start_UsesSuppliedIdsToCreateGoals()
        {
            Assert.AreEqual(_planner.PossibleGoalIds.Count, _builder.CreatedGoals.Count);
            Assert.AreEqual(3, _builder.GoalIdCount[EGoalID.Default]);
        }

        [Test]
        public void Start_RegistersAllCreatedGoals()
        {
            foreach (var createdGoal in _builder.CreatedGoals)
            {
                Assert.IsTrue(createdGoal.Registered);
            }
        }

        [Test]
        public void OnDestroy_UnregistersAllCreatedGoals()
        {
            _planner.TestDestroy();

            foreach (var createdGoal in _builder.CreatedGoals)
            {
                Assert.IsTrue(createdGoal.Unregistered);
            }
        }

        [Test]
        public void Update_InitialisesMostDesirableGoal()
        {
            var mostDesirableGoal = _builder.CreatedGoals.First();

            mostDesirableGoal.OverrideDesirabilityFunction = true;
            mostDesirableGoal.CalculateDesirabilityOverride = 1.0f;

            _planner.TestUpdate(1.0f);
            Assert.IsTrue(mostDesirableGoal.Initialised);
        }

        [Test]
        public void Update_DoesNotUpdateGoalWithValueOnInitialisation()
        {
            var initialDesirableGoal = _builder.CreatedGoals.First();

            initialDesirableGoal.OverrideDesirabilityFunction = true;
            initialDesirableGoal.CalculateDesirabilityOverride = 1.0f;

            const float updateValue = 1.0f;

            _planner.TestUpdate(updateValue);
            Assert.IsNull(initialDesirableGoal.UpdateValue);
        }

        [Test]
        public void Update_UpdatesGoalWithValueAfterFirst()
        {
            var initialDesirableGoal = _builder.CreatedGoals.First();

            initialDesirableGoal.OverrideDesirabilityFunction = true;
            initialDesirableGoal.CalculateDesirabilityOverride = 1.0f;
            
            _planner.TestUpdate(1.0f);

            const float updateValue = 1.0f;
            _planner.TestUpdate(updateValue);
            Assert.AreEqual(updateValue, initialDesirableGoal.UpdateValue);
        }

        [Test]
        public void Update_TerminatesGoalIfCompletes()
        {
            var initialDesirableGoal = _builder.CreatedGoals.First();

            initialDesirableGoal.OverrideDesirabilityFunction = true;
            initialDesirableGoal.CalculateDesirabilityOverride = 1.0f;

            _planner.TestUpdate(1.0f);

            initialDesirableGoal.UpdateResult = EGoalStatus.Completed;

            _planner.TestUpdate(1.0f);
            Assert.IsTrue(initialDesirableGoal.Terminated);
        }

        [Test]
        public void Update_TerminatesGoalIfFailed()
        {
            var initialDesirableGoal = _builder.CreatedGoals.First();

            initialDesirableGoal.OverrideDesirabilityFunction = true;
            initialDesirableGoal.CalculateDesirabilityOverride = 1.0f;

            _planner.TestUpdate(1.0f);

            initialDesirableGoal.UpdateResult = EGoalStatus.Failed;

            _planner.TestUpdate(1.0f);
            Assert.IsTrue(initialDesirableGoal.Terminated);
        }

        [Test]
        public void Update_TerminatesGoalIfMoreDesirableOneCropsUp()
        {
            var initialDesirableGoal = _builder.CreatedGoals.First();

            initialDesirableGoal.OverrideDesirabilityFunction = true;
            initialDesirableGoal.CalculateDesirabilityOverride = 1.0f;

            _planner.TestUpdate(1.0f);

            var newDesirableGoal = _builder.CreatedGoals.Last();
            newDesirableGoal.OverrideDesirabilityFunction = true;
            newDesirableGoal.CalculateDesirabilityOverride = 1.0f;

            initialDesirableGoal.CalculateDesirabilityOverride = 0.1f;

            _planner.TestUpdate(1.0f);
            
            Assert.IsTrue(initialDesirableGoal.Terminated);
        }

        [Test]
        public void Update_InitialisesMoreDesirableGoal()
        {
            var initialDesirableGoal = _builder.CreatedGoals.First();

            initialDesirableGoal.OverrideDesirabilityFunction = true;
            initialDesirableGoal.CalculateDesirabilityOverride = 1.0f;

            _planner.TestUpdate(1.0f);

            var newDesirableGoal = _builder.CreatedGoals.Last();
            newDesirableGoal.OverrideDesirabilityFunction = true;
            newDesirableGoal.CalculateDesirabilityOverride = 1.0f;

            initialDesirableGoal.CalculateDesirabilityOverride = 0.1f;

            _planner.TestUpdate(1.0f);

            Assert.IsTrue(newDesirableGoal.Initialised);
        }

        [Test]
        public void Update_UpdatesMoreDesirableGoal()
        {
            var initialDesirableGoal = _builder.CreatedGoals.First();

            initialDesirableGoal.OverrideDesirabilityFunction = true;
            initialDesirableGoal.CalculateDesirabilityOverride = 1.0f;

            _planner.TestUpdate(1.0f);

            var newDesirableGoal = _builder.CreatedGoals.Last();
            newDesirableGoal.OverrideDesirabilityFunction = true;
            newDesirableGoal.CalculateDesirabilityOverride = 1.0f;

            initialDesirableGoal.CalculateDesirabilityOverride = 0.1f;

            _planner.TestUpdate(1.0f);
            _planner.TestUpdate(1.0f);

            Assert.IsTrue(newDesirableGoal.Updated);
        }

        [Test]
        public void Update_GoalsAll0Desirability_NoneSelectedForUpdate()
        {
            var initialDesirableGoal = _builder.CreatedGoals.First();

            initialDesirableGoal.OverrideDesirabilityFunction = true;
            initialDesirableGoal.CalculateDesirabilityOverride = 0.0f;

            _planner.TestUpdate(1.0f);

            Assert.IsFalse(initialDesirableGoal.Updated);
        }
    }
}

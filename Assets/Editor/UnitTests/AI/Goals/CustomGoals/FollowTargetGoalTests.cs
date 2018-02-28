// Copyright (C) Threetee Gang All Rights Reserved

using Assets.Editor.UnitTests.Helpers;
using Assets.Scripts.AI.Goals;
using Assets.Scripts.AI.Goals.CustomGoals;
using Assets.Scripts.Components.Species;
using Assets.Scripts.Services.Wildlife;
using Assets.Scripts.Test.AI.Pathfinding;
using Assets.Scripts.Test.Components.Species;
using Assets.Scripts.Test.Services.Wildlife;
using NUnit.Framework;
using UnityEngine;

namespace Assets.Editor.UnitTests.AI.Goals.CustomGoals
{
    [TestFixture]
    public class FollowTargetGoalTestFixture
    {
        private FollowTargetGoal _goal;
        private FollowTargetGoalParams _params;
        private MockWildlifeService _wildlifeService;
        private MockPathfindingComponent _pathfinding;

        private MockSpeciesComponent _ownerSpecies;
        private MockSpeciesComponent _species;
        private MockSpeciesComponent _otherSpecies;

        [SetUp]
        public void BeforeTest()
        {
            _params = new FollowTargetGoalParams{ValidTargetDesirability = 1.0f, LoseFollowRadiusSquared = 2000.0f, FollowRadius = 10.0f};
            _wildlifeService = new MockWildlifeService();

            var owner = new GameObject();
            _pathfinding = owner.AddComponent<MockPathfindingComponent>();
            _ownerSpecies = owner.AddComponent<MockSpeciesComponent>();

            _species = new GameObject().AddComponent<MockSpeciesComponent>();
            _otherSpecies = new GameObject().AddComponent<MockSpeciesComponent>();

            _goal = new FollowTargetGoal(owner, _params, _wildlifeService);
        }

        [TearDown]
        public void AfterTest()
        {
            _goal = null;

            _otherSpecies = null;
            _species = null;

            _ownerSpecies = null;
            _pathfinding = null;

            _wildlifeService = null;
            _params = null;
        }

        [Test]
        public void Update_Inactive()
        {
            Assert.AreEqual(EGoalStatus.Inactive, _goal.Update(1.0f));
        }

        [Test]
        public void Update_InitialisedWithNullTarget_Failed()
        {
            _goal.Initialise();

            Assert.AreEqual(EGoalStatus.Failed, _goal.Update(1.0f));
        }

        [Test]
        public void Update_Terminated_Inactive()
        {
            _goal.Initialise();
            _goal.Terminate();

            Assert.AreEqual(EGoalStatus.Inactive, _goal.Update(1.0f));
        }

        [Test]
        public void Update_Following_InProgress()
        {
            _ownerSpecies.GetCurrentSpeciesTypeResult = ESpeciesType.Human;
            _species.GetCurrentSpeciesTypeResult = ESpeciesType.Human;
            _species.IsSpeciesCryInProgressResult = true;

            _wildlifeService.GetWildlifeInRadiusResult.Add(new LocalWildlifeResult(new WildlifeService.WildlifeEntry(_species.gameObject), _params.FollowRadius));

            _goal.CalculateDesirability();

            _goal.Initialise();

            _species.IsSpeciesCryInProgressResult = false;

            Assert.AreEqual(EGoalStatus.InProgress, _goal.Update(1.0f));
        }

        [Test]
        public void Update_LeavesLoseFollowRadius_Failed()
        {
            _ownerSpecies.GetCurrentSpeciesTypeResult = ESpeciesType.Human;
            _species.GetCurrentSpeciesTypeResult = ESpeciesType.Human;
            _species.IsSpeciesCryInProgressResult = true;

            _wildlifeService.GetWildlifeInRadiusResult.Add(new LocalWildlifeResult(new WildlifeService.WildlifeEntry(_species.gameObject), _params.FollowRadius));

            Assert.AreEqual(_params.ValidTargetDesirability, _goal.CalculateDesirability());

            _goal.Initialise();

            _species.IsSpeciesCryInProgressResult = false;

            _species.gameObject.transform.position = new Vector3(_params.LoseFollowRadiusSquared + 1.0f, _params.LoseFollowRadiusSquared + 1.0f, _params.LoseFollowRadiusSquared + 1.0f);

            Assert.AreEqual(EGoalStatus.Failed, _goal.Update(1.0f));
        }

        [Test]
        public void Update_DoesNegativeCry_Failed()
        {
            _ownerSpecies.GetCurrentSpeciesTypeResult = ESpeciesType.Human;
            _species.GetCurrentSpeciesTypeResult = ESpeciesType.Human;
            _species.IsSpeciesCryInProgressResult = true;

            _wildlifeService.GetWildlifeInRadiusResult.Add(new LocalWildlifeResult(new WildlifeService.WildlifeEntry(_species.gameObject), _params.FollowRadius));

            Assert.AreEqual(_params.ValidTargetDesirability, _goal.CalculateDesirability());

            _goal.Initialise();

            Assert.AreEqual(EGoalStatus.Failed, _goal.Update(1.0f));
            Assert.AreEqual(ECryType.Negative, _species.IsSpeciesCryInProgressInput);
        }

        [Test]
        public void CalculateDesirability_NoLocalWildlife_ReturnsZero()
        {
            Assert.AreEqual(0.0f, _goal.CalculateDesirability());
        }

        [Test]
        public void CalculateDesirability_ChecksWildlifeServiceWithCurrentPositionAndFollowRadius()
        {
            _ownerSpecies.transform.position = new Vector3(12.0f, 13.0f, 1.0f);
            _goal.CalculateDesirability();

            ExtendedAssertions.AssertVectorsNearlyEqual(_ownerSpecies.gameObject.transform.position, _wildlifeService.GetWildlifeInRadiusLocationInput);
            Assert.AreEqual(_params.FollowRadius, _wildlifeService.GetWildlifeInRadiusDesiredRadiusInput);
        }

        [Test]
        public void CalculateDesirability_LocalWildlifeOfWrongType_ReturnsZero()
        {
            _ownerSpecies.GetCurrentSpeciesTypeResult = ESpeciesType.Human;
            _species.GetCurrentSpeciesTypeResult = ESpeciesType.Rat;
            _species.IsSpeciesCryInProgressResult = true;

            _wildlifeService.GetWildlifeInRadiusResult.Add(new LocalWildlifeResult(new WildlifeService.WildlifeEntry(_species.gameObject), _params.FollowRadius));

            Assert.AreEqual(0.0f, _goal.CalculateDesirability());
        }

        [Test]
        public void CalculateDesirability_LocalWildlifeNotCalling_ReturnsZero()
        {
            _ownerSpecies.GetCurrentSpeciesTypeResult = ESpeciesType.Human;
            _species.GetCurrentSpeciesTypeResult = ESpeciesType.Human;
            _species.IsSpeciesCryInProgressResult = false;

            _wildlifeService.GetWildlifeInRadiusResult.Add(new LocalWildlifeResult(new WildlifeService.WildlifeEntry(_species.gameObject), _params.FollowRadius));

            Assert.AreEqual(0.0f, _goal.CalculateDesirability());
            Assert.AreEqual(ECryType.Positive, _species.IsSpeciesCryInProgressInput);
        }

        [Test]
        public void CalculateDesirability_LocalWildlifeOutOfRadius_ReturnsZero()
        {
            _ownerSpecies.GetCurrentSpeciesTypeResult = ESpeciesType.Human;
            _species.GetCurrentSpeciesTypeResult = ESpeciesType.Human;
            _species.IsSpeciesCryInProgressResult = true;

            _wildlifeService.GetWildlifeInRadiusResult.Add(new LocalWildlifeResult(new WildlifeService.WildlifeEntry(_species.gameObject), Mathf.Pow(_params.FollowRadius, 3)));

            Assert.AreEqual(0.0f, _goal.CalculateDesirability());
        }

        [Test]
        public void CalculateDesirability_SatisfiesConstraints_ReturnsValidTargetDesirability()
        {
            _ownerSpecies.GetCurrentSpeciesTypeResult = ESpeciesType.Human;
            _species.GetCurrentSpeciesTypeResult = ESpeciesType.Human;
            _species.IsSpeciesCryInProgressResult = true;

            _wildlifeService.GetWildlifeInRadiusResult.Add(new LocalWildlifeResult(new WildlifeService.WildlifeEntry(_species.gameObject), _params.FollowRadius));

            Assert.AreEqual(_params.ValidTargetDesirability, _goal.CalculateDesirability());
        }

        [Test]
        public void CalculateDesirability_RemembersPriorResults()
        {
            _ownerSpecies.GetCurrentSpeciesTypeResult = ESpeciesType.Human;
            _species.GetCurrentSpeciesTypeResult = ESpeciesType.Human;
            _species.IsSpeciesCryInProgressResult = true;

            _wildlifeService.GetWildlifeInRadiusResult.Add(new LocalWildlifeResult(new WildlifeService.WildlifeEntry(_species.gameObject), _params.FollowRadius));

            _goal.CalculateDesirability();

            _wildlifeService.GetWildlifeInRadiusResult.RemoveAt(0);

            Assert.AreEqual(_params.ValidTargetDesirability, _goal.CalculateDesirability());
        }

        [Test]
        public void CalculateDesirability_MutlipleSatisfiesConstraints_PathfindFirst()
        {
            _ownerSpecies.GetCurrentSpeciesTypeResult = ESpeciesType.Human;

            _species.GetCurrentSpeciesTypeResult = ESpeciesType.Human;
            _species.IsSpeciesCryInProgressResult = true;

            _otherSpecies.GetCurrentSpeciesTypeResult = ESpeciesType.Human;
            _otherSpecies.IsSpeciesCryInProgressResult = true;

            _wildlifeService.GetWildlifeInRadiusResult.Add(new LocalWildlifeResult(new WildlifeService.WildlifeEntry(_species.gameObject), _params.FollowRadius));
            _wildlifeService.GetWildlifeInRadiusResult.Add(new LocalWildlifeResult(new WildlifeService.WildlifeEntry(_otherSpecies.gameObject), _params.FollowRadius));

            _goal.CalculateDesirability();
            _goal.Initialise();

            Assert.AreSame(_species.gameObject, _pathfinding.SetFollowTargetResult);
        }

        [Test]
        public void CalculateDesirability_PriorFailure_ForgetsOldStatus()
        {
            _ownerSpecies.GetCurrentSpeciesTypeResult = ESpeciesType.Human;

            _species.GetCurrentSpeciesTypeResult = ESpeciesType.Human;
            _species.IsSpeciesCryInProgressResult = true;

            _wildlifeService.GetWildlifeInRadiusResult.Add(new LocalWildlifeResult(new WildlifeService.WildlifeEntry(_species.gameObject), _params.FollowRadius));

            _goal.CalculateDesirability();
            _goal.Initialise();

            _goal.Update(1.0f);

            _species.IsSpeciesCryInProgressResult = false;

            Assert.AreEqual(0.0f, _goal.CalculateDesirability());
        }
    }
}

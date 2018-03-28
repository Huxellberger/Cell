// Copyright (C) Threetee Gang All Rights Reserved

using Assets.Scripts.AI.Goals;
using Assets.Scripts.AI.Goals.CustomGoals;
using Assets.Scripts.Components.ActionStateMachine;
using Assets.Scripts.Test.Components.ActionStateMachine;
using Assets.Scripts.Test.Components.Character.Attack;
using NUnit.Framework;
using UnityEngine;

namespace Assets.Editor.UnitTests.AI.Goals.CustomGoals
{
    [TestFixture]
    public class AttackTargetGoalTestFixture
    {
        private MockActionStateMachineComponent _actionStateMachine;
        private MockAttackComponent _attack;

        [SetUp]
        public void BeforeTest()
        {
            _actionStateMachine = new GameObject().AddComponent<MockActionStateMachineComponent>();
            _actionStateMachine.IsActionStateActiveResult = false;
            _attack = new GameObject().AddComponent<MockAttackComponent>();
        }
	
        [TearDown]
        public void AfterTest()
        {
            _attack = null;
            _actionStateMachine = null;
        }
	
        [Test]
        public void Update_NoOwner_Fails()
        {
            var goal = new AttackTargetGoal(null, _actionStateMachine.gameObject);

            Assert.AreEqual(EGoalStatus.Failed, goal.Update(1.9f));
        }

        [Test]
        public void Update_NoTarget_Fails()
        {
            var goal = new AttackTargetGoal(_attack.gameObject, null);

            Assert.AreEqual(EGoalStatus.Failed, goal.Update(1.9f));
        }

        [Test]
        public void Update_NoAttackInterface_Fails()
        {
            var goal = new AttackTargetGoal(new GameObject(), _actionStateMachine.gameObject);

            Assert.AreEqual(EGoalStatus.Failed, goal.Update(1.9f));
        }

        [Test]
        public void Update_NoActionStateMachine_Fails()
        {
            var goal = new AttackTargetGoal(_attack.gameObject, new GameObject());

            Assert.AreEqual(EGoalStatus.Failed, goal.Update(1.9f));
        }

        [Test]
        public void Update_CannotAttack_DoesNotAttack()
        {
            _attack.CanAttackResult = false;
            var goal = new AttackTargetGoal(_attack.gameObject, _actionStateMachine.gameObject);

            goal.Update(1.0f);

            Assert.IsFalse(_attack.AttackCalled);
        }

        [Test]
        public void Update_CannotAttack_InProgress()
        {
            _attack.CanAttackResult = false;
            var goal = new AttackTargetGoal(_attack.gameObject, _actionStateMachine.gameObject);

            Assert.AreEqual(EGoalStatus.InProgress, goal.Update(1.9f));
        }

        [Test]
        public void Update_CanAttack_Attacks()
        {
            _attack.CanAttackResult = true;
            var goal = new AttackTargetGoal(_attack.gameObject, _actionStateMachine.gameObject);

            goal.Update(1.0f);

            Assert.IsTrue(_attack.AttackCalled);
        }

        [Test]
        public void Update_CanAttack_InProgress()
        {
            _attack.CanAttackResult = true;
            var goal = new AttackTargetGoal(_attack.gameObject, _actionStateMachine.gameObject);

            Assert.AreEqual(EGoalStatus.InProgress, goal.Update(1.9f));
        }

        [Test]
        public void Update_QueriesIfCharacterDead()
        {
            var goal = new AttackTargetGoal(_attack.gameObject, _actionStateMachine.gameObject);

            goal.Update(1.0f);

            Assert.AreEqual(EActionStateMachineTrack.Locomotion, _actionStateMachine.IsActionStateActiveTrackQuery);
            Assert.AreEqual(EActionStateId.Dead, _actionStateMachine.IsActionStateActiveIdQuery);
        }

        [Test]
        public void Update_CharacterDead_ReturnsCompleted()
        {
            _actionStateMachine.IsActionStateActiveResult = true;
            var goal = new AttackTargetGoal(_attack.gameObject, _actionStateMachine.gameObject);

            Assert.AreEqual(EGoalStatus.Completed, goal.Update(1.9f));
        }
    }
}

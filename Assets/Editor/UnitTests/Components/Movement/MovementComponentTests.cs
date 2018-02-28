// Copyright (C) Threetee Gang All Rights Reserved

using Assets.Scripts.Test.Components.Movement;
using Assets.Scripts.Test.Components.Stamina;
using NUnit.Framework;
using UnityEngine;

namespace Assets.Editor.UnitTests.Components.Movement
{
    [TestFixture]
    public class MovementComponentTestFixture
    {
        private Rigidbody _rigidbody;
        private MockStaminaComponent _stamina;
        private TestMovementComponent _movement;
        private const float TimeDelta = 1.5f;

        [SetUp]
        public void BeforeTest()
        {
            _rigidbody = new GameObject().AddComponent<Rigidbody>();
            _stamina = _rigidbody.gameObject.AddComponent<MockStaminaComponent>();
            _movement = _rigidbody.gameObject.AddComponent<TestMovementComponent>();
            _movement.TestAwake();
            _movement.Velocity = 5.0f;

            _stamina.CanExpendStaminaResult = true;
            _stamina.IsStaminaDepletedResult = false;
        }

        [TearDown]
        public void AfterTest()
        {
            _movement = null;

            _stamina = null;
            _rigidbody = null;
        }

        #region Motion
        [Test]
        public void Update_ResetsModifiers()
        {
            const float expectedMagnitude = 1.0f;

            _movement.ApplyForwardMotion(expectedMagnitude);
            _movement.ApplySidewaysMotion(expectedMagnitude);

            _movement.TestUpdate(TimeDelta);
            _movement.TestUpdate(TimeDelta);

            Assert.AreEqual(Vector3.zero, _rigidbody.velocity);
        }

        [Test]
        public void Update_NoModifiers_NoChange()
        {
            _movement.TestUpdate(TimeDelta);
            
            Assert.AreEqual(Vector3.zero, _rigidbody.velocity);
        }

        [Test]
        public void ApplyForwardMotion_AppliesMovement()
        {
            const float expectedMagnitude = 1.0f;

            _movement.ApplyForwardMotion(expectedMagnitude);

            _movement.TestUpdate(TimeDelta);

            var expectedVector = new Vector3(0.0f, 0.0f, expectedMagnitude).normalized * _movement.Velocity;

            Assert.AreEqual(expectedVector, _rigidbody.velocity);
        }

        [Test]
        public void ApplyForwardMotion_AppliesClampedMovement()
        {
            const float expectedMagnitude = 1.0f;

            _movement.ApplyForwardMotion(expectedMagnitude + 100.1f);

            _movement.TestUpdate(TimeDelta);

            var expectedVector = new Vector3(0.0f, 0.0f, expectedMagnitude).normalized * _movement.Velocity;

            Assert.AreEqual(expectedVector, _rigidbody.velocity);
        }

        [Test]
        public void ApplyForwardMotion_AlignedToObjectsRotation()
        {
            const float expectedMagnitude = 1.0f;

            var expectedRotation = new Vector3(10.0f, 30.0f, 100.0f);
            _movement.gameObject.transform.Rotate(expectedRotation);
            _movement.ApplyForwardMotion(expectedMagnitude);

            _movement.TestUpdate(TimeDelta);

            var expectedVector = _movement.gameObject.transform.rotation * new Vector3(0.0f, 0.0f,
                                     expectedMagnitude).normalized * _movement.Velocity;

            expectedVector.y = 0.0f;

            Assert.AreEqual(expectedVector, _rigidbody.velocity);
        }

        [Test]
        public void ApplySidewaysMotion_AppliesMovement()
        {
            const float expectedMagnitude = -0.5f;

            _movement.ApplySidewaysMotion(expectedMagnitude);

            _movement.TestUpdate(TimeDelta);

            var expectedVector = new Vector3(expectedMagnitude, 0.0f, 0.0f).normalized * _movement.Velocity;

            Assert.AreEqual(expectedVector, _rigidbody.velocity);
        }

        [Test]
        public void ApplySidewaysMotion_AppliesClampedMovement()
        {
            const float expectedMagnitude = -1.0f;

            _movement.ApplySidewaysMotion(expectedMagnitude - 100.1f);

            _movement.TestUpdate(TimeDelta);

            var expectedVector = new Vector3(expectedMagnitude, 0.0f, 0.0f).normalized * _movement.Velocity;

            Assert.AreEqual(expectedVector, _rigidbody.velocity);
        }

        [Test]
        public void ApplySidewaysMotion_AlignedToObjectsRotation()
        {
            const float expectedMagnitude = -1.0f;

            var expectedRotation = new Vector3(10.0f, 30.0f, 100.0f);
            _movement.gameObject.transform.Rotate(expectedRotation);

            _movement.ApplySidewaysMotion(expectedMagnitude);

            _movement.TestUpdate(TimeDelta);

            var expectedVector =  _movement.gameObject.transform.rotation * new Vector3(
                _movement.Velocity, 0.0f, 0.0f).normalized * _movement.Velocity;

            expectedVector.y = 0.0f;
            expectedVector *= -1;

            Assert.AreEqual(expectedVector, _rigidbody.velocity);
        }

        [Test]
        public void ApplyBothMotions_InputsAreBothNormalisedAndApplied()
        {
            const float expectedForwardMagnitude = -1.0f;
            const float expectedSidewaysMagnitude = 1.0f;

            _movement.ApplySidewaysMotion(expectedSidewaysMagnitude);
            _movement.ApplyForwardMotion(expectedForwardMagnitude);

            _movement.TestUpdate(TimeDelta);

            var expectedVector = new Vector3
            (
                expectedSidewaysMagnitude,
                0.0f,
                expectedForwardMagnitude
            ).normalized * _movement.Velocity;

            Assert.AreEqual(expectedVector, _rigidbody.velocity);
        }
        #endregion

        #region Sprinting
        [Test]
        public void SetSprintEnabled_True_MultiplySpeedByModifierOnSingleAxis()
        {
            const float expectedSidewaysMagnitude = -1.0f;

            _movement.ApplySidewaysMotion(expectedSidewaysMagnitude);
            _movement.SetSprintEnabled(true);

            _movement.TestUpdate(TimeDelta);

            var expectedVector = new Vector3 (expectedSidewaysMagnitude, 0.0f, 0.0f)
                .normalized * _movement.Velocity * _movement.SprintMultiplier;

            Assert.AreEqual(expectedVector, _rigidbody.velocity);
        }

        [Test]
        public void SetSprintEnabled_True_MultiplySpeedByModifierForBothAxis()
        {
            const float expectedSidewaysMagnitude = -1.0f;
            const float expectedForwardMagnitude = 1.0f;

            _movement.ApplyForwardMotion(expectedForwardMagnitude);
            _movement.ApplySidewaysMotion(expectedSidewaysMagnitude);
            _movement.SetSprintEnabled(true);

            _movement.TestUpdate(TimeDelta);

            var expectedVector = new Vector3(expectedSidewaysMagnitude, 0.0f, expectedForwardMagnitude)
                                      .normalized * _movement.Velocity * _movement.SprintMultiplier;

            Assert.AreEqual(expectedVector, _rigidbody.velocity);
        }

        [Test]
        public void SetSprintEnabled_True_PersistsBetweenUpdates()
        {
            const float expectedSidewaysMagnitude = -1.0f;

            _movement.ApplySidewaysMotion(expectedSidewaysMagnitude);
            _movement.SetSprintEnabled(true);

            _movement.TestUpdate(TimeDelta);

            _movement.ApplySidewaysMotion(expectedSidewaysMagnitude);

            _movement.TestUpdate(TimeDelta);

            var expectedVector = new Vector3(expectedSidewaysMagnitude, 0.0f, 0.0f)
                .normalized * _movement.Velocity * _movement.SprintMultiplier;

            Assert.AreEqual(expectedVector, _rigidbody.velocity);
        }

        [Test]
        public void SetSprintEnabled_True_ExpendsSprintStaminaCostPerUpdate()
        {
            const float expectedSidewaysMagnitude = -1.0f;

            _movement.ApplySidewaysMotion(expectedSidewaysMagnitude);
            _movement.SetSprintEnabled(true);

            _movement.TestUpdate(TimeDelta);

            Assert.AreEqual(_movement.SprintStaminaCostPerUpdate * -1, _stamina.AlterStaminaResult);
        }

        [Test]
        public void SetSprintEnabled_False_DisabledModifier()
        {
            const float expectedSidewaysMagnitude = -1.0f;

            _movement.ApplySidewaysMotion(expectedSidewaysMagnitude);
            _movement.SetSprintEnabled(true);
            _movement.SetSprintEnabled(false);

            _movement.TestUpdate(TimeDelta);

            var expectedVector = new Vector3(_movement.Velocity * expectedSidewaysMagnitude, 0.0f, 0.0f)
                .normalized * _movement.Velocity;

            Assert.AreEqual(expectedVector, _rigidbody.velocity);
        }

        [Test]
        public void SetSprintEnabled_NoModifier_DoesNotExpendSprintCost()
        {
            _movement.SetSprintEnabled(true);
            _movement.TestUpdate(TimeDelta);

            Assert.AreNotEqual(_movement.SprintStaminaCostPerUpdate * -1, _stamina.AlterStaminaResult);
        }

        [Test]
        public void SetSprintEnabled_NotEnoughStamina_MovesNormalSpeed()
        {
            _stamina.CanExpendStaminaResult = false;
            const float expectedSidewaysMagnitude = -1.0f;

            _movement.ApplySidewaysMotion(expectedSidewaysMagnitude);;

            _movement.SetSprintEnabled(true);

            _movement.TestUpdate(TimeDelta);

            var expectedVector = new Vector3(expectedSidewaysMagnitude, 0.0f, 0.0f).normalized * _movement.Velocity;

            Assert.AreEqual(expectedVector, _rigidbody.velocity);
        }

        [Test]
        public void NoStamina_MovesExhaustedSpeed()
        {
            _stamina.IsStaminaDepletedResult = true;
            _stamina.CanExpendStaminaResult = false;
            const float expectedForwardMagnitude = 1.0f;

            _movement.ApplyForwardMotion(expectedForwardMagnitude);

            _movement.SetSprintEnabled(true);

            _movement.TestUpdate(TimeDelta);

            var expectedVector = new Vector3(0.0f, 0.0f, expectedForwardMagnitude)
                .normalized * _movement.Velocity * _movement.ExhaustedMultiplier;

            Assert.AreEqual(expectedVector, _rigidbody.velocity);
        }

        [Test]
        public void SetSprintEnabled_Jumping_DoesNotAlterState()
        {
            const float expectedForwardMagnitude = 1.0f;
            const float expectedYVelocity = 100.0f;

            // Sprint And Jump
            _movement.ApplyForwardMotion(expectedForwardMagnitude);
            _movement.RequestJump();
            _movement.SetSprintEnabled(true);
            _movement.TestUpdate(TimeDelta);

            // Deplete Stamina
            _rigidbody.velocity = new Vector3(_rigidbody.velocity.x, expectedYVelocity, _rigidbody.velocity.z);
            _stamina.CanExpendStaminaResult = false;
            _stamina.IsStaminaDepletedResult = true;
            _movement.TestUpdate(TimeDelta);

            var expectedVector = new Vector3(0.0f, 0.0f, expectedForwardMagnitude)
                .normalized * _movement.Velocity * _movement.SprintMultiplier + new Vector3(0.0f, expectedYVelocity, 0.0f);

            Assert.AreEqual(expectedVector, _rigidbody.velocity);
        }
        #endregion

        #region Jumping
        [Test]
        public void RequestJump_SetsRigidbodyYVelocityToJumpVelocity()
        {
            _movement.RequestJump();
            _movement.TestUpdate(TimeDelta);

            Assert.AreEqual(_movement.JumpVelocity, _rigidbody.velocity.y);
        }

        [Test]
        public void RequestJump_ExpendsCorrectStamina()
        {
            _movement.RequestJump();
            _movement.TestUpdate(TimeDelta);

            Assert.AreEqual(_movement.JumpStaminaCost * -1, _stamina.AlterStaminaResult);
        }

        [Test]
        public void RequestJump_VelocityContinuesChanging_JumpCannotBeReRequested()
        {
            _movement.RequestJump();
            _movement.TestUpdate(TimeDelta);

            _rigidbody.velocity = new Vector3(0.0f, _movement.JumpVelocity * 2.0f, 0.0f);

            _movement.RequestJump();
            _movement.TestUpdate(TimeDelta);

            Assert.AreNotEqual(_movement.JumpVelocity, _rigidbody.velocity.y);
        }

        [Test]
        public void RequestJump_VelocityDoesNotChange_JumpCanBeReRequested()
        {
            _movement.RequestJump();
            _movement.TestUpdate(TimeDelta);

            _movement.TestUpdate(TimeDelta);

            _rigidbody.velocity = new Vector3(0.0f, _movement.JumpVelocity * 2.0f, 0.0f);

            _movement.RequestJump();
            _movement.TestUpdate(TimeDelta);

            Assert.AreEqual(_movement.JumpVelocity, _rigidbody.velocity.y);
        }

        [Test]
        public void RequestJump_NotEnoughStamina_JumpNotPerformed()
        {
            _stamina.CanExpendStaminaResult = false;
            _movement.RequestJump();
            _movement.TestUpdate(TimeDelta);

            Assert.AreNotEqual(_movement.JumpVelocity, _rigidbody.velocity.y);
        }

        [Test]
        public void JumpInProgress_CannotAlterVelocity()
        {
            const float expectedSidewaysMagnitude = -1.0f;

            _movement.RequestJump();
            _movement.ApplySidewaysMotion(expectedSidewaysMagnitude);
            _movement.TestUpdate(TimeDelta);

            var alteredYVelocity = _movement.JumpVelocity * 2.0f;

            _rigidbody.velocity = new Vector3(_rigidbody.velocity.x, alteredYVelocity, _rigidbody.velocity.z);
            _movement.ApplySidewaysMotion(expectedSidewaysMagnitude * 0.5f);
            _movement.TestUpdate(TimeDelta);

            var expectedVelocity = new Vector3
            (
                expectedSidewaysMagnitude * _movement.Velocity,
                alteredYVelocity,
                0.0f
            );

            Assert.AreEqual(expectedVelocity, _rigidbody.velocity);
        }
        #endregion
    }
}

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
        private Rigidbody2D _rigidbody;
        private MockStaminaComponent _stamina;
        private TestMovementComponent _movement;
        private const float TimeDelta = 1.5f;

        [SetUp]
        public void BeforeTest()
        {
            _rigidbody = new GameObject().AddComponent<Rigidbody2D>();
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

            Assert.AreEqual(Vector2.zero, _rigidbody.velocity);
        }

        [Test]
        public void Update_NoModifiers_NoChange()
        {
            _movement.TestUpdate(TimeDelta);
            
            Assert.AreEqual(Vector2.zero, _rigidbody.velocity);
        }

        [Test]
        public void ApplyForwardMotion_AppliesMovement()
        {
            const float expectedMagnitude = 1.0f;

            _movement.ApplyForwardMotion(expectedMagnitude);

            _movement.TestUpdate(TimeDelta);

            var expectedVector = new Vector2(0.0f, expectedMagnitude).normalized * _movement.Velocity;

            Assert.AreEqual(expectedVector, _rigidbody.velocity);
        }

        [Test]
        public void ApplyForwardMotion_AppliesClampedMovement()
        {
            const float expectedMagnitude = 1.0f;

            _movement.ApplyForwardMotion(expectedMagnitude + 100.1f);

            _movement.TestUpdate(TimeDelta);

            var expectedVector = new Vector2(0.0f, expectedMagnitude).normalized * _movement.Velocity;

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

            var expectedVector = _movement.gameObject.transform.rotation * new Vector2(0.0f,
                                     expectedMagnitude).normalized * _movement.Velocity;

            var expectedVector2D = new Vector2(expectedVector.x, expectedVector.y);

            Assert.AreEqual(expectedVector2D, _rigidbody.velocity);
        }

        [Test]
        public void ApplySidewaysMotion_AppliesMovement()
        {
            const float expectedMagnitude = -0.5f;

            _movement.ApplySidewaysMotion(expectedMagnitude);

            _movement.TestUpdate(TimeDelta);

            var expectedVector = new Vector2(expectedMagnitude, 0.0f).normalized * _movement.Velocity;

            Assert.AreEqual(expectedVector, _rigidbody.velocity);
        }

        [Test]
        public void ApplySidewaysMotion_AppliesClampedMovement()
        {
            const float expectedMagnitude = -1.0f;

            _movement.ApplySidewaysMotion(expectedMagnitude - 100.1f);

            _movement.TestUpdate(TimeDelta);

            var expectedVector = new Vector2(expectedMagnitude, 0.0f).normalized * _movement.Velocity;

            Assert.AreEqual(expectedVector, _rigidbody.velocity);
        }

        [Test]
        public void ApplySidewaysMotion_AlignedToObjectsRotation()
        {
            const float expectedMagnitude = -1.0f;

            var expectedRotation = new Vector2(10.0f, 30.0f);
            _movement.gameObject.transform.Rotate(expectedRotation);

            _movement.ApplySidewaysMotion(expectedMagnitude);

            _movement.TestUpdate(TimeDelta);

            var expectedVector = _movement.gameObject.transform.rotation * new Vector2(
                                     _movement.Velocity, 0.0f).normalized * _movement.Velocity;

            var expectedVector2D = new Vector2(expectedVector.x, expectedVector.y) * -1;

            Assert.AreEqual(expectedVector2D, _rigidbody.velocity);
        }

        [Test]
        public void ApplyBothMotions_InputsAreBothNormalisedAndApplied()
        {
            const float expectedForwardMagnitude = -1.0f;
            const float expectedSidewaysMagnitude = 1.0f;

            _movement.ApplySidewaysMotion(expectedSidewaysMagnitude);
            _movement.ApplyForwardMotion(expectedForwardMagnitude);

            _movement.TestUpdate(TimeDelta);

            var expectedVector = new Vector2
            (
                expectedSidewaysMagnitude,
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

            var expectedVector = new Vector2(expectedSidewaysMagnitude, 0.0f)
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

            var expectedVector = new Vector2(expectedSidewaysMagnitude, expectedForwardMagnitude)
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

            var expectedVector = new Vector2(expectedSidewaysMagnitude, 0.0f)
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

            var expectedVector = new Vector2(_movement.Velocity * expectedSidewaysMagnitude, 0.0f)
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

            var expectedVector = new Vector2(expectedSidewaysMagnitude, 0.0f).normalized * _movement.Velocity;

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

            var expectedVector = new Vector2(0.0f, expectedForwardMagnitude)
                .normalized * _movement.Velocity * _movement.ExhaustedMultiplier;

            Assert.AreEqual(expectedVector, _rigidbody.velocity);
        }
        #endregion
    }
}

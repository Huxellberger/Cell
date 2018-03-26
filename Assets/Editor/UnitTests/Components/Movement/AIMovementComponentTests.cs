// Copyright (C) Threetee Gang All Rights Reserved

using Assets.Scripts.Test.Components.Movement;
using Assets.Scripts.Test.Components.Stamina;
using NUnit.Framework;
using UnityEngine;

namespace Assets.Editor.UnitTests.Components.Movement
{
    [TestFixture]
    public class AIMovementComponentTestFixture
    {
        private Rigidbody2D _rigidbody;
        private MockStaminaComponent _stamina;
        private TestAIMovementComponent _movement;
        private const float TimeDelta = 1.5f;

        [SetUp]
        public void BeforeTest()
        {
            _rigidbody = new GameObject().AddComponent<Rigidbody2D>();
            _stamina = _rigidbody.gameObject.AddComponent<MockStaminaComponent>();
            _movement = _rigidbody.gameObject.AddComponent<TestAIMovementComponent>();
            _movement.TestAwake();
            _movement.Velocity = 5.0f;
            _movement.TurningSpeed = 2.0f;

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


        [Test]
        public void Update_NoMovement_DoesNotTurn()
        {
            var initialRotation = _movement.gameObject.transform.eulerAngles;

            _movement.TestUpdate(1.0f);

            Assert.AreEqual(initialRotation, _movement.gameObject.transform.eulerAngles);
        }

        [Test]
        public void Update_Movement_TurnsByClampedAmountToSuitNewDirection()
        {
            var initialRotation = _movement.gameObject.transform.eulerAngles;

            _movement.ApplyForwardMotion(-1.0f);
            _movement.ApplySidewaysMotion(-1.0f);
            _movement.TestUpdate(1.0f);

            var movementVector = new Vector3(-1.0f, -1.0f, 0.0f).normalized;

            var appliedRotation = Mathf.Clamp(Vector3.SignedAngle(movementVector, _movement.gameObject.transform.up, _movement.gameObject.transform.up), -_movement.TurningSpeed, _movement.TurningSpeed);

            var exampleToRotate = new GameObject();

            exampleToRotate.transform.Rotate(new Vector3(0.0f, 0.0f, appliedRotation));

            Assert.AreEqual(exampleToRotate.transform.eulerAngles, _movement.gameObject.transform.eulerAngles);
        }
    }
}

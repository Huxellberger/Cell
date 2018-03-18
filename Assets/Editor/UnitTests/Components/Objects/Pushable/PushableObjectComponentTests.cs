// Copyright (C) Threetee Gang All Rights Reserved

using Assets.Scripts.Test.Components.Objects.Pushable;
using NUnit.Framework;
using UnityEngine;

namespace Assets.Editor.UnitTests.Components.Objects.Pushable
{
    [TestFixture]
    public class PushableObjectComponentTestFixture
    {
        private TestPushableObjectComponent _pushable;
        private Rigidbody2D _rigidbody;

        [SetUp]
        public void BeforeTest()
        {
            _rigidbody = new GameObject().AddComponent<Rigidbody2D>();
            _pushable = _rigidbody.gameObject.AddComponent<TestPushableObjectComponent>();

            _pushable.PushModifier = 1.5f;
            _pushable.TestStart();
        }

        [TearDown]
        public void AfterTest()
        {
            _pushable = null;
            _rigidbody = null;
        }

        [Test]
        public void Update_NoPushes_NothingChanges()
        {
            _pushable.TestUpdate(1.0f);

            Assert.AreEqual(Vector3.zero, _rigidbody.velocity);
        }

        [Test]
        public void Update_Push_ChangesVelocity()
        {
            var pushVector = new Vector3(1.0f, 2.0f, -1.0f);
            const float delta = 0.5f;

            _pushable.Push(pushVector);
            _pushable.TestUpdate(delta);

            Assert.AreEqual(pushVector * delta * _pushable.PushModifier, _rigidbody.velocity);
        }

        [Test]
        public void Update_MultiplePushes_ChangesVelocity()
        {
            var pushVector = new Vector3(1.0f, 2.0f, -1.0f);
            var otherPushVector = new Vector3(2.0f, -12.0f, 3.0f);
            const float delta = 0.5f;

            _pushable.Push(pushVector);
            _pushable.Push(otherPushVector);
            _pushable.TestUpdate(delta);

            Assert.AreEqual((pushVector + otherPushVector) * delta * _pushable.PushModifier, _rigidbody.velocity);
        }

        [Test]
        public void Update_ZeroesPreviousUpdate()
        {
            var pushVector = new Vector3(1.0f, 2.0f, -1.0f);
            const float delta = 0.5f;

            _pushable.Push(pushVector);
            _pushable.TestUpdate(delta);
            _pushable.TestUpdate(delta);

            Assert.AreEqual(pushVector * delta * _pushable.PushModifier, _rigidbody.velocity);
        }
    }
}

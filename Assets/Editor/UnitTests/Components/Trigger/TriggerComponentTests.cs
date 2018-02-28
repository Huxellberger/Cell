// Copyright (C) Threetee Gang All Rights Reserved

using Assets.Editor.UnitTests.Messaging;
using Assets.Scripts.Components.Trigger;
using Assets.Scripts.Messaging;
using Assets.Scripts.Test.Components.Trigger;
using Assets.Scripts.Test.Messaging;
using NUnit.Framework;
using UnityEngine;

namespace Assets.Editor.UnitTests.Components.Trigger
{
    [TestFixture]
    public class TriggerComponentTestFixture
    {
        private TestTriggerComponent _trigger;

        [SetUp]
        public void BeforeTest()
        {
            var gameObject = new GameObject();
            gameObject.AddComponent<TestUnityMessageEventDispatcherComponent>().TestAwake();

            _trigger = gameObject.AddComponent<TestTriggerComponent>();
        }

        [TearDown]
        public void AfterTest()
        {
            _trigger = null;
        }

        [Test]
        public void OnCollision_CanTrigger_TriggerMessageSent()
        {
            var expectedGameObject = new GameObject();
            _trigger.CanTriggerResult = true;

            var messageSpy = new UnityTestMessageHandleResponseObject<TriggerMessage>();

            var handle = UnityMessageEventFunctions.RegisterActionWithDispatcher<TriggerMessage>
                (_trigger.gameObject, messageSpy.OnResponse);

            _trigger.TestCollide(expectedGameObject);

            Assert.IsTrue(messageSpy.ActionCalled);

            Assert.AreSame(expectedGameObject, messageSpy.MessagePayload.TriggeringObject);

            UnityMessageEventFunctions.UnregisterActionWithDispatcher(_trigger.gameObject, handle);
        }

        [Test]
        public void OnCollision_CannotTrigger_NoTriggerMessageSent()
        {
            var expectedGameObject = new GameObject();
            _trigger.CanTriggerResult = false;

            var messageSpy = new UnityTestMessageHandleResponseObject<TriggerMessage>();

            var handle = UnityMessageEventFunctions.RegisterActionWithDispatcher<TriggerMessage>
                (_trigger.gameObject, messageSpy.OnResponse);

            _trigger.TestCollide(expectedGameObject);

            Assert.IsFalse(messageSpy.ActionCalled);

            UnityMessageEventFunctions.UnregisterActionWithDispatcher(_trigger.gameObject, handle);
        }

        [Test]
        public void OnCollisionStop_CanCancelTrigger_CancelTriggerMessageSent()
        {
            var expectedGameObject = new GameObject();
            _trigger.CanCancelTriggerResult = true;

            var messageSpy = new UnityTestMessageHandleResponseObject<CancelTriggerMessage>();

            var handle = UnityMessageEventFunctions.RegisterActionWithDispatcher<CancelTriggerMessage>
                (_trigger.gameObject, messageSpy.OnResponse);

            _trigger.TestStopColliding(expectedGameObject);

            Assert.IsTrue(messageSpy.ActionCalled);

            Assert.AreSame(expectedGameObject, messageSpy.MessagePayload.TriggeringObject);

            UnityMessageEventFunctions.UnregisterActionWithDispatcher(_trigger.gameObject, handle);
        }

        [Test]
        public void OnCollisionStop_CannotCancelTrigger_NoCancelTriggerMessageSent()
        {
            var expectedGameObject = new GameObject();
            _trigger.CanCancelTriggerResult = false;

            var messageSpy = new UnityTestMessageHandleResponseObject<CancelTriggerMessage>();

            var handle = UnityMessageEventFunctions.RegisterActionWithDispatcher<CancelTriggerMessage>
                (_trigger.gameObject, messageSpy.OnResponse);

            _trigger.TestStopColliding(expectedGameObject);

            Assert.IsFalse(messageSpy.ActionCalled);

            UnityMessageEventFunctions.UnregisterActionWithDispatcher(_trigger.gameObject, handle);
        }
    }
}

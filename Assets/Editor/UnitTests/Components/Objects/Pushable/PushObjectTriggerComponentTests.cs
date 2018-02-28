// Copyright (C) Threetee Gang All Rights Reserved

using Assets.Editor.UnitTests.Messaging;
using Assets.Scripts.Components.Species;
using Assets.Scripts.Components.Trigger;
using Assets.Scripts.Messaging;
using Assets.Scripts.Test.Components.Objects.Pushable;
using Assets.Scripts.Test.Components.Species;
using Assets.Scripts.Test.Messaging;
using NUnit.Framework;
using UnityEngine;

namespace Assets.Editor.UnitTests.Components.Objects.Pushable
{
    [TestFixture]
    public class PushObjectTriggerComponentTestFixture
    {
        private TestPushObjectTriggerComponent _pushTrigger;
        
        private MockPushableObjectComponent _pushable;
        private MockPushableObjectComponent _otherPushable;
        private MockSpeciesComponent _species;

        [SetUp]
        public void BeforeTest()
        {
            _pushTrigger = new GameObject().AddComponent<TestPushObjectTriggerComponent>();
            _pushTrigger.gameObject.AddComponent<TestUnityMessageEventDispatcherComponent>().TestAwake();

            _pushable = new GameObject().AddComponent<MockPushableObjectComponent>();
            _otherPushable = new GameObject().AddComponent<MockPushableObjectComponent>();

            _species = new GameObject().AddComponent<MockSpeciesComponent>();
        }

        [TearDown]
        public void AfterTest()
        {
            _species = null;

            _otherPushable = null;
            _pushable= null;

            _pushTrigger = null;
        }

        [Test]
        public void NullCollides_NoTriggerMessage()
        {
            var messageSpy = new UnityTestMessageHandleResponseObject<TriggerMessage>();

            var handle = UnityMessageEventFunctions.RegisterActionWithDispatcher<TriggerMessage>
                (_pushTrigger.gameObject, messageSpy.OnResponse);

           _pushTrigger.TestCollide(null);

            Assert.IsFalse(messageSpy.ActionCalled);

            UnityMessageEventFunctions.UnregisterActionWithDispatcher(_pushTrigger.gameObject, handle);
        }

        [Test]
        public void NoPushableCollides_NoTriggerMessage()
        {
            var expectedGameObject = new GameObject();

            var messageSpy = new UnityTestMessageHandleResponseObject<TriggerMessage>();

            var handle = UnityMessageEventFunctions.RegisterActionWithDispatcher<TriggerMessage>
                (_pushTrigger.gameObject, messageSpy.OnResponse);

            _pushTrigger.TestCollide(expectedGameObject);

            Assert.IsFalse(messageSpy.ActionCalled);

            UnityMessageEventFunctions.UnregisterActionWithDispatcher(_pushTrigger.gameObject, handle);
        }

        [Test]
        public void PushableCollides_TriggerMessage()
        {
            var messageSpy = new UnityTestMessageHandleResponseObject<TriggerMessage>();

            var handle = UnityMessageEventFunctions.RegisterActionWithDispatcher<TriggerMessage>
                (_pushTrigger.gameObject, messageSpy.OnResponse);

            _pushTrigger.TestCollide(_pushable.gameObject);

            Assert.IsTrue(messageSpy.ActionCalled);
            Assert.AreSame(_pushable.gameObject, messageSpy.MessagePayload.TriggeringObject);

            UnityMessageEventFunctions.UnregisterActionWithDispatcher(_pushTrigger.gameObject, handle);
        }

        [Test]
        public void IncorrectSpeciesCollides_NoTriggerMessage()
        {
            _pushTrigger.TriggeringSpeciesTypes.Add(ESpeciesType.Human);
            _species.GetCurrentSpeciesTypeResult = ESpeciesType.Rat;

            var messageSpy = new UnityTestMessageHandleResponseObject<TriggerMessage>();

            var handle = UnityMessageEventFunctions.RegisterActionWithDispatcher<TriggerMessage>
                (_pushTrigger.gameObject, messageSpy.OnResponse);

            _pushTrigger.TestCollide(_species.gameObject);

            Assert.IsFalse(messageSpy.ActionCalled);

            UnityMessageEventFunctions.UnregisterActionWithDispatcher(_pushTrigger.gameObject, handle);
        }

        [Test]
        public void CorrectSpeciesCollides_TriggerMessage()
        {
            const ESpeciesType correctType = ESpeciesType.Human;

            _pushTrigger.TriggeringSpeciesTypes.Add(correctType);
            _species.GetCurrentSpeciesTypeResult = correctType;

            var messageSpy = new UnityTestMessageHandleResponseObject<TriggerMessage>();

            var handle = UnityMessageEventFunctions.RegisterActionWithDispatcher<TriggerMessage>
                (_pushTrigger.gameObject, messageSpy.OnResponse);

            _pushTrigger.TestCollide(_species.gameObject);

            Assert.IsTrue(messageSpy.ActionCalled);
            Assert.AreSame(_species.gameObject, messageSpy.MessagePayload.TriggeringObject);

            UnityMessageEventFunctions.UnregisterActionWithDispatcher(_pushTrigger.gameObject, handle);
        }

        [Test]
        public void SecondPushableCollides_NoTriggerMessage()
        {
            _pushTrigger.TestCollide(_pushable.gameObject);

            var messageSpy = new UnityTestMessageHandleResponseObject<TriggerMessage>();

            var handle = UnityMessageEventFunctions.RegisterActionWithDispatcher<TriggerMessage>
                (_pushTrigger.gameObject, messageSpy.OnResponse);

            _pushTrigger.TestCollide(_otherPushable.gameObject);

            Assert.IsFalse(messageSpy.ActionCalled);

            UnityMessageEventFunctions.UnregisterActionWithDispatcher(_pushTrigger.gameObject, handle);
        }

        [Test]
        public void PushableStopsColliding_OtherColliding_NoCancelTriggerMessage()
        {
            _pushTrigger.TestCollide(_pushable.gameObject);
            _pushTrigger.TestCollide(_otherPushable.gameObject);

            var messageSpy = new UnityTestMessageHandleResponseObject<CancelTriggerMessage>();

            var handle = UnityMessageEventFunctions.RegisterActionWithDispatcher<CancelTriggerMessage>
                (_pushTrigger.gameObject, messageSpy.OnResponse);

            _pushTrigger.TestStopColliding(_pushable.gameObject);

            Assert.IsFalse(messageSpy.ActionCalled);

            UnityMessageEventFunctions.UnregisterActionWithDispatcher(_pushTrigger.gameObject, handle);
        }

        [Test]
        public void PushableStopsColliding_NotColliding_NoCancelTriggerMessage()
        {
            var messageSpy = new UnityTestMessageHandleResponseObject<CancelTriggerMessage>();

            var handle = UnityMessageEventFunctions.RegisterActionWithDispatcher<CancelTriggerMessage>
                (_pushTrigger.gameObject, messageSpy.OnResponse);

            _pushTrigger.TestStopColliding(_pushable.gameObject);

            Assert.IsFalse(messageSpy.ActionCalled);

            UnityMessageEventFunctions.UnregisterActionWithDispatcher(_pushTrigger.gameObject, handle);
        }

        [Test]
        public void PushableStopsColliding_NoOtherColliding_CancelTriggerMessage()
        {
            _pushTrigger.TestCollide(_pushable.gameObject);
            _pushTrigger.TestCollide(_otherPushable.gameObject);

            _pushTrigger.TestStopColliding(_otherPushable.gameObject);

            var messageSpy = new UnityTestMessageHandleResponseObject<CancelTriggerMessage>();

            var handle = UnityMessageEventFunctions.RegisterActionWithDispatcher<CancelTriggerMessage>
                (_pushTrigger.gameObject, messageSpy.OnResponse);

            _pushTrigger.TestStopColliding(_pushable.gameObject);

            Assert.IsTrue(messageSpy.ActionCalled);
            Assert.AreSame(_pushable.gameObject, messageSpy.MessagePayload.TriggeringObject);

            UnityMessageEventFunctions.UnregisterActionWithDispatcher(_pushTrigger.gameObject, handle);
        }

        [Test]
        public void CorrectSpeciesStopsColliding_CancelTriggerMessage()
        {
            const ESpeciesType correctType = ESpeciesType.Human;

            _pushTrigger.TriggeringSpeciesTypes.Add(correctType);
            _species.GetCurrentSpeciesTypeResult = correctType;

            _pushTrigger.TestCollide(_species.gameObject);

            var messageSpy = new UnityTestMessageHandleResponseObject<CancelTriggerMessage>();

            var handle = UnityMessageEventFunctions.RegisterActionWithDispatcher<CancelTriggerMessage>
                (_pushTrigger.gameObject, messageSpy.OnResponse);
           
            _pushTrigger.TestStopColliding(_species.gameObject);

            Assert.IsTrue(messageSpy.ActionCalled);
            Assert.AreSame(_species.gameObject, messageSpy.MessagePayload.TriggeringObject);

            UnityMessageEventFunctions.UnregisterActionWithDispatcher(_pushTrigger.gameObject, handle);
        }
    }
}

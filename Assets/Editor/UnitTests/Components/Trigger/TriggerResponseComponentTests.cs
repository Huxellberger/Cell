// Copyright (C) Threetee Gang All Rights Reserved

using Assets.Scripts.Components.Trigger;
using Assets.Scripts.Messaging;
using Assets.Scripts.Test.Components.Trigger;
using Assets.Scripts.Test.Messaging;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Assets.Editor.UnitTests.Components.Trigger
{
    [TestFixture]
    public class TriggerResponseComponentTestFixture
    {
        private TestTriggerResponseComponent _triggerResponse;

        [SetUp]
        public void BeforeTest()
        {
            _triggerResponse = new GameObject().AddComponent<TestTriggerResponseComponent>();

            _triggerResponse.TriggerObject = new GameObject();
            _triggerResponse.TriggerObject.AddComponent<TestUnityMessageEventDispatcherComponent>().TestAwake();
        }

        [TearDown]
        public void AfterTest()
        {
            _triggerResponse = null;
        }

        [Test]
        public void Start_NoTrigger_ErrorThrown()
        {
            LogAssert.Expect(LogType.Error, "Cannot register as trigger not set!");

            _triggerResponse.TriggerObject = null;

            _triggerResponse.TestStart();
        }

        [Test]
        public void Start_Trigger_RegistersForTriggerMessage()
        {
            var expectedObject = new GameObject();
            _triggerResponse.TestStart();

            UnityMessageEventFunctions.InvokeMessageEventWithDispatcher(_triggerResponse.TriggerObject, new TriggerMessage(expectedObject));

            Assert.AreSame(expectedObject, _triggerResponse.OnTriggerGameObject);
        }

        [Test]
        public void Start_TriggerAgain_DefaultIsOneMessage()
        {
            var expectedObject = new GameObject();
            _triggerResponse.TestStart();

            UnityMessageEventFunctions.InvokeMessageEventWithDispatcher(_triggerResponse.TriggerObject, new TriggerMessage(expectedObject));
            UnityMessageEventFunctions.InvokeMessageEventWithDispatcher(_triggerResponse.TriggerObject, new TriggerMessage(null));

            Assert.AreSame(expectedObject, _triggerResponse.OnTriggerGameObject);
        }

        [Test]
        public void Start_MultiTrigger_MultipleMessagesReceived()
        {
            var expectedObject = new GameObject();
            _triggerResponse.TestStart();

            _triggerResponse.MultiTrigger = true;

            UnityMessageEventFunctions.InvokeMessageEventWithDispatcher(_triggerResponse.TriggerObject, new TriggerMessage(expectedObject));
            UnityMessageEventFunctions.InvokeMessageEventWithDispatcher(_triggerResponse.TriggerObject, new TriggerMessage(null));

            Assert.IsNull(_triggerResponse.OnTriggerGameObject);
        }

        [Test]
        public void Start_MultiTriggerFalse_NoCancel()
        {
            _triggerResponse.TestStart();

            UnityMessageEventFunctions.InvokeMessageEventWithDispatcher(_triggerResponse.TriggerObject, new CancelTriggerMessage(new GameObject()));

            Assert.IsNull(_triggerResponse.OnCancelTriggerGameObject);
        }

        [Test]
        public void Start_MultiTrigger_CancelMessage()
        {
            var expectedObject = new GameObject();
            _triggerResponse.TestStart();

            _triggerResponse.MultiTrigger = true;

            UnityMessageEventFunctions.InvokeMessageEventWithDispatcher(_triggerResponse.TriggerObject, new CancelTriggerMessage(expectedObject));

            Assert.AreSame(expectedObject, _triggerResponse.OnCancelTriggerGameObject);
        }

        [Test]
        public void OnDestroy_Trigger_UnregistersForTriggerMessage()
        {
            var expectedObject = new GameObject();
            _triggerResponse.TestStart();
            _triggerResponse.TestDestroy();

            UnityMessageEventFunctions.InvokeMessageEventWithDispatcher(_triggerResponse.TriggerObject, new TriggerMessage(expectedObject));

            Assert.AreNotSame(expectedObject, _triggerResponse.OnTriggerGameObject);
        }
    }
}

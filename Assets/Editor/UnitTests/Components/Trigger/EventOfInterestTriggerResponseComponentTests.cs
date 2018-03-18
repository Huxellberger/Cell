// Copyright (C) Threetee Gang All Rights Reserved

using Assets.Scripts.Components.Trigger;
using Assets.Scripts.Messaging;
using Assets.Scripts.Services;
using Assets.Scripts.Services.EventsOfInterest;
using Assets.Scripts.Test.Components.Trigger;
using Assets.Scripts.Test.Messaging;
using Assets.Scripts.Test.Services;
using Assets.Scripts.Test.Services.EventsOfInterest;
using NUnit.Framework;
using UnityEngine;

namespace Assets.Editor.UnitTests.Components.Trigger
{
    [TestFixture]
    public class EventOfInterestTriggerResponseComponentTestFixture
    {
        private TestEventOfInterestTriggerResponseComponent _interest;
        private MockEventsOfInterestService _service;

        [SetUp]
        public void BeforeTest()
        {
            new GameObject().AddComponent<TestGameServiceProvider>().TestAwake();

            _service = new MockEventsOfInterestService();

            GameServiceProvider.CurrentInstance.AddService<IEventsOfInterestServiceInterface>(_service);

            _interest = new GameObject().AddComponent<TestEventOfInterestTriggerResponseComponent>();
            _interest.TriggerObject = new GameObject();
            _interest.TriggerObject.AddComponent<TestUnityMessageEventDispatcherComponent>().TestAwake();
            _interest.EventOfInterestNameForTrigger = "Name";
            _interest.EventOfInterestNameForCancelTrigger = "ThisThing";
            _interest.MultiTrigger = true;

            _interest.TestStart();
        }
	
        [TearDown]
        public void AfterTest()
        {
            _interest.TestDestroy();

            _interest = null;

            _service = null;

            GameServiceProvider.ClearGameServiceProvider();
        }
	
        [Test]
        public void ReceivesTrigger_EventOfInterestNameSet_RecordsWithService() 
        {
            UnityMessageEventFunctions.InvokeMessageEventWithDispatcher(_interest.TriggerObject, new TriggerMessage(null));

            Assert.AreEqual(_interest.EventOfInterestNameForTrigger, _service.LastRecordedEvent);
        }

        [Test]
        public void ReceivesTrigger_EventOfInterestNameNotSet_DoesNotRecordWithService()
        {
            _interest.EventOfInterestNameForTrigger = "";
            UnityMessageEventFunctions.InvokeMessageEventWithDispatcher(_interest.TriggerObject, new TriggerMessage(null));

            Assert.IsFalse(_service.EventRecorded);
        }

        [Test]
        public void ReceivesCancelTrigger_EventOfInterestCancelNameSet_RecordsWithService()
        {
            UnityMessageEventFunctions.InvokeMessageEventWithDispatcher(_interest.TriggerObject, new CancelTriggerMessage(null));

            Assert.AreEqual(_interest.EventOfInterestNameForCancelTrigger, _service.LastRecordedEvent);
        }

        [Test]
        public void ReceivesCancelTrigger_EventOfInterestCancelNameNotSet_DoesNotRecordWithService()
        {
            _interest.EventOfInterestNameForCancelTrigger = "";
            UnityMessageEventFunctions.InvokeMessageEventWithDispatcher(_interest.TriggerObject, new CancelTriggerMessage(null));

            Assert.IsFalse(_service.EventRecorded);
        }
    }
}

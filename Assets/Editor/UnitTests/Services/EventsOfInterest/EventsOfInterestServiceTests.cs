// Copyright (C) Threetee Gang All Rights Reserved

using Assets.Scripts.Services.EventsOfInterest;
using NUnit.Framework;

namespace Assets.Editor.UnitTests.Services.EventsOfInterest
{
    [TestFixture]
    public class EventsOfInterestServiceTestFixture 
    {
        private EventsOfInterestService _service;

        private bool _firstEventTriggered;
        private string _firstEventKey;
        private int _firstEventResponseCount;

        private bool _secondEventTriggered;
        private string _secondEventKey;
        private int _secondEventResponseCount;

        [SetUp]
        public void BeforeTest()
        {
            _service = new EventsOfInterestService();

            _firstEventTriggered = false;
            _secondEventTriggered = false;

            _firstEventKey = "";
            _secondEventKey = "";

            _firstEventResponseCount = 0;
            _secondEventResponseCount = 0;
        }
	
        [TearDown]
        public void AfterTest()
        {
            _service = null;
        }
	
        [Test]
        public void ListenForEventOfInterest_DifferentKeyRecorded_NoResponse() 
        {
            _service.ListenForEventOfInterest(new EventOfInterestRegistration("key", FirstResponse, EEventOfInterestType.Persistant));

            _service.RecordEventOfInterest("other key");

            Assert.IsFalse(_firstEventTriggered);
        }


        [Test]
        public void ListenForEventOfInterest_SameKeyRecorded_Response()
        {
            const string expectedKey = "key";
            _service.ListenForEventOfInterest(new EventOfInterestRegistration(expectedKey, FirstResponse, EEventOfInterestType.Persistant));

            _service.RecordEventOfInterest(expectedKey);

            Assert.IsTrue(_firstEventTriggered);
            Assert.AreEqual(expectedKey, _firstEventKey);
        }

        [Test]
        public void ListenForEventOfInterest_SameKeyRecorded_Persistant_MultipleResponses()
        {
            const string expectedKey = "key";
            _service.ListenForEventOfInterest(new EventOfInterestRegistration(expectedKey, FirstResponse, EEventOfInterestType.Persistant));

            _service.RecordEventOfInterest(expectedKey);
            _service.RecordEventOfInterest(expectedKey);

            Assert.AreEqual(2, _firstEventResponseCount);
        }

        [Test]
        public void ListenForEventOfInterest_SameKeyRecorded_OneShot_SingleResponse()
        {
            const string expectedKey = "key";
            _service.ListenForEventOfInterest(new EventOfInterestRegistration(expectedKey, FirstResponse, EEventOfInterestType.OneShot));

            _service.RecordEventOfInterest(expectedKey);
            _service.RecordEventOfInterest(expectedKey);

            Assert.AreEqual(1, _firstEventResponseCount);
        }

        [Test]
        public void ListenForEventOfInterest_SameKeyRecorded_MultipleEvents_AllRespond()
        {
            const string expectedKey = "key";
            _service.ListenForEventOfInterest(new EventOfInterestRegistration(expectedKey, FirstResponse, EEventOfInterestType.Persistant));
            _service.ListenForEventOfInterest(new EventOfInterestRegistration(expectedKey, SecondResponse, EEventOfInterestType.Persistant));

            _service.RecordEventOfInterest(expectedKey);
            _service.RecordEventOfInterest(expectedKey);

            Assert.AreEqual(2, _firstEventResponseCount);
            Assert.AreEqual(2, _secondEventResponseCount);
        }

        [Test]
        public void ListenForEventOfInterest_DifferentKeyRecorded_CorrectRespond()
        {
            const string expectedKey = "key";
            _service.ListenForEventOfInterest(new EventOfInterestRegistration(expectedKey, FirstResponse, EEventOfInterestType.Persistant));
            _service.ListenForEventOfInterest(new EventOfInterestRegistration("bad key", SecondResponse, EEventOfInterestType.Persistant));

            _service.RecordEventOfInterest(expectedKey);

            Assert.IsTrue(_firstEventTriggered);
            Assert.IsFalse(_secondEventTriggered);
        }

        [Test]
        public void ListenForEventOfInterest_SameKeyRecorded_MultipleEventsOfDifferentTypes_AllRespondAsExpected()
        {
            const string expectedKey = "key";
            _service.ListenForEventOfInterest(new EventOfInterestRegistration(expectedKey, FirstResponse, EEventOfInterestType.OneShot));
            _service.ListenForEventOfInterest(new EventOfInterestRegistration(expectedKey, SecondResponse, EEventOfInterestType.Persistant));

            _service.RecordEventOfInterest(expectedKey);
            _service.RecordEventOfInterest(expectedKey);

            Assert.AreEqual(1, _firstEventResponseCount);
            Assert.AreEqual(2, _secondEventResponseCount);
        }

        [Test]
        public void StopListening_NoMoreResponsesReceived()
        {
            const string expectedKey = "key";
            var registration = new EventOfInterestRegistration(expectedKey, FirstResponse, EEventOfInterestType.Persistant);
            _service.ListenForEventOfInterest(registration);
            _service.StopListeningForEventOfInterest(registration);

            _service.RecordEventOfInterest(expectedKey);

            Assert.IsFalse(_firstEventTriggered);
        }

        private void FirstResponse(string key)
        {
            _firstEventTriggered = true;
            _firstEventKey = key;
            _firstEventResponseCount++;
        }

        private void SecondResponse(string key)
        {
            _secondEventTriggered = true;
            _secondEventKey = key;
            _secondEventResponseCount++;
        }
    }
}

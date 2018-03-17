// Copyright (C) Threetee Gang All Rights Reserved

using System.Collections;
using Assets.Scripts.Services;
using Assets.Scripts.Services.EventsOfInterest;
using Assets.Scripts.Test.Components.ActionStateMachine;
using Assets.Scripts.Test.Components.Character;
using Assets.Scripts.Test.Input;
using Assets.Scripts.Test.Services;
using Assets.Scripts.Test.Services.EventsOfInterest;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Assets.Editor.UnitTests.Services.EventsOfInterest
{
    [TestFixture]
    public class PointOfInterestComponentTestFixture
    {
        private TestCharacterComponent _character;
        private TestPointOfInterestComponent _point;
        private MockEventsOfInterestService _interest;

        [SetUp]
        public void BeforeTest()
        {
            var gameObject = new GameObject();
            gameObject.AddComponent<MockInputBinderComponent>();
            gameObject.AddComponent<MockActionStateMachineComponent>();

            _character = gameObject.AddComponent<TestCharacterComponent>();

            _point = new GameObject().AddComponent<TestPointOfInterestComponent>();
            _point.PointOfInterestEventKey = "Test Key";

            _interest = new MockEventsOfInterestService();

            new GameObject().AddComponent<TestGameServiceProvider>().TestAwake();
            GameServiceProvider.CurrentInstance.AddService<IEventsOfInterestServiceInterface>(_interest);
        }
	
        [TearDown]
        public void AfterTest()
        {
            GameServiceProvider.ClearGameServiceProvider();

            _interest = null;
            _point = null;
            _character = null;
        }
	
        [Test]
        public void OnCollides_Null_NoRecordedEvent() 
        {
            _point.TestCollide(null);

            Assert.AreNotEqual(_point.PointOfInterestEventKey, _interest.LastRecordedEvent);
        }

        [Test]
        public void OnCollides_NoCharacterComponent_NoRecordedEvent()
        {
            _point.TestCollide(new GameObject());

            Assert.AreNotEqual(_point.PointOfInterestEventKey, _interest.LastRecordedEvent);
        }

        [Test]
        public void OnCollides_CharacterComponent_RecordsEvent()
        {
            _point.TestCollide(_character.gameObject);

            Assert.AreEqual(_point.PointOfInterestEventKey, _interest.LastRecordedEvent);
        }
    }
}

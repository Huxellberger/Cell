// Copyright (C) Threetee Gang All Rights Reserved

using Assets.Editor.UnitTests.Helpers;
using Assets.Scripts.Components.Trigger;
using Assets.Scripts.Core;
using Assets.Scripts.Messaging;
using Assets.Scripts.Test.Components.Objects.Movable;
using Assets.Scripts.Test.Messaging;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Assets.Editor.UnitTests.Components.Objects.Movable
{
    [TestFixture]
    public class MovableTriggerResponseComponentTestFixture
    {
        private TestMovableTriggerResponseComponent _triggerResponse;

        [SetUp]
        public void BeforeTest()
        {
            _triggerResponse = new GameObject().AddComponent<TestMovableTriggerResponseComponent>();

            _triggerResponse.TriggerObject = new GameObject();
            _triggerResponse.TriggerObject.AddComponent<TestUnityMessageEventDispatcherComponent>().TestAwake();

            _triggerResponse.FinalPosition = new GameObject();
            _triggerResponse.FinalPosition.transform.position = new Vector3(12.0f, -120.0f, 30.0f);
            _triggerResponse.FinalPosition.transform.eulerAngles = new Vector3(30.0f, 2.0f, 4.0f);
            _triggerResponse.MoveDuration = 2.5f;

            _triggerResponse.TestStart();
        }

        [TearDown]
        public void AfterTest()
        {
            _triggerResponse.TestDestroy();

            _triggerResponse = null;
        }

        [Test]
        public void OnTriggerMessage_NoFinalPosition_LogsError()
        {
            LogAssert.Expect(LogType.Error, "No final position set for moving trigger response!");

            _triggerResponse.FinalPosition = null;

            BeginTriggerResponse();
        }

        [Test]
        public void Update_NoTrigger_NoLerp()
        {
            _triggerResponse.TestUpdate(_triggerResponse.MoveDuration + 0.1f);

            Assert.AreEqual(Vector3.zero, _triggerResponse.gameObject.transform.position);
            Assert.AreEqual(Vector3.zero, _triggerResponse.gameObject.transform.eulerAngles);
        }

        [Test]
        public void Update_Trigger_LerpsToExpectedValue()
        {
            const float expectedLerp = 0.5f;
            BeginTriggerResponse();
            _triggerResponse.TestUpdate(_triggerResponse.MoveDuration * expectedLerp);

            Assert.AreEqual(VectorFunctions.LerpVector(Vector3.zero, _triggerResponse.FinalPosition.transform.position, expectedLerp), _triggerResponse.gameObject.transform.position);
            Assert.AreEqual(VectorFunctions.LerpVector(Vector3.zero, _triggerResponse.FinalPosition.transform.eulerAngles, expectedLerp), _triggerResponse.gameObject.transform.eulerAngles);
        }

        [Test]
        public void Update_Trigger_LerpsToFinalPositionMax()
        {
            BeginTriggerResponse();
            _triggerResponse.TestUpdate(_triggerResponse.MoveDuration + 1.0f);

            ExtendedAssertions.AssertVectorsNearlyEqual(_triggerResponse.FinalPosition.transform.position, _triggerResponse.gameObject.transform.position);
            ExtendedAssertions.AssertVectorsNearlyEqual(_triggerResponse.FinalPosition.transform.eulerAngles, _triggerResponse.gameObject.transform.eulerAngles);
        }

        [Test]
        public void Update_TriggerAfterCancel_LerpsToFinalPositionAfterDelay()
        {
            _triggerResponse.MultiTrigger = true;

            const float expectedMoveDuration = 0.5f;
            const float expectedCancelDuration = 0.6f;

            BeginTriggerResponse();
            _triggerResponse.TestUpdate(_triggerResponse.MoveDuration * expectedMoveDuration);

            BeginCancelTriggerResponse();
            _triggerResponse.TestUpdate(_triggerResponse.MoveDuration * expectedCancelDuration);

            BeginTriggerResponse();
            _triggerResponse.TestUpdate(_triggerResponse.MoveDuration * (expectedMoveDuration + expectedCancelDuration));

            ExtendedAssertions.AssertVectorsNearlyEqual(_triggerResponse.FinalPosition.transform.position, _triggerResponse.gameObject.transform.position);
            ExtendedAssertions.AssertVectorsNearlyEqual(_triggerResponse.FinalPosition.transform.eulerAngles, _triggerResponse.gameObject.transform.eulerAngles);
        }

        [Test]
        public void Update_TriggerAgain_RemainsInFinalPosition()
        {
            BeginTriggerResponse();
            _triggerResponse.TestUpdate(_triggerResponse.MoveDuration + 1.0f);
            BeginTriggerResponse();

            ExtendedAssertions.AssertVectorsNearlyEqual(_triggerResponse.FinalPosition.transform.position, _triggerResponse.gameObject.transform.position);
            ExtendedAssertions.AssertVectorsNearlyEqual(_triggerResponse.FinalPosition.transform.eulerAngles, _triggerResponse.gameObject.transform.eulerAngles);
        }

        [Test]
        public void Update_ZeroDuration_InstantTransform()
        {
            _triggerResponse.MoveDuration = 0.0f;
            BeginTriggerResponse();
            _triggerResponse.TestUpdate(1.0f);

            ExtendedAssertions.AssertVectorsNearlyEqual(_triggerResponse.FinalPosition.transform.position, _triggerResponse.gameObject.transform.position);
            ExtendedAssertions.AssertVectorsNearlyEqual(_triggerResponse.FinalPosition.transform.eulerAngles, _triggerResponse.gameObject.transform.eulerAngles);
        }

        [Test]
        public void Update_TriggerParent_LerpsToFinalPositionMax()
        {
            var expectedFinalPosition = _triggerResponse.FinalPosition.transform.position;
            var expectedFinalRotation = _triggerResponse.FinalPosition.transform.eulerAngles;

            _triggerResponse.FinalPosition.transform.parent = _triggerResponse.transform;
            
            BeginTriggerResponse();
            _triggerResponse.TestUpdate(_triggerResponse.MoveDuration * 0.5f);
            _triggerResponse.TestUpdate(_triggerResponse.MoveDuration * 0.5f);

            ExtendedAssertions.AssertVectorsNearlyEqual(expectedFinalPosition, _triggerResponse.gameObject.transform.position);
            ExtendedAssertions.AssertVectorsNearlyEqual(expectedFinalRotation, _triggerResponse.gameObject.transform.eulerAngles);
        }

        [Test]
        public void Update_CancelTrigger_LerpsToStartAfterOriginalDurationElapsed()
        {
            const float expectedLerp = 0.5f;

            _triggerResponse.MultiTrigger = true;

            BeginTriggerResponse();
            _triggerResponse.TestUpdate(_triggerResponse.MoveDuration * expectedLerp);

            BeginCancelTriggerResponse();
            _triggerResponse.TestUpdate((_triggerResponse.MoveDuration * expectedLerp) + 0.1f);

            Assert.AreEqual(Vector3.zero, _triggerResponse.gameObject.transform.position);
            Assert.AreEqual(Vector3.zero, _triggerResponse.gameObject.transform.eulerAngles);
        }

        [Test]
        public void Update_CancelTrigger_NoMulti_DoesNotLerpToStartAfterOriginalDurationElapsed()
        {
            const float expectedLerp = 0.5f;

            _triggerResponse.MultiTrigger = false;

            BeginTriggerResponse();
            _triggerResponse.TestUpdate(_triggerResponse.MoveDuration * expectedLerp);

            BeginCancelTriggerResponse();
            _triggerResponse.TestUpdate((_triggerResponse.MoveDuration * expectedLerp) + 0.1f);

            ExtendedAssertions.AssertVectorsNearlyEqual(_triggerResponse.FinalPosition.transform.position, _triggerResponse.gameObject.transform.position);
            ExtendedAssertions.AssertVectorsNearlyEqual(_triggerResponse.FinalPosition.gameObject.transform.eulerAngles, _triggerResponse.gameObject.transform.eulerAngles);
        }

        [Test]
        public void Update_CancelTrigger_RemainsAtStart()
        {
            _triggerResponse.MultiTrigger = true;

            BeginCancelTriggerResponse();
            _triggerResponse.TestUpdate(_triggerResponse.MoveDuration);

            Assert.AreEqual(Vector3.zero, _triggerResponse.gameObject.transform.position);
            Assert.AreEqual(Vector3.zero, _triggerResponse.gameObject.transform.eulerAngles);
        }

        private void BeginTriggerResponse()
        {
            UnityMessageEventFunctions.InvokeMessageEventWithDispatcher(_triggerResponse.TriggerObject.gameObject, new TriggerMessage(null));
        }

        private void BeginCancelTriggerResponse()
        {
            UnityMessageEventFunctions.InvokeMessageEventWithDispatcher(_triggerResponse.TriggerObject.gameObject, new CancelTriggerMessage(null));
        }
    }
}

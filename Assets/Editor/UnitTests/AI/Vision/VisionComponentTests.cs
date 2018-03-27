// Copyright (C) Threetee Gang All Rights Reserved

using Assets.Editor.UnitTests.Messaging;
using Assets.Scripts.AI.Vision;
using Assets.Scripts.Components.ActionStateMachine.States.Dead;
using Assets.Scripts.Messaging;
using Assets.Scripts.Test.AI.Vision;
using Assets.Scripts.Test.Messaging;
using NUnit.Framework;
using UnityEngine;

namespace Assets.Editor.UnitTests.AI.Vision
{
    [TestFixture]
    public class VisionComponentTestFixture
    {
        private GameObject _detector;
        private TestVisionComponent _vision;

        [SetUp]
        public void BeforeTest()
        {
            _detector = new GameObject();
            _detector.AddComponent<TestUnityMessageEventDispatcherComponent>().TestAwake();

            var visionObject = new GameObject().AddComponent<PolygonCollider2D>();
            _vision = visionObject.gameObject.AddComponent<TestVisionComponent>();

            _vision.DetectingObject = _detector;
            _vision.TimeUntilDetection = 1.5f;

            _vision.TestAwake();
        }
	
        [TearDown]
        public void AfterTest()
        {
            _vision.TestDestroy();

            _vision = null;
            _detector = null;
        }
	
        [Test]
        public void OnSuspiciousObjectCollides_SendsSightingMessageToDetector()
        {
            _vision.IsSuspiciousResult = true;
            var detectedObject = new GameObject();

            var messageSpy = new UnityTestMessageHandleResponseObject<SuspiciousObjectSightedMessage>();
            var handle =
                UnityMessageEventFunctions.RegisterActionWithDispatcher<SuspiciousObjectSightedMessage>(_detector,
                    messageSpy.OnResponse);

            _vision.TestCollide(detectedObject);

            Assert.IsTrue(messageSpy.ActionCalled);
            Assert.AreSame(detectedObject, messageSpy.MessagePayload.SuspiciousGameObject);

            UnityMessageEventFunctions.UnregisterActionWithDispatcher(_detector, handle);
        }

        [Test]
        public void OnNonSuspiciousObjectCollides_DoesNotSendSightingMessageToDetector()
        {
            _vision.IsSuspiciousResult = false;
            var detectedObject = new GameObject();

            var messageSpy = new UnityTestMessageHandleResponseObject<SuspiciousObjectSightedMessage>();
            var handle =
                UnityMessageEventFunctions.RegisterActionWithDispatcher<SuspiciousObjectSightedMessage>(_detector,
                    messageSpy.OnResponse);

            _vision.TestCollide(detectedObject);

            Assert.IsFalse(messageSpy.ActionCalled);

            UnityMessageEventFunctions.UnregisterActionWithDispatcher(_detector, handle);
        }

        [Test]
        public void OnNonSuspiciousObjectCollides_BecomesSuspicious_SendsSightingMessage()
        {
            _vision.IsSuspiciousResult = false;
            var detectedObject = new GameObject();

            var messageSpy = new UnityTestMessageHandleResponseObject<SuspiciousObjectSightedMessage>();
            var handle =
                UnityMessageEventFunctions.RegisterActionWithDispatcher<SuspiciousObjectSightedMessage>(_detector,
                    messageSpy.OnResponse);

            _vision.TestCollide(detectedObject);

            _vision.IsSuspiciousResult = true;

            _vision.TestUpdate(1.0f);

            Assert.IsTrue(messageSpy.ActionCalled);
            Assert.AreSame(detectedObject, messageSpy.MessagePayload.SuspiciousGameObject);

            UnityMessageEventFunctions.UnregisterActionWithDispatcher(_detector, handle);
        }

        [Test]
        public void OnSuspiciousObjectCollides_UpdatesLessThanDetectionLimit_DoesNotSendDetectionMessageToDetector()
        {
            _vision.IsSuspiciousResult = true;
            var detectedObject = new GameObject();

            var messageSpy = new UnityTestMessageHandleResponseObject<SuspiciousObjectDetectedMessage>();
            var handle =
                UnityMessageEventFunctions.RegisterActionWithDispatcher<SuspiciousObjectDetectedMessage>(_detector,
                    messageSpy.OnResponse);

            _vision.TestCollide(detectedObject);

            _vision.TestUpdate(_vision.TimeUntilDetection - 0.1f);

            Assert.IsFalse(messageSpy.ActionCalled);

            UnityMessageEventFunctions.UnregisterActionWithDispatcher(_detector, handle);
        }

        [Test]
        public void OnSuspiciousObjectCollides_UpdatesGreaterThanDetectionLimit_SendsDetectionMessageToDetector()
        {
            _vision.IsSuspiciousResult = true;
            var detectedObject = new GameObject();

            var messageSpy = new UnityTestMessageHandleResponseObject<SuspiciousObjectDetectedMessage>();
            var handle =
                UnityMessageEventFunctions.RegisterActionWithDispatcher<SuspiciousObjectDetectedMessage>(_detector,
                    messageSpy.OnResponse);

            _vision.TestCollide(detectedObject);

            _vision.TestUpdate(_vision.TimeUntilDetection + 0.1f);

            Assert.IsTrue(messageSpy.ActionCalled);
            Assert.AreSame(detectedObject, messageSpy.MessagePayload.SuspiciousGameObject);

            UnityMessageEventFunctions.UnregisterActionWithDispatcher(_detector, handle);
        }

        [Test]
        public void OnNonSuspiciousObjectCollides_UpdatesGreaterThanDetectionLimit_DoesNotSendDetectionMessageToDetector()
        {
            _vision.IsSuspiciousResult = false;
            var detectedObject = new GameObject();

            var messageSpy = new UnityTestMessageHandleResponseObject<SuspiciousObjectDetectedMessage>();
            var handle =
                UnityMessageEventFunctions.RegisterActionWithDispatcher<SuspiciousObjectDetectedMessage>(_detector,
                    messageSpy.OnResponse);

            _vision.TestCollide(detectedObject);

            _vision.TestUpdate(_vision.TimeUntilDetection + 0.1f);

            Assert.IsFalse(messageSpy.ActionCalled);

            UnityMessageEventFunctions.UnregisterActionWithDispatcher(_detector, handle);
        }

        [Test]
        public void OnSuspiciousObjectCollides_UpdateBecomesSuspicious_SendsDetectionMessageToDetector()
        {
            _vision.IsSuspiciousResult = false;
            var detectedObject = new GameObject();

            var messageSpy = new UnityTestMessageHandleResponseObject<SuspiciousObjectDetectedMessage>();
            var handle =
                UnityMessageEventFunctions.RegisterActionWithDispatcher<SuspiciousObjectDetectedMessage>(_detector,
                    messageSpy.OnResponse);

            _vision.TestCollide(detectedObject);

            _vision.IsSuspiciousResult = true;

            _vision.TestUpdate(_vision.TimeUntilDetection + 0.1f);

            Assert.IsFalse(messageSpy.ActionCalled);

            _vision.TestUpdate(_vision.TimeUntilDetection + 0.1f);

            Assert.IsTrue(messageSpy.ActionCalled);
            Assert.AreSame(detectedObject, messageSpy.MessagePayload.SuspiciousGameObject);

            UnityMessageEventFunctions.UnregisterActionWithDispatcher(_detector, handle);
        }

        [Test]
        public void OnSuspiciousObjectCollides_UpdateTogglesSuspicions_SendsExpectedMessages()
        {
            _vision.IsSuspiciousResult = false;
            var detectedObject = new GameObject();

            _vision.TestCollide(detectedObject);

            _vision.IsSuspiciousResult = true;

            _vision.TestUpdate(_vision.TimeUntilDetection + 0.1f);
            _vision.TestUpdate(_vision.TimeUntilDetection + 0.1f);

            var messageSpy = new UnityTestMessageHandleResponseObject<SuspiciousObjectDetectedMessage>();
            var handle =
                UnityMessageEventFunctions.RegisterActionWithDispatcher<SuspiciousObjectDetectedMessage>(_detector,
                    messageSpy.OnResponse);

            _vision.IsSuspiciousResult = false;

            _vision.TestUpdate(_vision.TimeUntilDetection + 0.1f);

            Assert.IsFalse(messageSpy.ActionCalled);

            UnityMessageEventFunctions.UnregisterActionWithDispatcher(_detector, handle);
        }

        [Test]
        public void OnSuspiciousObjectCollides_StopsColliding_SendsNoMessages()
        {
            _vision.IsSuspiciousResult = false;
            var detectedObject = new GameObject();

            var messageSpy = new UnityTestMessageHandleResponseObject<SuspiciousObjectDetectedMessage>();
            var handle =
                UnityMessageEventFunctions.RegisterActionWithDispatcher<SuspiciousObjectDetectedMessage>(_detector,
                    messageSpy.OnResponse);

            _vision.TestCollide(detectedObject);

            _vision.TestStopColliding(detectedObject);

            _vision.IsSuspiciousResult = true;

            _vision.TestUpdate(_vision.TimeUntilDetection + 0.1f);
            _vision.TestUpdate(_vision.TimeUntilDetection + 0.1f);

            Assert.IsFalse(messageSpy.ActionCalled);

            UnityMessageEventFunctions.UnregisterActionWithDispatcher(_detector, handle);
        }

        [Test]
        public void OnSuspiciousObjectCollides_UpdatesNoLongerSuspicious_DoesNotSendDetectionMessage()
        {
            _vision.IsSuspiciousResult = true;
            var detectedObject = new GameObject();

            var messageSpy = new UnityTestMessageHandleResponseObject<SuspiciousObjectDetectedMessage>();
            var handle =
                UnityMessageEventFunctions.RegisterActionWithDispatcher<SuspiciousObjectDetectedMessage>(_detector,
                    messageSpy.OnResponse);

            _vision.TestCollide(detectedObject);

            _vision.IsSuspiciousResult = false;

            _vision.TestUpdate(_vision.TimeUntilDetection + 0.1f);

            Assert.IsFalse(messageSpy.ActionCalled);

            UnityMessageEventFunctions.UnregisterActionWithDispatcher(_detector, handle);
        }

        [Test]
        public void OnSuspiciousObjectCollides_UpdatesGreaterThanDetectionLimit_OneDetectionMessageToDetector()
        {
            _vision.IsSuspiciousResult = true;
            var detectedObject = new GameObject();

            _vision.TestCollide(detectedObject);

            _vision.TestUpdate(_vision.TimeUntilDetection + 0.1f);

            var messageSpy = new UnityTestMessageHandleResponseObject<SuspiciousObjectDetectedMessage>();
            var handle =
                UnityMessageEventFunctions.RegisterActionWithDispatcher<SuspiciousObjectDetectedMessage>(_detector,
                    messageSpy.OnResponse);

            _vision.TestUpdate(_vision.TimeUntilDetection + 0.1f);
            _vision.TestUpdate(_vision.TimeUntilDetection + 0.1f);

            Assert.IsFalse(messageSpy.ActionCalled);

            UnityMessageEventFunctions.UnregisterActionWithDispatcher(_detector, handle);
        }

        [Test]
        public void OnSuspiciousObjectCollides_Updates_SendsDetectionMessageToDetectorForCorrectObject()
        {
            _vision.IsSuspiciousResult = true;
            var detectedObject = new GameObject();
            var laterObject = new GameObject();

            var messageSpy = new UnityTestMessageHandleResponseObject<SuspiciousObjectDetectedMessage>();
            var handle =
                UnityMessageEventFunctions.RegisterActionWithDispatcher<SuspiciousObjectDetectedMessage>(_detector,
                    messageSpy.OnResponse);

            _vision.TestCollide(detectedObject);

            _vision.TestUpdate(_vision.TimeUntilDetection - 0.1f);

            _vision.TestCollide(laterObject);

            _vision.TestUpdate(0.2f);

            Assert.IsTrue(messageSpy.ActionCalled);
            Assert.AreSame(detectedObject, messageSpy.MessagePayload.SuspiciousGameObject);

            _vision.TestUpdate(_vision.TimeUntilDetection + 0.1f);

            Assert.AreSame(laterObject, messageSpy.MessagePayload.SuspiciousGameObject);

            UnityMessageEventFunctions.UnregisterActionWithDispatcher(_detector, handle);
        }

        [Test]
        public void OnSuspiciousObjectCollides_StopsColliding_DoesNotSendDetectionMessage()
        {
            _vision.IsSuspiciousResult = true;
            var detectedObject = new GameObject();

            var messageSpy = new UnityTestMessageHandleResponseObject<SuspiciousObjectDetectedMessage>();
            var handle =
                UnityMessageEventFunctions.RegisterActionWithDispatcher<SuspiciousObjectDetectedMessage>(_detector,
                    messageSpy.OnResponse);

            _vision.TestCollide(detectedObject);

            _vision.TestStopColliding(detectedObject);

            _vision.TestUpdate(_vision.TimeUntilDetection + 0.1f);

            Assert.IsFalse(messageSpy.ActionCalled);

            UnityMessageEventFunctions.UnregisterActionWithDispatcher(_detector, handle);
        }

        [Test]
        public void EnterDeadMessage_SetInactive()
        {
            UnityMessageEventFunctions.InvokeMessageEventWithDispatcher(_detector, new EnterDeadActionStateMessage());
            Assert.IsFalse(_vision.gameObject.activeSelf);
        }

        [Test]
        public void LeftDeadMessage_SetActive()
        {
            UnityMessageEventFunctions.InvokeMessageEventWithDispatcher(_detector, new EnterDeadActionStateMessage());
            UnityMessageEventFunctions.InvokeMessageEventWithDispatcher(_detector, new LeftDeadActionStateMessage());

            Assert.IsTrue(_vision.gameObject.activeSelf);
        }
    }
}

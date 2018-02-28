// Copyright (C) Threetee Gang All Rights Reserved

using Assets.Scripts.Components.Trigger;
using Assets.Scripts.Messaging;
using Assets.Scripts.Test.Components.Trigger;
using Assets.Scripts.Test.Messaging;
using NUnit.Framework;
using UnityEngine;

namespace Assets.Editor.UnitTests.Components.Trigger
{
    [TestFixture]
    public class TactileFeedbackTriggerResponseComponentTestFixture
    {
        private MeshRenderer _meshRenderer;

        private TestTactileFeedbackTriggerResponseComponent _tactile;

        [SetUp]
        public void BeforeTest()
        {
            var initialObject = new GameObject();
            initialObject.AddComponent<AudioSource>();

            initialObject.AddComponent<MeshFilter>();
            _meshRenderer = initialObject.AddComponent<MeshRenderer>();
            _meshRenderer.sharedMaterial = Resources.Load<Material>("Test/Material/BasicMaterial");
            _meshRenderer.sharedMaterial.color = Color.cyan;

            _tactile = initialObject.AddComponent<TestTactileFeedbackTriggerResponseComponent>();
            _tactile.TriggerObject = new GameObject();
            _tactile.TriggerObject.AddComponent<TestUnityMessageEventDispatcherComponent>().TestAwake();
            _tactile.CancelAudioClip = new AudioClip();
            _tactile.TriggerAudioClip = new AudioClip();
            _tactile.TriggerColor = Color.grey;

            _tactile.TestStart();
        }

        [TearDown]
        public void AfterTest()
        {
            _tactile.TestDestroy();

            _tactile = null;

            _meshRenderer = null;
        }

        [Test]
        public void TriggerMessage_PlaysTriggerAudioClip()
        {
            BeginTriggerResponse();

            Assert.AreSame(_tactile.TriggerAudioClip, _tactile.PlayedAudioClip);
        }

        [Test]
        public void CancelTriggerMessage_MultiTrigger_PlaysCancelTriggerAudioClip()
        {
            _tactile.MultiTrigger = true;

            BeginTriggerResponse();
            BeginCancelTriggerResponse();

            Assert.AreSame(_tactile.CancelAudioClip, _tactile.PlayedAudioClip);
        }

        [Test]
        public void CancelTriggerMessage_NoMultiTrigger_DoesNotPlayCancelTriggerAudioClip()
        {
            _tactile.MultiTrigger = false;

            BeginTriggerResponse();
            BeginCancelTriggerResponse();

            Assert.AreSame(_tactile.TriggerAudioClip, _tactile.PlayedAudioClip);
        }

        private void BeginTriggerResponse()
        {
            UnityMessageEventFunctions.InvokeMessageEventWithDispatcher(_tactile.TriggerObject.gameObject, new TriggerMessage(null));
        }

        private void BeginCancelTriggerResponse()
        {
            UnityMessageEventFunctions.InvokeMessageEventWithDispatcher(_tactile.TriggerObject.gameObject, new CancelTriggerMessage(null));
        }
    }
}

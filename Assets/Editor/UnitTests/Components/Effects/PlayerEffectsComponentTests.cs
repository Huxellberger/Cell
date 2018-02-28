// Copyright (C) Threetee Gang All Rights Reserved

using Assets.Scripts.Components.ActionStateMachine.States.Dead;
using Assets.Scripts.Messaging;
using Assets.Scripts.Services.Time;
using Assets.Scripts.Test.Components.ActionStateMachine;
using Assets.Scripts.Test.Components.Character;
using Assets.Scripts.Test.Components.Controller;
using Assets.Scripts.Test.Components.Effects;
using Assets.Scripts.Test.Input;
using Assets.Scripts.Test.Messaging;
using NUnit.Framework;
using UnityEngine;

namespace Assets.Editor.UnitTests.Components.Effects
{
    [TestFixture]
    public class PlayerEffectsComponentTestFixture
    {
        private MockCameraPostProcessingComponent _postProcess;
        private TestPlayerEffectsComponent _effects;

        [SetUp]
        public void BeforeTest()
        {
            var dispatcher = new GameObject().AddComponent<TestUnityMessageEventDispatcherComponent>();
            dispatcher.TestAwake();

            dispatcher.gameObject.AddComponent<MockInputBinderComponent>();
            dispatcher.gameObject.AddComponent<MockActionStateMachineComponent>();

            var character = dispatcher.gameObject.AddComponent<TestCharacterComponent>();
            character.ActiveController = new GameObject().AddComponent<TestControllerComponent>();
            _postProcess = character.ActiveController.gameObject.AddComponent<MockCameraPostProcessingComponent>();

            _effects = dispatcher.gameObject.AddComponent<TestPlayerEffectsComponent>();
            _effects.TestStart();

            // Make different so we can identify values
            _effects.FadeOutTime = _effects.FadeInTime + 1.0f;
        }

        [TearDown]
        public void AfterTest()
        {
            _effects.TestDestroy();

            _postProcess = null;
        }

        [Test]
        public void EnterDeadActionStateMessageReceived_StartsCameraFade()
        {
            UnityMessageEventFunctions.InvokeMessageEventWithDispatcher(_effects.gameObject, new EnterDeadActionStateMessage());

            Assert.AreEqual(1.0f, _postProcess.RequestCameraFadeAlpha);
            Assert.AreEqual(_effects.FadeInTime, _postProcess.RequestCameraFadeFadeTime);
        }

        [Test]
        public void LeftDeadActionStateMessageReceived_StartsCameraFade()
        {
            UnityMessageEventFunctions.InvokeMessageEventWithDispatcher(_effects.gameObject, new LeftDeadActionStateMessage());

            Assert.AreEqual(0.0f, _postProcess.RequestCameraFadeAlpha);
            Assert.AreEqual(_effects.FadeOutTime, _postProcess.RequestCameraFadeFadeTime);
        }

        [Test]
        public void PauseStatusChangedMessageReceived_Paused_InstantFadeWithAlpha()
        {
            UnityMessageEventFunctions.InvokeMessageEventWithDispatcher(_effects.gameObject, new PauseStatusChangedMessage(EPauseStatus.Paused));

            Assert.AreEqual(_effects.PauseAlpha, _postProcess.RequestCameraFadeAlpha);
            Assert.AreEqual(0.0f, _postProcess.RequestCameraFadeFadeTime);
        }

        [Test]
        public void PauseStatusChangedMessageReceived_Unpaused_InstantFadeToZero()
        {
            UnityMessageEventFunctions.InvokeMessageEventWithDispatcher(_effects.gameObject, new PauseStatusChangedMessage(EPauseStatus.Unpaused));

            Assert.AreEqual(0.0f, _postProcess.RequestCameraFadeAlpha);
            Assert.AreEqual(0.0f, _postProcess.RequestCameraFadeFadeTime);
        }
    }
}

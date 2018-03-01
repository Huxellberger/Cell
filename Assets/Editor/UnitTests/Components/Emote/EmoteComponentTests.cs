// Copyright (C) Threetee Gang All Rights Reserved

using Assets.Editor.UnitTests.Messaging;
using Assets.Scripts.Components.Emote;
using Assets.Scripts.Messaging;
using Assets.Scripts.Test.Messaging;
using NUnit.Framework;
using UnityEngine;

namespace Assets.Editor.UnitTests.Components.Emote
{
    [TestFixture]
    public class EmoteComponentTestFixture
    {
        private EmoteComponent _emote;

        [SetUp]
        public void BeforeTest()
        {
            var gameObject = new GameObject();
            gameObject.AddComponent<TestUnityMessageEventDispatcherComponent>().TestAwake();

            _emote = gameObject.AddComponent<EmoteComponent>();
        }

        [TearDown]
        public void AfterTest()
        {
            _emote = null;
        }

        [Test]
        public void GetEmoteState_Start_None()
        {
            Assert.AreEqual(EEmoteState.None, _emote.GetEmoteState());
        }

        [Test]
        public void GetEmoteState_Set_ChangedState()
        {
            const EEmoteState newState = EEmoteState.Alerted;
            _emote.SetEmoteState(newState);

            Assert.AreEqual(newState, _emote.GetEmoteState());
        }

        [Test]
        public void SetEmoteState_Changed_SendsMessageWithNewState()
        {
            var messageSpy = new UnityTestMessageHandleResponseObject<EmoteStatusChangedMessage>();

            var handle =
                UnityMessageEventFunctions.RegisterActionWithDispatcher<EmoteStatusChangedMessage>(_emote.gameObject,
                    messageSpy.OnResponse);

            const EEmoteState newState = EEmoteState.Alerted;
            _emote.SetEmoteState(newState);

            Assert.IsTrue(messageSpy.ActionCalled);
            Assert.AreEqual(newState, messageSpy.MessagePayload.State);

            UnityMessageEventFunctions.UnregisterActionWithDispatcher(_emote.gameObject, handle);
        }

        [Test]
        public void SetEmoteState_NotChanged_DoesNotSendMessageWithNewState()
        {
            var messageSpy = new UnityTestMessageHandleResponseObject<EmoteStatusChangedMessage>();

            var handle =
                UnityMessageEventFunctions.RegisterActionWithDispatcher<EmoteStatusChangedMessage>(_emote.gameObject,
                    messageSpy.OnResponse);

            _emote.SetEmoteState(_emote.GetEmoteState());

            Assert.IsFalse(messageSpy.ActionCalled);

            UnityMessageEventFunctions.UnregisterActionWithDispatcher(_emote.gameObject, handle);
        }
    }
}

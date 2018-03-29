// Copyright (C) Threetee Gang All Rights Reserved 

using Assets.Scripts.Components.Health;
using Assets.Scripts.Messaging;
using Assets.Scripts.Test.Components.Audio;
using Assets.Scripts.Test.Messaging;
using NUnit.Framework;
using UnityEngine;

namespace Assets.Editor.UnitTests.Components.Audio
{
    [TestFixture]
    public class PlayerAudioComponentTestFixture
    {
        private TestPlayerAudioComponent _audio;
        
        [SetUp]
        public void BeforeTest()
        {
            var source = new GameObject().AddComponent<AudioSource>();
            source.gameObject.AddComponent<TestUnityMessageEventDispatcherComponent>().TestAwake();
            _audio = source.gameObject.AddComponent<TestPlayerAudioComponent>();
            _audio.SoundEffectsAudioSource = source;

            _audio.DamageSound = new AudioClip();

            _audio.TestStart();
        }

        [TearDown]
        public void AfterTest()
        {
            _audio.TestDestroy();

            _audio = null;
        }

        [Test]
        public void HealthChangedMessage_HealthLoss_PlaysDamageSound()
        {
            UnityMessageEventFunctions.InvokeMessageEventWithDispatcher(_audio.gameObject, new HealthChangedMessage(-12, 2, null));

            Assert.AreSame(_audio.DamageSound, _audio.PlayedClip);
        }

        [Test]
        public void HealthChangedMessage_NoHealthChange_DoesNotPlayDamageSound()
        {
            UnityMessageEventFunctions.InvokeMessageEventWithDispatcher(_audio.gameObject, new HealthChangedMessage(0, 2, null));

            Assert.IsNull(_audio.PlayedClip);
        }

        [Test]
        public void HealthChangedMessage_HealthGain_DoesNotPlayDamageSound()
        {
            UnityMessageEventFunctions.InvokeMessageEventWithDispatcher(_audio.gameObject, new HealthChangedMessage(12, 2, null));

            Assert.IsNull(_audio.PlayedClip);
        }
    }
}

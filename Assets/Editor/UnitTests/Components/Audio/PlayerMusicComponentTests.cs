// Copyright (C) Threetee Gang All Rights Reserved 

using Assets.Scripts.Components.ActionStateMachine.States.Dead;
using Assets.Scripts.Components.ActionStateMachine.States.Spawning;
using Assets.Scripts.Messaging;
using Assets.Scripts.Test.Components.Audio;
using Assets.Scripts.Test.Messaging;
using NUnit.Framework;
using UnityEngine;

namespace Assets.Editor.UnitTests.Components.Audio
{
    [TestFixture]
    public class PlayerMusicComponentTestFixture
    {
        private TestPlayerMusicComponent _musicComponent;
        private AudioClip _audioClip;

        [SetUp]
        public void BeforeTest()
        {
            _audioClip = new AudioClip();

            var audioSouce = new GameObject().AddComponent<AudioSource>();
            audioSouce.gameObject.AddComponent<TestUnityMessageEventDispatcherComponent>().TestAwake();

            _musicComponent = audioSouce.gameObject.AddComponent<TestPlayerMusicComponent>();
            _musicComponent.DefaultMusic = _audioClip;
            _musicComponent.MusicAudioSource = audioSouce;
            _musicComponent.TestAwake();
        }

        [TearDown]
        public void AfterTest()
        {
            _musicComponent.TestDestroy();
            _musicComponent = null;

            _audioClip = null;
        }

        [Test]
        public void Start_PlaysNoMusic()
        {
            Assert.IsNull(_musicComponent.PlayedAudioClip);
        }

        [Test]
        public void EnterSpawningActionState_PlaysDefaultMusic()
        {
            UnityMessageEventFunctions.InvokeMessageEventWithDispatcher(_musicComponent.gameObject, new EnterSpawningActionStateMessage());

            Assert.AreSame(_audioClip, _musicComponent.PlayedAudioClip);
        }

        [Test]
        public void EnterDeadActionStateMessage_StopsPlayingMusic()
        {
            UnityMessageEventFunctions.InvokeMessageEventWithDispatcher(_musicComponent.gameObject, new EnterDeadActionStateMessage());

            Assert.IsTrue(_musicComponent.StopPlayingCalled);
        }
    }
}

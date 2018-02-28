// Copyright (C) Threetee Gang All Rights Reserved

using Assets.Scripts.Components.ActionStateMachine;
using Assets.Scripts.Components.Audio;
using Assets.Scripts.Components.Controller;
using Assets.Scripts.Input;
using Assets.Scripts.Instance;
using Assets.Scripts.Test.Components.ActionStateMachine;
using Assets.Scripts.Test.Components.Character;
using Assets.Scripts.Test.Input;
using Assets.Scripts.Test.Instance;
using NUnit.Framework;
using UnityEngine;

namespace Assets.Editor.UnitTests.Components.Character
{
    [TestFixture]
    public class CharacterComponentTestFixture
    {
        private TestCharacterComponent _character;
        private MockActionStateMachineComponent _stateMachine;
        private MockInputBinderComponent _inputBinder;

        private PlayerMusicComponent _music;
        private AudioSource _controllerAudio;

        [SetUp]
        public void BeforeTest()
        {
            _stateMachine = new GameObject().AddComponent<MockActionStateMachineComponent>();
            _inputBinder = _stateMachine.gameObject.AddComponent<MockInputBinderComponent>();

            _character = _stateMachine.gameObject.AddComponent<TestCharacterComponent>();
            _music = _character.gameObject.AddComponent<PlayerMusicComponent>();

            var controller = new GameObject().AddComponent<ControllerComponent>();
            _controllerAudio = controller.gameObject.AddComponent<AudioSource>();

            _character.ActiveController = controller;

            var instance = new GameObject();
            instance.AddComponent<MockInputComponent>();
            instance.AddComponent<TestGameInstance>().TestAwake();
        }

        [TearDown]
        public void AfterTest()
        {
            GameInstance.ClearGameInstance();

            _controllerAudio = null;
            _music = null;

            _character = null;
            _stateMachine = null;

            
        }

        [Test]
        public void NewControllerSet_SetsMusicComponentToUseControllerAudioSource()
        {
            _character.ActiveController = _controllerAudio.gameObject.GetComponent<ControllerComponent>();

            Assert.AreSame(_controllerAudio, _music.MusicAudioSource);
        }

        [Test]
        public void Start_SetsLocomotionTrackToLocomotionActionStateDefault()
        {
            _character.TestStart();

            Assert.AreEqual(EActionStateId.Locomotion, _stateMachine.RequestedId);
            Assert.AreEqual(EActionStateMachineTrack.Locomotion, _stateMachine.RequestedTrack);
        }

        [Test]
        public void Start_SetsLocomotionTrackToStartingState()
        {
            const EActionStateId expectedStartingActionState = EActionStateId.Spawning;
            _character.StartingState = expectedStartingActionState;
            _character.TestStart();

            Assert.AreEqual(expectedStartingActionState, _stateMachine.RequestedId);
            Assert.AreEqual(EActionStateMachineTrack.Locomotion, _stateMachine.RequestedTrack);
        }

        [Test]
        public void Start_NullState_DoesNotSetState()
        {
            const EActionStateId expectedStartingActionState = EActionStateId.Null;
            _character.StartingState = expectedStartingActionState;
            _character.TestStart();

            Assert.IsNull(_stateMachine.RequestedId);
        }

        [Test]
        public void Start_SetsInputInterfaceFromGameInstance()
        {
            _character.TestStart();

            Assert.AreEqual(GameInstance.CurrentInstance.gameObject.GetComponent<IInputInterface>(), _inputBinder.InputInterface);
        }
    }
}

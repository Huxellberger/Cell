// Copyright (C) Threetee Gang All Rights Reserved

using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.AI.Chatter;
using Assets.Scripts.Localisation;
using Assets.Scripts.Messaging;
using Assets.Scripts.Test.Localisation;
using Assets.Scripts.Test.UI.HUD.Conversation;
using Assets.Scripts.UI.HUD.Conversation;
using Castle.Core.Internal;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Editor.UnitTests.UI.HUD.Conversation
{
    [TestFixture]
    public class DialogueHUDComponentTestFixture
    {
        public const string SpritePath = "Test/Sprites/TestSprite";
        public const string SecondSpritePath = "Test/Sprites/TestSprite2";

        public const string FirstText = "TestuText";
        public const string SecondText = "IAteWayTooMuchCookieMans";

        private TestDialogueHUDComponent _hud;

        private DialogueLineEntry _firstLine;
        private DialogueLineEntry _secondLine;

        private MockLocalisationInterface _localisation;

        private bool _firstDelegateCalled;
        private bool _secondDelegateCalled;

        [SetUp]
        public void BeforeTest()
        {
            _hud = new GameObject().AddComponent<TestDialogueHUDComponent>();

            _hud.DispatcherOverride = new UnityMessageEventDispatcher();
            _hud.DisplayedDialogueText = new GameObject().AddComponent<Text>();
            _hud.NameText = new GameObject().AddComponent<Text>();
            _hud.PortraitImage = new GameObject().AddComponent<Image>();

            _firstDelegateCalled = false;
            _secondDelegateCalled = false;

            _firstLine = new DialogueLineEntry
            {
                DialogueKey = new LocalisationKey("FirstNameSpace", "FirstKey"),
                NameKey = new LocalisationKey("FirstNameySpace", "FirstKey"),
                DialogueSpeed = 2.0f,
                Portrait = Resources.Load<Sprite>(SpritePath),
                TalkNoise = new AudioClip()
            };

            _secondLine = new DialogueLineEntry
            {
                DialogueKey = new LocalisationKey("SecondNameSpace", "SecondKey"),
                NameKey = new LocalisationKey("SecondNameySpace", "SecondKey"),
                DialogueSpeed = 2.0f,
                Portrait = Resources.Load<Sprite>(SecondSpritePath),
                TalkNoise = new AudioClip()
            };

            _localisation = new MockLocalisationInterface();
            _localisation.GetTextForLocalisationKeyResult = new LocalisedText
            (
                new LocalisedTextEntries
                (
                    new List<LocalisedTextEntry>
                    {
                        new LocalisedTextEntry
                        (ELanguageOptions.EnglishUK, FirstText)
                    }
                 )
            );

            LocalisationManager.CurrentLocalisationInterface = _localisation;

            _hud.TestStart();
        }
	
        [TearDown]
        public void AfterTest()
        {
            LocalisationManager.CurrentLocalisationInterface = null;
            _localisation = null;

            _hud.TestDestroy();

            _secondLine = null;
            _firstLine = null;

            _hud = null;
        }
	
        [Test]
        public void Start_FieldsEmpty() 
        {
            Assert.IsTrue(_hud.DisplayedDialogueText.text.IsNullOrEmpty());
            Assert.IsTrue(_hud.NameText.text.IsNullOrEmpty());
        }

        [Test]
        public void Start_Inactive()
        {
            Assert.IsFalse(_hud.isActiveAndEnabled);
        }

        [Test]
        public void NoMessage_Update_NothingUpdated()
        {
            _hud.TestUpdate(12.0f);
            Assert.IsTrue(_hud.DisplayedDialogueText.text.IsNullOrEmpty());
            Assert.IsTrue(_hud.NameText.text.IsNullOrEmpty());
            Assert.IsNull(_hud.PlayedAudioClip);
        }


        [Test]
        public void ReceiveMessage_TextEmpty()
        {
            _hud.DispatcherOverride.InvokeMessageEvent
            (
                new RequestDialogueUIMessage(new List<DialogueLineEntry> { _firstLine, _secondLine }, 1, OnFirstDelegate)
            );

            Assert.IsTrue(_hud.DisplayedDialogueText.text.IsNullOrEmpty());
            Assert.IsNull(_hud.PlayedAudioClip);
        }

        [Test]
        public void ReceiveMessage_NameUpdated()
        {
            _hud.DispatcherOverride.InvokeMessageEvent
            (
                new RequestDialogueUIMessage(new List<DialogueLineEntry> { _firstLine, _secondLine }, 1, OnFirstDelegate)
            );

            Assert.AreEqual(FirstText, _hud.NameText.text);
            Assert.IsNull(_hud.PlayedAudioClip);
        }

        [Test]
        public void ReceiveMessage_ImageUpdated()
        {
            _hud.DispatcherOverride.InvokeMessageEvent
            (
                new RequestDialogueUIMessage(new List<DialogueLineEntry> { _firstLine, _secondLine }, 1, OnFirstDelegate)
            );

            Assert.AreSame(_firstLine.Portrait, _hud.PortraitImage.sprite);
        }

        [Test]
        public void ReceiveMessage_UpdateLessThanTimeToLoadOneCharacter_TextEmpty()
        {
            _hud.DispatcherOverride.InvokeMessageEvent
            (
                new RequestDialogueUIMessage(new List<DialogueLineEntry> { _firstLine, _secondLine }, 1, OnFirstDelegate)
            );

            _hud.TestUpdate(0.01f);

            Assert.IsTrue(_hud.DisplayedDialogueText.text.IsNullOrEmpty());
            Assert.IsNull(_hud.PlayedAudioClip);
        }

        [Test]
        public void ReceiveMessage_UpdateMoreThanTimeToLoadOneCharacter_TextHasExpectedCharacters()
        {
            const float delta = 1.0f;
            _hud.DispatcherOverride.InvokeMessageEvent
            (
                new RequestDialogueUIMessage(new List<DialogueLineEntry> { _firstLine, _secondLine }, 1, OnFirstDelegate)
            );

            _hud.TestUpdate(delta);

            Assert.AreEqual(_hud.DisplayedDialogueText.text, FirstText.Substring(0, (int)(delta * _firstLine.DialogueSpeed)));
        }

        [Test]
        public void ReceiveMessage_UpdateMoreThanTimeToLoadOneCharacter_PlaysChatterSound()
        {
            _hud.DispatcherOverride.InvokeMessageEvent
            (
                new RequestDialogueUIMessage(new List<DialogueLineEntry> { _firstLine, _secondLine }, 1, OnFirstDelegate)
            );

            _hud.TestUpdate(1.0f);

            Assert.AreSame(_firstLine.TalkNoise, _hud.PlayedAudioClip);
        }

        [Test]
        public void ReceiveMessage_UpdateMoreThanTimeToLoadAllCharacters_RemainsOnLine()
        {
            const float delta = 1.0f;
            _hud.DispatcherOverride.InvokeMessageEvent
            (
                new RequestDialogueUIMessage(new List<DialogueLineEntry> { _firstLine, _secondLine }, 1, OnFirstDelegate)
            );

            _hud.TestUpdate(delta * (FirstText.Length / _firstLine.DialogueSpeed) + 0.1f);

            Assert.AreEqual(_hud.DisplayedDialogueText.text, FirstText);
        }

        [Test]
        public void ReceiveMessage_UpdateMoreThanLineDelay_ClearsLine()
        {
            const float delta = 1.0f;
            _hud.DispatcherOverride.InvokeMessageEvent
            (
                new RequestDialogueUIMessage(new List<DialogueLineEntry> { _firstLine, _secondLine }, 1, OnFirstDelegate)
            );

            _hud.TestUpdate(delta * FirstText.Length + 0.1f);
            _hud.TestUpdate(_hud.AdvanceDelay + 0.1f);

            Assert.IsTrue(_hud.DisplayedDialogueText.text.IsNullOrEmpty());
        }

        [Test]
        public void ReceiveMessage_UpdateMoreThanLineDelay_ImageSetToNextLine()
        {
            const float delta = 1.0f;
            _hud.DispatcherOverride.InvokeMessageEvent
            (
                new RequestDialogueUIMessage(new List<DialogueLineEntry> { _firstLine, _secondLine }, 1, OnFirstDelegate)
            );

            _hud.TestUpdate(delta * FirstText.Length + 0.1f);
            _hud.TestUpdate(_hud.AdvanceDelay + 0.1f);

            Assert.AreSame(_secondLine.Portrait, _hud.PortraitImage.sprite);
        }

        [Test]
        public void ReceiveMessage_UpdateMoreThanLineDelay_NameSetToNextLine()
        {
            const float delta = 1.0f;
            _hud.DispatcherOverride.InvokeMessageEvent
            (
                new RequestDialogueUIMessage(new List<DialogueLineEntry> { _firstLine, _secondLine }, 1, OnFirstDelegate)
            );

            _hud.TestUpdate(delta * FirstText.Length + 0.1f);
            _hud.TestUpdate(_hud.AdvanceDelay + 0.1f);

            Assert.AreEqual(_localisation.SubmittedGetTextLocalisationKey, _secondLine.DialogueKey);
        }

        [Test]
        public void ReceiveMessage_UpdateMoreThanLineDelay_PlaysNextLineAudioClip()
        {
            const float delta = 1.0f;
            _hud.DispatcherOverride.InvokeMessageEvent
            (
                new RequestDialogueUIMessage(new List<DialogueLineEntry> { _firstLine, _secondLine }, 1, OnFirstDelegate)
            );

            _hud.TestUpdate(delta * FirstText.Length + 0.1f);
            _hud.TestUpdate(_hud.AdvanceDelay + 0.1f);
            _hud.TestUpdate(delta * FirstText.Length + 0.1f);

            Assert.AreSame(_secondLine.TalkNoise, _hud.PlayedAudioClip);
        }

        [Test]
        public void ReceiveMessage_UpdateMoreThanLineDelayButNotComplete_DoesNotCallDelegate()
        {
            const float delta = 1.0f;
            _hud.DispatcherOverride.InvokeMessageEvent
            (
                new RequestDialogueUIMessage(new List<DialogueLineEntry> { _firstLine, _secondLine }, 1, OnFirstDelegate)
            );

            _hud.TestUpdate(delta * FirstText.Length + 0.1f);
            _hud.TestUpdate(_hud.AdvanceDelay + 0.1f);
            _hud.TestUpdate(delta * FirstText.Length + 0.1f);

            Assert.IsFalse(_firstDelegateCalled);
        }

        [Test]
        public void ReceiveMessage_UpdateToCompletion_CallsDelegate()
        {
            const float delta = 1.0f;
            _hud.DispatcherOverride.InvokeMessageEvent
            (
                new RequestDialogueUIMessage(new List<DialogueLineEntry> { _firstLine, _secondLine }, 1, OnFirstDelegate)
            );

            _hud.TestUpdate(delta * FirstText.Length + 0.1f);
            _hud.TestUpdate(_hud.AdvanceDelay + 0.1f);
            _hud.TestUpdate(delta * FirstText.Length + 0.1f);
            _hud.TestUpdate(_hud.AdvanceDelay + 0.1f);

            Assert.IsTrue(_firstDelegateCalled);
        }

        [Test]
        public void ReceiveMessage_LowerPriority_PlaysDelegateAndIgnores()
        {
            const float delta = 1.0f;
            _hud.DispatcherOverride.InvokeMessageEvent
            (
                new RequestDialogueUIMessage(new List<DialogueLineEntry> { _firstLine, _secondLine }, 2, OnFirstDelegate)
            );

            _hud.DispatcherOverride.InvokeMessageEvent
            (
                new RequestDialogueUIMessage(new List<DialogueLineEntry> { _firstLine, _secondLine }, 1, OnSecondDelegate)
            );

            _hud.TestUpdate(delta * FirstText.Length + 0.1f);
            _hud.TestUpdate(_hud.AdvanceDelay + 0.1f);
            _hud.TestUpdate(delta * FirstText.Length + 0.1f);
            _hud.TestUpdate(_hud.AdvanceDelay + 0.1f);

            Assert.IsTrue(_firstDelegateCalled);
            Assert.IsFalse(_secondDelegateCalled);
        }

        [Test]
        public void ReceiveMessage_HigherPriority_OverridesAndCallsFirstDelegate()
        {
            const float delta = 1.0f;
            _hud.DispatcherOverride.InvokeMessageEvent
            (
                new RequestDialogueUIMessage(new List<DialogueLineEntry> { _firstLine, _secondLine }, 1, OnFirstDelegate)
            );

            _hud.DispatcherOverride.InvokeMessageEvent
            (
                new RequestDialogueUIMessage(new List<DialogueLineEntry> { _firstLine, _secondLine }, 2, OnSecondDelegate)
            );

            Assert.IsTrue(_firstDelegateCalled);

            _hud.TestUpdate(delta * FirstText.Length + 0.1f);
            _hud.TestUpdate(_hud.AdvanceDelay + 0.1f);
            _hud.TestUpdate(delta * FirstText.Length + 0.1f);
            _hud.TestUpdate(_hud.AdvanceDelay + 0.1f);

            Assert.IsTrue(_secondDelegateCalled);
        }

        private void OnFirstDelegate()
        {
            _firstDelegateCalled = true;
        }

        private void OnSecondDelegate()
        {
            _secondDelegateCalled = true;
        }
    }
}

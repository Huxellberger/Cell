// Copyright (C) Threetee Gang All Rights Reserved

using System.Collections.Generic;
using Assets.Scripts.Components.Emote;
using Assets.Scripts.Messaging;
using Assets.Scripts.UI.Local;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Editor.UnitTests.UI.Local
{
    [TestFixture]
    public class EmoteLocalUIElementComponentTestFixture 
    {
        private EmoteLocalUIElementComponent _emote;

        private Image _image;
        private Image _otherImage;

        private UnityMessageEventDispatcher _dispatcher;
	
        [SetUp]
        public void BeforeTest()
        {
            _dispatcher = new UnityMessageEventDispatcher();

            _emote = new GameObject().AddComponent<EmoteLocalUIElementComponent>();

            _image = new GameObject().AddComponent<Image>();
            _otherImage = new GameObject().AddComponent<Image>();

            _emote.EmoteGraphics = new List<EmoteGraphicEntry>
            {
                new EmoteGraphicEntry{DisplayImage = _image, State = EEmoteState.Alerted},
                new EmoteGraphicEntry{DisplayImage = _otherImage, State = EEmoteState.None}
            };

            _emote.OnElementInitialised(_dispatcher);
        }
	
        [TearDown]
        public void AfterTest()
        {
            _emote.OnElementUninitialised();

            _otherImage = null;
            _image = null;

            _emote = null;

            _dispatcher = null;
        }
	
        [Test]
        public void Initialised_SetsAllImagesToInactive() 
        {
            Assert.IsFalse(_image.gameObject.activeSelf);
            Assert.IsFalse(_image.gameObject.activeSelf);
        }

        [Test]
        public void ReceivesStateChangeMessage_RelevantMessageSetActive()
        {
            const EEmoteState state = EEmoteState.Alerted;

            _dispatcher.InvokeMessageEvent(new EmoteStatusChangedUIMessage(state));

            Assert.IsTrue(_emote.EmoteGraphics.Find((image) => image.State == state).DisplayImage.gameObject.activeSelf);
        }

        [Test]
        public void ReceivesStateChangeMessage_OldImageSetInactive()
        {
            const EEmoteState state = EEmoteState.Alerted;

            _dispatcher.InvokeMessageEvent(new EmoteStatusChangedUIMessage(state));
            _dispatcher.InvokeMessageEvent(new EmoteStatusChangedUIMessage(EEmoteState.None));

            Assert.IsFalse(_emote.EmoteGraphics.Find((image) => image.State == state).DisplayImage.gameObject.activeSelf);
        }

        [Test]
        public void ReceivesStateChangeMessage_NoneStateRemainsInactive()
        {
            const EEmoteState state = EEmoteState.None;

            _dispatcher.InvokeMessageEvent(new EmoteStatusChangedUIMessage(state));

            Assert.IsFalse(_emote.EmoteGraphics.Find((image) => image.State == state).DisplayImage.gameObject.activeSelf);
        }
    }
}

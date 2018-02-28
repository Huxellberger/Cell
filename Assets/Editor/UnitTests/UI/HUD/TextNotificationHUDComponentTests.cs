// Copyright (C) Threetee Gang All Rights Reserved

using Assets.Scripts.Components.Health;
using Assets.Scripts.Test.UI.HUD;
using Assets.Scripts.UI.HUD;
using Castle.Core.Internal;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Editor.UnitTests.UI.HUD
{
    [TestFixture]
    public class TextNotificationHUDComponentTestFixture
    {
        private Text _text;
        private Image _image;
        private TestTextNotificationHUDComponent _textNotification;

        [SetUp]
        public void BeforeTest()
        {
            _text = new GameObject().AddComponent<Text>();

            _image = new GameObject().AddComponent<Image>();
            _textNotification = _image.gameObject.AddComponent<TestTextNotificationHUDComponent>();

            _text.transform.parent = _textNotification.transform;
        }

        [TearDown]
        public void AfterTest()
        {
            _textNotification = null;

            _text = null;
        }

        [Test]
        public void OnStart_TextIsEmpty()
        {
            _text.text = "BLAH";

            _textNotification.TestStart();

            Assert.IsTrue(_text.text.IsNullOrEmpty());

            _textNotification.TestDestroy();
        }

        [Test]
        public void OnStart_ImageIsInvisible()
        {
            _image.enabled = true;

            _textNotification.TestStart();

            Assert.IsFalse(_image.enabled);

            _textNotification.TestDestroy();
        }

        [Test]
        public void OnStart_UpdatesTextValueOnReceivingTextSentEvent()
        {
            _textNotification.TestStart();

            const string expectedText = "TEST";

            _textNotification.TestDispatcher.InvokeMessageEvent(new TextNotificationSentUIMessage(expectedText));

            Assert.IsTrue(_text.text.Equals(expectedText));

            _textNotification.TestDestroy();
        }

        [Test]
        public void OnStart_ImageVisibleOnReceivingTextMessage()
        {
            _textNotification.TestStart();

            const string expectedText = "TEST";

            _textNotification.TestDispatcher.InvokeMessageEvent(new TextNotificationSentUIMessage(expectedText));

            Assert.IsTrue(_image.enabled);

            _textNotification.TestDestroy();
        }

        [Test]
        public void OnStart_AdditionalMessagesBlockPriorOnes()
        {
            _textNotification.TestStart();

            const string expectedText = "TEST";
            const string otherText = "TIMMEBUM";

            _textNotification.TestDispatcher.InvokeMessageEvent(new TextNotificationSentUIMessage(otherText));
            _textNotification.TestDispatcher.InvokeMessageEvent(new TextNotificationSentUIMessage(expectedText));

            Assert.IsTrue(_text.text.Equals(expectedText));

            _textNotification.TestDestroy();
        }

        [Test]
        public void OnEnd_DoesNotUpdateTextValueOnReceivingTextSentEvent()
        {
            _textNotification.TestStart();
            _textNotification.TestDestroy();

            const string expectedText = "TEST";

            _textNotification.TestDispatcher.InvokeMessageEvent(new TextNotificationSentUIMessage(expectedText));

            Assert.IsTrue(_text.text.IsNullOrEmpty());
        }

        [Test]
        public void OnStart_LastRemovedTextLeavesEmpty()
        {
            _textNotification.TestStart();

            _textNotification.TestDispatcher.InvokeMessageEvent(new TextNotificationSentUIMessage("TEST"));
            _textNotification.TestDispatcher.InvokeMessageEvent(new TextNotificationClearedUIMessage());

            Assert.IsTrue(_text.text.IsNullOrEmpty());

            _textNotification.TestDestroy();
        }

        [Test]
        public void OnStart_LastRemovedRemovesImage()
        {
            _textNotification.TestStart();

            _textNotification.TestDispatcher.InvokeMessageEvent(new TextNotificationSentUIMessage("TEST"));
            _textNotification.TestDispatcher.InvokeMessageEvent(new TextNotificationClearedUIMessage());

            Assert.IsFalse(_image.enabled);

            _textNotification.TestDestroy();
        }

        [Test]
        public void OnStart_RemovingTextDisplaysPriorMessage()
        {
            _textNotification.TestStart();

            const string expectedText = "TEST";
            const string otherText = "TIMMEBUM";

            _textNotification.TestDispatcher.InvokeMessageEvent(new TextNotificationSentUIMessage(expectedText));
            _textNotification.TestDispatcher.InvokeMessageEvent(new TextNotificationSentUIMessage(otherText));
            _textNotification.TestDispatcher.InvokeMessageEvent(new TextNotificationClearedUIMessage());

            Assert.IsTrue(_text.text.Equals(expectedText));

            _textNotification.TestDestroy();
        }

        [Test]
        public void OnEnd_DoesNotClearText()
        {
            _textNotification.TestStart();
           
            const string expectedText = "TEST";

            _textNotification.TestDispatcher.InvokeMessageEvent(new TextNotificationSentUIMessage(expectedText));

            _textNotification.TestDestroy();

            _textNotification.TestDispatcher.InvokeMessageEvent(new TextNotificationClearedUIMessage());

            Assert.IsTrue(_text.text.Equals(expectedText));
        }
    }
}

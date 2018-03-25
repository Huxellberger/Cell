// Copyright (C) Threetee Gang All Rights Reserved

using Assets.Scripts.Test.UI.HUD;
using Assets.Scripts.UI;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Editor.UnitTests.UI.HUD
{
    [TestFixture]
    public class ToastNotificationHUDComponentTestFixture
    {
        private TestToastNotificationHUDComponent _toast;
        private Text _text;

        [SetUp]
        public void BeforeTest()
        {
            var hudObject = new GameObject();
            hudObject.AddComponent<AudioSource>();
            _toast = hudObject.AddComponent<TestToastNotificationHUDComponent>();

            _text = new GameObject().AddComponent<Text>();

            _toast.DisplayText = _text;
            _toast.TimePerNotification = 2.0f;
            _toast.TestStart();
        }
	
        [TearDown]
        public void AfterTest()
        {
            _toast.TestDestroy();

            _text = null;

            _toast = null;
        }
	
        [Test]
        public void Start_Inactive() 
        {
            Assert.IsFalse(_toast.gameObject.activeSelf);
        }

        [Test]
        public void ReceivesToast_NonePrior_Activates()
        {
            _toast.TestDispatcher.InvokeMessageEvent(new DisplayToastUIMessage("Test", new AudioClip()));
            Assert.IsTrue(_toast.gameObject.activeSelf);
        }

        [Test]
        public void ReceivesToast_NonePrior_UpdatesDisplayText()
        {
            const string expectedMessage = "Toast....YEEESSS";
            _toast.TestDispatcher.InvokeMessageEvent(new DisplayToastUIMessage(expectedMessage, new AudioClip()));
            Assert.AreEqual(expectedMessage, _text.text);
        }

        [Test]
        public void ReceivesToast_NonePrior_PlaysAudio()
        {
            var expectedAudio = new AudioClip();
            _toast.TestDispatcher.InvokeMessageEvent(new DisplayToastUIMessage("Test", expectedAudio));
            Assert.AreSame(expectedAudio, _toast.LastPlayedClip);
        }

        [Test]
        public void ReceivesToast_UpdatesLessThanChangeThreshold_RemainsDisplaying()
        {
            _toast.TestDispatcher.InvokeMessageEvent(new DisplayToastUIMessage("Test", new AudioClip()));

            _toast.TestUpdate(_toast.TimePerNotification * 0.8f);
            Assert.IsTrue(_toast.gameObject.activeSelf);
        }

        [Test]
        public void ReceivesToast_UpdatesMoreThanChangeThreshold_Vanishes()
        {
            _toast.TestDispatcher.InvokeMessageEvent(new DisplayToastUIMessage("Test", new AudioClip()));

            _toast.TestUpdate(_toast.TimePerNotification * 1.1f);
            Assert.IsFalse(_toast.gameObject.activeSelf);
        }

        [Test]
        public void ReceivesToast_AlreadyDisplaying_QueuesNotification()
        {
            const string expectedMessage = "Toast....YEEESSS";
            _toast.TestDispatcher.InvokeMessageEvent(new DisplayToastUIMessage(expectedMessage, new AudioClip()));
            _toast.TestDispatcher.InvokeMessageEvent(new DisplayToastUIMessage("Other message", new AudioClip()));
            Assert.AreEqual(expectedMessage, _text.text);
        }

        [Test]
        public void ReceivesToast_AlreadyDisplaying_DisplaysOnPriorExpiration()
        {
            const string expectedMessage = "Toast....YEEESSS";
            _toast.TestDispatcher.InvokeMessageEvent(new DisplayToastUIMessage("other message", new AudioClip()));
            _toast.TestDispatcher.InvokeMessageEvent(new DisplayToastUIMessage(expectedMessage, new AudioClip()));

            _toast.TestUpdate(_toast.TimePerNotification * 1.1f);
            Assert.AreEqual(expectedMessage, _text.text);
        }
    }
}

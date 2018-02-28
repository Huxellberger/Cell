// Copyright (C) Threetee Gang All Rights Reserved

using Assets.Scripts.Components.Interaction;
using Assets.Scripts.Test.Components.Interaction;
using Assets.Scripts.Test.Localisation;
using Assets.Scripts.Test.UI.HUD;
using Castle.Core.Internal;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Editor.UnitTests.UI.HUD
{
    [TestFixture]
    public class InteractionPromptHUDComponentTestFixture
    {
        private TestLocalisableUIText _text;
        private Text _interactableText;
        private Image _image;
        private TestInteractionPromptHUDComponent _interactionPrompt;

        [SetUp]
        public void BeforeTest()
        {
            _image = new GameObject().AddComponent<Image>();
            _interactionPrompt = _image.gameObject.AddComponent<TestInteractionPromptHUDComponent>();

            _text = new GameObject().AddComponent<TestLocalisableUIText>();
            _text.BlockStart = true;

            _interactableText = new GameObject().AddComponent<Text>();

            _interactionPrompt.InteractionVerbText = _text;
            _interactionPrompt.InteractableNameText = _interactableText;

            _interactionPrompt.TestStart();
        }

        [TearDown]
        public void AfterTest()
        {
            _interactionPrompt.TestDestroy();

            _interactableText = null;
            _text = null;

            _interactionPrompt = null;
            _image = null;
        }

        [Test]
        public void Start_DisablesTextAndImage()
        {
            Assert.IsFalse(_text.enabled);
            Assert.IsFalse(_interactableText.enabled);
            Assert.IsFalse(_image.enabled);
        }

        [Test]
        public void Start_InteractionStatusUpdatedMessage_True_EnablesTextAndImage()
        {
            _interactionPrompt.TestDispatcher.InvokeMessageEvent(new InteractionStatusUpdatedUIMessage(true));

            Assert.IsTrue(_text.enabled);
            Assert.IsTrue(_interactableText.enabled);
            Assert.IsTrue(_image.enabled);
        }

        [Test]
        public void Start_InteractionStatusUpdatedMessage_False_DisablesTextAndImage()
        {
            _interactionPrompt.TestDispatcher.InvokeMessageEvent(new InteractionStatusUpdatedUIMessage(true));
            _interactionPrompt.TestDispatcher.InvokeMessageEvent(new InteractionStatusUpdatedUIMessage(false));

            Assert.IsFalse(_text.enabled);
            Assert.IsFalse(_interactableText.enabled);
            Assert.IsFalse(_image.enabled);
        }

        [Test]
        public void Start_ActiveUpdatedMessage_Null_EmptyTextForNameText()
        {
            _interactableText.text = "HELLO!";
            _interactionPrompt.TestDispatcher.InvokeMessageEvent(new ActiveInteractableUpdatedUIMessage(null));

            Assert.IsTrue(_interactableText.text.IsNullOrEmpty());
        }

        [Test]
        public void Start_ActiveUpdatedMessage_Interactable_GetInteractableNameForNameText()
        {
            var interactable = new GameObject().AddComponent<MockInteractableComponent>();
            interactable.GetInteractableNameResult = "TEST NAME OOOH";
            _interactionPrompt.TestDispatcher.InvokeMessageEvent(new ActiveInteractableUpdatedUIMessage(interactable));

            Assert.IsTrue(_interactableText.text.Equals(interactable.GetInteractableNameResult));
        }
    }
}


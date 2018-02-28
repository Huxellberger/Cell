// Copyright (C) Threetee Gang All Rights Reserved

using System.Collections.Generic;
using Assets.Scripts.Localisation;
using Assets.Scripts.Test.Components.Interaction;
using Assets.Scripts.Test.Localisation;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Assets.Editor.UnitTests.Components.Interaction
{
    [TestFixture]
    public class InteractableComponentTestFixture
    {
        private TestInteractableComponent _interactable;

        [SetUp]
        public void BeforeTest()
        {
            _interactable = new GameObject().AddComponent<TestInteractableComponent>();
            _interactable.LocalisedInteractableKey = new LocalisationKey("TEST", "BIG TEST");
        }

        [TearDown]
        public void AfterTest()
        {
            _interactable = null;
        }

        [Test]
        public void CanInteract_CallsCanInteractImpl()
        {
            _interactable.CanInteract(_interactable.gameObject);

            Assert.IsTrue(_interactable.CanInteractImplCalled);
        }

        [Test]
        public void CanInteract_CallsCanInteractImplWithInteractingGameObject()
        {
            _interactable.CanInteract(_interactable.gameObject);

            Assert.AreSame(_interactable.gameObject, _interactable.CanInteractImplGameObject);
        }

        [Test]
        public void CanInteract_ReturnsCanInteractImplResult()
        {
            _interactable.CanInteractImplResult = false;

            Assert.IsFalse(_interactable.CanInteract(_interactable.gameObject));

            _interactable.CanInteractImplResult = true;

            Assert.IsTrue(_interactable.CanInteract(_interactable.gameObject));
        }

        [Test]
        public void OnInteract_CannotInteract_LogsErrorAndDoesNotCallImpl()
        {
            _interactable.CanInteractImplResult = false;

            LogAssert.Expect(LogType.Error, "Tried to interact when interaction was invalid!");

            _interactable.OnInteract(_interactable.gameObject);

            Assert.IsFalse(_interactable.OnInteractImplCalled);
        }

        [Test]
        public void OnInteract_CanInteract_CallsOnInteractImpl()
        {
            _interactable.CanInteractImplResult = true;

            _interactable.OnInteract(_interactable.gameObject);

            Assert.IsTrue(_interactable.OnInteractImplCalled);
        }

        [Test]
        public void OnInteract_CanInteract_CallsOnInteractImplWithInteractingGameObject()
        {
            _interactable.CanInteractImplResult = true;

            _interactable.OnInteract(_interactable.gameObject);

            Assert.AreSame(_interactable.gameObject, _interactable.OnInteractImplGameObject);
        }

        [Test]
        public void GetInteractableName_UsesStringBasedOnLocalisedTextRef()
        {
            var expectedText = "HALDO";

            var localisationInterface = new MockLocalisationInterface();
            LocalisationManager.CurrentLocalisationInterface = localisationInterface;
            localisationInterface.GetTextForLocalisationKeyResult = new LocalisedText(new LocalisedTextEntries(new List<LocalisedTextEntry>{new LocalisedTextEntry(ELanguageOptions.EnglishUK, expectedText)}));

            Assert.IsTrue(_interactable.GetInteractableName().Equals(expectedText));

            LocalisationManager.CurrentLocalisationInterface = null;
        }
    }
}

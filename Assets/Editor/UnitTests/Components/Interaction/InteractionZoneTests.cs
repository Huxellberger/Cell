// Copyright (C) Threetee Gang All Rights Reserved

using Assets.Scripts.Test.Components.Interaction;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Assets.Editor.UnitTests.Components.Interaction
{
    [TestFixture]
    public class InteractionZoneTestFixture
    {
        private MockInteractableComponent _interactable;
        private MockInteractableComponent _otherInteractable;
        private MockInteractionComponent _interaction;
        private MockInteractionComponent _otherInteraction;
        private TestInteractionZone _zone;

        [SetUp]
        public void BeforeTest()
        {
            _interactable = new GameObject().AddComponent<MockInteractableComponent>();
            _otherInteractable = new GameObject().AddComponent<MockInteractableComponent>();

            _interaction = new GameObject().AddComponent<MockInteractionComponent>();
            _otherInteraction = new GameObject().AddComponent<MockInteractionComponent>();

            _zone = new GameObject().AddComponent<TestInteractionZone>();
            _zone.AttachedInteractable = _interactable.gameObject;
        }

        [TearDown]
        public void AfterTest()
        {
            _zone = null;
            _otherInteraction = null;
            _interaction = null;

            _otherInteractable = null;
            _interactable = null;
        }

        [Test]
        public void Start_NoAttachedInteractable_ErrorThrown()
        {
            _zone.AttachedInteractable = null;

            LogAssert.Expect(LogType.Error, "Failed to retrieve attached interactable!");

            _zone.TestStart();
        }

        [Test]
        public void Start_NoAttachedInteractableInterface_ErrorThrown()
        {
            _zone.AttachedInteractable = _interaction.gameObject;

            LogAssert.Expect(LogType.Error, "Failed to retrieve attached interactable!");

            _zone.TestStart();
        }

        [Test]
        public void OnCollide_AddsActiveInteractableToAttachedInteractable()
        {
            _zone.TestStart();

            _zone.TestCollide(_interaction.gameObject);

            Assert.AreSame(_interactable, _interaction.AddActiveInteractableResult);
        }

        [Test]
        public void OnCollideStop_NotActive_RemovesActiveInteractable()
        {
            _zone.TestStart();

            _interaction.AddActiveInteractable(_interactable);

            _zone.TestCollideStop(_interaction.gameObject);

            Assert.AreSame(_interactable, _interaction.RemoveActiveInteractableResult);
        }

        [Test]
        public void OnCollideStop_Active_RemovesActiveInteractable()
        {
            _zone.TestStart();

            _interaction.AddActiveInteractable(_interactable);

            _interaction.GetActiveInteractableResult = _interactable;

            _zone.TestCollideStop(_interaction.gameObject);

            Assert.AreSame(_interactable, _interaction.RemoveActiveInteractableResult);
        }

        [Test]
        public void OnDisable_RemovesAllMatchingActiveInteractables()
        {
            _zone.TestStart();

            _interaction.GetActiveInteractableResult = _interactable;
            _otherInteraction.GetActiveInteractableResult = _interactable;

            _zone.TestCollide(_interaction.gameObject);
            _zone.TestCollide(_otherInteraction.gameObject);

            _zone.TestDisable();

            Assert.AreSame(_interactable, _interaction.RemoveActiveInteractableResult);
            Assert.AreSame(_interactable, _otherInteraction.RemoveActiveInteractableResult);
        }
    }
}


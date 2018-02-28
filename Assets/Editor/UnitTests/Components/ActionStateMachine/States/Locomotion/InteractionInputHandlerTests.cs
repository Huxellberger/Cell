// Copyright (C) Threetee Gang All Rights Reserved

using Assets.Scripts.Components.ActionStateMachine.States.Locomotion;
using Assets.Scripts.Input;
using Assets.Scripts.Test.Components.Interaction;
using NUnit.Framework;
using UnityEngine;

namespace Assets.Editor.UnitTests.Components.ActionStateMachine.States.Locomotion
{
    [TestFixture]
    public class InteractionInputHandlerTestFixture
    {
        private MockInteractionComponent _interaction;

        [SetUp]
        public void BeforeTest()
        {
            _interaction = new GameObject().AddComponent<MockInteractionComponent>();
        }

        [TearDown]
        public void AfterTest()
        {
            _interaction = null;
        }

        [Test]
        public void OnInteractKey_NoInteractionInterface_Unhandled()
        {
            var handler = new InteractionInputHandler(null);

            Assert.AreEqual(EInputHandlerResult.Unhandled, handler.HandleButtonInput(EInputKey.Interact, true));
        }

        [Test]
        public void OnInteractKey_InteractionInterface_Pressed_Handled()
        {
            var handler = new InteractionInputHandler(_interaction);

            Assert.AreEqual(EInputHandlerResult.Handled, handler.HandleButtonInput(EInputKey.Interact, true));
        }

        [Test]
        public void OnInteractKey_InteractionInterface_Pressed_TryInteractCalled()
        {
            var handler = new InteractionInputHandler(_interaction);

            handler.HandleButtonInput(EInputKey.Interact, true);

            Assert.IsTrue(_interaction.TryInteractCalled);
        }

        [Test]
        public void OnInteractKey_InteractionInterface_NotPressed_Handled()
        {
            var handler = new InteractionInputHandler(_interaction);

            Assert.AreEqual(EInputHandlerResult.Handled, handler.HandleButtonInput(EInputKey.Interact, false));
        }

        [Test]
        public void OnInteractKey_InteractionInterface_NotPressed_TryInteractNotCalled()
        {
            var handler = new InteractionInputHandler(_interaction);

            handler.HandleButtonInput(EInputKey.Interact, false);

            Assert.IsFalse(_interaction.TryInteractCalled);
        }
    }
}

// Copyright (C) Threetee Gang All Rights Reserved

using Assets.Editor.UnitTests.Messaging;
using Assets.Scripts.Components.Interaction;
using Assets.Scripts.Messaging;
using Assets.Scripts.Test.Components.Interaction;
using Assets.Scripts.Test.Messaging;
using NUnit.Framework;
using UnityEngine;

namespace Assets.Editor.UnitTests.Components.Interaction
{
    [TestFixture]
    public class InteractionComponentTestFixture
    {
        private MockInteractableComponent _interactable;
        private MockInteractableComponent _otherInteractable;
        private TestInteractionComponent _interaction;

        [SetUp]
        public void BeforeTest()
        {
            _interactable = new GameObject().AddComponent<MockInteractableComponent>();
            _otherInteractable = new GameObject().AddComponent<MockInteractableComponent>();

            _interaction = new GameObject().AddComponent<TestInteractionComponent>();
            _interaction.gameObject.AddComponent<TestUnityMessageEventDispatcherComponent>().TestAwake();
        }

        [TearDown]
        public void AfterTest()
        {
            _interaction = null;

            _otherInteractable = null;
            _interactable = null;
        }

        [Test]
        public void Created_ActiveInteractableIsNull()
        {
            Assert.IsNull(_interaction.GetActiveInteractable());
        }

        [Test]
        public void Start_SendsInteractionUpdatedMessage()
        {
            var messageSpy = new UnityTestMessageHandleResponseObject<InteractionStatusUpdatedMessage>();

            var handler =
                UnityMessageEventFunctions.RegisterActionWithDispatcher<InteractionStatusUpdatedMessage>(
                    _interaction.gameObject, messageSpy.OnResponse);

            _interaction.TestStart();

            Assert.IsTrue(messageSpy.ActionCalled);
            Assert.IsFalse(messageSpy.MessagePayload.Interactable);

            UnityMessageEventFunctions.UnregisterActionWithDispatcher(_interaction.gameObject, handler);
        }

        [Test]
        public void AddActiveInteractable_ActiveInteractableIsNotUpdated()
        {
            _interaction.AddActiveInteractable(_interactable);

            Assert.IsNull(_interaction.GetActiveInteractable());
        }

        [Test]
        public void AddActiveInteractable_DoesNotSendInteractableUpdatedMessage()
        {
            var messageSpy = new UnityTestMessageHandleResponseObject<ActiveInteractableUpdatedMessage>();

            var handler =
                UnityMessageEventFunctions.RegisterActionWithDispatcher<ActiveInteractableUpdatedMessage>(
                    _interaction.gameObject, messageSpy.OnResponse);

            _interaction.AddActiveInteractable(_interactable);

            Assert.IsFalse(messageSpy.ActionCalled);

            UnityMessageEventFunctions.UnregisterActionWithDispatcher(_interaction.gameObject, handler);
        }

        [Test]
        public void AddActiveInteractable_StatusChanged_DoesNotSendStatusMessage()
        {
            var messageSpy = new UnityTestMessageHandleResponseObject<InteractionStatusUpdatedMessage>();

            var handler =
                UnityMessageEventFunctions.RegisterActionWithDispatcher<InteractionStatusUpdatedMessage>(
                    _interaction.gameObject, messageSpy.OnResponse);

            _interactable.CanInteractResult = true;
            _interaction.AddActiveInteractable(_interactable);

            Assert.IsFalse(messageSpy.ActionCalled);

            UnityMessageEventFunctions.UnregisterActionWithDispatcher(_interaction.gameObject, handler);
        }

        [Test]
        public void RemoveActiveInteractable_CurrentlyActive_DoesNotSendStatusMessage()
        {
            var messageSpy = new UnityTestMessageHandleResponseObject<InteractionStatusUpdatedMessage>();

            _interactable.CanInteractResult = true;
            _interaction.AddActiveInteractable(_interactable);

            _interaction.TestUpdate();

            var handler =
                UnityMessageEventFunctions.RegisterActionWithDispatcher<InteractionStatusUpdatedMessage>(
                    _interaction.gameObject, messageSpy.OnResponse);

            _interaction.RemoveActiveInteractable(_interactable);

            Assert.IsFalse(messageSpy.ActionCalled);

            UnityMessageEventFunctions.UnregisterActionWithDispatcher(_interaction.gameObject, handler);
        }

        [Test]
        public void RemoveActiveInteractable_CurrentlyActive_DoesNotSendInteractableMessage()
        {
            var messageSpy = new UnityTestMessageHandleResponseObject<ActiveInteractableUpdatedMessage>();

            _interactable.CanInteractResult = true;
            _interaction.AddActiveInteractable(_interactable);
            _interaction.TestUpdate();

            var handler =
                UnityMessageEventFunctions.RegisterActionWithDispatcher<ActiveInteractableUpdatedMessage>(
                    _interaction.gameObject, messageSpy.OnResponse);

            _interaction.RemoveActiveInteractable(_interactable);

            Assert.IsFalse(messageSpy.ActionCalled);

            UnityMessageEventFunctions.UnregisterActionWithDispatcher(_interaction.gameObject, handler);
        }

        [Test]
        public void Update_StatusChanged_SendsStatusMessage()
        {
            var messageSpy = new UnityTestMessageHandleResponseObject<InteractionStatusUpdatedMessage>();

            _interactable.CanInteractResult = true;
            _interaction.AddActiveInteractable(_interactable);

            var handler =
                UnityMessageEventFunctions.RegisterActionWithDispatcher<InteractionStatusUpdatedMessage>(
                    _interaction.gameObject, messageSpy.OnResponse);

            _interaction.TestUpdate();

            Assert.IsTrue(messageSpy.ActionCalled);
            Assert.IsTrue(messageSpy.MessagePayload.Interactable);

            UnityMessageEventFunctions.UnregisterActionWithDispatcher(_interaction.gameObject, handler);
        }

        [Test]
        public void Update_InteractableChanged_SendsInteractableUpdatedMessage()
        {
            var messageSpy = new UnityTestMessageHandleResponseObject<ActiveInteractableUpdatedMessage>();

            _interactable.CanInteractResult = true;
            _interaction.AddActiveInteractable(_interactable);

            var handler =
                UnityMessageEventFunctions.RegisterActionWithDispatcher<ActiveInteractableUpdatedMessage>(
                    _interaction.gameObject, messageSpy.OnResponse);

            _interaction.TestUpdate();

            Assert.IsTrue(messageSpy.ActionCalled);
            Assert.AreSame(_interactable, messageSpy.MessagePayload.UpdatedInteractable);

            UnityMessageEventFunctions.UnregisterActionWithDispatcher(_interaction.gameObject, handler);
        }

        [Test]
        public void Update_CanNoLongerInteract_StatusMessage()
        {
            var messageSpy = new UnityTestMessageHandleResponseObject<InteractionStatusUpdatedMessage>();

            _interactable.CanInteractResult = true;
            _interaction.AddActiveInteractable(_interactable);

            _interaction.TestUpdate();

            var handler =
                UnityMessageEventFunctions.RegisterActionWithDispatcher<InteractionStatusUpdatedMessage>(
                    _interaction.gameObject, messageSpy.OnResponse);

            _interactable.CanInteractResult = false;
            _interaction.TestUpdate();

            Assert.IsTrue(messageSpy.ActionCalled);
            Assert.IsFalse(messageSpy.MessagePayload.Interactable);

            UnityMessageEventFunctions.UnregisterActionWithDispatcher(_interaction.gameObject, handler);
        }

        [Test]
        public void Update_CanNoLongerInteract_InteractableMessage()
        {
            var messageSpy = new UnityTestMessageHandleResponseObject<ActiveInteractableUpdatedMessage>();

            _interactable.CanInteractResult = true;
            _interaction.AddActiveInteractable(_interactable);

            _interaction.TestUpdate();

            var handler =
                UnityMessageEventFunctions.RegisterActionWithDispatcher<ActiveInteractableUpdatedMessage>(
                    _interaction.gameObject, messageSpy.OnResponse);

            _interactable.CanInteractResult = false;
            _interaction.TestUpdate();

            Assert.IsTrue(messageSpy.ActionCalled);
            Assert.IsNull(messageSpy.MessagePayload.UpdatedInteractable);

            UnityMessageEventFunctions.UnregisterActionWithDispatcher(_interaction.gameObject, handler);
        }

        [Test]
        public void Update_StatusSame_NoStatusMessage()
        {
            var messageSpy = new UnityTestMessageHandleResponseObject<InteractionStatusUpdatedMessage>();

            _interactable.CanInteractResult = true;
            _interaction.AddActiveInteractable(_interactable);

            _interaction.TestUpdate();

            var handler =
                UnityMessageEventFunctions.RegisterActionWithDispatcher<InteractionStatusUpdatedMessage>(
                    _interaction.gameObject, messageSpy.OnResponse);

            _interaction.TestUpdate();

            Assert.IsFalse(messageSpy.ActionCalled);

            UnityMessageEventFunctions.UnregisterActionWithDispatcher(_interaction.gameObject, handler);
        }

        [Test]
        public void Update_ActiveRemoved_StatusMessage()
        {
            var messageSpy = new UnityTestMessageHandleResponseObject<InteractionStatusUpdatedMessage>();

            _interactable.CanInteractResult = true;
            _interaction.AddActiveInteractable(_interactable);

            _interaction.TestUpdate();

            var handler =
                UnityMessageEventFunctions.RegisterActionWithDispatcher<InteractionStatusUpdatedMessage>(
                    _interaction.gameObject, messageSpy.OnResponse);

            _interaction.RemoveActiveInteractable(_interactable);
            _interaction.TestUpdate();

            Assert.IsTrue(messageSpy.ActionCalled);
            Assert.IsFalse(messageSpy.MessagePayload.Interactable);

            UnityMessageEventFunctions.UnregisterActionWithDispatcher(_interaction.gameObject, handler);
        }

        [Test]
        public void Update_ActiveRemoved_InteractableMessage()
        {
            var messageSpy = new UnityTestMessageHandleResponseObject<ActiveInteractableUpdatedMessage>();

            _interactable.CanInteractResult = true;
            _interaction.AddActiveInteractable(_interactable);

            _interaction.TestUpdate();

            var handler =
                UnityMessageEventFunctions.RegisterActionWithDispatcher<ActiveInteractableUpdatedMessage>(
                    _interaction.gameObject, messageSpy.OnResponse);

            _interaction.RemoveActiveInteractable(_interactable);
            _interaction.TestUpdate();

            Assert.IsTrue(messageSpy.ActionCalled);
            Assert.IsNull(messageSpy.MessagePayload.UpdatedInteractable);

            UnityMessageEventFunctions.UnregisterActionWithDispatcher(_interaction.gameObject, handler);
        }

        [Test]
        public void Update_MultipleValidAdded_FirstIsMadeActive()
        {
            _interactable.CanInteractResult = true;
            _interaction.AddActiveInteractable(_interactable);

            _otherInteractable.CanInteractResult = true;
            _interaction.AddActiveInteractable(_otherInteractable);

            _interaction.TestUpdate();

            Assert.AreSame(_interactable, _interaction.GetActiveInteractable());
        }

        [Test]
        public void Update_MultipleAdded_FirstValidInteractableIsMadeActive()
        {
            _interactable.CanInteractResult = false;
            _interaction.AddActiveInteractable(_interactable);

            _otherInteractable.CanInteractResult = true;
            _interaction.AddActiveInteractable(_otherInteractable);

            _interaction.TestUpdate();

            Assert.AreSame(_otherInteractable, _interaction.GetActiveInteractable());
        }

        [Test]
        public void Update_MultipleAddedAndRemoved_RemainingIsMadeActive()
        {
            _interactable.CanInteractResult = true;
            _interaction.AddActiveInteractable(_interactable);

            _otherInteractable.CanInteractResult = true;
            _interaction.AddActiveInteractable(_otherInteractable);

            _interaction.TestUpdate();

            _interaction.RemoveActiveInteractable(_interactable);

            _interaction.TestUpdate();

            Assert.AreSame(_otherInteractable, _interaction.GetActiveInteractable());
        }

        [Test]
        public void GetActiveInteratable_UpdatesActive()
        {
            _interactable.CanInteractResult = true;
            _interaction.AddActiveInteractable(_interactable);

            Assert.AreSame(_interactable, _interaction.GetActiveInteractable());
        }

        [Test]
        public void TryInteract_NullActive_False()
        {
            Assert.IsFalse(_interaction.TryInteract());
        }

        [Test]
        public void TryInteract_ActiveValid_ChecksCanInteract()
        {
            _interaction.AddActiveInteractable(_interactable);

            _interaction.TryInteract();

            Assert.IsTrue(_interactable.CanInteractCalled);
        }

        [Test]
        public void TryInteract_ActiveValid_CannotInteract_False()
        {
            _interactable.CanInteractResult = false;

            _interaction.AddActiveInteractable(_interactable);

            Assert.IsFalse(_interaction.TryInteract());
        }

        [Test]
        public void TryInteract_ActiveValid_CanInteract_CallsOnInteract()
        {
            _interactable.CanInteractResult = true;

            _interaction.AddActiveInteractable(_interactable);

            _interaction.TryInteract();

            Assert.IsTrue(_interactable.OnInteractCalled);
        }

        [Test]
        public void TryInteract_ActiveValid_CanInteract_True()
        {
            _interactable.CanInteractResult = true;

            _interaction.AddActiveInteractable(_interactable);

            Assert.IsTrue(_interaction.TryInteract());
        }

        [Test]
        public void TryInteract_StatusChangedSinceLastUpdate_SendsStatusMessage()
        {
            _interactable.CanInteractResult = true;

            _interaction.AddActiveInteractable(_interactable);
            _interaction.TestUpdate();

            var messageSpy = new UnityTestMessageHandleResponseObject<InteractionStatusUpdatedMessage>();

            var handler =
                UnityMessageEventFunctions.RegisterActionWithDispatcher<InteractionStatusUpdatedMessage>(
                    _interaction.gameObject, messageSpy.OnResponse);

            _interactable.CanInteractResult = false;
            _interaction.TryInteract();

            Assert.IsTrue(messageSpy.ActionCalled);
            Assert.IsFalse(messageSpy.MessagePayload.Interactable);

            UnityMessageEventFunctions.UnregisterActionWithDispatcher(_interaction.gameObject, handler);
        }

        [Test]
        public void TryInteract_StatusChangedSinceLastUpdate_SendsInteractableMessage()
        {
            _interactable.CanInteractResult = true;
            _interaction.AddActiveInteractable(_interactable);

            var messageSpy = new UnityTestMessageHandleResponseObject<ActiveInteractableUpdatedMessage>();

            var handler =
                UnityMessageEventFunctions.RegisterActionWithDispatcher<ActiveInteractableUpdatedMessage>(
                    _interaction.gameObject, messageSpy.OnResponse);

            _interaction.TryInteract();

            Assert.IsTrue(messageSpy.ActionCalled);
            Assert.AreSame(_interactable, messageSpy.MessagePayload.UpdatedInteractable);

            UnityMessageEventFunctions.UnregisterActionWithDispatcher(_interaction.gameObject, handler);
        }
    }
}

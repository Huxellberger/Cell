// Copyright (C) Threetee Gang All Rights Reserved

#if UNITY_EDITOR

using Assets.Editor.UnitTests.Messaging;
using Assets.Scripts.Components.ActionStateMachine;
using Assets.Scripts.Components.Health;
using Assets.Scripts.Test.Components.ActionStateMachine;
using Assets.Scripts.Test.Components.Health;
using Assets.Scripts.Test.Messaging;
using NUnit.Framework;
using UnityEngine;

namespace Assets.Editor.UnitTests.Components.Health
{
    [TestFixture]
    public class HealthComponentTestFixture
    {
        private TestHealthComponent _healthComponent;
        private MockActionStateMachineComponent _actionStateMachineComponent;
        private TestUnityMessageEventDispatcherComponent _dispatcherComponent;

        [SetUp]
        public void BeforeTest()
        {
            _actionStateMachineComponent = new GameObject().AddComponent<MockActionStateMachineComponent>();
            
            _dispatcherComponent = _actionStateMachineComponent.gameObject
                .AddComponent<TestUnityMessageEventDispatcherComponent>();
            _dispatcherComponent.TestAwake();

            _healthComponent = _actionStateMachineComponent.gameObject.AddComponent<TestHealthComponent>();
        }

        [TearDown]
        public void AfterTest()
        {
            _dispatcherComponent = null;
            _actionStateMachineComponent = null;
            _healthComponent = null;
        }

        [Test]
        public void Start_DefaultMaxHealthGreaterThanZero()
        {
            _healthComponent.TestStart();
            Assert.Greater(_healthComponent.GetMaxHealth(), 0);
        }

        [Test]
        public void Start_SendsMaxHealthSetEvent()
        {
            var eventCapture = new UnityTestMessageHandleResponseObject<MaxHealthChangedMessage>();

            var handle = _dispatcherComponent.GetUnityMessageEventDispatcher().RegisterForMessageEvent<MaxHealthChangedMessage>
            (
                eventCapture.OnResponse
            );

            _healthComponent.TestStart();

            Assert.IsTrue(eventCapture.ActionCalled);
            Assert.AreEqual(_healthComponent.InitialHealth, eventCapture.MessagePayload.MaxHealth);

            _dispatcherComponent.GetUnityMessageEventDispatcher().UnregisterForMessageEvent(handle);
        }

        [Test]
        public void Start_SendsHealthChangedEvent()
        {
            var eventCapture = new UnityTestMessageHandleResponseObject<HealthChangedMessage>();

            var handle = _dispatcherComponent.GetUnityMessageEventDispatcher().RegisterForMessageEvent<HealthChangedMessage>
            (
                eventCapture.OnResponse
            );

            _healthComponent.TestStart();

            Assert.IsTrue(eventCapture.ActionCalled);
            Assert.AreEqual(_healthComponent.InitialHealth, eventCapture.MessagePayload.NewHealth);
            Assert.IsNull(eventCapture.MessagePayload.Author);

            _dispatcherComponent.GetUnityMessageEventDispatcher().UnregisterForMessageEvent(handle);
        }

        [Test]
        public void Start_HasMaxHealth()
        {
            _healthComponent.TestStart();

            Assert.AreEqual(_healthComponent.GetCurrentHealth(), _healthComponent.GetMaxHealth());
        }

        [Test]
        public void AdjustHealth_ClampedToMax()
        {
            _healthComponent.TestStart();

            _healthComponent.AdjustHealth(new HealthAdjustmentUnit(100, null));
            Assert.AreEqual(_healthComponent.GetCurrentHealth(), _healthComponent.GetMaxHealth());
        }

        [Test]
        public void AdjustHealth_ChangesHealth()
        {
            _healthComponent.TestStart();

            var expectedHealthChange = _healthComponent.GetCurrentHealth() / 2;

            _healthComponent.AdjustHealth(new HealthAdjustmentUnit(-expectedHealthChange, null));

            Assert.AreEqual(_healthComponent.GetMaxHealth() - expectedHealthChange, _healthComponent.GetCurrentHealth());
        }

        [Test]
        public void AdjustHealth_FiresHealthChangedMessage()
        {
            var expectedAuthor = new GameObject();
            _healthComponent.TestStart();

            var expectedHealthChange = _healthComponent.GetCurrentHealth() / 2;

            var eventCapture = new UnityTestMessageHandleResponseObject<HealthChangedMessage>();

            var handle = _dispatcherComponent.GetUnityMessageEventDispatcher().RegisterForMessageEvent<HealthChangedMessage>
            (
                eventCapture.OnResponse
            );

            _healthComponent.AdjustHealth(new HealthAdjustmentUnit(-expectedHealthChange, expectedAuthor));

            Assert.IsTrue(eventCapture.ActionCalled);
            Assert.AreEqual(_healthComponent.GetMaxHealth() - expectedHealthChange, eventCapture.MessagePayload.NewHealth);
            Assert.AreEqual(-expectedHealthChange, eventCapture.MessagePayload.HealthChange);
            Assert.AreSame(expectedAuthor, eventCapture.MessagePayload.Author);

            _dispatcherComponent.GetUnityMessageEventDispatcher().UnregisterForMessageEvent(handle);
        }

        [Test]
        public void AdjustHealth_HealthChangeDisabled_NoEffect()
        {
            _healthComponent.TestStart();

            _healthComponent.SetHealthChangedEnabled(false, EHealthLockReason.Cinematic);

            _healthComponent.AdjustHealth(new HealthAdjustmentUnit(-1 *(_healthComponent.GetCurrentHealth() / 2), null));

            Assert.AreEqual(_healthComponent.GetMaxHealth(), _healthComponent.GetCurrentHealth());
        }

        [Test]
        public void AdjustHealth_HealthChangeDisabledAndReEnabled_Effective()
        {
            _healthComponent.TestStart();

            const int expectedAdjustment = -2;

            _healthComponent.SetHealthChangedEnabled(false, EHealthLockReason.Cinematic);
            _healthComponent.SetHealthChangedEnabled(true, EHealthLockReason.Cinematic);

            _healthComponent.AdjustHealth(new HealthAdjustmentUnit(expectedAdjustment, null));

            Assert.AreEqual(_healthComponent.GetMaxHealth() + expectedAdjustment, _healthComponent.GetCurrentHealth());
        }

        [Test]
        public void AdjustHealth_HealthChangeDisabled_NoEventFired()
        {
            _healthComponent.TestStart();

            var eventCapture = new UnityTestMessageHandleResponseObject<HealthChangedMessage>();

            var handle = _dispatcherComponent.GetUnityMessageEventDispatcher().RegisterForMessageEvent<HealthChangedMessage>
            (
                eventCapture.OnResponse
            );

            _healthComponent.SetHealthChangedEnabled(false, EHealthLockReason.Cinematic);
            _healthComponent.AdjustHealth(new HealthAdjustmentUnit(-1 * (_healthComponent.GetCurrentHealth() / 2), null));

            Assert.IsFalse(eventCapture.ActionCalled);

            _dispatcherComponent.GetUnityMessageEventDispatcher().UnregisterForMessageEvent(handle);
        }

        [Test]
        public void AdjustHealth_ReachesZero_EntersDeadActionState()
        {
            _healthComponent.TestStart();

            _healthComponent.AdjustHealth(new HealthAdjustmentUnit(_healthComponent.GetMaxHealth() * -1, null));

            Assert.AreEqual(EActionStateMachineTrack.Locomotion, _actionStateMachineComponent.RequestedTrack);
            Assert.AreEqual(EActionStateId.Dead, _actionStateMachineComponent.RequestedId);
        }

        [Test]
        public void ReplenishHealth_SetsToMax()
        {
            _healthComponent.TestStart();

            _healthComponent.AdjustHealth(new HealthAdjustmentUnit(-1 * (_healthComponent.GetCurrentHealth() / 2), null));
            _healthComponent.ReplenishHealth();

            Assert.AreEqual(_healthComponent.GetMaxHealth(), _healthComponent.GetCurrentHealth());
        }

        [Test]
        public void ReplenishHealth_HealthChangeDisabled_NoEffect()
        {
            _healthComponent.TestStart();

            _healthComponent.SetHealthChangedEnabled(false, EHealthLockReason.Cinematic);
            _healthComponent.AdjustHealth(new HealthAdjustmentUnit(-1 * (_healthComponent.GetCurrentHealth() / 2), null));
            _healthComponent.ReplenishHealth();

            Assert.AreEqual(_healthComponent.GetMaxHealth(), _healthComponent.GetCurrentHealth());
        }
    }
}

#endif

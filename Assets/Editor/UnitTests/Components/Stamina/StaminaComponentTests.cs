// Copyright (C) Threetee Gang All Rights Reserved

using Assets.Editor.UnitTests.Messaging;
using Assets.Scripts.Components.Stamina;
using Assets.Scripts.Messaging;
using Assets.Scripts.Test.Components.Stamina;
using Assets.Scripts.Test.Messaging;
using NUnit.Framework;
using UnityEngine;

namespace Assets.Editor.UnitTests.Components.Stamina
{
    [TestFixture]
    public class StaminaComponentTestFixture
    {
        private TestStaminaComponent _stamina;

        [SetUp]
        public void BeforeTest()
        {
            var staminaObject = new GameObject();
            staminaObject.AddComponent<TestUnityMessageEventDispatcherComponent>().TestAwake();

            _stamina = staminaObject.AddComponent<TestStaminaComponent>();
        }

        [TearDown]
        public void AfterTest()
        {
            _stamina = null;
        }

        private void UpdateForRegenTime()
        {
            _stamina.TestUpdate(_stamina.RegenRate + 0.1f);
        }

        private void UpdateForBlockTime()
        {
            _stamina.TestUpdate(_stamina.RegenBlockTime + 0.1f);
        }

        #region BasicFunctions
        [Test]
        public void Start_StaminaIsMax()
        {
            _stamina.TestStart();

            Assert.AreEqual(_stamina.InitialStamina, _stamina.GetCurrentStamina());
        }

        [Test]
        public void Start_FiresSetEventWithMaxStamina()
        {
            var messageSpy = new UnityTestMessageHandleResponseObject<StaminaChangedMessage>();

            var handle = 
                UnityMessageEventFunctions.RegisterActionWithDispatcher<StaminaChangedMessage>(_stamina.gameObject, messageSpy.OnResponse);

            _stamina.TestStart();

            Assert.AreEqual(_stamina.InitialStamina, messageSpy.MessagePayload.NewStamina);

            UnityMessageEventFunctions.UnregisterActionWithDispatcher(_stamina.gameObject, handle);
        }

        [Test]
        public void Start_FiresSetMaxEventWithMaxStamina()
        {
            var messageSpy = new UnityTestMessageHandleResponseObject<MaxStaminaChangedMessage>();

            var handle =
                UnityMessageEventFunctions.RegisterActionWithDispatcher<MaxStaminaChangedMessage>(_stamina.gameObject, messageSpy.OnResponse);

            _stamina.TestStart();

            Assert.AreEqual(_stamina.InitialStamina, messageSpy.MessagePayload.NewMaxStamina);

            UnityMessageEventFunctions.UnregisterActionWithDispatcher(_stamina.gameObject, handle);
        }

        [Test]
        public void CanExpendStamina_HasEqualOrMoreThanExpendAmout_True()
        {
            _stamina.TestStart();

            Assert.IsTrue(_stamina.CanExpendStamina(_stamina.InitialStamina));
        }

        [Test]
        public void CanExpendStamina_HasLessThanExpendAmout_False()
        {
            _stamina.TestStart();

            Assert.IsFalse(_stamina.CanExpendStamina(_stamina.InitialStamina + 1));
        }

        [Test]
        public void IsStaminaDepleted_NonZero_False()
        {
            _stamina.TestStart();

            _stamina.AlterStamina((_stamina.GetCurrentStamina() - 1) * -1);

            Assert.IsFalse(_stamina.IsStaminaDepleted());
        }

        [Test]
        public void IsStaminaDepleted_Zero_True()
        {
            _stamina.TestStart();

            _stamina.AlterStamina(_stamina.GetCurrentStamina() * -1);

            Assert.IsTrue(_stamina.IsStaminaDepleted());
        }

        [Test]
        public void AlterStamina_CannotBeReducedBelowZero()
        {
            _stamina.TestStart();

            _stamina.AlterStamina((_stamina.GetCurrentStamina() + 1) * -1);

            Assert.AreEqual(_stamina.GetCurrentStamina(), 0);
        }

        [Test]
        public void AlterStamina_CannotBeRaisedAboveMaxima()
        {
            _stamina.TestStart();

            _stamina.AlterStamina((_stamina.GetCurrentStamina() + 1));

            Assert.AreEqual(_stamina.GetCurrentStamina(), _stamina.InitialStamina);
        }

        [Test]
        public void AlterStamina_UpdatesCurrentAmount()
        {
            _stamina.TestStart();

            const int expectedAdjustAmount = -2;

            _stamina.AlterStamina(expectedAdjustAmount);

            Assert.AreEqual(_stamina.GetCurrentStamina(), _stamina.InitialStamina + expectedAdjustAmount);
        }

        [Test]
        public void AlterStamina_SendUIEventWithNewAmount()
        {
            _stamina.TestStart();

            var messageSpy = new UnityTestMessageHandleResponseObject<StaminaChangedMessage>();

            var handle =
                UnityMessageEventFunctions.RegisterActionWithDispatcher<StaminaChangedMessage>(_stamina.gameObject, messageSpy.OnResponse);

            const int expectedAdjustAmount = -10;

            _stamina.AlterStamina(expectedAdjustAmount);

            Assert.AreEqual(_stamina.InitialStamina + expectedAdjustAmount, messageSpy.MessagePayload.NewStamina);

            UnityMessageEventFunctions.UnregisterActionWithDispatcher(_stamina.gameObject, handle);
        }

        [Test]
        public void AlterStamina_DoesNotSendUIEventIfUnaltered()
        {
            _stamina.TestStart();

            var messageSpy = new UnityTestMessageHandleResponseObject<StaminaChangedMessage>();

            var handle =
                UnityMessageEventFunctions.RegisterActionWithDispatcher<StaminaChangedMessage>(_stamina.gameObject, messageSpy.OnResponse);

            _stamina.AlterStamina(0);

            Assert.IsFalse(messageSpy.ActionCalled);

            UnityMessageEventFunctions.UnregisterActionWithDispatcher(_stamina.gameObject, handle);
        }

        [Test]
        public void RefreshStamina_ResetsToMax()
        {
            _stamina.TestStart();

            _stamina.AlterStamina(-67);

            _stamina.RefreshStamina();

            Assert.AreEqual(_stamina.InitialStamina, _stamina.GetCurrentStamina());
        }

        [Test]
        public void RefreshStamina_SendsUIEvent()
        {
            _stamina.TestStart();

            _stamina.AlterStamina(-20);

            var messageSpy = new UnityTestMessageHandleResponseObject<StaminaChangedMessage>();

            var handle =
                UnityMessageEventFunctions.RegisterActionWithDispatcher<StaminaChangedMessage>(_stamina.gameObject, messageSpy.OnResponse);

            _stamina.RefreshStamina();

            Assert.AreEqual(_stamina.InitialStamina, messageSpy.MessagePayload.NewStamina);

            UnityMessageEventFunctions.UnregisterActionWithDispatcher(_stamina.gameObject, handle);
        }
        #endregion

        #region Regen
        [Test]
        public void AlterStaminaNegative_RegenAfterBlockDelay()
        {
            _stamina.TestStart();

            const int expectedAdjustAmount = -2;

            _stamina.AlterStamina(expectedAdjustAmount);

            UpdateForBlockTime();
            UpdateForRegenTime();

            Assert.AreEqual(_stamina.GetCurrentStamina(), _stamina.InitialStamina + expectedAdjustAmount + 1);
        }

        [Test]
        public void AlterStaminaPositive_RegenInstant()
        {
            _stamina.TestStart();

            const int expectedAdjustAmount = -30;
            const int expectedSecondAdjustAmount = 2;

            _stamina.AlterStamina(expectedAdjustAmount);

            UpdateForBlockTime();
            UpdateForRegenTime();

            _stamina.AlterStamina(expectedSecondAdjustAmount);

            UpdateForRegenTime();

            Assert.AreEqual(_stamina.GetCurrentStamina(), _stamina.InitialStamina + expectedAdjustAmount +  expectedSecondAdjustAmount + 2);
        }

        [Test]
        public void NoAlteration_RegenInstant()
        {
            _stamina.TestStart();

            const int expectedAdjustAmount = -2;

            _stamina.AlterStamina(expectedAdjustAmount);

            UpdateForBlockTime();
            UpdateForRegenTime();
            UpdateForRegenTime();

            Assert.AreEqual(_stamina.GetCurrentStamina(), _stamina.InitialStamina + expectedAdjustAmount + 2);
        }

        [Test]
        public void AlterStaminaNegative_DoNotRegenBeforeBlockDelay()
        {
            _stamina.TestStart();

            const int expectedAdjustAmount = -2;

            _stamina.AlterStamina(expectedAdjustAmount);

            UpdateForRegenTime();

            Assert.AreEqual(_stamina.GetCurrentStamina(), _stamina.InitialStamina + expectedAdjustAmount);
        }
        #endregion

        #region StaminaChangeEnabled
        [Test]
        public void SetStaminaChangeEnabled_False_CanNoLongerBeAdjusted()
        {
            _stamina.TestStart();

            _stamina.SetStaminaChangeEnabled(false, ELockStaminaReason.Dead);

            _stamina.AlterStamina(-2);

            Assert.AreEqual(_stamina.GetCurrentStamina(), _stamina.InitialStamina);
        }

        [Test]
        public void SetStaminaChangeEnabled_FalseWhileRegen_CanNoLongerBeAdjusted()
        {
            _stamina.TestStart();

            _stamina.SetStaminaChangeEnabled(false, ELockStaminaReason.Dead);

            _stamina.AlterStamina(-10);

            UpdateForRegenTime();

            Assert.AreEqual(_stamina.GetCurrentStamina(), _stamina.InitialStamina);
        }

        [Test]
        public void SetStaminaChangeEnabled_FalseThenTrue_CanBeAdjusted()
        {
            _stamina.TestStart();

            _stamina.SetStaminaChangeEnabled(false, ELockStaminaReason.Dead);
            _stamina.SetStaminaChangeEnabled(true, ELockStaminaReason.Dead);

            const int expectedAdjustAmount = -2;

            _stamina.AlterStamina(expectedAdjustAmount);

            Assert.AreEqual(_stamina.GetCurrentStamina(), _stamina.InitialStamina + expectedAdjustAmount);
        }
        #endregion
    }
}

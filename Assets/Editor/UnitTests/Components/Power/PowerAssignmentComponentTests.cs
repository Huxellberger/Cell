// Copyright (C) Threetee Gang All Rights Reserved

using Assets.Editor.UnitTests.Messaging;
using Assets.Scripts.Components.Power;
using Assets.Scripts.Messaging;
using Assets.Scripts.Test.Components.Power;
using Assets.Scripts.Test.Messaging;
using NUnit.Framework;
using UnityEngine;

namespace Assets.Editor.UnitTests.Components.Power
{
    [TestFixture]
    public class PowerAssignmentComponentTestFixture
    {
        private TestPowerAssignmentComponent _assign;
        private MockPowerComponent _power;
        private MockPowerComponent _otherPower;

        [SetUp]
        public void BeforeTest()
        {
            _assign = new GameObject().AddComponent<TestPowerAssignmentComponent>();
            _assign.gameObject.AddComponent<TestUnityMessageEventDispatcherComponent>().TestAwake();

            _power = new GameObject().AddComponent<MockPowerComponent>();
            _otherPower = new GameObject().AddComponent<MockPowerComponent>();
        }
	
        [TearDown]
        public void AfterTest()
        {
            _otherPower = null;
            _power = null;

            _assign = null;
        }
	
        [Test]
        public void SetPower_SendsPowerSetMessageContainingPower()
        {
            const EPowerSetting expectedSetting = EPowerSetting.Primary;

            var messageSpy = new UnityTestMessageHandleResponseObject<PowerSetMessage>();

            var handle =
                UnityMessageEventFunctions.RegisterActionWithDispatcher<PowerSetMessage>(_assign.gameObject,
                    messageSpy.OnResponse);

            _assign.SetPower(_power, expectedSetting);

            UnityMessageEventFunctions.UnregisterActionWithDispatcher(_assign.gameObject, handle);

            Assert.IsTrue(messageSpy.ActionCalled);
            Assert.AreEqual(expectedSetting, messageSpy.MessagePayload.PowerSetting);
            Assert.AreSame(_power, messageSpy.MessagePayload.PowerInterface);
        }

        [Test]
        public void SetPower_SendsPowerUpdateMessageContainingCorrectPowerStatus()
        {
            const EPowerSetting expectedSetting = EPowerSetting.Primary;

            var messageSpy = new UnityTestMessageHandleResponseObject<PowerUpdateMessage>();

            var handle =
                UnityMessageEventFunctions.RegisterActionWithDispatcher<PowerUpdateMessage>(_assign.gameObject,
                    messageSpy.OnResponse);

            _assign.SetPower(_power, expectedSetting);

            UnityMessageEventFunctions.UnregisterActionWithDispatcher(_assign.gameObject, handle);

            Assert.IsTrue(messageSpy.ActionCalled);
            Assert.AreEqual(expectedSetting, messageSpy.MessagePayload.PowerSetting);
            Assert.AreEqual(_power.CanActivatePowerResult, messageSpy.MessagePayload.Active);
            Assert.AreEqual(_power.GetPowerCooldownPercentageResult, messageSpy.MessagePayload.UpdatePercentage);
        }

        [Test]
        public void SetPower_CallsOnPowerSetWithGameObject()
        {
            _assign.SetPower(_power, EPowerSetting.Primary);

            Assert.AreSame(_assign.gameObject, _power.OnPowerSetGameObject);
        }

        [Test]
        public void SetPower_CallsOnPowerClearedWithPriorPowerAssignedToSlot()
        {
            const EPowerSetting expectedSetting = EPowerSetting.Primary;

            _assign.SetPower(_power, expectedSetting);
            _assign.SetPower(_otherPower, expectedSetting);

            Assert.IsTrue(_power.OnPowerClearedCalled);
        }

        [Test]
        public void ActivatePower_NoPowerSet_SendsAttemptMessageRecordingFailure()
        {
            const EPowerSetting expectedSetting = EPowerSetting.Primary;

            var messageSpy = new UnityTestMessageHandleResponseObject<PowerActivationAttemptMessage>();

            var handle =
                UnityMessageEventFunctions.RegisterActionWithDispatcher<PowerActivationAttemptMessage>(_assign.gameObject,
                    messageSpy.OnResponse);

            _assign.ActivatePower(expectedSetting);

            UnityMessageEventFunctions.UnregisterActionWithDispatcher(_assign.gameObject, handle);

            Assert.IsTrue(messageSpy.ActionCalled);
            Assert.AreEqual(expectedSetting, messageSpy.MessagePayload.PowerSetting);
            Assert.AreEqual(false, messageSpy.MessagePayload.Success);
        }

        [Test]
        public void ActivatePower_PowerSet_SendsAttemptMessageRecordingPowerStatus()
        {
            const EPowerSetting expectedSetting = EPowerSetting.Primary;
            _power.CanActivatePowerResult = true;

            var messageSpy = new UnityTestMessageHandleResponseObject<PowerActivationAttemptMessage>();

            var handle =
                UnityMessageEventFunctions.RegisterActionWithDispatcher<PowerActivationAttemptMessage>(_assign.gameObject,
                    messageSpy.OnResponse);

            _assign.SetPower(_power, expectedSetting);
            _assign.ActivatePower(expectedSetting);

            UnityMessageEventFunctions.UnregisterActionWithDispatcher(_assign.gameObject, handle);

            Assert.IsTrue(messageSpy.ActionCalled);
            Assert.AreEqual(expectedSetting, messageSpy.MessagePayload.PowerSetting);
            Assert.AreEqual(_power.CanActivatePowerResult, messageSpy.MessagePayload.Success);
        }

        [Test]
        public void ActivatePower_CanActivatePowerFalse_DoesNotActivatePower()
        {
            const EPowerSetting expectedSetting = EPowerSetting.Primary;
            _power.CanActivatePowerResult = false;

            _assign.SetPower(_power, expectedSetting);
            _assign.ActivatePower(expectedSetting);

            Assert.IsNull(_power.ActivatePowerGameObject);
        }

        [Test]
        public void ActivatePower_CanActivatePowerTrue_DoesActivatePower()
        {
            const EPowerSetting expectedSetting = EPowerSetting.Primary;
            _power.CanActivatePowerResult = true;

            _assign.SetPower(_power, expectedSetting);
            _assign.ActivatePower(expectedSetting);

            Assert.AreSame(_power.ActivatePowerGameObject, _assign.gameObject);
        }

        [Test]
        public void Update_PowerInactive_SendsPowerUpdateMessageContainingCorrectPowerStatus()
        {
            const EPowerSetting expectedSetting = EPowerSetting.Primary;
            _power.CanActivatePowerResult = false;

            _assign.SetPower(_power, expectedSetting);

            var messageSpy = new UnityTestMessageHandleResponseObject<PowerUpdateMessage>();

            var handle =
                UnityMessageEventFunctions.RegisterActionWithDispatcher<PowerUpdateMessage>(_assign.gameObject,
                    messageSpy.OnResponse);

            _assign.TestUpdate();

            UnityMessageEventFunctions.UnregisterActionWithDispatcher(_assign.gameObject, handle);

            Assert.IsTrue(messageSpy.ActionCalled);
            Assert.AreEqual(expectedSetting, messageSpy.MessagePayload.PowerSetting);
            Assert.AreEqual(_power.CanActivatePowerResult, messageSpy.MessagePayload.Active);
            Assert.AreEqual(_power.GetPowerCooldownPercentageResult, messageSpy.MessagePayload.UpdatePercentage);
        }

        [Test]
        public void Update_PowerStateChanges_SendsPowerUpdateMessageContainingCorrectPowerStatus()
        {
            const EPowerSetting expectedSetting = EPowerSetting.Primary;
            _power.CanActivatePowerResult = true;

            _assign.SetPower(_power, expectedSetting);

            var messageSpy = new UnityTestMessageHandleResponseObject<PowerUpdateMessage>();

            var handle =
                UnityMessageEventFunctions.RegisterActionWithDispatcher<PowerUpdateMessage>(_assign.gameObject,
                    messageSpy.OnResponse);

            _power.CanActivatePowerResult = false;
            _assign.TestUpdate();

            UnityMessageEventFunctions.UnregisterActionWithDispatcher(_assign.gameObject, handle);

            Assert.IsTrue(messageSpy.ActionCalled);
            Assert.AreEqual(expectedSetting, messageSpy.MessagePayload.PowerSetting);
            Assert.AreEqual(_power.CanActivatePowerResult, messageSpy.MessagePayload.Active);
            Assert.AreEqual(_power.GetPowerCooldownPercentageResult, messageSpy.MessagePayload.UpdatePercentage);
        }

        [Test]
        public void Update_PowerActive_DoesNotSendsPowerUpdateMessage()
        {
            const EPowerSetting expectedSetting = EPowerSetting.Primary;
            _power.CanActivatePowerResult = true;

            _assign.SetPower(_power, expectedSetting);

            var messageSpy = new UnityTestMessageHandleResponseObject<PowerUpdateMessage>();

            var handle =
                UnityMessageEventFunctions.RegisterActionWithDispatcher<PowerUpdateMessage>(_assign.gameObject,
                    messageSpy.OnResponse);

            _assign.TestUpdate();

            UnityMessageEventFunctions.UnregisterActionWithDispatcher(_assign.gameObject, handle);

            Assert.IsFalse(messageSpy.ActionCalled);
        }
    }
}

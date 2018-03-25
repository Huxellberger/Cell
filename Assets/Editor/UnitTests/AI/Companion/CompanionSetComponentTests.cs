// Copyright (C) Threetee Gang All Rights Reserved

using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Assets.Editor.UnitTests.Messaging;
using Assets.Scripts.AI.Companion;
using Assets.Scripts.Messaging;
using Assets.Scripts.Test.AI.Companion;
using Assets.Scripts.Test.Messaging;
using NUnit.Framework;
using UnityEngine;

namespace Assets.Editor.UnitTests.AI.Companion
{
    [TestFixture]
    public class CompanionSetComponentTestFixture
    {
        public const string SpritePath = "Test/Sprites/TestSprite";

        private TestCompanionSetComponent _set;
        private MockCompanionComponent _companion;
        private MockCompanionComponent _otherCompanion;

        [SetUp]
        public void BeforeTest()
        {
            _set = new GameObject().AddComponent<TestCompanionSetComponent>();
            _set.gameObject.AddComponent<TestUnityMessageEventDispatcherComponent>().TestAwake();

            _set.TestAwake();

            _companion = new GameObject().AddComponent<MockCompanionComponent>();
            _otherCompanion = new GameObject().AddComponent<MockCompanionComponent>();
            _companion.GetCompanionDataResult = new CompanionData
            {
                Image = Resources.Load<Sprite>(SpritePath),
                CompanionPrefabReference = "Test/Prefabs/ExampleCompanion",
                PowerCooldown = 1.4f,
                PowerUseCount = 1
            };
        }
	
        [TearDown]
        public void AfterTest()
        {
            _otherCompanion = null;
            _companion = null;

            _set = null;
        }
	
        [Test]
        public void Update_NoneSet_NoUpdateMessage()
        {
            var messageSpy = new UnityTestMessageHandleResponseObject<CompanionSlotsUpdatedMessage>();

            var handle =
                UnityMessageEventFunctions.RegisterActionWithDispatcher<CompanionSlotsUpdatedMessage>(_set.gameObject,
                    messageSpy.OnResponse);

            _set.TestUpdate();

            Assert.IsFalse(messageSpy.ActionCalled);

            UnityMessageEventFunctions.UnregisterActionWithDispatcher(_set.gameObject, handle);
        }

        [Test]
        public void SetCompanion_CallsSetLeader()
        {
            _set.SetCompanion(_companion, ECompanionSlot.Primary);

            Assert.AreSame(_set.gameObject, _companion.SetLeaderGameObject);
        }

        [Test]
        public void SetCompanion_NoUpdateMessage()
        {
            var messageSpy = new UnityTestMessageHandleResponseObject<CompanionSlotsUpdatedMessage>();

            var handle =
                UnityMessageEventFunctions.RegisterActionWithDispatcher<CompanionSlotsUpdatedMessage>(_set.gameObject,
                    messageSpy.OnResponse);

            _set.SetCompanion(_companion, ECompanionSlot.Primary);

            Assert.IsFalse(messageSpy.ActionCalled);

            UnityMessageEventFunctions.UnregisterActionWithDispatcher(_set.gameObject, handle);
        }

        [Test]
        public void SetCompanion_ClearsPriorLeader()
        {
            _set.SetCompanion(_companion, ECompanionSlot.Primary);
            _set.SetCompanion(_otherCompanion, ECompanionSlot.Primary);

            Assert.IsTrue(_companion.ClearLeaderCalled);
        }

        [Test]
        public void ClearCompanion_ClearLeaderCalled()
        {
            _set.SetCompanion(_companion, ECompanionSlot.Primary);
            _set.ClearCompanion(ECompanionSlot.Primary);

            Assert.IsTrue(_companion.ClearLeaderCalled);
        }

        [Test]
        public void ClearCompanion_ClearsMatchingLeaderOnly()
        {
            _set.SetCompanion(_companion, ECompanionSlot.Primary);
            _set.SetCompanion(_otherCompanion, ECompanionSlot.Secondary);
            _set.ClearCompanion(ECompanionSlot.Primary);

            Assert.IsFalse(_otherCompanion.ClearLeaderCalled);
        }

        [Test]
        public void ClearCompanion_NoUpdateMessage()
        {
            var messageSpy = new UnityTestMessageHandleResponseObject<CompanionSlotsUpdatedMessage>();

            var handle =
                UnityMessageEventFunctions.RegisterActionWithDispatcher<CompanionSlotsUpdatedMessage>(_set.gameObject,
                    messageSpy.OnResponse);

            _set.SetCompanion(_companion, ECompanionSlot.Primary);
            _set.ClearCompanion(ECompanionSlot.Primary);

            Assert.IsFalse(messageSpy.ActionCalled);

            UnityMessageEventFunctions.UnregisterActionWithDispatcher(_set.gameObject, handle);
        }

        [Test]
        public void UseCompanionPower_NoEffectWhenNoCompanion()
        {
            _set.UseCompanionPower(ECompanionSlot.Primary);
        }

        [Test]
        public void UseCompanionPower_CannotUse_UsePowerNotCalledOnCompanion()
        {
            const ECompanionSlot slot = ECompanionSlot.Primary;
            _companion.CanUseCompanionPowerResult = false;
            _set.SetCompanion(_companion, slot);
            _set.UseCompanionPower(slot);

            Assert.IsFalse(_companion.UseCompanionPowerCalled);
        }

        [Test]
        public void UseCompanionPower_CanUse_UsePowerCalledOnCompanion()
        {
            const ECompanionSlot slot = ECompanionSlot.Primary;
            _companion.CanUseCompanionPowerResult = true;
            _set.SetCompanion(_companion, slot);
            _set.UseCompanionPower(slot);

            Assert.IsTrue(_companion.UseCompanionPowerCalled);
        }

        [Test]
        public void RequestCompanionDialogue_RequestDialogueCalledOnCompanion()
        {
            const ECompanionSlot slot = ECompanionSlot.Primary;
            _set.SetCompanion(_companion, slot);
            _set.RequestCompanionDialogue(slot);

            Assert.IsTrue(_companion.RequestDialogueCalled);
        }

        [Test]
        public void RequestCompanionDialogue_NoEffectWhenNoCompanion()
        {
            _set.RequestCompanionDialogue(ECompanionSlot.Primary);
        }

        [Test]
        public void Update_CompanionChanges_UpdateMessageSent()
        {
            const ECompanionSlot slot = ECompanionSlot.Primary;
            var messageSpy = new UnityTestMessageHandleResponseObject<CompanionSlotsUpdatedMessage>();

            var handle =
                UnityMessageEventFunctions.RegisterActionWithDispatcher<CompanionSlotsUpdatedMessage>(_set.gameObject,
                    messageSpy.OnResponse);

            _set.SetCompanion(_companion, slot);
            _set.TestUpdate();

            Assert.IsTrue(messageSpy.ActionCalled);
            Assert.AreEqual(_companion, messageSpy.MessagePayload.Updates[slot].PriorCompanion);
            Assert.AreEqual(_companion.CanUseCompanionPowerResult, messageSpy.MessagePayload.Updates[slot].PriorActive);
            Assert.AreEqual(_companion.GetCompanionDataResult.PowerCooldown, messageSpy.MessagePayload.Updates[slot].PriorCooldown);
            Assert.AreSame(_companion.GetCompanionDataResult.Image, messageSpy.MessagePayload.Updates[slot].PriorUIIcon);
            Assert.AreEqual(_companion.GetCompanionDataResult.PowerUseCount, messageSpy.MessagePayload.Updates[slot].PriorUseCount);

            UnityMessageEventFunctions.UnregisterActionWithDispatcher(_set.gameObject, handle);
        }

        [Test]
        public void Update_ActiveChanges_UpdateMessageSent()
        {
            const ECompanionSlot slot = ECompanionSlot.Primary;
            var messageSpy = new UnityTestMessageHandleResponseObject<CompanionSlotsUpdatedMessage>();

            _set.SetCompanion(_companion, slot);
            _set.TestUpdate();

            var handle =
                UnityMessageEventFunctions.RegisterActionWithDispatcher<CompanionSlotsUpdatedMessage>(_set.gameObject,
                    messageSpy.OnResponse);

            _companion.CanUseCompanionPowerResult = !_companion.CanUseCompanionPowerResult;
            _set.TestUpdate();

            Assert.IsTrue(messageSpy.ActionCalled);
            Assert.AreEqual(_companion, messageSpy.MessagePayload.Updates[slot].PriorCompanion);
            Assert.AreEqual(_companion.CanUseCompanionPowerResult, messageSpy.MessagePayload.Updates[slot].PriorActive);
            Assert.AreEqual(_companion.GetCompanionDataResult.PowerCooldown, messageSpy.MessagePayload.Updates[slot].PriorCooldown);
            Assert.AreSame(_companion.GetCompanionDataResult.Image, messageSpy.MessagePayload.Updates[slot].PriorUIIcon);
            Assert.AreEqual(_companion.GetCompanionDataResult.PowerUseCount, messageSpy.MessagePayload.Updates[slot].PriorUseCount);

            UnityMessageEventFunctions.UnregisterActionWithDispatcher(_set.gameObject, handle);
        }

        [Test]
        public void Update_CooldownChanges_UpdateMessageSent()
        {
            const ECompanionSlot slot = ECompanionSlot.Primary;
            var messageSpy = new UnityTestMessageHandleResponseObject<CompanionSlotsUpdatedMessage>();

            _set.SetCompanion(_companion, slot);
            _set.TestUpdate();

            var handle =
                UnityMessageEventFunctions.RegisterActionWithDispatcher<CompanionSlotsUpdatedMessage>(_set.gameObject,
                    messageSpy.OnResponse);

            _companion.GetCompanionDataResult.PowerCooldown += 1.0f;
            _set.TestUpdate();

            Assert.IsTrue(messageSpy.ActionCalled);
            Assert.AreEqual(_companion, messageSpy.MessagePayload.Updates[slot].PriorCompanion);
            Assert.AreEqual(_companion.CanUseCompanionPowerResult, messageSpy.MessagePayload.Updates[slot].PriorActive);
            Assert.AreEqual(_companion.GetCompanionDataResult.PowerCooldown, messageSpy.MessagePayload.Updates[slot].PriorCooldown);
            Assert.AreSame(_companion.GetCompanionDataResult.Image, messageSpy.MessagePayload.Updates[slot].PriorUIIcon);
            Assert.AreEqual(_companion.GetCompanionDataResult.PowerUseCount, messageSpy.MessagePayload.Updates[slot].PriorUseCount);

            UnityMessageEventFunctions.UnregisterActionWithDispatcher(_set.gameObject, handle);
        }

        [Test]
        public void Update_ImageChanges_UpdateMessageSent()
        {
            const ECompanionSlot slot = ECompanionSlot.Primary;
            var messageSpy = new UnityTestMessageHandleResponseObject<CompanionSlotsUpdatedMessage>();

            _set.SetCompanion(_companion, slot);
            _set.TestUpdate();

            var handle =
                UnityMessageEventFunctions.RegisterActionWithDispatcher<CompanionSlotsUpdatedMessage>(_set.gameObject,
                    messageSpy.OnResponse);

            _companion.GetCompanionDataResult.Image = null;
            _set.TestUpdate();

            Assert.IsTrue(messageSpy.ActionCalled);
            Assert.AreEqual(_companion, messageSpy.MessagePayload.Updates[slot].PriorCompanion);
            Assert.AreEqual(_companion.CanUseCompanionPowerResult, messageSpy.MessagePayload.Updates[slot].PriorActive);
            Assert.AreEqual(_companion.GetCompanionDataResult.PowerCooldown, messageSpy.MessagePayload.Updates[slot].PriorCooldown);
            Assert.AreSame(_companion.GetCompanionDataResult.Image, messageSpy.MessagePayload.Updates[slot].PriorUIIcon);
            Assert.AreEqual(_companion.GetCompanionDataResult.PowerUseCount, messageSpy.MessagePayload.Updates[slot].PriorUseCount);

            UnityMessageEventFunctions.UnregisterActionWithDispatcher(_set.gameObject, handle);
        }

        [Test]
        public void Update_ChargeCountChanges_UpdateMessageSent()
        {
            const ECompanionSlot slot = ECompanionSlot.Primary;
            var messageSpy = new UnityTestMessageHandleResponseObject<CompanionSlotsUpdatedMessage>();

            _set.SetCompanion(_companion, slot);
            _set.TestUpdate();

            var handle =
                UnityMessageEventFunctions.RegisterActionWithDispatcher<CompanionSlotsUpdatedMessage>(_set.gameObject,
                    messageSpy.OnResponse);

            _companion.GetCompanionDataResult.PowerUseCount++;
            _set.TestUpdate();

            Assert.IsTrue(messageSpy.ActionCalled);
            Assert.AreEqual(_companion, messageSpy.MessagePayload.Updates[slot].PriorCompanion);
            Assert.AreEqual(_companion.CanUseCompanionPowerResult, messageSpy.MessagePayload.Updates[slot].PriorActive);
            Assert.AreEqual(_companion.GetCompanionDataResult.PowerCooldown, messageSpy.MessagePayload.Updates[slot].PriorCooldown);
            Assert.AreSame(_companion.GetCompanionDataResult.Image, messageSpy.MessagePayload.Updates[slot].PriorUIIcon);
            Assert.AreEqual(_companion.GetCompanionDataResult.PowerUseCount, messageSpy.MessagePayload.Updates[slot].PriorUseCount);

            UnityMessageEventFunctions.UnregisterActionWithDispatcher(_set.gameObject, handle);
        }

        [Test]
        public void WriteData_WritesExpectedReferences()
        {
            _set.SetCompanion(_companion, ECompanionSlot.Primary);

            var stream = new MemoryStream();

            _set.WriteData(stream);

            var readStream = new MemoryStream(stream.ToArray());

            var bf = new BinaryFormatter();

            Assert.AreEqual(Enum.GetValues(typeof(ECompanionSlot)).Length, bf.Deserialize(readStream));
            Assert.AreEqual(ECompanionSlot.Primary, bf.Deserialize(readStream));
            Assert.AreEqual(_companion.GetCompanionDataResult.CompanionPrefabReference, bf.Deserialize(readStream));
            Assert.AreEqual(ECompanionSlot.Secondary, bf.Deserialize(readStream));
            Assert.AreEqual(CompanionConstants.InvalidCompanion, bf.Deserialize(readStream));
        }

        [Test]
        public void ReadData_SetsExpectedCompanions()
        {
            var messageSpy = new UnityTestMessageHandleResponseObject<CompanionSlotsUpdatedMessage>();

            var handle =
                UnityMessageEventFunctions.RegisterActionWithDispatcher<CompanionSlotsUpdatedMessage>(_set.gameObject,
                    messageSpy.OnResponse);

            _set.SetCompanion(_companion, ECompanionSlot.Primary);

            var stream = new MemoryStream();

            _set.WriteData(stream);

            _set.ClearCompanion(ECompanionSlot.Primary);

            var readStream = new MemoryStream(stream.ToArray());

            _set.ReadData(readStream);

            _set.TestUpdate();

            Assert.IsTrue(messageSpy.ActionCalled);
            Assert.IsNotNull(messageSpy.MessagePayload.Updates[ECompanionSlot.Primary].PriorCompanion);

            UnityMessageEventFunctions.UnregisterActionWithDispatcher(_set.gameObject, handle);
        }
    }
}

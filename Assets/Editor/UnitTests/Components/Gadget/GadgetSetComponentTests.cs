// Copyright (C) Threetee Gang All Rights Reserved

using Assets.Editor.UnitTests.Messaging;
using Assets.Scripts.Components.Gadget;
using Assets.Scripts.Messaging;
using Assets.Scripts.Test.Components.Gadget;
using Assets.Scripts.Test.Messaging;
using NUnit.Framework;
using UnityEngine;

namespace Assets.Editor.UnitTests.Components.Gadget
{
    [TestFixture]
    public class GadgetSetComponentTestFixture
    {
        private TestGadgetSetComponent _set;
        private MockGadgetComponent _gadget;
        private MockGadgetComponent _otherGadget;

        [SetUp]
        public void BeforeTest()
        {
            _set = new GameObject().AddComponent<TestGadgetSetComponent>();
            _set.gameObject.AddComponent<TestUnityMessageEventDispatcherComponent>().TestAwake();

            _gadget = new GameObject().AddComponent<MockGadgetComponent>();
            _otherGadget = new GameObject().AddComponent<MockGadgetComponent>();
        }

        [TearDown]
        public void AfterTest()
        {
            _otherGadget = null;
            _gadget = null;

            _set = null;
        }

        [Test]
        public void CanAddGadget_Null_False()
        {
            _set.TestAwake();

            Assert.IsFalse(_set.CanAddGadget(null));
        }

        [Test]
        public void CanAddGadget_NoSlotForType_False()
        {
            _set.InitialCapacities.Add(new GadgetSlotCapcity {Capacity = 100, Slot = EGadgetSlot.Mine});

            _gadget.GetGadgetSlotResult = EGadgetSlot.NoiseMaker;

            _set.TestAwake();

            Assert.IsFalse(_set.CanAddGadget(_gadget));
        }

        [Test]
        public void CanAddGadget_NoSpace_False()
        {
            const int slotCapacity = 4;
            _set.InitialCapacities.Add(new GadgetSlotCapcity {Capacity = slotCapacity, Slot = EGadgetSlot.Mine});

            _gadget.GetGadgetSlotResult = EGadgetSlot.Mine;

            _set.TestAwake();

            for (var i = 0; i < slotCapacity; i++)
            {
                _set.AddGadget(_gadget);
            }

            Assert.IsFalse(_set.CanAddGadget(_gadget));
        }

        [Test]
        public void CanAddGadget_Space_True()
        {
            const int slotCapacity = 4;
            _set.InitialCapacities.Add(new GadgetSlotCapcity {Capacity = slotCapacity, Slot = EGadgetSlot.Mine});

            _gadget.GetGadgetSlotResult = EGadgetSlot.Mine;

            _set.TestAwake();

            for (var i = 0; i < slotCapacity - 1; i++)
            {
                _set.AddGadget(_gadget);
            }

            Assert.IsTrue(_set.CanAddGadget(_gadget));
        }

        [Test]
        public void AddGadget_SendsUpdateMessage()
        {
            _set.InitialCapacities.Add(new GadgetSlotCapcity {Capacity = 2, Slot = EGadgetSlot.Mine});

            _gadget.GetGadgetSlotResult = EGadgetSlot.Mine;

            _set.TestAwake();

            var messageSpy = new UnityTestMessageHandleResponseObject<GadgetUpdatedMessage>();

            var handle = UnityMessageEventFunctions.RegisterActionWithDispatcher
                <GadgetUpdatedMessage>(_set.gameObject, messageSpy.OnResponse);

            _set.AddGadget(_gadget);

            Assert.IsTrue(messageSpy.ActionCalled);
            Assert.AreSame(_gadget, messageSpy.MessagePayload.NewGadget);
            Assert.AreEqual(1, messageSpy.MessagePayload.SlotCount);

            UnityMessageEventFunctions.UnregisterActionWithDispatcher(_set.gameObject, handle);
        }

        [Test]
        public void AddAnotherGadgetToSlot_SendsUpdateMessageWithNewGadget()
        {
            _set.InitialCapacities.Add(new GadgetSlotCapcity { Capacity = 2, Slot = EGadgetSlot.Mine });

            _gadget.GetGadgetSlotResult = EGadgetSlot.Mine;
            _otherGadget.GetGadgetSlotResult = EGadgetSlot.Mine;

            _set.TestAwake();

            var messageSpy = new UnityTestMessageHandleResponseObject<GadgetUpdatedMessage>();

            _set.AddGadget(_gadget);

            var handle = UnityMessageEventFunctions.RegisterActionWithDispatcher
                <GadgetUpdatedMessage>(_set.gameObject, messageSpy.OnResponse);

            _set.AddGadget(_otherGadget);

            Assert.IsTrue(messageSpy.ActionCalled);
            Assert.AreSame(_otherGadget, messageSpy.MessagePayload.NewGadget);
            Assert.AreEqual(2, messageSpy.MessagePayload.SlotCount);

            UnityMessageEventFunctions.UnregisterActionWithDispatcher(_set.gameObject, handle);
        }

        [Test]
        public void AddAnotherGadgetToDifferentSlot_SendsUpdateMessageWithOriginalGadget()
        {
            _set.InitialCapacities.Add(new GadgetSlotCapcity { Capacity = 2, Slot = EGadgetSlot.Mine });
            _set.InitialCapacities.Add(new GadgetSlotCapcity { Capacity = 2, Slot = EGadgetSlot.NoiseMaker });

            _gadget.GetGadgetSlotResult = EGadgetSlot.Mine;
            _otherGadget.GetGadgetSlotResult = EGadgetSlot.NoiseMaker;

            _set.TestAwake();

            var messageSpy = new UnityTestMessageHandleResponseObject<GadgetUpdatedMessage>();

            _set.AddGadget(_gadget);

            var handle = UnityMessageEventFunctions.RegisterActionWithDispatcher
                <GadgetUpdatedMessage>(_set.gameObject, messageSpy.OnResponse);

            _set.AddGadget(_otherGadget);

            Assert.IsTrue(messageSpy.ActionCalled);
            Assert.AreSame(_gadget, messageSpy.MessagePayload.NewGadget);
            Assert.AreEqual(1, messageSpy.MessagePayload.SlotCount);

            UnityMessageEventFunctions.UnregisterActionWithDispatcher(_set.gameObject, handle);
        }

        [Test]
        public void AddGadget_FailsToAdd_DoesNotSendUpdateMessage()
        {
            _set.InitialCapacities.Add(new GadgetSlotCapcity {Capacity = 3, Slot = EGadgetSlot.Mine});

            _gadget.GetGadgetSlotResult = EGadgetSlot.NoiseMaker;

            _set.TestAwake();

            var messageSpy = new UnityTestMessageHandleResponseObject<GadgetUpdatedMessage>();

            var handle = UnityMessageEventFunctions.RegisterActionWithDispatcher
                <GadgetUpdatedMessage>(_set.gameObject, messageSpy.OnResponse);

            _set.AddGadget(_gadget);

            Assert.IsFalse(messageSpy.ActionCalled);

            UnityMessageEventFunctions.UnregisterActionWithDispatcher(_set.gameObject, handle);
        }

        [Test]
        public void UseActiveGadget_NoGadgets_NoEffect()
        {
            _set.TestAwake();

            var messageSpy = new UnityTestMessageHandleResponseObject<GadgetUpdatedMessage>();

            var handle = UnityMessageEventFunctions.RegisterActionWithDispatcher
                <GadgetUpdatedMessage>(_set.gameObject, messageSpy.OnResponse);

            _set.UseActiveGadget();

            Assert.IsFalse(messageSpy.ActionCalled);

            UnityMessageEventFunctions.UnregisterActionWithDispatcher(_set.gameObject, handle);
        }

        [Test]
        public void UseActiveGadget_Gadget_UseGadgetCalled()
        {
            _set.InitialCapacities.Add(new GadgetSlotCapcity {Capacity = 3, Slot = EGadgetSlot.Mine});

            _gadget.GetGadgetSlotResult = EGadgetSlot.Mine;

            _set.TestAwake();

            _set.AddGadget(_gadget);
            _set.UseActiveGadget();

            Assert.AreSame(_set.gameObject, _gadget.UseGadgetGameObject);
        }

        [Test]
        public void UseActiveGadget_NoGadgetsLeft_SendsMessageWithNull()
        {
            _set.InitialCapacities.Add(new GadgetSlotCapcity {Capacity = 2, Slot = EGadgetSlot.Mine});

            _gadget.GetGadgetSlotResult = EGadgetSlot.Mine;

            _set.TestAwake();

            _set.AddGadget(_gadget);

            var messageSpy = new UnityTestMessageHandleResponseObject<GadgetUpdatedMessage>();

            var handle = UnityMessageEventFunctions.RegisterActionWithDispatcher
                <GadgetUpdatedMessage>(_set.gameObject, messageSpy.OnResponse);

            _set.UseActiveGadget();

            Assert.IsTrue(messageSpy.ActionCalled);
            Assert.IsNull(messageSpy.MessagePayload.NewGadget);
            Assert.AreEqual(0, messageSpy.MessagePayload.SlotCount);

            UnityMessageEventFunctions.UnregisterActionWithDispatcher(_set.gameObject, handle);
        }

        [Test]
        public void UseActiveGadget_GadgetsLeftInSameSlot_SendsMessageWithRemainingGadget()
        {
            _set.InitialCapacities.Add(new GadgetSlotCapcity { Capacity = 2, Slot = EGadgetSlot.Mine });

            _gadget.GetGadgetSlotResult = EGadgetSlot.Mine;
            _otherGadget.GetGadgetSlotResult = EGadgetSlot.Mine;

            _set.TestAwake();

            _set.AddGadget(_gadget);
            _set.AddGadget(_otherGadget);

            var messageSpy = new UnityTestMessageHandleResponseObject<GadgetUpdatedMessage>();

            var handle = UnityMessageEventFunctions.RegisterActionWithDispatcher
                <GadgetUpdatedMessage>(_set.gameObject, messageSpy.OnResponse);

            _set.UseActiveGadget();

            Assert.IsTrue(messageSpy.ActionCalled);
            Assert.AreSame(_gadget, messageSpy.MessagePayload.NewGadget);
            Assert.AreEqual(1, messageSpy.MessagePayload.SlotCount);

            UnityMessageEventFunctions.UnregisterActionWithDispatcher(_set.gameObject, handle);
        }

        [Test]
        public void UseActiveGadget_GadgetsLeftInDifferentSlots_SendsMessageWithRemainingGadget()
        {
            _set.InitialCapacities.Add(new GadgetSlotCapcity { Capacity = 2, Slot = EGadgetSlot.Mine });
            _set.InitialCapacities.Add(new GadgetSlotCapcity { Capacity = 2, Slot = EGadgetSlot.NoiseMaker });

            _gadget.GetGadgetSlotResult = EGadgetSlot.Mine;
            _otherGadget.GetGadgetSlotResult = EGadgetSlot.NoiseMaker;

            _set.TestAwake();

            _set.AddGadget(_gadget);
            _set.AddGadget(_otherGadget);

            var messageSpy = new UnityTestMessageHandleResponseObject<GadgetUpdatedMessage>();

            var handle = UnityMessageEventFunctions.RegisterActionWithDispatcher
                <GadgetUpdatedMessage>(_set.gameObject, messageSpy.OnResponse);

            _set.UseActiveGadget();

            Assert.IsTrue(messageSpy.ActionCalled);
            Assert.AreSame(_otherGadget, messageSpy.MessagePayload.NewGadget);
            Assert.AreEqual(1, messageSpy.MessagePayload.SlotCount);

            UnityMessageEventFunctions.UnregisterActionWithDispatcher(_set.gameObject, handle);
        }

        [Test]
        public void UseActiveGadget_GadgetsLeftInDifferentSlots_UsesOriginalGadget()
        {
            _set.InitialCapacities.Add(new GadgetSlotCapcity { Capacity = 2, Slot = EGadgetSlot.Mine });
            _set.InitialCapacities.Add(new GadgetSlotCapcity { Capacity = 2, Slot = EGadgetSlot.NoiseMaker });

            _gadget.GetGadgetSlotResult = EGadgetSlot.Mine;
            _otherGadget.GetGadgetSlotResult = EGadgetSlot.NoiseMaker;

            _set.TestAwake();

            _set.AddGadget(_gadget);
            _set.AddGadget(_otherGadget);

            _set.UseActiveGadget();

            Assert.AreSame(_set.gameObject, _gadget.UseGadgetGameObject);
        }

        [Test]
        public void UseActiveGadget_GadgetsLeftInSameSlots_UsesLatestGadget()
        {
            _set.InitialCapacities.Add(new GadgetSlotCapcity { Capacity = 2, Slot = EGadgetSlot.Mine });
            _set.InitialCapacities.Add(new GadgetSlotCapcity { Capacity = 2, Slot = EGadgetSlot.NoiseMaker });

            _gadget.GetGadgetSlotResult = EGadgetSlot.Mine;
            _otherGadget.GetGadgetSlotResult = EGadgetSlot.Mine;

            _set.TestAwake();

            _set.AddGadget(_gadget);
            _set.AddGadget(_otherGadget);

            _set.UseActiveGadget();

            Assert.AreSame(_set.gameObject, _otherGadget.UseGadgetGameObject);
        }

        [Test]
        public void CycleActiveGadget_GadgetsLeftInSameSlots_ReturnsToInitialGadget()
        {
            _set.InitialCapacities.Add(new GadgetSlotCapcity { Capacity = 2, Slot = EGadgetSlot.Mine });
            _set.InitialCapacities.Add(new GadgetSlotCapcity { Capacity = 2, Slot = EGadgetSlot.NoiseMaker });

            _gadget.GetGadgetSlotResult = EGadgetSlot.Mine;
            _otherGadget.GetGadgetSlotResult = EGadgetSlot.Mine;

            _set.TestAwake();

            _set.AddGadget(_gadget);
            _set.AddGadget(_otherGadget);

            _set.CycleActiveGadget(1);

            _set.UseActiveGadget();

            Assert.AreSame(_set.gameObject, _otherGadget.UseGadgetGameObject);
        }

        [Test]
        public void CycleActiveGadget_GadgetsLeftInDifferentSlots_UseUsesOtherGadget()
        {
            _set.InitialCapacities.Add(new GadgetSlotCapcity { Capacity = 2, Slot = EGadgetSlot.Mine });
            _set.InitialCapacities.Add(new GadgetSlotCapcity { Capacity = 2, Slot = EGadgetSlot.NoiseMaker });

            _gadget.GetGadgetSlotResult = EGadgetSlot.Mine;
            _otherGadget.GetGadgetSlotResult = EGadgetSlot.NoiseMaker;

            _set.TestAwake();

            _set.AddGadget(_gadget);
            _set.AddGadget(_otherGadget);

            _set.CycleActiveGadget(1);

            _set.UseActiveGadget();

            Assert.AreSame(_set.gameObject, _otherGadget.UseGadgetGameObject);
        }

        [Test]
        public void CycleActiveGadget_ActiveSlotChanges_SendsMessageWithNewActiveGadget()
        {
            _set.InitialCapacities.Add(new GadgetSlotCapcity { Capacity = 2, Slot = EGadgetSlot.Mine });
            _set.InitialCapacities.Add(new GadgetSlotCapcity { Capacity = 2, Slot = EGadgetSlot.NoiseMaker });

            _gadget.GetGadgetSlotResult = EGadgetSlot.Mine;
            _otherGadget.GetGadgetSlotResult = EGadgetSlot.NoiseMaker;

            _set.TestAwake();

            _set.AddGadget(_gadget);
            _set.AddGadget(_otherGadget);

            var messageSpy = new UnityTestMessageHandleResponseObject<GadgetUpdatedMessage>();

            var handle = UnityMessageEventFunctions.RegisterActionWithDispatcher
                <GadgetUpdatedMessage>(_set.gameObject, messageSpy.OnResponse);

            _set.CycleActiveGadget(1);

            Assert.IsTrue(messageSpy.ActionCalled);
            Assert.AreSame(_otherGadget, messageSpy.MessagePayload.NewGadget);
            Assert.AreEqual(1, messageSpy.MessagePayload.SlotCount);

            UnityMessageEventFunctions.UnregisterActionWithDispatcher(_set.gameObject, handle);
        }

        [Test]
        public void CycleActiveGadget_Negative_WrapsAround()
        {
            _set.InitialCapacities.Add(new GadgetSlotCapcity { Capacity = 2, Slot = EGadgetSlot.Mine });
            _set.InitialCapacities.Add(new GadgetSlotCapcity { Capacity = 2, Slot = EGadgetSlot.NoiseMaker });

            _gadget.GetGadgetSlotResult = EGadgetSlot.Mine;
            _otherGadget.GetGadgetSlotResult = EGadgetSlot.NoiseMaker;

            _set.TestAwake();

            _set.AddGadget(_gadget);
            _set.AddGadget(_otherGadget);

            var messageSpy = new UnityTestMessageHandleResponseObject<GadgetUpdatedMessage>();

            var handle = UnityMessageEventFunctions.RegisterActionWithDispatcher
                <GadgetUpdatedMessage>(_set.gameObject, messageSpy.OnResponse);

            _set.CycleActiveGadget(-1);

            Assert.IsTrue(messageSpy.ActionCalled);
            Assert.AreSame(_otherGadget, messageSpy.MessagePayload.NewGadget);
            Assert.AreEqual(1, messageSpy.MessagePayload.SlotCount);

            UnityMessageEventFunctions.UnregisterActionWithDispatcher(_set.gameObject, handle);
        }

        [Test]
        public void CycleActiveGadget_ActiveSlotDoesNotChange_DoesNotSendMessage()
        {
            _set.InitialCapacities.Add(new GadgetSlotCapcity { Capacity = 2, Slot = EGadgetSlot.Mine });
            _set.InitialCapacities.Add(new GadgetSlotCapcity { Capacity = 2, Slot = EGadgetSlot.NoiseMaker });

            _gadget.GetGadgetSlotResult = EGadgetSlot.Mine;
            _otherGadget.GetGadgetSlotResult = EGadgetSlot.NoiseMaker;

            _set.TestAwake();

            _set.AddGadget(_gadget);
            _set.AddGadget(_otherGadget);

            var messageSpy = new UnityTestMessageHandleResponseObject<GadgetUpdatedMessage>();

            var handle = UnityMessageEventFunctions.RegisterActionWithDispatcher
                <GadgetUpdatedMessage>(_set.gameObject, messageSpy.OnResponse);

            _set.CycleActiveGadget(200);

            Assert.IsFalse(messageSpy.ActionCalled);

            UnityMessageEventFunctions.UnregisterActionWithDispatcher(_set.gameObject, handle);
        }
    }
}

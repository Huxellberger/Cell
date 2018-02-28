// Copyright (C) Threetee Gang All Rights Reserved

using Assets.Scripts.Components.Equipment.Holdables;
using Assets.Scripts.Test.Components.Equipment.Holdables;
using NUnit.Framework;
using UnityEngine;

namespace Assets.Editor.UnitTests.Components.Equipment.Holdables
{
    [TestFixture]
    public class HeldItemComponentTestFixture
    {
        private HeldItemComponent _heldItem;
        private MockHoldableComponent _holdable;

        [SetUp]
        public void BeforeTest()
        {
            _heldItem = new GameObject().AddComponent<HeldItemComponent>();

            _holdable = new GameObject().AddComponent<MockHoldableComponent>();

            _heldItem.HeldItemSocketObject = _holdable.gameObject;
        }

        [TearDown]
        public void AfterTest()
        {
            _holdable = null;

            _heldItem = null;
        }

        [Test]
        public void UseCurrentHoldable_CallsOnCurrentHoldableWithCorrectAction()
        {
            const EHoldableAction expectedAction = EHoldableAction.Secondary;

            _heldItem.PickupHoldable(_holdable);

            _heldItem.UseCurrentHoldable(expectedAction);

            Assert.AreEqual(expectedAction, _holdable.UseHoldableInputAction);
        }

        [Test]
        public void PickupHoldable_OnHeldCalled()
        {
            _heldItem.PickupHoldable(_holdable);

            Assert.AreSame(_heldItem.gameObject, _holdable.OnHeldGameObject);
        }

        [Test]
        public void PickupHoldable_AlreadyHolding_OnDroppedCalledForOld()
        {
            _heldItem.PickupHoldable(_holdable);
            _heldItem.PickupHoldable(_holdable);

            Assert.IsTrue(_holdable.OnDroppedCalled);
        }

        [Test]
        public void DropHoldable_OnDroppedCalled()
        {
            _heldItem.PickupHoldable(_holdable);
            _heldItem.DropHoldable();

            Assert.IsTrue(_holdable.OnDroppedCalled);
        }

        [Test]
        public void GetHeldItem_NoHeldItem_Null()
        {
            Assert.IsNull(_heldItem.GetHeldItem());
        }

        [Test]
        public void GetHeldItem_HeldItem_ReturnsHeldItem()
        {
            _heldItem.PickupHoldable(_holdable);

            Assert.AreSame(_holdable, _heldItem.GetHeldItem());
        }

        [Test]
        public void GetHeldItem_HeldItemDropped_Null()
        {
            _heldItem.PickupHoldable(_holdable);
            _heldItem.DropHoldable();

            Assert.IsNull(_heldItem.GetHeldItem());
        }

        [Test]
        public void GetHeldItemSocket_ReturnsExpectedTransform()
        {
            Assert.AreSame(_heldItem.HeldItemSocketObject.transform, _heldItem.GetHeldItemSocket());
        }
    }
}

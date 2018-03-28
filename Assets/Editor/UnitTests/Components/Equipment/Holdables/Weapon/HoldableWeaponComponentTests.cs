// Copyright (C) Threetee Gang All Rights Reserved

using Assets.Editor.UnitTests.Helpers;
using Assets.Scripts.Components.Equipment.Holdables;
using Assets.Scripts.Test.Components.Equipment.Holdables;
using Assets.Scripts.Test.Components.Equipment.Holdables.Weapon;
using NUnit.Framework;
using UnityEngine;

namespace Assets.Editor.UnitTests.Components.Equipment.Holdables.Weapon
{
    [TestFixture]
    public class HoldableWeaponComponentTestFixture
    {
        private TestHoldableWeaponComponent _holdable;
        private GameObject _interactionZone;
        private MockHeldItemComponent _heldItem;
        private Rigidbody _rigidbody;
        private GameObject _target;

        [SetUp]
        public void BeforeTest()
        {
            _holdable = new GameObject().AddComponent<TestHoldableWeaponComponent>();
            _interactionZone = new GameObject();
            _holdable.InteractionZone = _interactionZone;
            _holdable.AttackRadiusSquared = 100.0f;
            _heldItem = new GameObject().AddComponent<MockHeldItemComponent>();

            _heldItem.GetHeldItemSocketResult = _interactionZone.transform;

            _rigidbody = _holdable.gameObject.AddComponent<Rigidbody>();

            _target = new GameObject();
        }

        [TearDown]
        public void AfterTest()
        {
            _target = null;
            _rigidbody = null;
            _heldItem = null;
            _interactionZone = null;
            _holdable = null;
        }

        [Test]
        public void CanInteract_Null_False()
        {
            Assert.IsFalse(_holdable.CanInteract(null));
        }

        [Test]
        public void CanInteract_NoHeldItemInterface_False()
        {
            Assert.IsFalse(_holdable.CanInteract(new GameObject()));
        }

        [Test]
        public void CanInteract_AlreadyOwned_False()
        {
            _holdable.OnHeld(_heldItem.gameObject);
            Assert.IsFalse(_holdable.CanInteract(_heldItem.gameObject));
        }

        [Test]
        public void CanInteract_HeldItemInterfaceAndNotOwned_True()
        {
            Assert.IsTrue(_holdable.CanInteract(_heldItem.gameObject));
        }

        [Test]
        public void OnInteract_PickupHoldableWithHeldItemInterface()
        {
            _holdable.OnInteract(_heldItem.gameObject);

            Assert.AreSame(_holdable, _heldItem.PickupHoldableInput);
        }

        [Test]
        public void UseHoldable_NoOwner_ImplNotCalled()
        {
            _holdable.UseHoldable(EHoldableAction.Primary);

            Assert.IsNull(_holdable.UseHoldableImplAction);
        }

        [Test]
        public void UseHoldable_Owner_ImplCalled()
        {
            const EHoldableAction expectedAction = EHoldableAction.Primary;

            _holdable.OnHeld(_heldItem.gameObject);
            _holdable.UseHoldable(expectedAction);

            Assert.AreEqual(expectedAction, _holdable.UseHoldableImplAction);
        }

        [Test]
        public void OnHeld_UpdatesOwner()
        {
            _holdable.OnHeld(_heldItem.gameObject);

            Assert.AreSame(_heldItem.gameObject, _holdable.GetOwner());
        }

        [Test]
        public void OnHeld_NullOwner_ImplNotCalled()
        {
            _holdable.OnHeld(null);

            Assert.IsFalse(_holdable.OnHeldImplCalled);
        }

        [Test]
        public void OnHeld_OwnerWithNoInterface_ImplNotCalled()
        {
            _holdable.OnHeld(new GameObject());

            Assert.IsFalse(_holdable.OnHeldImplCalled);
        }

        [Test]
        public void OnHeld_OwnerWithInterface_ImplCalled()
        {
            _holdable.OnHeld(_heldItem.gameObject);

            Assert.IsTrue(_holdable.OnHeldImplCalled);
        }

        [Test]
        public void OnHeld_OwnerWithInterface_InteractionZoneDisabled()
        {
            _holdable.OnHeld(_heldItem.gameObject);

            Assert.IsFalse(_interactionZone.activeSelf);
        }

        [Test]
        public void OnHeld_OwnerWithInterface_TransformParentUpdated()
        {
            _holdable.OnHeld(_heldItem.gameObject);

            Assert.AreSame(_heldItem.GetHeldItemSocketResult, _holdable.gameObject.transform.parent);
        }

        [Test]
        public void OnHeld_OwnerWithInterface_RigidbodyPhysicsDisabled()
        {
            _holdable.OnHeld(_heldItem.gameObject);

            Assert.AreEqual(RigidbodyConstraints.FreezeAll, _rigidbody.constraints);
        }

        [Test]
        public void OnHeld_OwnerWithInterface_PositionMatchesSocket()
        {
            _heldItem.GetHeldItemSocketResult.position = new Vector3(1.0f, 2.0f, 3.0f);
            _heldItem.GetHeldItemSocketResult.eulerAngles = new Vector3(4.0f, 7.0f, 12.0f);
            _holdable.OnHeld(_heldItem.gameObject);

            ExtendedAssertions.AssertVectorsNearlyEqual(_heldItem.GetHeldItemSocketResult.position, _holdable.transform.position);
            ExtendedAssertions.AssertVectorsNearlyEqual(_heldItem.GetHeldItemSocketResult.eulerAngles, _holdable.transform.eulerAngles);
        }

        [Test]
        public void OnDropped_NullOwner_OnDroppedImplNotCalled()
        {
            _holdable.OnDropped();

            Assert.IsFalse(_holdable.OnDroppedImplCalled);
        }

        [Test]
        public void OnDropped_Owner_OnDroppedImplCalled()
        {
            _holdable.OnHeld(_heldItem.gameObject);
            _holdable.OnDropped();

            Assert.IsTrue(_holdable.OnDroppedImplCalled);
        }

        [Test]
        public void OnDropped_Owner_ParentIsNull()
        {
            _holdable.OnHeld(_heldItem.gameObject);
            _holdable.OnDropped();

            Assert.IsNull(_holdable.gameObject.transform.parent);
        }

        [Test]
        public void OnDropped_Owner_InteractionZoneActive()
        {
            _holdable.OnHeld(_heldItem.gameObject);
            _holdable.OnDropped();

            Assert.IsTrue(_interactionZone.activeSelf);
        }

        [Test]
        public void OnDropped_Owner_OwnerSetToNull()
        {
            _holdable.OnHeld(_heldItem.gameObject);
            _holdable.OnDropped();

            Assert.IsNull(_holdable.GetOwner());
        }

        [Test]
        public void OnDropped_Owner_RigidbodyPhysicsAllowed()
        {
            _holdable.OnHeld(_heldItem.gameObject);
            _holdable.OnDropped();

            Assert.AreEqual(RigidbodyConstraints.None, _rigidbody.constraints);
        }

        [Test]
        public void GetHoldableObject_ReturnsGameObject()
        {
            Assert.AreSame(_holdable.gameObject, _holdable.GetHoldableObject());
        }

        [Test]
        public void CanUseWeapon_NoTarget_False()
        {
            _holdable.OnHeld(_heldItem.gameObject);
            Assert.IsFalse(_holdable.CanUseWeapon(null));
        }

        [Test]
        public void CanUseWeapon_OutsideAttackRadius_False()
        {
            _holdable.OnHeld(_heldItem.gameObject);
            _target.transform.position = new Vector3(_holdable.AttackRadiusSquared, _holdable.AttackRadiusSquared, 0.0f);
            Assert.IsFalse(_holdable.CanUseWeapon(_target));
        }

        [Test]
        public void CanUseWeapon_ImplReturnsFalse_False()
        {
            _holdable.OnHeld(_heldItem.gameObject);
            _holdable.CanUseWeaponResult = false;
            Assert.IsFalse(_holdable.CanUseWeapon(_target));
        }

        [Test]
        public void CanUseWeapon_ImplReturnsTrue_True()
        {
            _holdable.OnHeld(_heldItem.gameObject);
            _holdable.CanUseWeaponResult = true;
            Assert.IsTrue(_holdable.CanUseWeapon(_target));
        }

        [Test]
        public void UseWeapon_CannotUse_DoesNotUse()
        {
            _holdable.OnHeld(_heldItem.gameObject);
            _holdable.CanUseWeaponResult = false;
            _holdable.UseWeapon(_target);
            
            Assert.IsNull(_holdable.PrepareWeaponTarget);
            Assert.IsNull(_holdable.UseHoldableImplAction);
        }

        [Test]
        public void UseWeapon_CanUse_PrepareAndUsePrimary()
        {
            _holdable.OnHeld(_heldItem.gameObject);
            _holdable.CanUseWeaponResult = true;
            _holdable.UseWeapon(_target);

            Assert.AreSame(_target, _holdable.PrepareWeaponTarget);
            Assert.AreEqual(EHoldableAction.Primary, _holdable.UseHoldableImplAction);
        }
    }
}

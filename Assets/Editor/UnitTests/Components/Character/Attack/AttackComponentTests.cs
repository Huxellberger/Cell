// Copyright (C) Threetee Gang All Rights Reserved

using Assets.Scripts.Test.Components.Character.Attack;
using Assets.Scripts.Test.Components.Equipment.Holdables;
using Assets.Scripts.Test.Components.Equipment.Holdables.Weapon;
using NUnit.Framework;
using UnityEngine;

namespace Assets.Editor.UnitTests.Components.Character.Attack
{
    [TestFixture]
    public class AttackComponentTestFixture
    {
        private MockHeldItemComponent _heldItem;
        private TestAttackComponent _attack;

        private MockHoldableComponent _holdable;
        private MockWeaponComponent _weapon;

        private GameObject _target;

        [SetUp]
        public void BeforeTest()
        {
            _heldItem = new GameObject().AddComponent<MockHeldItemComponent>();
            _attack = _heldItem.gameObject.AddComponent<TestAttackComponent>();

            _attack.TestStart();

            _holdable = new GameObject().AddComponent<MockHoldableComponent>();
            _holdable.GetHoldableObjectResult = _holdable.gameObject;
            _weapon = _holdable.gameObject.AddComponent<MockWeaponComponent>();

            _target = new GameObject();
        }
	
        [TearDown]
        public void AfterTest()
        {
            _target = null;

            _weapon = null;
            _holdable = null;

		    _attack.TestDestroy();

            _attack = null;
            _heldItem = null;
        }

        [Test]
        public void CanAttack_NoTarget_False()
        {
            _heldItem.GetHeldItemResult = _holdable;
            _weapon.CanUseWeaponResult = true;

            Assert.IsFalse(_attack.CanAttack(null));
        }

        [Test]
        public void CanAttack_NoHeldItem_False()
        {
            _weapon.CanUseWeaponResult = true;

            Assert.IsFalse(_attack.CanAttack(_target));
        }

        [Test]
        public void CanAttack_NoWeaponInterface_False()
        {
            _weapon.CanUseWeaponResult = true;

            var nonWeapon = new GameObject().AddComponent<MockHoldableComponent>();
            nonWeapon.GetHoldableObjectResult = nonWeapon.gameObject;

            _heldItem.GetHeldItemResult = nonWeapon;

            Assert.IsFalse(_attack.CanAttack(_target));
        }

        [Test]
        public void CanAttack_CannotUseWeapon_False()
        {
            _heldItem.GetHeldItemResult = _holdable;
            _weapon.CanUseWeaponResult = false;

            Assert.IsFalse(_attack.CanAttack(_target));
        }

        [Test]
        public void CanAttack_CanUseWeapon_True()
        {
            _heldItem.GetHeldItemResult = _holdable;
            _weapon.CanUseWeaponResult = true;

            Assert.IsTrue(_attack.CanAttack(_target));
        }

        [Test]
        public void Attack_CannotAttack_DoesNotUseWeapon()
        {
            _heldItem.GetHeldItemResult = _holdable;
            _weapon.CanUseWeaponResult = false;

            _attack.Attack(_target);

            Assert.IsNull(_weapon.UseWeaponTargetObject);
        }

        [Test]
        public void Attack_CanAttack_UsesWeapon()
        {
            _heldItem.GetHeldItemResult = _holdable;
            _weapon.CanUseWeaponResult = true;

            _attack.Attack(_target);

            Assert.AreSame(_target, _weapon.UseWeaponTargetObject);
        }
    }
}

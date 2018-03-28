// Copyright (C) Threetee Gang All Rights Reserved

using Assets.Editor.UnitTests.Helpers;
using Assets.Scripts.Components.Equipment.Holdables;
using Assets.Scripts.Test.Components.Equipment.Holdables;
using Assets.Scripts.Test.Components.Equipment.Holdables.Spawner;
using Assets.Scripts.Test.Components.Spawning;
using NUnit.Framework;
using UnityEngine;

namespace Assets.Editor.UnitTests.Components.Equipment.Holdables.Spawner
{
    [TestFixture]
    public class HoldableSpawnerComponentTestFixture
    {
        private MockSpawnerComponent _spawner;
        private TestHoldableSpawnerComponent _holdable;

        private GameObject _owner;
        private GameObject _target;

        [SetUp]
        public void BeforeTest()
        {
            _spawner = new GameObject().AddComponent<MockSpawnerComponent>();

            _holdable = _spawner.gameObject.AddComponent<TestHoldableSpawnerComponent>();
            _holdable.InteractionZone = new GameObject();
            _holdable.TestStart();

            _target = new GameObject();

            _owner = new GameObject();
            var heldItem = _owner.AddComponent<MockHeldItemComponent>();
            heldItem.GetHeldItemSocketResult = _owner.transform;

            _holdable.AttackRadiusSquared = 200.0f;
            _target.transform.position = new Vector3(3.0f, 4.0f, 0.0f);
        }
	
        [TearDown]
        public void AfterTest()
        {
            _owner = null;
            _target = null;

            _holdable = null;

            _spawner = null;
        }
	
        [Test]
        public void UseHoldablePrimary_CallsSpawn() 
        {
            _holdable.OnHeld(_owner);
            _holdable.UseHoldable(EHoldableAction.Primary);

            Assert.IsTrue(_spawner.SpawnCalled);
        }

        [Test]
        public void UseHoldableSecondary_CallsSpawn()
        {
            _holdable.OnHeld(_owner);
            _holdable.UseHoldable(EHoldableAction.Secondary);

            Assert.IsTrue(_spawner.SpawnCalled);
        }

        [Test]
        public void CanUseWeapon_NoOwner_False()
        {
            Assert.IsFalse(_holdable.CanUseWeapon(_target));
        }

        [Test]
        public void CanUseWeapon_Owner_True()
        {
            _holdable.OnHeld(_owner);
            Assert.IsTrue(_holdable.CanUseWeapon(_target));
        }

        [Test]
        public void UseWeapon_SetsOwnerRotationToFaceTarget()
        {
            _holdable.OnHeld(_owner);
            _holdable.UseWeapon(_target);

            ExtendedAssertions.AssertVectorsNearlyEqual(_holdable.gameObject.transform.up, (_target.transform.position - _owner.transform.position).normalized);
        }

        [Test]
        public void UseWeapon_Spawns()
        {
            _holdable.OnHeld(_owner);
            _holdable.UseWeapon(_target);

            Assert.IsTrue(_spawner.SpawnCalled);
        }
    }
}

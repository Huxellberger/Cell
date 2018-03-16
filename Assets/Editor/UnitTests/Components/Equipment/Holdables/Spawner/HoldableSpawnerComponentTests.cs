// Copyright (C) Threetee Gang All Rights Reserved

using Assets.Scripts.Components.Equipment.Holdables;
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

        [SetUp]
        public void BeforeTest()
        {
            _spawner = new GameObject().AddComponent<MockSpawnerComponent>();

            _holdable = _spawner.gameObject.AddComponent<TestHoldableSpawnerComponent>();
            _holdable.TestStart();
        }
	
        [TearDown]
        public void AfterTest()
        {
            _holdable = null;

            _spawner = null;
        }
	
        [Test]
        public void UseHoldablePrimary_CallsSpawn() 
        {
            _holdable.UseHoldable(EHoldableAction.Primary);

            Assert.IsTrue(_spawner.SpawnCalled);
        }

        [Test]
        public void UseHoldableSecondary_CallsSpawn()
        {
            _holdable.UseHoldable(EHoldableAction.Secondary);

            Assert.IsTrue(_spawner.SpawnCalled);
        }
    }
}

// Copyright (C) Threetee Gang All Rights Reserved

using Assets.Scripts.AI.Companion;
using Assets.Scripts.Test.AI.Companion;
using NUnit.Framework;
using UnityEngine;

namespace Assets.Editor.UnitTests.AI.Companion
{
    [TestFixture]
    public class JoinableCompanionComponentTestFixture
    {
        private MockCompanionComponent _companion;
        private JoinableCompanionComponent _joinable;
        private MockCompanionSetComponent _set;

        [SetUp]
        public void BeforeTest()
        {
            _companion = new GameObject().AddComponent<MockCompanionComponent>();
            _joinable = _companion.gameObject.AddComponent<JoinableCompanionComponent>();
            _joinable.CompanionPrefab = _companion.gameObject;

            _set = new GameObject().AddComponent<MockCompanionSetComponent>();
        }
	
        [TearDown]
        public void AfterTest()
        {
            _set = null;

            _joinable = null;
            _companion = null;
        }
	
        [Test]
        public void CanInteract_Null_False() 
        {
            Assert.IsFalse(_joinable.CanInteract(null));
        }

        [Test]
        public void CanInteract_NoCompanionSet_False()
        {
            Assert.IsFalse(_joinable.CanInteract(new GameObject()));
        }

        [Test]
        public void CanInteract_CompanionSet_True()
        {
            Assert.IsTrue(_joinable.CanInteract(_set.gameObject));
        }

        [Test]
        public void Interact_SetsPrimaryCompanion()
        {
            _joinable.OnInteract(_set.gameObject);
            Assert.IsNotNull(_set.SetCompanionResult);
            Assert.AreEqual(ECompanionSlot.Primary, _set.SetCompanionSlotResult);
        }
    }
}

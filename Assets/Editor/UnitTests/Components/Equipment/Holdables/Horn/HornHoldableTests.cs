// Copyright (C) Threetee Gang All Rights Reserved 

using Assets.Scripts.Components.Equipment.Holdables;
using Assets.Scripts.Test.Components.Equipment.Holdables.Horn;
using NUnit.Framework;
using UnityEngine;

namespace Assets.Editor.UnitTests.Components.Equipment.Holdables.Horn
{
    [TestFixture]
    public class HornHoldableTestFixture
    {
        private TestHornHoldable _hornHoldable;

        [SetUp]
        public void BeforeTest()
        {
            var source = new GameObject().AddComponent<AudioSource>();

            _hornHoldable = source.gameObject.AddComponent<TestHornHoldable>();
            _hornHoldable.PrimaryHornSound = new AudioClip();
            _hornHoldable.SecondaryHornSound = new AudioClip();

            _hornHoldable.TestStart();
        }

        [TearDown]
        public void AfterTest()
        {
            _hornHoldable = null;
        }

        [Test]
        public void UseImpl_Primary_PlaysPrimarySound()
        {
            _hornHoldable.TestUseHoldableWithAction(EHoldableAction.Primary);

            Assert.AreSame(_hornHoldable.PrimaryHornSound, _hornHoldable.PlayedSound);
        }

        [Test]
        public void UseImpl_Secondary_PlaysSecondarySound()
        {
            _hornHoldable.TestUseHoldableWithAction(EHoldableAction.Secondary);

            Assert.AreSame(_hornHoldable.SecondaryHornSound, _hornHoldable.PlayedSound);
        }
    }
}

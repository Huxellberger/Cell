// Copyright (C) Threetee Gang All Rights Reserved 

using Assets.Scripts.Components.Equipment.Holdables;
using Assets.Scripts.Services.Noise;
using Assets.Scripts.Test.Components.Equipment.Holdables.Horn;
using Assets.Scripts.Test.Services.Noise;
using NUnit.Framework;
using UnityEngine;

namespace Assets.Editor.UnitTests.Components.Equipment.Holdables.Horn
{
    [TestFixture]
    public class HornHoldableTestFixture
    {
        private TestHornHoldable _hornHoldable;
        private MockNoiseEmitterComponent _emitter;

        [SetUp]
        public void BeforeTest()
        {
            var source = new GameObject().AddComponent<AudioSource>();

            _emitter = source.gameObject.AddComponent<MockNoiseEmitterComponent>();

            _hornHoldable = source.gameObject.AddComponent<TestHornHoldable>();
            _hornHoldable.PrimaryHornSound = new AudioClip();
            _hornHoldable.SecondaryHornSound = new AudioClip();

            _hornHoldable.TestStart();

            _hornHoldable.PrimaryHornNoiseData = new NoiseData{NoiseRadius = 200.0f};
            _hornHoldable.SecondaryHornNoiseData = new NoiseData {NoiseRadius = 300.0f};
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
        public void UseImpl_Primary_SendsPrimaryNoiseDataWithCurrentLocation()
        {
            _hornHoldable.transform.position = new Vector3(200.0f, 100.0f, 300.0f);

            var expectedData = _hornHoldable.PrimaryHornNoiseData;
            expectedData.NoiseLocation = _hornHoldable.transform.position;

            _hornHoldable.TestUseHoldableWithAction(EHoldableAction.Primary);

            Assert.AreEqual(expectedData, _emitter.RecordedNoise);
        }

        [Test]
        public void UseImpl_Secondary_PlaysSecondarySound()
        {
            _hornHoldable.TestUseHoldableWithAction(EHoldableAction.Secondary);

            Assert.AreSame(_hornHoldable.SecondaryHornSound, _hornHoldable.PlayedSound);
        }

        [Test]
        public void UseImpl_Secondary_SendsSecondaryNoiseDataWithCurrentLocation()
        {
            _hornHoldable.transform.position = new Vector3(200.0f, 100.0f, 300.0f);

            var expectedData = _hornHoldable.SecondaryHornNoiseData;
            expectedData.NoiseLocation = _hornHoldable.transform.position;

            _hornHoldable.TestUseHoldableWithAction(EHoldableAction.Secondary);

            Assert.AreEqual(expectedData, _emitter.RecordedNoise);
        }
    }
}

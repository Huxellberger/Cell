// Copyright (C) Threetee Gang All Rights Reserved

using Assets.Scripts.Services.Noise;
using Assets.Scripts.Test.Services.Noise;
using NUnit.Framework;
using UnityEngine;

namespace Assets.Editor.UnitTests.Services.Noise
{
    [TestFixture]
    public class NoiseServiceTestFixture 
    {
        private TestNoiseService _noise;
        private MockNoiseListenerComponent _listener;
        private MockNoiseListenerComponent _otherListener;
	
        [SetUp]
        public void BeforeTest()
        {
            _noise = new GameObject().AddComponent<TestNoiseService>();

            _listener = new GameObject().AddComponent<MockNoiseListenerComponent>();
            _otherListener = new GameObject().AddComponent<MockNoiseListenerComponent>();
        }
	
        [TearDown]
        public void AfterTest()
        {
            _otherListener = null;
            _listener = null;

            _noise = null;
        }
	
        [Test]
        public void Update_NoNoises_NoNoisesReceived() 
        {
            _noise.RegisterListener(_listener, Vector3.zero);

            _noise.TestUpdate();

            Assert.AreEqual(0, _listener.NoiseHeardCount);

            _noise.UnregisterListener(_listener);
        }

        [Test]
        public void Update_NoiseInRadiusOfListener_ListenerNotified()
        {
            var noiseData = new NoiseData
            {
                NoiseLocation = new Vector3(120.0f, 20.0f, 3.0f),
                NoiseType = ENoiseType.Talking,
                NoiseRadius = 1.0f
            };
            _noise.RecordNoise(noiseData);

            _noise.RegisterListener(_listener, noiseData.NoiseLocation);

            _noise.TestUpdate();

            Assert.AreEqual(noiseData, _listener.NoiseHeard);
            Assert.AreEqual(1, _listener.NoiseHeardCount);

            _noise.UnregisterListener(_listener);
        }

        [Test]
        public void Update_NoiseOutsideRadiusOfListener_ListenerNotNotified()
        {
            var noiseData = new NoiseData
            {
                NoiseLocation = new Vector3(120.0f, 20.0f, 3.0f),
                NoiseType = ENoiseType.Talking,
                NoiseRadius = 1.0f
            };
            _noise.RecordNoise(noiseData);

            _noise.RegisterListener(_listener, new Vector3
            (
                noiseData.NoiseLocation.x + noiseData.NoiseRadius + 0.1f, 
                noiseData.NoiseLocation.y, noiseData.NoiseLocation.z
             ));

            _noise.TestUpdate();

            Assert.AreEqual(0, _listener.NoiseHeardCount);

            _noise.UnregisterListener(_listener);
        }

        [Test]
        public void Update_NoiseInRadiusOfMultipleListeners_AllListenersNotified()
        {
            var noiseData = new NoiseData
            {
                NoiseLocation = new Vector3(120.0f, 20.0f, 3.0f),
                NoiseType = ENoiseType.Talking,
                NoiseRadius = 1.0f
            };
            _noise.RecordNoise(noiseData);

            _noise.RegisterListener(_listener, noiseData.NoiseLocation);
            _noise.RegisterListener(_otherListener, noiseData.NoiseLocation);

            _noise.TestUpdate();

            Assert.AreEqual(noiseData, _listener.NoiseHeard);
            Assert.AreEqual(1, _listener.NoiseHeardCount);

            Assert.AreEqual(noiseData, _otherListener.NoiseHeard);
            Assert.AreEqual(1, _otherListener.NoiseHeardCount);

            _noise.UnregisterListener(_listener);
        }

        [Test]
        public void Update_MultipleNoisesInRadiusOfListener_ListenerNotifiedForMaximumAmount()
        {
            var noiseData = new NoiseData
            {
                NoiseLocation = new Vector3(120.0f, 20.0f, 3.0f),
                NoiseType = ENoiseType.Talking,
                NoiseRadius = 1.0f
            };

            for (var i = 0; i < NoiseServiceConstants.NoisesPerUpdate + NoiseServiceConstants.NoisesPerUpdate; i++)
            {
                _noise.RecordNoise(noiseData);
            }

            _noise.RegisterListener(_listener, noiseData.NoiseLocation);

            _noise.TestUpdate();

            Assert.AreEqual(NoiseServiceConstants.NoisesPerUpdate, _listener.NoiseHeardCount);

            // Test other noises are eventually considered
            _noise.TestUpdate();

            Assert.AreEqual(NoiseServiceConstants.NoisesPerUpdate + NoiseServiceConstants.NoisesPerUpdate, _listener.NoiseHeardCount);

            _noise.UnregisterListener(_listener);
        }

        [Test]
        public void Update_NoiseAlreadyRecorded_NoLongerConsidered()
        {
            var noiseData = new NoiseData
            {
                NoiseLocation = new Vector3(120.0f, 20.0f, 3.0f),
                NoiseType = ENoiseType.Talking,
                NoiseRadius = 1.0f
            };
            _noise.RecordNoise(noiseData);

            _noise.RegisterListener(_listener, noiseData.NoiseLocation);

            _noise.TestUpdate();
            _noise.TestUpdate();

            Assert.AreEqual(noiseData, _listener.NoiseHeard);
            Assert.AreEqual(1, _listener.NoiseHeardCount);

            _noise.UnregisterListener(_listener);
        }

        [Test]
        public void Update_ListenerLocationUpdated_NewLocationUsedForRadius()
        {
            var noiseData = new NoiseData
            {
                NoiseLocation = new Vector3(120.0f, 20.0f, 3.0f),
                NoiseType = ENoiseType.Talking,
                NoiseRadius = 1.0f
            };
            _noise.RecordNoise(noiseData);

            _noise.RegisterListener(_listener, noiseData.NoiseLocation);
            _noise.UpdateListener(_listener, new Vector3
            (
                noiseData.NoiseLocation.x + noiseData.NoiseRadius + 0.1f,
                noiseData.NoiseLocation.y, noiseData.NoiseLocation.z
            ));

            _noise.TestUpdate();

            Assert.AreEqual(0, _listener.NoiseHeardCount);

            _noise.UnregisterListener(_listener);
        }
    }
}

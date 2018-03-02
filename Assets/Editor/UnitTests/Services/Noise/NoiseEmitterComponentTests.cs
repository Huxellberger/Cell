// Copyright (C) Threetee Gang All Rights Reserved

using Assets.Scripts.Services;
using Assets.Scripts.Services.Noise;
using Assets.Scripts.Test.Messaging;
using Assets.Scripts.Test.Services;
using Assets.Scripts.Test.Services.Noise;
using NUnit.Framework;
using UnityEngine;

namespace Assets.Editor.UnitTests.Services.Noise
{
    [TestFixture]
    public class NoiseEmitterComponentTestFixture 
    {
        private NoiseEmitterComponent _emitter;
        private MockNoiseService _noise;

        [SetUp]
        public void BeforeTest()
        {
            _emitter = new GameObject().AddComponent<NoiseEmitterComponent>();

            _emitter.gameObject.AddComponent<TestUnityMessageEventDispatcherComponent>().TestAwake();
            _emitter.gameObject.transform.position = new Vector3(12.0f, 20.0f, 30.0f);

            _noise = new MockNoiseService();
            new GameObject().AddComponent<TestGameServiceProvider>().TestAwake();
            GameServiceProvider.CurrentInstance.AddService<INoiseServiceInterface>(_noise);
        }

        [TearDown]
        public void AfterTest()
        {
            GameServiceProvider.ClearGameServiceProvider();

            _noise = null;
            _emitter = null;
        }

        [Test]
        public void RecordNoise_SubmitsNoiseToService() 
        {
            var data = new NoiseData{NoiseLocation = new Vector3(10.0f, 12.0f, 13.0f), NoiseType = ENoiseType.Talking, NoiseRadius = 200.0f};

            _emitter.RecordNoise(data);

            Assert.AreEqual(data, _noise.RecordedNoise);
        }
    }
}

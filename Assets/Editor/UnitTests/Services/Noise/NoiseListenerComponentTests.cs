// Copyright (C) Threetee Gang All Rights Reserved

using System.Collections.Generic;
using Assets.Editor.UnitTests.Messaging;
using Assets.Scripts.Messaging;
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
    public class NoiseListenerComponentTestFixture 
    {
        private TestNoiseListenerComponent _listener;
        private MockNoiseService _noise;
	
        [SetUp]
        public void BeforeTest()
        {
            _listener = new GameObject().AddComponent<TestNoiseListenerComponent>();

            _listener.gameObject.AddComponent<TestUnityMessageEventDispatcherComponent>().TestAwake();
            _listener.gameObject.transform.position = new Vector3(12.0f, 20.0f, 30.0f);

            _noise = new MockNoiseService();
            new GameObject().AddComponent<TestGameServiceProvider>().TestAwake();
            GameServiceProvider.CurrentInstance.AddService<INoiseServiceInterface>(_noise);
        }
	
        [TearDown]
        public void AfterTest()
        {
            GameServiceProvider.ClearGameServiceProvider();

            _noise = null;
            _listener = null;
        }
	
        [Test]
        public void Start_RegistersWithNoiseService() 
        {
            _listener.TestStart();

            Assert.AreSame(_listener, _noise.RegisteredListener);
            Assert.AreEqual(_listener.gameObject.transform.position, _noise.RegisteredLocation);
        }

        [Test]
        public void OnDestroy_UnregistersWithNoiseService()
        {
            _listener.TestStart();
            _listener.TestDestroy();

            Assert.AreSame(_listener, _noise.UnregisteredListener);
        }

        [Test]
        public void Update_WithinUpdateDistance_DoesNotUpdateService()
        {
            _listener.TestStart();

            _listener.gameObject.transform.position = new Vector3
            (
                _listener.gameObject.transform.position.x + Mathf.Sqrt(NoiseListenerConstants.UpdateDistanceSquared) -1.1f,
                _listener.gameObject.transform.position.y,
                _listener.gameObject.transform.position.z
            );

            _listener.TestUpdate();

            Assert.IsNull(_noise.UpdatedListener);
        }

        [Test]
        public void Update_OutsideUpdateDistance_UpdateService()
        {
            _listener.TestStart();

            _listener.gameObject.transform.position = new Vector3
            (
                _listener.gameObject.transform.position.x + Mathf.Sqrt(NoiseListenerConstants.UpdateDistanceSquared) + .1f,
                _listener.gameObject.transform.position.y,
                _listener.gameObject.transform.position.z
            );

            _listener.TestUpdate();

            Assert.AreSame(_listener, _noise.UpdatedListener);
            Assert.AreEqual(_listener.gameObject.transform.position, _noise.UpdatedLocation);
        }

        [Test]
        public void OnNoiseHeard_NotNoiseOfInterest_NoMessageSent()
        {
            var messageSpy = new UnityTestMessageHandleResponseObject<NoiseHeardMessage>();

            var handle =
                UnityMessageEventFunctions.RegisterActionWithDispatcher<NoiseHeardMessage>(_listener.gameObject,
                    messageSpy.OnResponse);

            _listener.OnNoiseHeard(new NoiseData{NoiseType = ENoiseType.Explosion});

            Assert.IsFalse(messageSpy.ActionCalled);

            UnityMessageEventFunctions.UnregisterActionWithDispatcher(_listener.gameObject, handle);
        }

        [Test]
        public void OnNoiseHeard_NoiseOfInterest_MessageSent()
        {
            const ENoiseType noiseOfInterest = ENoiseType.Talking;

            var messageSpy = new UnityTestMessageHandleResponseObject<NoiseHeardMessage>();

            var handle =
                UnityMessageEventFunctions.RegisterActionWithDispatcher<NoiseHeardMessage>(_listener.gameObject,
                    messageSpy.OnResponse);

            _listener.NoisesOfInterest = new List<ENoiseType>{noiseOfInterest};

            _listener.OnNoiseHeard(new NoiseData { NoiseType = noiseOfInterest });

            Assert.IsTrue(messageSpy.ActionCalled);
            Assert.AreEqual(noiseOfInterest, messageSpy.MessagePayload.HeardNoise.NoiseType);

            UnityMessageEventFunctions.UnregisterActionWithDispatcher(_listener.gameObject, handle);
        }
    }
}

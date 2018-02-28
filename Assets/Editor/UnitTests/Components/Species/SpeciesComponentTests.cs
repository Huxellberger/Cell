// Copyright (C) Threetee Gang All Rights Reserved

using System.Collections.Generic;
using Assets.Editor.UnitTests.Messaging;
using Assets.Scripts.Components.Species;
using Assets.Scripts.Messaging;
using Assets.Scripts.Services;
using Assets.Scripts.Services.Wildlife;
using Assets.Scripts.Test.Components.Species;
using Assets.Scripts.Test.Messaging;
using Assets.Scripts.Test.Services;
using Assets.Scripts.Test.Services.Wildlife;
using NUnit.Framework;
using UnityEngine;

namespace Assets.Editor.UnitTests.Components.Species
{
    [TestFixture]
    public class SpeciesComponentTestFixture
    {
        private TestSpeciesComponent _species;
        private MockWildlifeService _wildlife;

        private AudioClip _positiveClip;
        private AudioClip _negativeClip;

        [SetUp]
        public void BeforeTest()
        {
            _species = new GameObject().AddComponent<TestSpeciesComponent>();
            _species.gameObject.AddComponent<TestUnityMessageEventDispatcherComponent>().TestAwake();

            _positiveClip = Resources.Load<AudioClip>("Test/Audio/Test_Clip_1");
            _negativeClip = Resources.Load<AudioClip>("Test/Audio/Test_Clip_2");

            _species.SpeciesAudioSource = _species.gameObject.AddComponent<AudioSource>();
            _species.CryClips = new List<SpeciesComponent.CryAudioClipEntry>
            {
                {new SpeciesComponent.CryAudioClipEntry(ECryType.Positive, _positiveClip)},
                {new SpeciesComponent.CryAudioClipEntry(ECryType.Negative, _negativeClip)}
            };
            Assert.IsNotNull(_species.CryClips[0]);
            Assert.IsNotNull(_species.CryClips[1]);

            new GameObject().AddComponent<TestGameServiceProvider>().TestAwake();
            
            _wildlife = new MockWildlifeService();
            GameServiceProvider.CurrentInstance.AddService<IWildlifeServiceInterface>(_wildlife);
        }

        [TearDown]
        public void AfterTest()
        {
            GameServiceProvider.ClearGameServiceProvider();

            _negativeClip = null;
            _positiveClip = null;

            _species = null;
        }

        [Test]
        public void Start_SpeciesIsInitialSpecies()
        {
            _species.InitialSpeciesType = ESpeciesType.Human;

            _species.TestStart();

            Assert.AreEqual(_species.InitialSpeciesType, _species.GetCurrentSpeciesType());
        }

        [Test]
        public void Start_RegistersWithWildlifeService()
        {
            _species.TestStart();

            Assert.AreSame(_species.gameObject, _wildlife.RegisterWildlifeResult);
        }

        [Test]
        public void OnDestroy_UnregistersWithWildlifeService()
        {
            _species.TestDestroy();

            Assert.AreSame(_species.gameObject, _wildlife.UnregisterWildlifeResult);
        }

        [Test]
        public void SpeciesCry_SendsSpeciesCryMessage()
        {
            _species.TestStart();
            const ECryType expectedCryType = ECryType.Positive;

            var messageSpy = new UnityTestMessageHandleResponseObject<SpeciesCryMessage>();

            var handle =
                UnityMessageEventFunctions.RegisterActionWithDispatcher<SpeciesCryMessage>(_species.gameObject,
                    messageSpy.OnResponse);

            _species.SpeciesCry(expectedCryType);

            Assert.IsTrue(messageSpy.ActionCalled);
            Assert.AreEqual(expectedCryType, messageSpy.MessagePayload.Cry);

            UnityMessageEventFunctions.UnregisterActionWithDispatcher(_species.gameObject, handle);
        }

        [Test]
        public void SpeciesCry_PlaysAppropriateAudioClip()
        {
            _species.TestStart();
            const ECryType expectedCryType = ECryType.Positive;

            _species.SpeciesCry(expectedCryType);

            Assert.AreSame(_positiveClip, _species.PlayedAudioClip);
        }

        [Test]
        public void SpeciesCry_AlreadyCry_DoesNotSendAnotherMessage()
        {
            _species.TestStart();

            _species.SpeciesCry(ECryType.Positive);

            var messageSpy = new UnityTestMessageHandleResponseObject<SpeciesCryMessage>();

            var handle =
                UnityMessageEventFunctions.RegisterActionWithDispatcher<SpeciesCryMessage>(_species.gameObject,
                    messageSpy.OnResponse);

            _species.SpeciesCry(ECryType.Positive);

            Assert.False(messageSpy.ActionCalled);

            UnityMessageEventFunctions.UnregisterActionWithDispatcher(_species.gameObject, handle);
        }

        [Test]
        public void SpeciesCry_UpdateLengthOfClip_CanSendMessageAgain()
        {
            _species.TestStart();

            const ECryType expectedCryType = ECryType.Positive;

            _species.SpeciesCry(expectedCryType);

            _species.TestUpdate(_positiveClip.length + 0.1f);

            var messageSpy = new UnityTestMessageHandleResponseObject<SpeciesCryMessage>();

            var handle =
                UnityMessageEventFunctions.RegisterActionWithDispatcher<SpeciesCryMessage>(_species.gameObject,
                    messageSpy.OnResponse);

            _species.SpeciesCry(expectedCryType);

            Assert.IsTrue(messageSpy.ActionCalled);

            UnityMessageEventFunctions.UnregisterActionWithDispatcher(_species.gameObject, handle);
        }

        [Test]
        public void SpeciesCry_UpdateLessThanLengthOfClip_CannotSendMessageAgain()
        {
            _species.TestStart();

            const ECryType expectedCryType = ECryType.Positive;

            _species.SpeciesCry(expectedCryType);

            _species.TestUpdate(_positiveClip.length - 0.1f);

            var messageSpy = new UnityTestMessageHandleResponseObject<SpeciesCryMessage>();

            var handle =
                UnityMessageEventFunctions.RegisterActionWithDispatcher<SpeciesCryMessage>(_species.gameObject,
                    messageSpy.OnResponse);

            _species.SpeciesCry(expectedCryType);

            Assert.IsFalse(messageSpy.ActionCalled);

            UnityMessageEventFunctions.UnregisterActionWithDispatcher(_species.gameObject, handle);
        }

        [Test]
        public void IsSpeciesCryInProgress_InProgress_True()
        {
            _species.TestStart();

            const ECryType expectedCryType = ECryType.Positive;

            _species.SpeciesCry(expectedCryType);

            _species.TestUpdate(_positiveClip.length - 0.1f);

            Assert.IsTrue(_species.IsSpeciesCryInProgress(expectedCryType));
        }

        [Test]
        public void IsSpeciesCryInProgress_Finished_False()
        {
            _species.TestStart();

            const ECryType expectedCryType = ECryType.Positive;

            _species.SpeciesCry(expectedCryType);

            _species.TestUpdate(_positiveClip.length + 0.1f);

            Assert.IsFalse(_species.IsSpeciesCryInProgress(expectedCryType));
        }

        [Test]
        public void IsSpeciesCryInProgress_NotStarted_False()
        {
            _species.TestStart();

            Assert.IsFalse(_species.IsSpeciesCryInProgress(ECryType.Positive));
        }

        [Test]
        public void IsSpeciesCryInProgress_DifferentCry_False()
        {
            _species.TestStart();

            _species.SpeciesCry(ECryType.Positive);

            Assert.IsFalse(_species.IsSpeciesCryInProgress(ECryType.Negative));
        }
    }
}

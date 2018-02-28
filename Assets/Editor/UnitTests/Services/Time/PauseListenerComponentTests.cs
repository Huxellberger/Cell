// Copyright (C) Threetee Gang All Rights Reserved

using Assets.Editor.UnitTests.Messaging;
using Assets.Scripts.Messaging;
using Assets.Scripts.Services;
using Assets.Scripts.Services.Time;
using Assets.Scripts.Test.Messaging;
using Assets.Scripts.Test.Services;
using Assets.Scripts.Test.Services.Time;
using NUnit.Framework;
using UnityEngine;

namespace Assets.Editor.UnitTests.Services.Time
{
    [TestFixture]
    public class PauseListenerComponentTestFixture
    {
        private MockTimeService _timeService;
        private TestGameServiceProvider _serviceProvider;
        private TestPauseListenerComponent _pauseListener;
        

        [SetUp]
        public void BeforeTest()
        {
            _timeService = new MockTimeService();

            _serviceProvider = new GameObject().AddComponent<TestGameServiceProvider>();
            _serviceProvider.TestAwake();

            _serviceProvider.AddService<ITimeServiceInterface>(_timeService);

            _pauseListener = new GameObject().AddComponent<TestPauseListenerComponent>();
            _pauseListener.gameObject.AddComponent<TestUnityMessageEventDispatcherComponent>().TestAwake();
        }

        [TearDown]
        public void AfterTest()
        {
            GameServiceProvider.ClearGameServiceProvider();

            _timeService = null;
        }

        [Test]
        public void Start_RegistersWithService()
        {
            _pauseListener.TestStart();

            Assert.AreSame(_pauseListener, _timeService.AddPauseListenerResult);
        }

        [Test]
        public void OnDestroy_UnregistersWithService()
        {
            _pauseListener.TestStart();
            _pauseListener.TestDestroy();

            Assert.AreSame(_pauseListener, _timeService.RemovePauseListenerResult);
        }

        [Test]
        public void UpdatePauseStatus_FiresCorrespondingEvent()
        {
            var eventSpy = new UnityTestMessageHandleResponseObject<PauseStatusChangedMessage>();

            var handler =
                UnityMessageEventFunctions.RegisterActionWithDispatcher<PauseStatusChangedMessage>(
                    _pauseListener.gameObject, eventSpy.OnResponse);

            const EPauseStatus sentStatus = EPauseStatus.Paused;

            _pauseListener.UpdatePauseStatus(sentStatus);

            Assert.IsTrue(eventSpy.ActionCalled);
            Assert.AreEqual(sentStatus, eventSpy.MessagePayload.NewPauseStatus);

            UnityMessageEventFunctions.UnregisterActionWithDispatcher(_pauseListener.gameObject, handler);
        }
    }
}

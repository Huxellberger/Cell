// Copyright (C) Threetee Gang All Rights Reserved

using Assets.Scripts.Services.Time;
using Assets.Scripts.Test.Services.Time;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Assets.Editor.UnitTests.Services.Time
{
    [TestFixture]
    public class TimeServiceTestFixture
    {
        private TimeService _timeService;
        private MockPauseListenerComponent _listener;
        private MockPauseListenerComponent _otherListener;

        private float _priorTimeScale;

        [SetUp]
        public void BeforeTest()
        {
            _timeService = new TimeService();

            _listener = new GameObject().AddComponent<MockPauseListenerComponent>();
            _otherListener = new GameObject().AddComponent<MockPauseListenerComponent>();

            _priorTimeScale = UnityEngine.Time.timeScale;
        }

        [TearDown]
        public void AfterTest()
        {
            UnityEngine.Time.timeScale = _priorTimeScale;

            _otherListener = null;
            _listener = null;

            _timeService = null;
        }

        [Test]
        public void AddListener_ReceivesPauseStatusUpdates()
        {
            _timeService.AddPauseListener(_listener);

            const EPauseStatus newStatus = EPauseStatus.Paused;

            _timeService.SetPauseStatus(EPauseStatus.Paused);

            Assert.AreEqual(newStatus, _listener.UpdatePauseStatusResult);
        }

        [Test]
        public void AddListener_AlreadyAdded_Error()
        {
            LogAssert.Expect(LogType.Error, "Added a listener which already exists!");

            _timeService.AddPauseListener(_listener);
            _timeService.AddPauseListener(_listener);
        }

        [Test]
        public void AddListener_MultipleListeners_AllUpdated()
        {
            _timeService.AddPauseListener(_listener);
            _timeService.AddPauseListener(_otherListener);

            const EPauseStatus newStatus = EPauseStatus.Paused;

            _timeService.SetPauseStatus(EPauseStatus.Paused);

            Assert.AreEqual(newStatus, _listener.UpdatePauseStatusResult);
            Assert.AreEqual(newStatus, _otherListener.UpdatePauseStatusResult);
        }

        [Test]
        public void RemoveListener_NotUpdated()
        {
            _timeService.AddPauseListener(_listener);
            _timeService.AddPauseListener(_otherListener);

            _timeService.RemovePauseListener(_listener);

            const EPauseStatus newStatus = EPauseStatus.Paused;

            _timeService.SetPauseStatus(EPauseStatus.Paused);

            Assert.IsNull(_listener.UpdatePauseStatusResult);
            Assert.AreEqual(newStatus, _otherListener.UpdatePauseStatusResult);
        }

        [Test]
        public void RemoveListener_NotAdded_Error()
        {
            LogAssert.Expect(LogType.Error, "Tried to remove a listener which was not added!");

            _timeService.RemovePauseListener(_listener);
        }

        [Test]
        public void SetPauseStatus_Pause_TimeScaleSetToZero()
        {
            _timeService.SetPauseStatus(EPauseStatus.Paused);

            Assert.AreEqual(0.0f, UnityEngine.Time.timeScale);
        }

        [Test]
        public void SetPauseStatus_Pause_AlreadyPaused_NoUpdate()
        {
            _timeService.SetPauseStatus(EPauseStatus.Paused);

            _timeService.AddPauseListener(_listener);

            _timeService.SetPauseStatus(EPauseStatus.Paused);

            Assert.IsNull(_listener.UpdatePauseStatusResult);
        }

        [Test]
        public void SetPauseStatus_Unpaused_RevertScale()
        {
            _timeService.SetPauseStatus(EPauseStatus.Paused);
            _timeService.SetPauseStatus(EPauseStatus.Unpaused);

            Assert.AreEqual(_priorTimeScale, UnityEngine.Time.timeScale);
        }

        [Test]
        public void SetPauseStatus_Unpaused_AlreadyUnpaused_NoUpdate()
        {
            _timeService.AddPauseListener(_listener);

            _timeService.SetPauseStatus(EPauseStatus.Unpaused);

            Assert.IsNull(_listener.UpdatePauseStatusResult);
        }

        [Test]
        public void GetPauseStatus_Unpaused_ReturnsUnpaused()
        {
            Assert.AreEqual(EPauseStatus.Unpaused, _timeService.GetPauseStatus());
        }

        [Test]
        public void GetPauseStatus_Paused_ReturnsPaused()
        {
            _timeService.SetPauseStatus(EPauseStatus.Paused);

            Assert.AreEqual(EPauseStatus.Paused, _timeService.GetPauseStatus());
        }
    }
}

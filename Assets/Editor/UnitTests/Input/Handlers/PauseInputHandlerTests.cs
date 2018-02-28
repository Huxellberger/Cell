// Copyright (C) Threetee Gang All Rights Reserved

using Assets.Editor.UnitTests.Services.Time;
using Assets.Scripts.Input;
using Assets.Scripts.Input.Handlers;
using Assets.Scripts.Services.Time;
using NUnit.Framework;

namespace Assets.Editor.UnitTests.Input.Handlers
{
    [TestFixture]
    public class PauseInputHandlerTestFixture
    {
        private MockTimeService _timeService;
        private PauseInputHandler _inputHandler;

        [SetUp]
        public void BeforeTest()
        {
            _timeService = new MockTimeService();
        }

        [TearDown]
        public void AfterTest()
        {
            _timeService = null;
        }

        [Test]
        public void PauseInputKeyPressed_NoTimeService_Unhandled()
        {
            _inputHandler = new PauseInputHandler(null);

            Assert.AreEqual(EInputHandlerResult.Unhandled, _inputHandler.HandleButtonInput(EInputKey.TogglePause, true));
        }

        [Test]
        public void PauseInputKeyReleased_TimeService_Handled()
        {
            _inputHandler = new PauseInputHandler(_timeService);

            Assert.AreEqual(EInputHandlerResult.Handled, _inputHandler.HandleButtonInput(EInputKey.TogglePause, false));
        }

        [Test]
        public void PauseInputKeyReleased_TimeService_DoesNotToggle()
        {
            _inputHandler = new PauseInputHandler(_timeService);

            _inputHandler.HandleButtonInput(EInputKey.TogglePause, false);

            Assert.IsNull(_timeService.SetPauseStatusResult);
        }

        [Test]
        public void PauseInputKeyPressed_TimeService_Handled()
        {
            _inputHandler = new PauseInputHandler(_timeService);

            Assert.AreEqual(EInputHandlerResult.Handled, _inputHandler.HandleButtonInput(EInputKey.TogglePause, true));
        }

        [Test]
        public void PauseInputKeyPressed_TimeServicePaused_TogglesPause()
        {
            _inputHandler = new PauseInputHandler(_timeService);

            _timeService.GetPauseStatusResult = EPauseStatus.Paused;

            _inputHandler.HandleButtonInput(EInputKey.TogglePause, true);

            Assert.AreEqual(EPauseStatus.Unpaused, _timeService.SetPauseStatusResult);
        }

        [Test]
        public void PauseInputKeyPressed_TimeServiceUnpaused_TogglesPause()
        {
            _inputHandler = new PauseInputHandler(_timeService);

            _timeService.GetPauseStatusResult = EPauseStatus.Unpaused;

            _inputHandler.HandleButtonInput(EInputKey.TogglePause, true);

            Assert.AreEqual(EPauseStatus.Paused, _timeService.SetPauseStatusResult);
        }
    }
}

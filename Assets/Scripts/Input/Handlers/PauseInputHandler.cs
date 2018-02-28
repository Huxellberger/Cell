// Copyright (C) Threetee Gang All Rights Reserved

using Assets.Scripts.Services.Time;

namespace Assets.Scripts.Input.Handlers
{
    public class PauseInputHandler
        : InputHandler
    {
        private readonly ITimeServiceInterface _timeService;

        public PauseInputHandler(ITimeServiceInterface inTimeService)
            : base()
        {
            _timeService = inTimeService;

            ButtonResponses.Add(EInputKey.TogglePause, OnTogglePause);
        }

        private EInputHandlerResult OnTogglePause(bool pressed)
        {
            if (pressed)
            {
                if (_timeService != null)
                {
                    _timeService.SetPauseStatus(PauseStatusFunctions.Invert(_timeService.GetPauseStatus()));

                    return EInputHandlerResult.Handled;
                }

                return EInputHandlerResult.Unhandled;
            }

            return EInputHandlerResult.Handled;
        }
    }
}

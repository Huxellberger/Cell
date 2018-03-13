// Copyright (C) Threetee Gang All Rights Reserved

using Assets.Scripts.Input;

namespace Assets.Scripts.Components.Power
{
    public class PowerInputHandler 
        : InputHandler
    {
        private readonly IPowerAssignmentInterface _powerAssignment;

        public PowerInputHandler(IPowerAssignmentInterface powerAssignmentInterface)
            : base()
        {
            _powerAssignment = powerAssignmentInterface;

            ButtonResponses.Add(EInputKey.PrimaryPower, OnPrimaryPowerInput);
            ButtonResponses.Add(EInputKey.SecondaryPower, OnSecondaryPowerInput);
        }

        private EInputHandlerResult OnPrimaryPowerInput(bool pressed)
        {
            if (_powerAssignment != null)
            {
                if (pressed)
                {
                    _powerAssignment.ActivatePower(EPowerSetting.Primary);
                }

                return EInputHandlerResult.Handled;
            }

            return EInputHandlerResult.Unhandled;
        }

        private EInputHandlerResult OnSecondaryPowerInput(bool pressed)
        {
            if (_powerAssignment != null)
            {
                if (pressed)
                {
                    _powerAssignment.ActivatePower(EPowerSetting.Secondary);
                }

                return EInputHandlerResult.Handled;
            }

            return EInputHandlerResult.Unhandled;
        }
    }
}

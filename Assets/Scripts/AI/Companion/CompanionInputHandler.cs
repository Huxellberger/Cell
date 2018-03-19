// Copyright (C) Threetee Gang All Rights Reserved

using Assets.Scripts.Input;

namespace Assets.Scripts.AI.Companion
{
    public class CompanionInputHandler 
        : InputHandler
    {
        private readonly ICompanionSetInterface _companionSet;

        public CompanionInputHandler(ICompanionSetInterface inCompanionSetInterface)
        {
            _companionSet = inCompanionSetInterface;

            ButtonResponses.Add(EInputKey.PrimaryPower, OnPrimaryPowerInput);
            ButtonResponses.Add(EInputKey.SecondaryPower, OnSecondaryPowerInput);

            ButtonResponses.Add(EInputKey.PrimaryDialogue, OnPrimaryDialogueInput);
            ButtonResponses.Add(EInputKey.SecondaryDialogue, OnSecondaryDialogueInput);
        }

        private EInputHandlerResult OnPrimaryPowerInput(bool isPressed)
        {
            return OnPowerInput(ECompanionSlot.Primary, isPressed);
        }

        private EInputHandlerResult OnSecondaryPowerInput(bool isPressed)
        {
            return OnPowerInput(ECompanionSlot.Secondary, isPressed);
        }   

        private EInputHandlerResult OnPowerInput(ECompanionSlot slot, bool isPressed)
        {
            if (_companionSet != null)
            {
                if (isPressed)
                {
                    _companionSet.UseCompanionPower(slot);
                }

                return EInputHandlerResult.Handled;
            }

            return EInputHandlerResult.Unhandled;
        }

        private EInputHandlerResult OnPrimaryDialogueInput(bool isPressed)
        {
            return OnDialogueInput(ECompanionSlot.Primary, isPressed);
        }

        private EInputHandlerResult OnSecondaryDialogueInput(bool isPressed)
        {
            return OnDialogueInput(ECompanionSlot.Secondary, isPressed);
        }

        private EInputHandlerResult OnDialogueInput(ECompanionSlot slot, bool isPressed)
        {
            if (_companionSet != null)
            {
                if (isPressed)
                {
                    _companionSet.RequestCompanionDialogue(slot);
                }

                return EInputHandlerResult.Handled;
            }

            return EInputHandlerResult.Unhandled;
        }
    }
}

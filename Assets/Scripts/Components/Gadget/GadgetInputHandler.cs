// Copyright (C) Threetee Gang All Rights Reserved

using Assets.Scripts.Input;

namespace Assets.Scripts.Components.Gadget
{
    public class GadgetInputHandler 
        : InputHandler
    {
        private readonly IGadgetSetInterface _gadgetSetInterface;

        public GadgetInputHandler(IGadgetSetInterface inGadgetSetInterface)
            : base()
        {
            _gadgetSetInterface = inGadgetSetInterface;

            ButtonResponses.Add(EInputKey.UseActiveGadget, OnUseActiveGadget);
            ButtonResponses.Add(EInputKey.CycleGadgetPositive, OnCycleGadgetPositive);
            ButtonResponses.Add(EInputKey.CycleGadgetNegative, OnCycleGadgetNegative);
        }

        private EInputHandlerResult OnUseActiveGadget(bool isPressed)
        {
            if (_gadgetSetInterface != null)
            {
                if (isPressed)
                {
                    _gadgetSetInterface.UseActiveGadget();
                }

                return EInputHandlerResult.Handled;
            }

            return EInputHandlerResult.Unhandled;
        }

        private EInputHandlerResult OnCycleGadgetPositive(bool isPressed)
        {
            if (_gadgetSetInterface != null)
            {
                if (isPressed)
                {
                    _gadgetSetInterface.CycleActiveGadget(1);
                }

                return EInputHandlerResult.Handled;
            }

            return EInputHandlerResult.Unhandled;
        }

        private EInputHandlerResult OnCycleGadgetNegative(bool isPressed)
        {
            if (_gadgetSetInterface != null)
            {
                if (isPressed)
                {
                    _gadgetSetInterface.CycleActiveGadget(-1);
                }

                return EInputHandlerResult.Handled;
            }

            return EInputHandlerResult.Unhandled;
        }
    }
}

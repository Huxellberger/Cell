// Copyright (C) Threetee Gang All Rights Reserved

using System.Collections.Generic;

namespace Assets.Scripts.Input.Handlers
{
    public class CustomInputHandler 
        : InputHandler
    {
        public CustomInputHandler(IEnumerable<EInputKey> inInputs, OnButtonInputHandledDelegate inDelegate)
            : base()
        {
            foreach (var inputKey in inInputs)
            {
                ButtonResponses.Add(inputKey, inDelegate);
            }
        }

        public CustomInputHandler(IEnumerable<EInputKey> inInputs, OnAnalogInputHandledDelegate inDelegate)
            : base()
        {
            foreach (var inputKey in inInputs)
            {
                AnalogResponses.Add(inputKey, inDelegate);
            }
        }

        public CustomInputHandler(IEnumerable<EInputKey> inInputs, OnMouseInputHandledDelegate inDelegate)
            : base()
        {
            foreach (var inputKey in inInputs)
            {
                MouseResponses.Add(inputKey, inDelegate);
            }
        }
    }
}

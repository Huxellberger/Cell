// Copyright (C) Threetee Gang All Rights Reserved

#if UNITY_EDITOR

using Assets.Scripts.Input;

namespace Assets.Editor.UnitTests.Input
{
    public class TestInputHandler
        : InputHandler
    {
        public void AddButtonResponse(EInputKey inInputKey, OnButtonInputHandledDelegate func)
        {
            ButtonResponses.Add(inInputKey, func);
        }

        public void AddAnalogResponse(EInputKey inInputKey, OnAnalogInputHandledDelegate func)
        {
            AnalogResponses.Add(inInputKey, func);
        }

        public void AddMouseResponse(EInputKey inInputKey, OnMouseInputHandledDelegate func)
        {
            MouseResponses.Add(inInputKey, func);
        }

        public void ClearResponses()
        {
            ButtonResponses.Clear();
            AnalogResponses.Clear();
            MouseResponses.Clear();
        }
    }
}

#endif

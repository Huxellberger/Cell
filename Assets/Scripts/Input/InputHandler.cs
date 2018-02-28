// Copyright (C) Threetee Gang All Rights Reserved

using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Input
{
    public enum EInputHandlerResult
    {
        Handled,
        Unhandled
    }

    public delegate EInputHandlerResult OnButtonInputHandledDelegate(bool pressed);
    public delegate EInputHandlerResult OnAnalogInputHandledDelegate(float analogValue);
    public delegate EInputHandlerResult OnMouseInputHandledDelegate(Vector3 mousePosition);

    public abstract class InputHandler
    {
        protected InputHandler()
        {
            ButtonResponses = new Dictionary<EInputKey, OnButtonInputHandledDelegate>();
            AnalogResponses = new Dictionary<EInputKey, OnAnalogInputHandledDelegate>();
            MouseResponses = new Dictionary<EInputKey, OnMouseInputHandledDelegate>();
        }

        public EInputHandlerResult HandleButtonInput(EInputKey inInputKey, bool pressed)
        {
            if (ButtonResponses.ContainsKey(inInputKey))
            {
                return ButtonResponses[inInputKey](pressed);
            }
            return EInputHandlerResult.Unhandled;
        }

        public EInputHandlerResult HandleAnalogInput(EInputKey inInputKey, float analogValue)
        {
            if (AnalogResponses.ContainsKey(inInputKey))
            {
                return AnalogResponses[inInputKey](analogValue);
            }
            return EInputHandlerResult.Unhandled;
        }

        public EInputHandlerResult HandleMouseInput(EInputKey inInputKey, Vector3 mousePosition)
        {
            if (MouseResponses.ContainsKey(inInputKey))
            {
                return MouseResponses[inInputKey](mousePosition);
            }
            return EInputHandlerResult.Unhandled;
        }

        protected IDictionary<EInputKey, OnButtonInputHandledDelegate> ButtonResponses { get; set; }
        protected IDictionary<EInputKey, OnAnalogInputHandledDelegate> AnalogResponses { get; set; }
        protected IDictionary<EInputKey, OnMouseInputHandledDelegate> MouseResponses { get; set; }
    }
}

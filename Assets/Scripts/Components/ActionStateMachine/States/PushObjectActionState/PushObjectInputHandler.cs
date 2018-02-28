// Copyright (C) Threetee Gang All Rights Reserved

using Assets.Scripts.Components.Objects.Pushable;
using Assets.Scripts.Input;
using UnityEngine;

namespace Assets.Scripts.Components.ActionStateMachine.States.PushObjectActionState
{
    public class PushObjectInputHandler 
        : InputHandler
    {
        private readonly GameObject _pusher;
        private readonly IPushableObjectInterface _pushable;

        public PushObjectInputHandler(GameObject inPusher, IPushableObjectInterface inPushable)
            : base()
        {
            _pusher = inPusher;
            _pushable = inPushable;

            AnalogResponses.Add(EInputKey.VerticalAnalog, OnVerticalInput);
            AnalogResponses.Add(EInputKey.HorizontalAnalog, OnHorizontalInput);
        }

        private EInputHandlerResult OnVerticalInput(float inValue)
        {
            if (_pusher != null && _pushable != null)
            {
                _pushable.Push(_pusher.transform.forward * inValue);
                return EInputHandlerResult.Handled;
            }

            return EInputHandlerResult.Unhandled;
        }

        private EInputHandlerResult OnHorizontalInput(float inValue)
        {
            if (_pusher != null && _pushable != null)
            {
                _pushable.Push(_pusher.transform.right * inValue);
                return EInputHandlerResult.Handled;
            }

            return EInputHandlerResult.Unhandled;
        }
    }
}

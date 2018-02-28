// Copyright (C) Threetee Gang All Rights Reserved

using System.Collections.Generic;
using Assets.Scripts.Input;
using Assets.Scripts.Input.Handlers;
using UnityEngine;

namespace Assets.Scripts.Components.ActionStateMachine.ConditionRunner.Conditions
{
    public class InputReceivedActionStateCondition 
        : ActionStateCondition
    {
        private readonly CustomInputHandler _boundHandler;
        private readonly IInputBinderInterface _inputBinderInterface;

        public InputReceivedActionStateCondition(IEnumerable<EInputKey> inInputs,
            IInputBinderInterface inInputBinderInterface)
            : base()
        {
            _inputBinderInterface = inInputBinderInterface;
            _boundHandler = new CustomInputHandler(inInputs, OnInputReceived);
        }

        public override void Start()
        {
            if (_inputBinderInterface != null)
            {    
                _inputBinderInterface.RegisterInputHandler(_boundHandler);
            }
            else
            {
                Debug.LogError("Failed to register Custom handler on InputReceivedActionStateCondition!");
            }
        }

        public override void Update(float deltaTime)
        {
        }

        public override void End()
        {
            if (_inputBinderInterface != null)
            {
                _inputBinderInterface.UnregisterInputHandler(_boundHandler);
            }
            else
            {
                Debug.LogError("Failed to unregister Custom handler on InputReceivedActionStateCondition!");
            }
        }

        private EInputHandlerResult OnInputReceived(bool isPressed)
        {
            Complete = true;
            return EInputHandlerResult.Handled;
        }
    }
}

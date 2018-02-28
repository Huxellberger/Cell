// Copyright (C) Threetee Gang All Rights Reserved

using System.Collections.Generic;
using Assets.Scripts.Components.ActionStateMachine.ConditionRunner;
using Assets.Scripts.Components.ActionStateMachine.ConditionRunner.Conditions;
using Assets.Scripts.Components.Stamina;
using Assets.Scripts.Input;
using Assets.Scripts.Messaging;
using Assets.Scripts.Mode;

namespace Assets.Scripts.Components.ActionStateMachine.States.Dead
{
    public class DeadActionState 
        : ActionState
    {
        public readonly IList<EInputKey> ValidProgressingInputs = new List<EInputKey>{EInputKey.Interact};
        private readonly ActionStateConditionRunner _conditionRunner;

        public DeadActionState(ActionStateInfo inInfo) 
            : base(EActionStateId.Dead, inInfo)
        {
            _conditionRunner = new ActionStateConditionRunner();
        }

        protected override void OnStart()
        {
            UnityMessageEventFunctions.InvokeMessageEventWithDispatcher(Info.Owner, new EnterDeadActionStateMessage());

            var stamina = Info.Owner.GetComponent<IStaminaInterface>();
            if (stamina != null)
            {
                stamina.SetStaminaChangeEnabled(false, ELockStaminaReason.Dead);
            }

            InitialiseConditions();
        }

        protected override void OnUpdate(float deltaTime)
        {
            _conditionRunner.Update(deltaTime);

            if (_conditionRunner.IsComplete())
            {
                UnityMessageEventFunctions.InvokeMessageEventWithDispatcher(GameModeComponent.RegisteredGameMode.gameObject, new RequestRespawnMessage(Info.Owner));
            }
        }

        protected override void OnEnd()
        {
            var stamina = Info.Owner.GetComponent<IStaminaInterface>();
            if (stamina != null)
            {
                stamina.SetStaminaChangeEnabled(true, ELockStaminaReason.Dead);
            }

            UnityMessageEventFunctions.InvokeMessageEventWithDispatcher(Info.Owner, new LeftDeadActionStateMessage());
        }

        private void InitialiseConditions()
        {
            var inputBinder = Info.Owner.GetComponent<IInputBinderInterface>();
            if (inputBinder != null)
            {
                _conditionRunner.AddCondition(new InputReceivedActionStateCondition(ValidProgressingInputs, inputBinder));
            }
        }
    }
}

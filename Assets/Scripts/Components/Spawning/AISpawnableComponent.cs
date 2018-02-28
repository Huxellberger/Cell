// Copyright (C) Threetee Gang All Rights Reserved

using Assets.Scripts.Components.ActionStateMachine;
using Assets.Scripts.Components.ActionStateMachine.States.Dead;
using Assets.Scripts.Components.Health;
using Assets.Scripts.Messaging;
using Assets.Scripts.UnityLayer.GameObjects;

namespace Assets.Scripts.Components.Spawning
{
    public class AISpawnableComponent 
        : SpawnableComponent
    {
        private UnityMessageEventHandle<EnterDeadActionStateMessage> _enterDeadActionStateHandle;

        private IHealthInterface _healthInterface;
        private IActionStateMachineInterface _actionStateMachine;

        protected void Start()
        {
            RegisterForMessages();

            GetRefreshables();

            if (SpawnerInterface == null)
            {
                InitialiseAI();
            }
        }

        protected void OnDestroy()
        {
            UnregisterForMessages();
        }

        private void RegisterForMessages()
        {
            _enterDeadActionStateHandle =
                UnityMessageEventFunctions.RegisterActionWithDispatcher<EnterDeadActionStateMessage>(gameObject, OnEnterDeadActionState);
        }

        private void UnregisterForMessages()
        {
            if (_enterDeadActionStateHandle != null)
            {
                UnityMessageEventFunctions.UnregisterActionWithDispatcher(gameObject, _enterDeadActionStateHandle);
            }
        }

        protected override void OnSpawnedImpl()
        {
           InitialiseAI();
        }

        private void InitialiseAI()
        {
            if (_healthInterface != null)
            {
                _healthInterface.ReplenishHealth();
            }

            if (_actionStateMachine != null)
            {
                _actionStateMachine.RequestActionState(EActionStateMachineTrack.Locomotion, EActionStateId.Null, new ActionStateInfo(gameObject));
            }
        }

        private void GetRefreshables()
        {
            _healthInterface = gameObject.GetComponent<IHealthInterface>();
            _actionStateMachine = gameObject.GetComponent<IActionStateMachineInterface>();
        }

        private void OnEnterDeadActionState(EnterDeadActionStateMessage inMessage)
        {
            if (SpawnerInterface != null)
            {
                Despawn();
            }
            else
            {
                DestructionFunctions.DestroyGameObject(gameObject);
            }
        }
    }
}

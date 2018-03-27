// Copyright (C) Threetee Gang All Rights Reserved

using Assets.Scripts.Components.ActionStateMachine.States.Locomotion;
using Assets.Scripts.Components.Interaction;
using Assets.Scripts.Components.Objects.Pushable;
using Assets.Scripts.Input;
using UnityEngine;

namespace Assets.Scripts.Components.ActionStateMachine.States.PushObjectActionState
{
    public class PushObjectActionState 
        : ActionState
    {
        private readonly PushObjectActionStateInfo _pushInfo;

        private PushObjectInputHandler _pushObjectInputHandler;
        private InteractionInputHandler _interactionInputHandler;

        private RigidbodyConstraints2D _priorConstraints;

        public PushObjectActionState(PushObjectActionStateInfo inInfo) 
            : base (EActionStateId.PushObject, inInfo)
        {
            _pushInfo = inInfo;
        }

        protected override void OnStart()
        {
            AttachPusher();
            RegisterInputHandlers();
        }

        protected override void OnUpdate(float deltaTime)
        {
        }

        protected override void OnEnd()
        {
            UnregisterInputHandlers();
            DetatchPusher();
        }

        private void AttachPusher()
        {
            Info.Owner.transform.parent = _pushInfo.PushPointSocket.transform;
            Info.Owner.transform.position = _pushInfo.PushPointSocket.transform.position;
            Info.Owner.transform.rotation = _pushInfo.PushPointSocket.transform.rotation;

            var rigidbody = Info.Owner.GetComponent<Rigidbody2D>();
            if (rigidbody != null)
            {
                _priorConstraints = rigidbody.constraints;
                rigidbody.constraints = RigidbodyConstraints2D.FreezeAll;
                rigidbody.isKinematic = true;
            }
        }

        private void RegisterInputHandlers()
        {
            var inputBinder = Info.Owner.GetComponent<IInputBinderInterface>();
            if (inputBinder != null)
            {
                _pushObjectInputHandler = new PushObjectInputHandler(Info.Owner, _pushInfo.ObjectToPush.GetComponent<IPushableObjectInterface>());
                _interactionInputHandler = new InteractionInputHandler(Info.Owner.GetComponent<IInteractionInterface>());

                inputBinder.RegisterInputHandler(_pushObjectInputHandler);
                inputBinder.RegisterInputHandler(_interactionInputHandler);
            }
        }

        private void DetatchPusher()
        {
            var rigidbody = Info.Owner.GetComponent<Rigidbody2D>();
            if (rigidbody != null)
            {
                rigidbody.isKinematic = false;
                rigidbody.constraints = _priorConstraints;
            }

            Info.Owner.transform.parent = null;
        }

        private void UnregisterInputHandlers()
        {
            var inputBinder = Info.Owner.GetComponent<IInputBinderInterface>();
            if (inputBinder != null)
            {
                inputBinder.UnregisterInputHandler(_interactionInputHandler);
                inputBinder.UnregisterInputHandler(_pushObjectInputHandler);
            }
        }
    }
}

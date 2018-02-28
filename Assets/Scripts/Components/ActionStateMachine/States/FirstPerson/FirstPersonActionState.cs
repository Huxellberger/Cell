// Copyright (C) Threetee Gang All Rights Reserved

using Assets.Scripts.Components.Character;
using Assets.Scripts.Input;
using UnityEngine;

namespace Assets.Scripts.Components.ActionStateMachine.States.FirstPerson
{
    public class FirstPersonActionState 
        : ActionState
    {
        private readonly FirstPersonActionStateInfo _firstPersonInfo;
        private readonly FirstPersonActionStateParams _params;

        private FirstPersonInputHandler _firstPersonInputHandler;

        private Vector3 _startingLocalPosition;

        public FirstPersonActionState(FirstPersonActionStateInfo inInfo, FirstPersonActionStateParams inParams)
            : base(EActionStateId.FirstPerson, inInfo)
        {
            _firstPersonInfo = inInfo;
            _params = inParams;
        }

        protected override void OnStart()
        {
            _startingLocalPosition = _firstPersonInfo.CameraObject.transform.localPosition;
            _firstPersonInfo.CameraObject.transform.localPosition = _params.FirstPersonLocalPosition;

            RegisterInputHandlers(_firstPersonInfo.Owner.GetComponent<IInputBinderInterface>());
        }

        private void RegisterInputHandlers(IInputBinderInterface inputBinder)
        {
            if (inputBinder != null)
            {
                _firstPersonInputHandler = new FirstPersonInputHandler(_firstPersonInfo.CameraObject.GetComponent<IPlayerCameraInterface>());

                inputBinder.RegisterInputHandler(_firstPersonInputHandler);
            }
        }

        protected override void OnUpdate(float deltaTime)
        {
        }

        protected override void OnEnd()
        {
            UnregisterInputHandlers(_firstPersonInfo.Owner.GetComponent<IInputBinderInterface>());

            _firstPersonInfo.CameraObject.transform.localPosition = _startingLocalPosition;
        }

        private void UnregisterInputHandlers(IInputBinderInterface inputBinder)
        {
            if (inputBinder != null)
            {
                inputBinder.UnregisterInputHandler(_firstPersonInputHandler);
            }
        }
    }
}

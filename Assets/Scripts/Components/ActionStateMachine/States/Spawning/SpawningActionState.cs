// Copyright (C) Threetee Gang All Rights Reserved

using Assets.Scripts.Components.Character;
using Assets.Scripts.Messaging;

namespace Assets.Scripts.Components.ActionStateMachine.States.Spawning
{
    public class SpawningActionState 
        : ActionState
    {
        private readonly SpawningActionStateParams _params;

        public SpawningActionState(ActionStateInfo inInfo, SpawningActionStateParams inParams) 
            : base(EActionStateId.Spawning, inInfo)
        {
            _params = inParams;
        }

        protected override void OnStart()
        {
            UnityMessageEventFunctions.InvokeMessageEventWithDispatcher(Info.Owner, new EnterSpawningActionStateMessage());

            SetCamera(Info.Owner.GetComponent<CharacterComponent>());

            var actionStateMachine = Info.Owner.GetComponent<IActionStateMachineInterface>();
            actionStateMachine.RequestActionState(EActionStateMachineTrack.Locomotion, EActionStateId.Locomotion, Info);
        }

        protected override void OnUpdate(float deltaTime)
        {
        }

        protected override void OnEnd()
        {
            UnityMessageEventFunctions.InvokeMessageEventWithDispatcher(Info.Owner, new LeftSpawningActionStateMessage());
        }

        private void SetCamera(CharacterComponent inCharacter)
        {
            if (inCharacter.ActiveController != null)
            {
                var cameraInterface = inCharacter.ActiveController.gameObject.GetComponent<IPlayerCameraInterface>();
                if (cameraInterface != null)
                {
                    cameraInterface.SetRelativeCameraPosition(_params.InitialCameraLocation, _params.InitialCameraRotation);
                }
            }
        }
    }
}

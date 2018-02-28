// Copyright (C) Threetee Gang All Rights Reserved

using Assets.Scripts.Components.ActionStateMachine.States.OpenMenuUI;
using Assets.Scripts.Input;
using Assets.Scripts.UI.VirtualMouse;

namespace Assets.Scripts.Components.ActionStateMachine.States.MainMenu
{
    public class MainMenuActionState 
        : ActionState
    {
        private IInputBinderInterface _inputBinderInterface;
        private VirtualMouseInputHandler _virtualMouseInputHandler;

        public MainMenuActionState(ActionStateInfo inInfo) : base(EActionStateId.MainMenu, inInfo)
        {
        }

        protected override void OnStart()
        {
            _inputBinderInterface = Info.Owner.GetComponent<IInputBinderInterface>();

            _virtualMouseInputHandler = new VirtualMouseInputHandler(VirtualMouseInstance.CurrentVirtualMouse);

            _inputBinderInterface.RegisterInputHandler(_virtualMouseInputHandler);
        }

        protected override void OnUpdate(float deltaTime)
        {
        }

        protected override void OnEnd()
        {
            _inputBinderInterface.UnregisterInputHandler(_virtualMouseInputHandler);
        }
    }
}

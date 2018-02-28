// Copyright (C) Threetee Gang All Rights Reserved 

using Assets.Scripts.Input;
using Assets.Scripts.Instance;
using Assets.Scripts.Messaging;
using Assets.Scripts.UI.Menu;
using Assets.Scripts.UI.VirtualMouse;
using UnityEngine;

namespace Assets.Scripts.Components.ActionStateMachine.States.OpenMenuUI
{
    public class OpenMenuUIActionState 
        : ActionState
    {
        private UnityMessageEventDispatcher _uiDispatcher;

        private IInputBinderInterface _inputBinder;
        private InGameMenuInputHandler _menuInputHandler;
        private VirtualMouseInputHandler _virtualMouseInputHandler;
        private bool _mousePreviouslyEnabled;

        public OpenMenuUIActionState(ActionStateInfo inInfo) : base(EActionStateId.OpenMenuUI, inInfo)
        {
        }

        protected override void OnStart()
        {
            _mousePreviouslyEnabled = Cursor.visible;
            Cursor.visible = true;

            _uiDispatcher = GameInstance.CurrentInstance.GetUIMessageDispatcher();

            _uiDispatcher.InvokeMessageEvent(new RequestInGameMenuActivationMessage());

            RegisterInputHandlers();
        }

        private void RegisterInputHandlers()
        {
            _inputBinder = Info.Owner.GetComponent<IInputBinderInterface>();

            if (_inputBinder != null)
            {
                _menuInputHandler = new InGameMenuInputHandler();
                _inputBinder.RegisterInputHandler(_menuInputHandler);

                _virtualMouseInputHandler = new VirtualMouseInputHandler(VirtualMouseInstance.CurrentVirtualMouse);
                _inputBinder.RegisterInputHandler(_virtualMouseInputHandler);
            }
        }

        protected override void OnUpdate(float deltaTime)
        {
        }

        protected override void OnEnd()
        {
            UnregisterInputHandlers();

            _uiDispatcher.InvokeMessageEvent(new RequestInGameMenuDeactivationMessage());

            _uiDispatcher = null;

            if (!_mousePreviouslyEnabled)
            { 
                if (VirtualMouseInstance.CurrentVirtualMouse != null)
                {
                    VirtualMouseInstance.CurrentVirtualMouse.SetMouseVisibile(false);
                }
                Cursor.visible = false;
            }
        }

        private void UnregisterInputHandlers()
        {
            if (_inputBinder != null)
            {
                _inputBinder.UnregisterInputHandler(_virtualMouseInputHandler);
                _inputBinder.UnregisterInputHandler(_menuInputHandler);
            }
        }
    }
}

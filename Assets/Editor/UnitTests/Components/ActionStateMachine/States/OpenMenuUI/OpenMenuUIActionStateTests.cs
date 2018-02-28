// Copyright (C) Threetee Gang All Rights Reserved 

using Assets.Editor.UnitTests.Messaging;
using Assets.Scripts.Components.ActionStateMachine;
using Assets.Scripts.Components.ActionStateMachine.States.OpenMenuUI;
using Assets.Scripts.Instance;
using Assets.Scripts.Test.Input;
using Assets.Scripts.Test.Instance;
using Assets.Scripts.Test.UI.VirtualMouse;
using Assets.Scripts.UI.Menu;
using Assets.Scripts.UI.VirtualMouse;
using NUnit.Framework;
using UnityEngine;

namespace Assets.Editor.UnitTests.Components.ActionStateMachine.States.OpenMenuUI
{
    [TestFixture]
    public class OpenMenuUIActionStateTestFixture
    {
        private OpenMenuUIActionState _actionState;
        private MockInputBinderComponent _inputBinder;
        private bool _priorCursorStatus;

        [SetUp]
        public void BeforeTest()
        {
            _priorCursorStatus = Cursor.visible;

            var owner = new GameObject();
            _inputBinder = owner.AddComponent<MockInputBinderComponent>();

            _actionState = new OpenMenuUIActionState(new ActionStateInfo(owner));

            var instanceObject = new GameObject().AddComponent<MockInputComponent>();
            instanceObject.gameObject.AddComponent<TestGameInstance>().TestAwake();
        }

        [TearDown]
        public void AfterTest()
        {
            GameInstance.ClearGameInstance();

            _inputBinder = null;

            Cursor.visible = _priorCursorStatus;
        }

        [Test]
        public void Created_OpenMenuUIIdSet()
        {
            Assert.AreEqual(EActionStateId.OpenMenuUI, _actionState.ActionStateId);
        }

        [Test]
        public void Start_SetsCursorVisible()
        {
            Cursor.visible = false;

            _actionState.Start();

            Assert.IsTrue(Cursor.visible);
        }

        [Test]
        public void Start_FiresActivationEvent()
        {
            var eventSpy = new UnityTestMessageHandleResponseObject<RequestInGameMenuActivationMessage>();

            var dispatcher = GameInstance.CurrentInstance.GetUIMessageDispatcher();
            var handle = dispatcher.RegisterForMessageEvent<RequestInGameMenuActivationMessage>(eventSpy.OnResponse);

            _actionState.Start();

            Assert.IsTrue(eventSpy.ActionCalled);

            dispatcher.UnregisterForMessageEvent(handle);
        }

        [Test]
        public void Start_RegistersMenuInputHandler()
        {
            _actionState.Start();

            Assert.IsTrue(_inputBinder.IsHandlerOfTypeRegistered<InGameMenuInputHandler>());
        }

        [Test]
        public void Start_RegistersVirtualMouseInputHandler()
        {
            _actionState.Start();

            Assert.IsTrue(_inputBinder.IsHandlerOfTypeRegistered<VirtualMouseInputHandler>());
        }

        [Test]
        public void End_CursorInvisiblePrior_ResetsToInvisible()
        {
            Cursor.visible = false;

            _actionState.Start();
            _actionState.End();

            Assert.IsFalse(Cursor.visible);
        }

        [Test]
        public void End_CursorVisiblePrior_ResetsToVisible()
        {
            Cursor.visible = true;

            _actionState.Start();
            _actionState.End();

            Assert.IsTrue(Cursor.visible);
        }

        [Test]
        public void End_CursorInvisiblePrior_VirtualCursorReset()
        {
            Cursor.visible = false;

            var virtualMouse = new GameObject().AddComponent<MockVirtualMouseComponent>();

            VirtualMouseInstance.CurrentVirtualMouse = virtualMouse;

            _actionState.Start();
            _actionState.End();

            Assert.IsFalse(virtualMouse.SetMouseVisibleResult);

            VirtualMouseInstance.CurrentVirtualMouse = null;
        }

        [Test]
        public void End_FiresDeactivationEvent()
        {
            _actionState.Start();

            var eventSpy = new UnityTestMessageHandleResponseObject<RequestInGameMenuDeactivationMessage>();

            var dispatcher = GameInstance.CurrentInstance.GetUIMessageDispatcher();
            var handle = dispatcher.RegisterForMessageEvent<RequestInGameMenuDeactivationMessage>(eventSpy.OnResponse);

            _actionState.End();

            Assert.IsTrue(eventSpy.ActionCalled);

            dispatcher.UnregisterForMessageEvent(handle);
        }

        [Test]
        public void End_UnregistersMenuInputHandler()
        {
            _actionState.Start();
            _actionState.End();

            Assert.IsTrue(_inputBinder.IsHandlerOfTypeUnregistered<InGameMenuInputHandler>());
        }

        [Test]
        public void End_UnregistersVirtualMouseInputHandler()
        {
            _actionState.Start();
            _actionState.End();

            Assert.IsTrue(_inputBinder.IsHandlerOfTypeUnregistered<VirtualMouseInputHandler>());
        }
    }
}

// Copyright (C) Threetee Gang All Rights Reserved

using Assets.Scripts.Components.ActionStateMachine;
using Assets.Scripts.Components.ActionStateMachine.States.MainMenu;
using Assets.Scripts.Components.ActionStateMachine.States.OpenMenuUI;
using Assets.Scripts.Test.Input;
using NUnit.Framework;
using UnityEngine;

namespace Assets.Editor.UnitTests.Components.ActionStateMachine.States.MainMenu
{
    [TestFixture]
    public class MainMenuActionStateTestFixture
    {
        private MainMenuActionState _actionState;
        private MockInputBinderComponent _inputBinder;

        [SetUp]
        public void BeforeTest()
        {
            _inputBinder = new GameObject().AddComponent<MockInputBinderComponent>();

            _actionState = new MainMenuActionState(new ActionStateInfo(_inputBinder.gameObject));
        }

        [TearDown]
        public void AfterTest()
        {
            _actionState = null;
            _inputBinder = null;
        }

        [Test]
        public void Creation_IdIsMainMenuActionState()
        {
            Assert.AreEqual(EActionStateId.MainMenu, _actionState.ActionStateId);
        }

        [Test]
        public void Start_RegistersVirtualMouseInputHandler()
        {
            _actionState.Start();

            Assert.IsTrue(_inputBinder.IsHandlerOfTypeRegistered<VirtualMouseInputHandler>());
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

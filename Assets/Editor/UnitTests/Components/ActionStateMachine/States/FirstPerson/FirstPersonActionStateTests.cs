// Copyright (C) Threetee Gang All Rights Reserved

using Assets.Scripts.Components.ActionStateMachine;
using Assets.Scripts.Components.ActionStateMachine.States.FirstPerson;
using Assets.Scripts.Test.Input;
using NUnit.Framework;
using UnityEngine;

namespace Assets.Editor.UnitTests.Components.ActionStateMachine.States.FirstPerson
{
    [TestFixture]
    public class FirstPersonActionStateTestFixture
    {
        private FirstPersonActionState _actionState;
        private FirstPersonActionStateParams _params;

        private MockInputBinderComponent _inputBinder;
        private GameObject _camera;

        [SetUp]
        public void BeforeTest()
        {
            _params = new FirstPersonActionStateParams
            {
                FirstPersonLocalPosition = new Vector3(1.0f, 2.0f, -12.0f)
            };

            _inputBinder = new GameObject().AddComponent<MockInputBinderComponent>();

            _camera = new GameObject();

            _actionState = new FirstPersonActionState(new FirstPersonActionStateInfo(_inputBinder.gameObject, _camera), _params);
        }

        [TearDown]
        public void AfterTest()
        {
            _actionState = null;

            _camera = null;

            _inputBinder = null;
        }

        [Test]
        public void Created_HasFirstPersonActionStateID()
        {
            Assert.AreEqual(EActionStateId.FirstPerson, _actionState.ActionStateId);
        }

        [Test]
        public void Start_SetsCameraLocalPositionToParams()
        {
            _actionState.Start();

            Assert.AreEqual(_params.FirstPersonLocalPosition, _camera.transform.localPosition);
        }

        [Test]
        public void Start_RegistersFirstPersonInputHandler()
        {
            _actionState.Start();

            Assert.IsTrue(_inputBinder.IsHandlerOfTypeRegistered<FirstPersonInputHandler>());
        }

        [Test]
        public void End_RevertsCameraToInitialPosition()
        {
            var initialLocation = new Vector3(1.0f, -12.0f, 30.0f);

            _camera.transform.localPosition = initialLocation;

            _actionState.Start();
            _actionState.End();

            Assert.AreEqual(initialLocation, _camera.transform.localPosition);
        }

        [Test]
        public void End_UnregistersFirstPersonInputHandler()
        {
            _actionState.Start();
            _actionState.End();

            Assert.IsTrue(_inputBinder.IsHandlerOfTypeUnregistered<FirstPersonInputHandler>());
        }
    }
}

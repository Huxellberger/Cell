// Copyright (C) Threetee Gang All Rights Reserved

using Assets.Editor.UnitTests.Helpers;
using Assets.Scripts.Components.ActionStateMachine;
using Assets.Scripts.Components.ActionStateMachine.States.Locomotion;
using Assets.Scripts.Components.ActionStateMachine.States.SurfaceSticking;
using Assets.Scripts.Test.Components.ActionStateMachine;
using Assets.Scripts.Test.Components.Character;
using Assets.Scripts.Test.Components.Controller;
using Assets.Scripts.Test.Input;
using NUnit.Framework;
using UnityEngine;

namespace Assets.Editor.UnitTests.Components.ActionStateMachine.States.SurfaceSticking
{
    [TestFixture]
    public class SurfaceStickingActionStateTestFixture
    {
        private SurfaceStickingActionStateParams _params;
        private SurfaceStickingActionState _actionState;

        private TestCharacterComponent _character;
        private MockInputBinderComponent _inputBinder;
        private Rigidbody _rigidbody;
        private GameObject _surface;

        [SetUp]
        public void BeforeTest()
        {
            _surface = new GameObject();
            _surface.transform.position = new Vector3(1.0f, 3.0f, 5.0f);
            _surface.transform.eulerAngles = new Vector3(2.0f, 1.0f, 5.0f);

            _inputBinder = new GameObject().AddComponent<MockInputBinderComponent>();
            _inputBinder.gameObject.AddComponent<MockActionStateMachineComponent>();
            _character = _inputBinder.gameObject.AddComponent<TestCharacterComponent>();
            _rigidbody = _character.gameObject.AddComponent<Rigidbody>();
            _rigidbody.useGravity = true;

            _character.ActiveController = new GameObject().AddComponent<TestControllerComponent>();

            _params = new SurfaceStickingActionStateParams
            {
                CameraLocalPosition = 3.0f,
                CameraRotation = new Vector3(12.0f, 4.0f, 3.0f)
            };

            _actionState = new SurfaceStickingActionState(new SurfaceStickingActionStateInfo(_character.gameObject, _surface), _params);
        }

        [TearDown]
        public void AfterTest()
        {
            _actionState = null;

            _rigidbody = null;
            _character = null;
            _inputBinder = null;
            _surface = null;
        }

        [Test]
        public void Created_HasSurfaceStickingId()
        {
            Assert.AreEqual(EActionStateId.SurfaceSticking, _actionState.ActionStateId);
        }

        [Test]
        public void Start_OwnerParentSetToSurface()
        {
            _actionState.Start();

            Assert.AreSame(_surface.transform, _character.gameObject.transform.parent);
        }

        [Test]
        public void Start_GravityDisabled()
        {
            _actionState.Start();

            Assert.IsFalse(_rigidbody.useGravity);
        }

        [Test]
        public void Start_ControllerSetToParamsPositionAndRotation()
        {
            _actionState.Start();

            ExtendedAssertions.AssertVectorsNearlyEqual(_params.CameraLocalPosition * _character.gameObject.transform.up, _character.ActiveController.gameObject.transform.localPosition);
            ExtendedAssertions.AssertVectorsNearlyEqual(_params.CameraRotation, _character.ActiveController.gameObject.transform.eulerAngles);
        }

        [Test]
        public void Start_OwnerRotatedToMatchSurface()
        {
            _actionState.Start();

            ExtendedAssertions.AssertVectorsNearlyEqual(_surface.transform.eulerAngles, _character.gameObject.transform.eulerAngles);
        }

        [Test]
        public void Start_OwnerPositionTheSame()
        {
            _actionState.Start();

            ExtendedAssertions.AssertVectorsNearlyEqual(Vector3.zero, _character.gameObject.transform.position);
        }

        [Test]
        public void Start_RegistersInteractionInputHandler()
        {
            _actionState.Start();

            Assert.IsTrue(_inputBinder.IsHandlerOfTypeRegistered<InteractionInputHandler>());
        }

        [Test]
        public void Start_RegistersSurfaceInputHandler()
        {
            _actionState.Start();

            Assert.IsTrue(_inputBinder.IsHandlerOfTypeRegistered<SurfaceStickingInputHandler>());
        }

        [Test]
        public void End_OwnerParentSetToNull()
        {
            _actionState.Start();
            _actionState.End();

            Assert.IsNull(_character.gameObject.transform.parent);
        }

        [Test]
        public void End_GravityEnabled()
        {
            _actionState.Start();
            _actionState.End();

            Assert.IsTrue(_rigidbody.useGravity);
        }

        [Test]
        public void End_ControllerPositionAndRotationReset()
        {
            _actionState.Start();
            _actionState.End();

            ExtendedAssertions.AssertVectorsNearlyEqual(Vector3.zero, _character.ActiveController.gameObject.transform.localPosition);
            ExtendedAssertions.AssertVectorsNearlyEqual(Vector3.zero, _character.ActiveController.gameObject.transform.eulerAngles);
        }

        [Test]
        public void End_OwnerRotationReset()
        {
            _actionState.Start();
            _actionState.End();

            ExtendedAssertions.AssertVectorsNearlyEqual(Vector3.zero, _character.gameObject.transform.eulerAngles);
        }

        [Test]
        public void End_UnregistersInteractionInputHandler()
        {
            _actionState.Start();
            _actionState.End();

            Assert.IsTrue(_inputBinder.IsHandlerOfTypeUnregistered<InteractionInputHandler>());
        }

        [Test]
        public void End_UnregistersSurfaceInputHandler()
        {
            _actionState.Start();
            _actionState.End();

            Assert.IsTrue(_inputBinder.IsHandlerOfTypeUnregistered<SurfaceStickingInputHandler>());
        }
    }
}

// Copyright (C) Threetee Gang All Rights Reserved

using Assets.Scripts.Components.ActionStateMachine.States.FirstPerson;
using Assets.Scripts.Components.Character;
using Assets.Scripts.Input;
using Assets.Scripts.Test.Components.Character;
using NUnit.Framework;
using UnityEngine;

namespace Assets.Editor.UnitTests.Components.ActionStateMachine.States.FirstPerson
{
    [TestFixture]
    public class FirstPersonInputHandlerTestFixture
    {
        private MockPlayerCameraComponent _camera;

        [SetUp]
        public void BeforeTest()
        {
            _camera = new GameObject().AddComponent<MockPlayerCameraComponent>();
        }

        [TearDown]
        public void AfterTest()
        {
            _camera = null;
        }

        [Test]
        public void ReceivesCameraHorizontal_PlayerCameraInterface_AppliesHorizontalMotion()
        {
            var locomotionHandler = new FirstPersonInputHandler(_camera);

            const float expectedAppliedInput = 0.5f;

            locomotionHandler.HandleAnalogInput(EInputKey.CameraHorizontal, expectedAppliedInput);

            Assert.AreEqual(expectedAppliedInput, _camera.RotateHorizontalResult);
        }

        [Test]
        public void ReceivesCameraHorizontal_PlayerCameraInterface_ReturnsHandled()
        {
            var locomotionHandler = new FirstPersonInputHandler(_camera);

            Assert.AreEqual(EInputHandlerResult.Handled, locomotionHandler.HandleAnalogInput(EInputKey.CameraHorizontal, 1.0f));
        }

        [Test]
        public void ReceivesCameraHorizontal_NoPlayerCameraInterface_DoesNotApplyHorizontalMotion()
        {
            var locomotionHandler = new FirstPersonInputHandler(null);

            const float expectedAppliedInput = 0.5f;

            locomotionHandler.HandleAnalogInput(EInputKey.CameraHorizontal, expectedAppliedInput);

            Assert.AreNotEqual(expectedAppliedInput, _camera.RotateHorizontalResult);
        }

        [Test]
        public void ReceivesCameraHorizontal_NoPlayerCameraInterface_ReturnsUnhandled()
        {
            var locomotionHandler = new FirstPersonInputHandler(null);

            Assert.AreEqual(EInputHandlerResult.Unhandled, locomotionHandler.HandleAnalogInput(EInputKey.CameraHorizontal, 1.0f));
        }

        [Test]
        public void ReceivesHorizontalAnalog_PlayerCameraInterface_AppliesHorizontalMotion()
        {
            var locomotionHandler = new FirstPersonInputHandler(_camera);

            const float expectedAppliedInput = 0.5f;

            locomotionHandler.HandleAnalogInput(EInputKey.HorizontalAnalog, expectedAppliedInput);

            Assert.AreEqual(expectedAppliedInput, _camera.RotateHorizontalResult);
        }

        [Test]
        public void ReceivesHorizontalAnalog_PlayerCameraInterface_ReturnsHandled()
        {
            var locomotionHandler = new FirstPersonInputHandler(_camera);

            Assert.AreEqual(EInputHandlerResult.Handled, locomotionHandler.HandleAnalogInput(EInputKey.HorizontalAnalog, 1.0f));
        }

        [Test]
        public void ReceivesHorizontalAnalog_NoPlayerCameraInterface_DoesNotApplyHorizontalMotion()
        {
            var locomotionHandler = new FirstPersonInputHandler(null);

            const float expectedAppliedInput = 0.5f;

            locomotionHandler.HandleAnalogInput(EInputKey.HorizontalAnalog, expectedAppliedInput);

            Assert.AreNotEqual(expectedAppliedInput, _camera.RotateHorizontalResult);
        }

        [Test]
        public void ReceivesHorizontalAnalog_NoPlayerCameraInterface_ReturnsUnhandled()
        {
            var locomotionHandler = new FirstPersonInputHandler(null);

            Assert.AreEqual(EInputHandlerResult.Unhandled, locomotionHandler.HandleAnalogInput(EInputKey.HorizontalAnalog, 1.0f));
        }

        [Test]
        public void ReceivesCameraVertical_PlayerCameraInterface_AppliesInverseVerticalMotion()
        {
            var locomotionHandler = new FirstPersonInputHandler(_camera);

            const float expectedAppliedInput = 0.5f;

            locomotionHandler.HandleAnalogInput(EInputKey.CameraVertical, expectedAppliedInput);

            Assert.AreEqual(expectedAppliedInput, _camera.RotateVerticalResult * -1.0f);
        }

        [Test]
        public void ReceivesCameraVertical_PlayerCameraInterface_ReturnsHandled()
        {
            var locomotionHandler = new FirstPersonInputHandler(_camera);

            Assert.AreEqual(EInputHandlerResult.Handled, locomotionHandler.HandleAnalogInput(EInputKey.CameraVertical, 1.0f));
        }

        [Test]
        public void ReceivesCameraVertical_NoPlayerCameraInterface_DoesNotApplyVerticalMotion()
        {
            var locomotionHandler = new FirstPersonInputHandler(null);

            const float expectedAppliedInput = 0.5f;

            locomotionHandler.HandleAnalogInput(EInputKey.CameraVertical, expectedAppliedInput);

            Assert.AreNotEqual(expectedAppliedInput, _camera.RotateVerticalResult);
        }

        [Test]
        public void ReceivesCameraVertical_NoPlayerCameraInterface_ReturnsUnhandled()
        {
            var locomotionHandler = new FirstPersonInputHandler(null);

            Assert.AreEqual(EInputHandlerResult.Unhandled, locomotionHandler.HandleAnalogInput(EInputKey.CameraVertical, 1.0f));
        }

        [Test]
        public void ReceivesVerticalAnalog_PlayerCameraInterface_AppliesInverseVerticalMotion()
        {
            var locomotionHandler = new FirstPersonInputHandler(_camera);

            const float expectedAppliedInput = 0.5f;

            locomotionHandler.HandleAnalogInput(EInputKey.VerticalAnalog, expectedAppliedInput);

            Assert.AreEqual(expectedAppliedInput, _camera.RotateVerticalResult * -1.0f);
        }

        [Test]
        public void ReceivesVerticalAnalog_PlayerCameraInterface_ReturnsHandled()
        {
            var locomotionHandler = new FirstPersonInputHandler(_camera);

            Assert.AreEqual(EInputHandlerResult.Handled, locomotionHandler.HandleAnalogInput(EInputKey.VerticalAnalog, 1.0f));
        }

        [Test]
        public void ReceivesVerticalAnalog_NoPlayerCameraInterface_DoesNotApplyVerticalMotion()
        {
            var locomotionHandler = new FirstPersonInputHandler(null);

            const float expectedAppliedInput = 0.5f;

            locomotionHandler.HandleAnalogInput(EInputKey.VerticalAnalog, expectedAppliedInput);

            Assert.AreNotEqual(expectedAppliedInput, _camera.RotateVerticalResult);
        }

        [Test]
        public void ReceivesVerticalAnalog_NoPlayerCameraInterface_ReturnsUnhandled()
        {
            var locomotionHandler = new FirstPersonInputHandler(null);

            Assert.AreEqual(EInputHandlerResult.Unhandled, locomotionHandler.HandleAnalogInput(EInputKey.VerticalAnalog, 1.0f));
        }

        [Test]
        public void ReceivesCameraToggle_PlayerCameraInterface_SetsCameraModeThirdPerson()
        {
            var locomotionHandler = new FirstPersonInputHandler(_camera);

            locomotionHandler.HandleButtonInput(EInputKey.CameraToggle, true);

            Assert.AreEqual(EPlayerCameraMode.ThirdPerson, _camera.SetCameraModeResult);
        }

        [Test]
        public void ReceivesCameraToggle_PlayerCameraInterface_False_DoesNotSetCameraModeThirdPerson()
        {
            var locomotionHandler = new FirstPersonInputHandler(_camera);

            locomotionHandler.HandleButtonInput(EInputKey.CameraToggle, false);

            Assert.IsNull(_camera.SetCameraModeResult);
        }

        [Test]
        public void ReceivesCameraToggle_PlayerCameraInterface_False_ReturnsHandled()
        {
            var locomotionHandler = new FirstPersonInputHandler(_camera);

            Assert.AreEqual(EInputHandlerResult.Handled, locomotionHandler.HandleButtonInput(EInputKey.CameraToggle, false));
        }

        [Test]
        public void ReceivesCameraToggle_PlayerCameraInterface_ReturnsHandled()
        {
            var locomotionHandler = new FirstPersonInputHandler(_camera);

            Assert.AreEqual(EInputHandlerResult.Handled, locomotionHandler.HandleButtonInput(EInputKey.CameraToggle, true));
        }

        [Test]
        public void ReceivesCameraToggle_NoPlayerCameraInterface_DoesNotApplyCameraToggle()
        {
            var locomotionHandler = new FirstPersonInputHandler(null);

            locomotionHandler.HandleButtonInput(EInputKey.CameraToggle, false);

            Assert.IsNull(_camera.SetCameraModeResult);
        }

        [Test]
        public void ReceivesCameraToggle_NoPlayerCameraInterface_ReturnsUnhandled()
        {
            var locomotionHandler = new FirstPersonInputHandler(null);

            Assert.AreEqual(EInputHandlerResult.Unhandled, locomotionHandler.HandleButtonInput(EInputKey.CameraToggle, true));
        }
    }
}

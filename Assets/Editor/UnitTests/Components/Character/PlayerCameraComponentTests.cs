// Copyright (C) Threetee Gang All Rights Reserved

using Assets.Editor.UnitTests.Helpers;
using Assets.Scripts.Components.Character;
using Assets.Scripts.Test.Components.Character;
using Assets.Scripts.Test.Components.Controller;
using NUnit.Framework;
using UnityEngine;

namespace Assets.Editor.UnitTests.Components.Character
{
    [TestFixture]
    public class PlayerCameraComponentTestFixture
    {
        private UnityEngine.Camera _camera;
        private TestPlayerCameraComponent _cameraComponent;
        private TestControllerComponent _controller;
        private GameObject _pawn;

        [SetUp]
        public void BeforeTest()
        {
            _pawn = new GameObject();
            _pawn.transform.position = new Vector3(100.0f, 300.0f, 700.0f);

            _controller = new GameObject().AddComponent<TestControllerComponent>();
            _controller.SetPawn(_pawn);

            _camera = _controller.gameObject.AddComponent<UnityEngine.Camera>();

            _cameraComponent = _controller.gameObject.AddComponent<TestPlayerCameraComponent>();
            
            _cameraComponent.ZoomSpeed = 2.0f;

            _cameraComponent.TestStart();
        }

        [TearDown]
        public void AfterTest()
        {
            _cameraComponent = null;
            _camera = null;
            _controller = null;

            _pawn = null;
        }

        [Test]
        public void Start_SetsCameraParentToPawn()
        {
		    Assert.AreSame(_pawn.transform, _camera.gameObject.transform.parent);
        }

        [Test]
        public void SetRelativeCameraPosition_CameraSetToGivenOrientation()
        {
            var expectedLocation = new Vector3(25.0f, 50.0f, -10.0f);
            var expectedRotation = new Vector3(10.0f, 20.0f, 30.0f);
            const float expectedOrthoSize = 3.0f;
            
            _cameraComponent.SetRelativeCameraPosition(expectedLocation, expectedRotation, expectedOrthoSize);

            ExtendedAssertions.AssertVectorsNearlyEqual(expectedLocation, _cameraComponent.gameObject.transform.localPosition);
            ExtendedAssertions.AssertVectorsNearlyEqual(expectedRotation, _cameraComponent.gameObject.transform.rotation.eulerAngles);
            Assert.AreEqual(expectedOrthoSize, _camera.orthographicSize);
        }

        [Test]
        public void SetRelativeCameraPosition_AdjustsZoomParametersToMatch()
        {
            var currentInitialLocation = new Vector3(0.0f, 50.0f, -10.0f);
            var currentInitialRotation = new Vector3(10.0f, 20.0f, 30.0f);
            const float expectedOrthoSize = 3.0f;

            _cameraComponent.SetRelativeCameraPosition(currentInitialLocation, currentInitialRotation, expectedOrthoSize);

            Assert.AreEqual(expectedOrthoSize, _camera.orthographicSize);
        }

        [Test]
        public void RotateHorizontal_RotatesParentExpectedAmount()
        {
            const float rotationValue = 0.5f;
            const float deltaTime = 1.2f;

            _cameraComponent.RotateHorizontal(rotationValue);
            _cameraComponent.TestUpdate(deltaTime);

            var expectedRotation = new Vector3(0.0f, 0.0f, 120.0f);

            ExtendedAssertions.AssertVectorsNearlyEqual(expectedRotation, _cameraComponent.gameObject.transform.parent.rotation.eulerAngles);
        }

        [Test]
        public void RotateHorizontal_ClampsToRange()
        {
            const float rotationValue = 1.5f;
            const float deltaTime = 1.2f;

            _cameraComponent.RotateHorizontal(rotationValue);
            _cameraComponent.TestUpdate(deltaTime);

            var expectedRotation = new Vector3(0.0f, 0.0f, 240.0f);

            ExtendedAssertions.AssertVectorsNearlyEqual(expectedRotation, _cameraComponent.gameObject.transform.parent.rotation.eulerAngles);
        }

        [Test]
        public void Rotate_ResetsToZeroAfterUpdate()
        {
            const float rotationValue = 1.5f;
            const float deltaTime = 1.2f;

            _cameraComponent.RotateHorizontal(rotationValue);
            _cameraComponent.TestUpdate(deltaTime);
            _cameraComponent.TestUpdate(deltaTime);

            var expectedRotation = new Vector3(0.0f, 0.0f, 240.0f);

            ExtendedAssertions.AssertVectorsNearlyEqual(expectedRotation, _cameraComponent.gameObject.transform.parent.rotation.eulerAngles);
        }

        [Test]
        public void Zoom_AltersOrthographicSizeAsExpected()
        {
            const float zoomValue = 0.5f;
            const float deltaTime = 0.1f;

            const float expectedOrthoSize = 3.0f;

            _cameraComponent.SetRelativeCameraPosition(Vector3.zero, Vector3.zero, expectedOrthoSize);

            _cameraComponent.Zoom(zoomValue);

            var expectedSize = expectedOrthoSize + (deltaTime * zoomValue * - 1 * _cameraComponent.ZoomSpeed);

            _cameraComponent.TestUpdate(deltaTime);

            Assert.AreEqual(expectedSize, _camera.orthographicSize);
        }

        [Test]
        public void Zoom_AltersOrthographicSizeInClampedRegion()
        {
            const float zoomValue = 1.0f;
            const float deltaTime = 0.1f;

            const float expectedOrthoSize = 3.0f;

            _cameraComponent.SetRelativeCameraPosition(Vector3.zero, Vector3.zero, expectedOrthoSize);

            _cameraComponent.Zoom(zoomValue + 100.0f);

            var expectedSize = expectedOrthoSize + (deltaTime * zoomValue * -1 * _cameraComponent.ZoomSpeed);

            _cameraComponent.TestUpdate(deltaTime);

            Assert.AreEqual(expectedSize, _camera.orthographicSize);
        }

        [Test]
        public void Zoom_ResetsToZeroAfterUpdate()
        {
            const float zoomValue = 0.5f;
            const float deltaTime = 0.1f;

            const float expectedOrthoSize = 3.0f;

            _cameraComponent.SetRelativeCameraPosition(Vector3.zero, Vector3.zero, expectedOrthoSize);

            _cameraComponent.Zoom(zoomValue);

            var expectedSize = expectedOrthoSize + (deltaTime * zoomValue * -1 * _cameraComponent.ZoomSpeed);

            _cameraComponent.TestUpdate(deltaTime);
            _cameraComponent.TestUpdate(deltaTime);

            Assert.AreEqual(expectedSize, _camera.orthographicSize);
        }

        [Test]
        public void Zoom_BoundByMaxRegion()
        {
            const float zoomValue = 1.0f;
            const float deltaTime = 0.1f;

            const float expectedOrthoSize = 3.0f;

            _cameraComponent.ZoomSpeed = 20000.0f;
            _cameraComponent.SetRelativeCameraPosition(Vector3.zero, Vector3.zero, expectedOrthoSize);

            _cameraComponent.Zoom(zoomValue);

            var expectedSize = expectedOrthoSize * PlayerCameraConstants.MaxZoomModifier;

            _cameraComponent.TestUpdate(deltaTime);

            Assert.AreEqual(expectedSize, _camera.orthographicSize);
        }

        [Test]
        public void Zoom_BoundByMinRegion()
        {
            const float zoomValue = -1.0f;
            const float deltaTime = 0.1f;

            const float expectedOrthoSize = 3.0f;

            _cameraComponent.ZoomSpeed = 20000.0f;
            _cameraComponent.SetRelativeCameraPosition(Vector3.zero, Vector3.zero, expectedOrthoSize);

            _cameraComponent.Zoom(zoomValue);

            var expectedSize = expectedOrthoSize * PlayerCameraConstants.MinZoomModifier;

            _cameraComponent.TestUpdate(deltaTime);

            Assert.AreEqual(expectedSize, _camera.orthographicSize);
        }

        [Test]
        public void ResetZoom_SetsCameraOrthographicSizeToInitialValue()
        {
            const float expectedOrthoSize = 3.0f;

            _cameraComponent.SetRelativeCameraPosition(Vector3.zero, Vector3.zero, expectedOrthoSize);

            _cameraComponent.Zoom(1.0f);

            _cameraComponent.TestUpdate(1.0f);

            _cameraComponent.ResetZoom();
            _cameraComponent.TestUpdate(1.0f);

            Assert.AreEqual(expectedOrthoSize, _camera.orthographicSize);
        }

        [Test]
        public void ResetZoom_OverridesZoomChanges()
        {
            const float expectedOrthoSize = 3.0f;

            _cameraComponent.SetRelativeCameraPosition(Vector3.zero, Vector3.zero, expectedOrthoSize);

            _cameraComponent.Zoom(1.0f);
            _cameraComponent.ResetZoom();
            _cameraComponent.TestUpdate(1.0f);

            Assert.AreEqual(expectedOrthoSize, _camera.orthographicSize);
        }

        [Test]
        public void ResetZoom_ResetFlagAfterUpdate()
        {
            const float zoomValue = 0.5f;
            const float deltaTime = 0.1f;

            const float expectedOrthoSize = 3.0f;

            _cameraComponent.SetRelativeCameraPosition(Vector3.zero, Vector3.zero, expectedOrthoSize);

            _cameraComponent.ResetZoom();
            _cameraComponent.Zoom(zoomValue);
            _cameraComponent.TestUpdate(deltaTime);

            var expectedSize = expectedOrthoSize + (deltaTime * zoomValue * -1 * _cameraComponent.ZoomSpeed);

            _cameraComponent.Zoom(zoomValue);
            _cameraComponent.TestUpdate(deltaTime);

            Assert.AreEqual(expectedSize, _camera.orthographicSize);
        }
    }
}

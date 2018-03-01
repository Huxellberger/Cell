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
            
            _cameraComponent.InitialLocation = new Vector3(0.0f, 200.0f, -10.0f);
            _cameraComponent.InitialRotation = new Vector3(0.5f, 0.8f, 0.3f);
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
        public void Start_LocalPositionMatchesInitialLocation()
        {
            Assert.AreEqual(_cameraComponent.InitialLocation, _camera.gameObject.transform.localPosition);
        }

        [Test]
        public void Start_LocalRotationMatchesInitialRotation()
        {
            var rotation = _camera.gameObject.transform.rotation.eulerAngles;

            ExtendedAssertions.AssertVectorsNearlyEqual(_cameraComponent.InitialRotation, rotation);
        }

        [Test]
        public void SetRelativeCameraPosition_CameraSetToGivenOrientation()
        {
            var expectedLocation = new Vector3(25.0f, 50.0f, -10.0f);
            var expectedRotation = new Vector3(10.0f, 20.0f, 30.0f);
            
            _cameraComponent.SetRelativeCameraPosition(expectedLocation, expectedRotation);

            ExtendedAssertions.AssertVectorsNearlyEqual(expectedLocation, _cameraComponent.gameObject.transform.localPosition);
            ExtendedAssertions.AssertVectorsNearlyEqual(expectedRotation, _cameraComponent.gameObject.transform.rotation.eulerAngles);
        }

        [Test]
        public void SetRelativeCameraPosition_UpdatesInitialLocationAndRotation()
        {
            var expectedLocation = new Vector3(25.0f, 50.0f, -10.0f);
            var expectedRotation = new Vector3(10.0f, 20.0f, 30.0f);

            _cameraComponent.SetRelativeCameraPosition(expectedLocation, expectedRotation);

            ExtendedAssertions.AssertVectorsNearlyEqual(expectedLocation, _cameraComponent.InitialLocation);
            ExtendedAssertions.AssertVectorsNearlyEqual(expectedRotation, _cameraComponent.InitialRotation);
        }

        [Test]
        public void SetRelativeCameraPosition_AdjustsZoomParametersToMatch()
        {
            var currentInitialLocation = new Vector3(0.0f, 50.0f, -10.0f);
            var currentInitialRotation = new Vector3(10.0f, 20.0f, 30.0f);

            _cameraComponent.SetRelativeCameraPosition(currentInitialLocation, currentInitialRotation);

            const float zoomValue = 1.0f;
            const float deltaTime = 0.1f;

            _cameraComponent.Zoom(zoomValue + 100.0f);

            var expectedPosition = new Vector3
            (
                0.0f,
                0.0f,
                _cameraComponent.transform.localPosition.z - (currentInitialLocation.z * zoomValue * deltaTime * _cameraComponent.ZoomSpeed)
            );

            _cameraComponent.TestUpdate(deltaTime);

            ExtendedAssertions.AssertVectorsNearlyEqual(expectedPosition, _cameraComponent.gameObject.transform.localPosition);
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
        public void Zoom_AltersLocalPositionAsExpected()
        {
            const float zoomValue = 0.5f;
            const float deltaTime = 0.1f;

            _cameraComponent.Zoom(zoomValue);

            var expectedPosition = new Vector3
            (
                0.0f, 
                0.0f, 
                _cameraComponent.transform.localPosition.z - (_cameraComponent.InitialLocation.z * zoomValue * deltaTime * _cameraComponent.ZoomSpeed)
            );

            _cameraComponent.TestUpdate(deltaTime);

            ExtendedAssertions.AssertVectorsNearlyEqual(expectedPosition, _cameraComponent.gameObject.transform.localPosition);
        }

        [Test]
        public void Zoom_AltersLocalPositionInClampedRegion()
        {
            const float zoomValue = 1.0f;
            const float deltaTime = 0.1f;

            _cameraComponent.Zoom(zoomValue + 100.0f);

            var expectedPosition = new Vector3
            (
                0.0f,
                0.0f,
                _cameraComponent.transform.localPosition.z - (_cameraComponent.InitialLocation.z * zoomValue * deltaTime * _cameraComponent.ZoomSpeed)
            );

            _cameraComponent.TestUpdate(deltaTime);

            ExtendedAssertions.AssertVectorsNearlyEqual(expectedPosition, _cameraComponent.gameObject.transform.localPosition);
        }

        [Test]
        public void Zoom_ResetsToZeroAfterUpdate()
        {
            const float zoomValue = 0.5f;
            const float deltaTime = 0.1f;

            _cameraComponent.Zoom(zoomValue);

            var expectedPosition = new Vector3
            (
                0.0f,
                0.0f,
                _cameraComponent.transform.localPosition.z - (_cameraComponent.InitialLocation.z * zoomValue * deltaTime * _cameraComponent.ZoomSpeed)
            );

            _cameraComponent.TestUpdate(deltaTime);
            _cameraComponent.TestUpdate(deltaTime);

            ExtendedAssertions.AssertVectorsNearlyEqual(expectedPosition, _cameraComponent.gameObject.transform.localPosition);
        }

        [Test]
        public void Zoom_BoundByMaxRegion()
        {
            const float zoomValue = 1.0f;
            const float deltaTime = 1.2f;
            _cameraComponent.ZoomSpeed = 10000.0f;

            _cameraComponent.Zoom(zoomValue);

            var expectedPosition = new Vector3(0.0f, 0.0f, (_cameraComponent.InitialLocation * PlayerCameraConstants.MaxZoomModifier).z);

            _cameraComponent.TestUpdate(deltaTime);

            ExtendedAssertions.AssertVectorsNearlyEqual(expectedPosition, _cameraComponent.gameObject.transform.localPosition);
        }

        [Test]
        public void Zoom_BoundByMinRegion()
        {
            const float zoomValue = -1.0f;
            const float deltaTime = 1.2f;
            _cameraComponent.ZoomSpeed = 10000.0f;

            _cameraComponent.Zoom(zoomValue);

            var expectedPosition = new Vector3(0.0f, 0.0f, (_cameraComponent.InitialLocation * PlayerCameraConstants.MinZoomModifier).z);

            _cameraComponent.TestUpdate(deltaTime);

            ExtendedAssertions.AssertVectorsNearlyEqual(expectedPosition, _cameraComponent.gameObject.transform.localPosition);
        }

        [Test]
        public void ResetZoom_SetsLocalPositionToInitialLocation()
        {
            _cameraComponent.Zoom(1.2f);

            _cameraComponent.TestUpdate(12.1f);

            _cameraComponent.ResetZoom();

            _cameraComponent.TestUpdate(12.1f);

            ExtendedAssertions.AssertVectorsNearlyEqual(_cameraComponent.InitialLocation, _cameraComponent.gameObject.transform.localPosition);
        }

        [Test]
        public void ResetZoom_SetsXRotationToInitialRotation()
        {
            var newRotation = new Vector3(20.0f, 12.0f, 3.0f);
            _cameraComponent.transform.eulerAngles = newRotation;

            _cameraComponent.ResetZoom();

            _cameraComponent.TestUpdate(12.1f);

            var expectedRotation = new Vector3(_cameraComponent.InitialRotation.x, newRotation.y, newRotation.z);

            ExtendedAssertions.AssertVectorsNearlyEqual(expectedRotation, _cameraComponent.gameObject.transform.eulerAngles);
        }

        [Test]
        public void ResetZoom_OverridesZoomChanges()
        {
            _cameraComponent.Zoom(1.2f);
            _cameraComponent.ResetZoom();

            _cameraComponent.TestUpdate(12.1f);

            ExtendedAssertions.AssertVectorsNearlyEqual(_cameraComponent.InitialLocation, _cameraComponent.gameObject.transform.localPosition);
        }

        [Test]
        public void ResetZoom_ResetFlagAfterUpdate()
        {
            _cameraComponent.Zoom(1.2f);
            _cameraComponent.ResetZoom();

            _cameraComponent.TestUpdate(12.1f);

            _cameraComponent.Zoom(1.2f);

            _cameraComponent.TestUpdate(12.1f);

            ExtendedAssertions.AssertVectorsNotNearlyEqual(_cameraComponent.InitialLocation, _cameraComponent.gameObject.transform.localPosition);
        }
    }
}

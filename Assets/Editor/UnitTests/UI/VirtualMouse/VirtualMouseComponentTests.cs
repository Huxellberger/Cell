// Copyright (C) Threetee Gang All Rights Reserved 

using Assets.Editor.UnitTests.Helpers;
using Assets.Scripts.Test.UI.VirtualMouse;
using Assets.Scripts.UI.VirtualMouse;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.Editor.UnitTests.UI.VirtualMouse
{
    [TestFixture]
    public class VirtualMouseComponentTestFixture
    {
        private TestVirtualMouseComponent _virtualMouse;
        private Image _image;

        private bool _priorCursorSetting;

        [SetUp]
        public void BeforeTest()
        {
            _priorCursorSetting = Cursor.visible;

            _virtualMouse = new GameObject().AddComponent<TestVirtualMouseComponent>();

            _image = new GameObject().AddComponent<Image>();

            _virtualMouse.MouseGraphic = _image;

            _virtualMouse.TestAwake();
        }

        [TearDown]
        public void AfterTest()
        {
            _virtualMouse.TestDestroy();

            _image = null;

            _virtualMouse = null;

            Cursor.visible = _priorCursorSetting;
        }

        [Test]
        public void Awake_RegistersAsInstance()
        {
            Assert.AreSame(VirtualMouseInstance.CurrentVirtualMouse, _virtualMouse);
        }

        [Test]
        public void Awake_MouseGraphicDoesNotBlockRaycast()
        {
            Assert.IsFalse(_image.raycastTarget);
        }

        [Test]
        public void Awake_DisablesGraphic()
        {
            Assert.IsFalse(_image.enabled);
        }

        [Test]
        public void Destroy_RegistersAsInstance()
        {
            _virtualMouse.TestDestroy();

            Assert.IsNull(VirtualMouseInstance.CurrentVirtualMouse);
        }

        [Test]
        public void SetVirtualMousePosition_UpdatesPositionIgnoringZ()
        {
            var expectedVector = new Vector3(200.0f, 300.0f, 0.0f);

            _virtualMouse.SetVirtualMousePosition(expectedVector + new Vector3(0.0f, 0.0f, 100.0f));

            ExtendedAssertions.AssertVectorsNearlyEqual(expectedVector, _image.rectTransform.position);
        }

        [Test]
        public void SetMouseVisible_True_GraphicEnabled()
        {
            _virtualMouse.SetMouseVisibile(true);

            Assert.IsTrue(_image.enabled);
        }

        [Test]
        public void SetMouseVisible_True_CursorDisabled()
        {
            _virtualMouse.SetMouseVisibile(true);

            Assert.IsFalse(Cursor.visible);
        }

        [Test]
        public void SetMouseVisible_True_IsVisibleReturnsTrue()
        {
            _virtualMouse.SetMouseVisibile(true);

            Assert.IsTrue(_virtualMouse.IsMouseVisible());
        }

        [Test]
        public void SetMouseVisible_False_GraphicDisabled()
        {
            _virtualMouse.SetMouseVisibile(false);

            Assert.IsFalse(_image.enabled);
        }

        [Test]
        public void SetMouseVisible_False_CursorResetToPriorStatus()
        {
            _virtualMouse.SetMouseVisibile(false);

            Assert.IsTrue(Cursor.visible == _priorCursorSetting);
        }

        [Test]
        public void SetMouseVisible_False_IsVisibleReturnsFalse()
        {
            _virtualMouse.SetMouseVisibile(false);

            Assert.IsFalse(_virtualMouse.IsMouseVisible());
        }

        [Test]
        public void GetButtonState_DefaultStateIsClear()
        {
            foreach (var buttonEntry in _virtualMouse.GetVirtualMouseData().ButtonEntries)
            {
                Assert.IsFalse(buttonEntry.Value.Pressed);
                Assert.IsFalse(buttonEntry.Value.Released);
            }
        }

        [Test]
        public void SetButtonState_ChangesButtonStateAsExpected()
        {
            const PointerEventData.InputButton expectedButton = PointerEventData.InputButton.Left;
            
            _virtualMouse.SetButtonState(expectedButton, true);

            Assert.IsTrue(_virtualMouse.GetVirtualMouseData().ButtonEntries[expectedButton].Pressed);
        }

        [Test]
        public void SetButtonState_UpdateClearsState()
        {
            const PointerEventData.InputButton expectedButton = PointerEventData.InputButton.Left;

            _virtualMouse.SetButtonState(expectedButton, true);
            _virtualMouse.TestUpdate(1.0f);

            Assert.IsFalse(_virtualMouse.GetVirtualMouseData().ButtonEntries[expectedButton].Pressed);
        }

        [Test]
        public void ApplyHorizontalMovement_PositionIsTransformedAsExpected()
        {
            const float movementMod = 0.5f;
            const float updateDelta = 1.5f;

            _virtualMouse.ApplyHorizontalMovement(movementMod);
            _virtualMouse.TestUpdate(updateDelta);

            var expectedPosition = new Vector3(movementMod * updateDelta * _virtualMouse.MouseSpeed, 0.0f, 0.0f);

            ExtendedAssertions.AssertVectorsNearlyEqual(expectedPosition, _image.rectTransform.position);
        }

        [Test]
        public void ApplyHorizontalMovement_ModifierIsClampedToMax()
        {
            const float movementMod = 1.0f;
            const float updateDelta = 1.5f;

            _virtualMouse.ApplyHorizontalMovement(movementMod + 100.0f);
            _virtualMouse.TestUpdate(updateDelta);

            var expectedPosition = new Vector3(movementMod * updateDelta * _virtualMouse.MouseSpeed, 0.0f, 0.0f);

            ExtendedAssertions.AssertVectorsNearlyEqual(expectedPosition, _image.rectTransform.position);
        }

        [Test]
        public void ApplyHorizontalMovement_ModifierIsClampedToMin()
        {
            const float movementMod = -1.0f;
            const float updateDelta = 1.5f;

            _virtualMouse.ApplyHorizontalMovement(movementMod - 100.0f);
            _virtualMouse.TestUpdate(updateDelta);

            var expectedPosition = new Vector3(movementMod * updateDelta * _virtualMouse.MouseSpeed, 0.0f, 0.0f);

            ExtendedAssertions.AssertVectorsNearlyEqual(expectedPosition, _image.rectTransform.position);
        }

        [Test]
        public void ApplyHorizontalMovement_ResetOnUpdate()
        {
            const float movementMod = -0.5f;
            const float updateDelta = 1.5f;

            _virtualMouse.ApplyHorizontalMovement(movementMod);
            _virtualMouse.TestUpdate(updateDelta);
            _virtualMouse.TestUpdate(updateDelta);

            var expectedPosition = new Vector3(movementMod * updateDelta * _virtualMouse.MouseSpeed, 0.0f, 0.0f);

            ExtendedAssertions.AssertVectorsNearlyEqual(expectedPosition, _image.rectTransform.position);
        }

        [Test]
        public void ApplyVerticalMovement_PositionIsTransformedAsExpected()
        {
            const float movementMod = 0.5f;
            const float updateDelta = 1.5f;

            _virtualMouse.ApplyVerticalMovement(movementMod);
            _virtualMouse.TestUpdate(updateDelta);

            var expectedPosition = new Vector3(0.0f, movementMod * updateDelta * _virtualMouse.MouseSpeed, 0.0f);

            ExtendedAssertions.AssertVectorsNearlyEqual(expectedPosition, _image.rectTransform.position);
        }

        [Test]
        public void ApplyVerticalMovement_ModifierIsClampedToMax()
        {
            const float movementMod = 1.0f;
            const float updateDelta = 1.5f;

            _virtualMouse.ApplyVerticalMovement(movementMod + 100.0f);
            _virtualMouse.TestUpdate(updateDelta);

            var expectedPosition = new Vector3(0.0f, movementMod * updateDelta * _virtualMouse.MouseSpeed, 0.0f);

            ExtendedAssertions.AssertVectorsNearlyEqual(expectedPosition, _image.rectTransform.position);
        }

        [Test]
        public void ApplyVerticalMovement_ModifierIsClampedToMin()
        {
            const float movementMod = -1.0f;
            const float updateDelta = 1.5f;

            _virtualMouse.ApplyVerticalMovement(movementMod - 100.0f);
            _virtualMouse.TestUpdate(updateDelta);

            var expectedPosition = new Vector3(0.0f, movementMod * updateDelta * _virtualMouse.MouseSpeed, 0.0f);

            ExtendedAssertions.AssertVectorsNearlyEqual(expectedPosition, _image.rectTransform.position);
        }

        [Test]
        public void ApplyVerticalMovement_ResetOnUpdate()
        {
            const float movementMod = -0.5f;
            const float updateDelta = 1.5f;

            _virtualMouse.ApplyVerticalMovement(movementMod);
            _virtualMouse.TestUpdate(updateDelta);
            _virtualMouse.TestUpdate(updateDelta);

            var expectedPosition = new Vector3(0.0f, movementMod * updateDelta * _virtualMouse.MouseSpeed, 0.0f);

            ExtendedAssertions.AssertVectorsNearlyEqual(expectedPosition, _image.rectTransform.position);
        }

        [Test]
        public void ApplyVerticalAndHorizontalMovement_Normalised()
        {
            const float movementMod = 1.0f;
            const float updateDelta = 1.5f;

            _virtualMouse.ApplyVerticalMovement(movementMod);
            _virtualMouse.ApplyHorizontalMovement(movementMod);
            _virtualMouse.TestUpdate(updateDelta);

            var expectedPosition = new Vector3(movementMod, movementMod, 0.0f).normalized * updateDelta * _virtualMouse.MouseSpeed;

            ExtendedAssertions.AssertVectorsNearlyEqual(expectedPosition, _image.rectTransform.position);
        }
    }
}

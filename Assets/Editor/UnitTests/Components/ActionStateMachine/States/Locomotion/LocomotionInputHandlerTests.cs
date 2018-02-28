// Copyright (C) Threetee Gang All Rights Reserved

using Assets.Scripts.Components.ActionStateMachine.States.Locomotion;
using Assets.Scripts.Components.Character;
using Assets.Scripts.Components.Equipment.Holdables;
using Assets.Scripts.Input;
using Assets.Scripts.Test.Components.Character;
using Assets.Scripts.Test.Components.Equipment.Holdables;
using Assets.Scripts.Test.Components.Movement;
using NUnit.Framework;
using UnityEngine;

namespace Assets.Editor.UnitTests.Components.ActionStateMachine.States.Locomotion
{
    [TestFixture]
    public class LocomotionInputHandlerTestFixture
    {
        private MockMovementComponent _movement;
        private MockPlayerCameraComponent _camera;
        private MockHeldItemComponent _heldItem;

        [SetUp]
        public void BeforeTest()
        {
            _movement = new GameObject().AddComponent<MockMovementComponent>();
            _camera = _movement.gameObject.AddComponent<MockPlayerCameraComponent>();
            _heldItem = _movement.gameObject.AddComponent<MockHeldItemComponent>();
        }

        [TearDown]
        public void AfterTest()
        {
            _camera = null;
            _movement = null;
            _heldItem = null;
        }

        #region Movement
        [Test]
        public void ReceivesHorizontalAnalog_MovementInterface_AppliesSidewaysMotion()
        {
            var locomotionHandler = new LocomotionInputHandler(_movement, _camera, _heldItem);

            const float expectedAppliedInput = 0.5f;

            locomotionHandler.HandleAnalogInput(EInputKey.HorizontalAnalog, expectedAppliedInput);

            Assert.AreEqual(expectedAppliedInput, _movement.ApplySidewaysMotionResult);
        }

        [Test]
        public void ReceivesHorizontalAnalog_MovementInterface_ReturnsHandled()
        {
            var locomotionHandler = new LocomotionInputHandler(_movement, _camera, _heldItem);

            Assert.AreEqual(EInputHandlerResult.Handled, locomotionHandler.HandleAnalogInput(EInputKey.HorizontalAnalog, 1.0f));
        }

        [Test]
        public void ReceivesHorizontalAnalog_NoMovementInterface_DoesNotApplySidewaysMotion()
        {
            var locomotionHandler = new LocomotionInputHandler(null, _camera, _heldItem);

            const float expectedAppliedInput = 0.5f;

            locomotionHandler.HandleAnalogInput(EInputKey.HorizontalAnalog, expectedAppliedInput);

            Assert.AreNotEqual(expectedAppliedInput, _movement.ApplySidewaysMotionResult);
        }

        [Test]
        public void ReceivesHorizontalAnalog_NoMovementInterface_ReturnsUnhandled()
        {
            var locomotionHandler = new LocomotionInputHandler(null, _camera, _heldItem);

            Assert.AreEqual(EInputHandlerResult.Unhandled, locomotionHandler.HandleAnalogInput(EInputKey.HorizontalAnalog, 1.0f));
        }

        [Test]
        public void ReceivesVerticalAnalog_MovementInterface_AppliesForwardMotion()
        {
            var locomotionHandler = new LocomotionInputHandler(_movement, _camera, _heldItem);

            const float expectedAppliedInput = 0.5f;

            locomotionHandler.HandleAnalogInput(EInputKey.VerticalAnalog, expectedAppliedInput);

            Assert.AreEqual(expectedAppliedInput, _movement.ApplyForwardMotionResult);
        }

        [Test]
        public void ReceivesVerticalAnalog_MovementInterface_ReturnsHandled()
        {
            var locomotionHandler = new LocomotionInputHandler(_movement, _camera, _heldItem);

            Assert.AreEqual(EInputHandlerResult.Handled, locomotionHandler.HandleAnalogInput(EInputKey.VerticalAnalog, 1.0f));
        }

        [Test]
        public void ReceivesVerticalAnalog_NoMovementInterface_DoesNotApplyForwardMotion()
        {
            var locomotionHandler = new LocomotionInputHandler(null, _camera, _heldItem);

            const float expectedAppliedInput = 0.5f;

            locomotionHandler.HandleAnalogInput(EInputKey.VerticalAnalog, expectedAppliedInput);

            Assert.AreNotEqual(expectedAppliedInput, _movement.ApplyForwardMotionResult);
        }

        [Test]
        public void ReceivesVerticalAnalog_NoMovementInterface_ReturnsUnhandled()
        {
            var locomotionHandler = new LocomotionInputHandler(null, _camera, _heldItem);

            Assert.AreEqual(EInputHandlerResult.Unhandled, locomotionHandler.HandleAnalogInput(EInputKey.VerticalAnalog, 1.0f));
        }

        [Test]
        public void ReceivesSprintButton_MovementInterface_SetsFlag()
        {
            var locomotionHandler = new LocomotionInputHandler(_movement, _camera, _heldItem);

            locomotionHandler.HandleButtonInput(EInputKey.SprintButton, true);

            Assert.IsTrue(_movement.SetSprintEnabledResult);
        }

        [Test]
        public void ReceivesSprintButton_MovementInterface_False_SetsFlag()
        {
            var locomotionHandler = new LocomotionInputHandler(_movement, _camera, _heldItem);

            locomotionHandler.HandleButtonInput(EInputKey.SprintButton, false);

            Assert.IsFalse(_movement.SetSprintEnabledResult);
        }

        [Test]
        public void ReceivesSprintButton_MovementInterface_False_ReturnsHandled()
        {
            var locomotionHandler = new LocomotionInputHandler(_movement, _camera, _heldItem);

            Assert.AreEqual(EInputHandlerResult.Handled, locomotionHandler.HandleButtonInput(EInputKey.SprintButton, false));
        }

        [Test]
        public void ReceivesSprintButton_MovementInterface_ReturnsHandled()
        {
            var locomotionHandler = new LocomotionInputHandler(_movement, _camera, _heldItem);

            Assert.AreEqual(EInputHandlerResult.Handled, locomotionHandler.HandleButtonInput(EInputKey.SprintButton, true));
        }

        [Test]
        public void ReceivesSprintButton_NoMovementInterface_DoesNotSetSprint()
        {
            var locomotionHandler = new LocomotionInputHandler(null, _camera, _heldItem);

            locomotionHandler.HandleButtonInput(EInputKey.SprintButton, false);

            Assert.IsNull(_movement.SetSprintEnabledResult);
        }

        [Test]
        public void ReceivesSprintButton_NoMovementInterface_ReturnsUnhandled()
        {
            var locomotionHandler = new LocomotionInputHandler(null, _camera, _heldItem);

            Assert.AreEqual(EInputHandlerResult.Unhandled, locomotionHandler.HandleButtonInput(EInputKey.SprintButton, true));
        }
        #endregion

        #region Camera
        [Test]
        public void ReceivesCameraHorizontal_PlayerCameraInterface_AppliesHorizontalMotion()
        {
            var locomotionHandler = new LocomotionInputHandler(_movement, _camera, _heldItem);

            const float expectedAppliedInput = 0.5f;

            locomotionHandler.HandleAnalogInput(EInputKey.CameraHorizontal, expectedAppliedInput);

            Assert.AreEqual(expectedAppliedInput, _camera.RotateHorizontalResult);
        }

        [Test]
        public void ReceivesCameraHorizontal_PlayerCameraInterface_ReturnsHandled()
        {
            var locomotionHandler = new LocomotionInputHandler(_movement, _camera, _heldItem);

            Assert.AreEqual(EInputHandlerResult.Handled, locomotionHandler.HandleAnalogInput(EInputKey.CameraHorizontal, 1.0f));
        }

        [Test]
        public void ReceivesCameraHorizontal_NoPlayerCameraInterface_DoesNotApplyHorizontalMotion()
        {
            var locomotionHandler = new LocomotionInputHandler(_movement, null, _heldItem);

            const float expectedAppliedInput = 0.5f;

            locomotionHandler.HandleAnalogInput(EInputKey.CameraHorizontal, expectedAppliedInput);

            Assert.AreNotEqual(expectedAppliedInput, _camera.RotateHorizontalResult);
        }

        [Test]
        public void ReceivesCameraHorizontal_NoPlayerCameraInterface_ReturnsUnhandled()
        {
            var locomotionHandler = new LocomotionInputHandler(_movement, null, _heldItem);

            Assert.AreEqual(EInputHandlerResult.Unhandled, locomotionHandler.HandleAnalogInput(EInputKey.CameraHorizontal, 1.0f));
        }

        [Test]
        public void ReceivesCameraZoom_PlayerCameraInterface_AppliesZoomMotion()
        {
            var locomotionHandler = new LocomotionInputHandler(_movement, _camera, _heldItem);

            const float expectedAppliedInput = 0.5f;

            locomotionHandler.HandleAnalogInput(EInputKey.CameraZoom, expectedAppliedInput);

            Assert.AreEqual(expectedAppliedInput, _camera.ZoomResult);
        }

        [Test]
        public void ReceivesCameraZoom_PlayerCameraInterface_ReturnsHandled()
        {
            var locomotionHandler = new LocomotionInputHandler(_movement, _camera, _heldItem);

            Assert.AreEqual(EInputHandlerResult.Handled, locomotionHandler.HandleAnalogInput(EInputKey.CameraZoom, 1.0f));
        }

        [Test]
        public void ReceivesCameraZoom_NoPlayerCameraInterface_DoesNotApplyZoomMotion()
        {
            var locomotionHandler = new LocomotionInputHandler(_movement, null, _heldItem);

            const float expectedAppliedInput = 0.5f;

            locomotionHandler.HandleAnalogInput(EInputKey.CameraZoom, expectedAppliedInput);

            Assert.AreNotEqual(expectedAppliedInput, _camera.ZoomResult);
        }

        [Test]
        public void ReceivesCameraZoom_NoPlayerCameraInterface_ReturnsUnhandled()
        {
            var locomotionHandler = new LocomotionInputHandler(_movement, null, _heldItem);

            Assert.AreEqual(EInputHandlerResult.Unhandled, locomotionHandler.HandleAnalogInput(EInputKey.CameraZoom, 1.0f));
        }

        [Test]
        public void ReceivesCameraZoomReset_PlayerCameraInterface_AppliesZoomReset()
        {
            var locomotionHandler = new LocomotionInputHandler(_movement, _camera, _heldItem);

            locomotionHandler.HandleButtonInput(EInputKey.CameraZoomReset, true);

            Assert.IsTrue(_camera.ResetZoomCalled);
        }

        [Test]
        public void ReceivesCameraZoomReset_PlayerCameraInterface_False_DoesNotApplyZoomReset()
        {
            var locomotionHandler = new LocomotionInputHandler(_movement, _camera, _heldItem);

            locomotionHandler.HandleButtonInput(EInputKey.CameraZoomReset, false);

            Assert.IsFalse(_camera.ResetZoomCalled);
        }

        [Test]
        public void ReceivesCameraZoomReset_PlayerCameraInterface_False_ReturnsHandled()
        {
            var locomotionHandler = new LocomotionInputHandler(_movement, _camera, _heldItem);

            Assert.AreEqual(EInputHandlerResult.Handled, locomotionHandler.HandleButtonInput(EInputKey.CameraZoomReset, false));
        }

        [Test]
        public void ReceivesCameraZoomReset_PlayerCameraInterface_ReturnsHandled()
        {
            var locomotionHandler = new LocomotionInputHandler(_movement, _camera, _heldItem);

            Assert.AreEqual(EInputHandlerResult.Handled, locomotionHandler.HandleButtonInput(EInputKey.CameraZoomReset, true));
        }

        [Test]
        public void ReceivesCameraZoomReset_NoPlayerCameraInterface_DoesNotApplyZoomReset()
        {
            var locomotionHandler = new LocomotionInputHandler(_movement, null, _heldItem);

            locomotionHandler.HandleButtonInput(EInputKey.CameraZoomReset, false);

            Assert.IsFalse(_camera.ResetZoomCalled);
        }

        [Test]
        public void ReceivesCameraZoomReset_NoPlayerCameraInterface_ReturnsUnhandled()
        {
            var locomotionHandler = new LocomotionInputHandler(_movement, null, _heldItem);

            Assert.AreEqual(EInputHandlerResult.Unhandled, locomotionHandler.HandleButtonInput(EInputKey.CameraZoomReset, true));
        }
        #endregion

        #region HeldItem
        [Test]
        public void ReceivesPrimaryHeldAction_HeldItemInterface_AppliesPrimaryAction()
        {
            var locomotionHandler = new LocomotionInputHandler(_movement, _camera, _heldItem);

            locomotionHandler.HandleButtonInput(EInputKey.PrimaryHeldAction, true);

            Assert.AreEqual(EHoldableAction.Primary, _heldItem.UseCurrentHoldableInput);
        }

        [Test]
        public void ReceivesPrimaryHeldAction_HeldItemInterface_False_DoesNotApplyPrimaryAction()
        {
            var locomotionHandler = new LocomotionInputHandler(_movement, _camera, _heldItem);

            locomotionHandler.HandleButtonInput(EInputKey.PrimaryHeldAction, false);

            Assert.IsNull(_heldItem.UseCurrentHoldableInput);
        }

        [Test]
        public void ReceivesPrimaryHeldAction_HeldItemInterface_False_ReturnsHandled()
        {
            var locomotionHandler = new LocomotionInputHandler(_movement, _camera, _heldItem);

            Assert.AreEqual(EInputHandlerResult.Handled, locomotionHandler.HandleButtonInput(EInputKey.PrimaryHeldAction, false));
        }

        [Test]
        public void ReceivesPrimaryHeldAction_HeldItemInterface_ReturnsHandled()
        {
            var locomotionHandler = new LocomotionInputHandler(_movement, _camera, _heldItem);

            Assert.AreEqual(EInputHandlerResult.Handled, locomotionHandler.HandleButtonInput(EInputKey.PrimaryHeldAction, true));
        }

        [Test]
        public void ReceivesPrimaryHeldAction_NoHeldItemInterface_DoesNotApplyPrimaryAction()
        {
            var locomotionHandler = new LocomotionInputHandler(_movement, _camera, null);

            locomotionHandler.HandleButtonInput(EInputKey.PrimaryHeldAction, false);

            Assert.IsNull(_heldItem.UseCurrentHoldableInput);
        }

        [Test]
        public void ReceivesPrimaryHeldAction_NoHeldItemInterface_ReturnsUnhandled()
        {
            var locomotionHandler = new LocomotionInputHandler(_movement, _camera, null);

            Assert.AreEqual(EInputHandlerResult.Unhandled, locomotionHandler.HandleButtonInput(EInputKey.PrimaryHeldAction, true));
        }

        [Test]
        public void ReceivesSecondaryHeldAction_HeldItemInterface_AppliesSecondaryAction()
        {
            var locomotionHandler = new LocomotionInputHandler(_movement, _camera, _heldItem);

            locomotionHandler.HandleButtonInput(EInputKey.SecondaryHeldAction, true);

            Assert.AreEqual(EHoldableAction.Secondary, _heldItem.UseCurrentHoldableInput);
        }

        [Test]
        public void ReceivesSecondaryHeldAction_HeldItemInterface_False_DoesNotApplySecondaryAction()
        {
            var locomotionHandler = new LocomotionInputHandler(_movement, _camera, _heldItem);

            locomotionHandler.HandleButtonInput(EInputKey.SecondaryHeldAction, false);

            Assert.IsNull(_heldItem.UseCurrentHoldableInput);
        }

        [Test]
        public void ReceivesSecondaryHeldAction_HeldItemInterface_False_ReturnsHandled()
        {
            var locomotionHandler = new LocomotionInputHandler(_movement, _camera, _heldItem);

            Assert.AreEqual(EInputHandlerResult.Handled, locomotionHandler.HandleButtonInput(EInputKey.SecondaryHeldAction, false));
        }

        [Test]
        public void ReceivesSecondaryHeldAction_HeldItemInterface_ReturnsHandled()
        {
            var locomotionHandler = new LocomotionInputHandler(_movement, _camera, _heldItem);

            Assert.AreEqual(EInputHandlerResult.Handled, locomotionHandler.HandleButtonInput(EInputKey.SecondaryHeldAction, true));
        }

        [Test]
        public void ReceivesSecondaryHeldAction_NoHeldItemInterface_DoesNotApplySecondaryAction()
        {
            var locomotionHandler = new LocomotionInputHandler(_movement, _camera, null);

            locomotionHandler.HandleButtonInput(EInputKey.SecondaryHeldAction, false);

            Assert.IsNull(_heldItem.UseCurrentHoldableInput);
        }

        [Test]
        public void ReceivesSecondaryHeldAction_NoHeldItemInterface_ReturnsUnhandled()
        {
            var locomotionHandler = new LocomotionInputHandler(_movement, _camera, null);

            Assert.AreEqual(EInputHandlerResult.Unhandled, locomotionHandler.HandleButtonInput(EInputKey.SecondaryHeldAction, true));
        }

        [Test]
        public void ReceivesDropHeldItem_HeldItemInterface_DropsItem()
        {
            var locomotionHandler = new LocomotionInputHandler(_movement, _camera, _heldItem);

            locomotionHandler.HandleButtonInput(EInputKey.DropHeldItem, true);

            Assert.IsTrue(_heldItem.DropHoldableCalled);
        }

        [Test]
        public void ReceivesDropHeldItem_HeldItemInterface_False_DoesNotDropItem()
        {
            var locomotionHandler = new LocomotionInputHandler(_movement, _camera, _heldItem);

            locomotionHandler.HandleButtonInput(EInputKey.DropHeldItem, false);

            Assert.IsFalse(_heldItem.DropHoldableCalled);
        }

        [Test]
        public void ReceivesDropHeldItem_HeldItemInterface_False_ReturnsHandled()
        {
            var locomotionHandler = new LocomotionInputHandler(_movement, _camera, _heldItem);

            Assert.AreEqual(EInputHandlerResult.Handled, locomotionHandler.HandleButtonInput(EInputKey.DropHeldItem, false));
        }

        [Test]
        public void ReceivesDropHeldItem_HeldItemInterface_ReturnsHandled()
        {
            var locomotionHandler = new LocomotionInputHandler(_movement, _camera, _heldItem);

            Assert.AreEqual(EInputHandlerResult.Handled, locomotionHandler.HandleButtonInput(EInputKey.DropHeldItem, true));
        }

        [Test]
        public void ReceivesDropHeldItem_NoHeldItemInterface_DoesNotDropItem()
        {
            var locomotionHandler = new LocomotionInputHandler(_movement, _camera, null);

            locomotionHandler.HandleButtonInput(EInputKey.DropHeldItem, false);

            Assert.IsFalse(_heldItem.DropHoldableCalled);
        }

        [Test]
        public void ReceivesDropHeldItem_NoHeldItemInterface_ReturnsUnhandled()
        {
            var locomotionHandler = new LocomotionInputHandler(_movement, _camera, null);

            Assert.AreEqual(EInputHandlerResult.Unhandled, locomotionHandler.HandleButtonInput(EInputKey.DropHeldItem, true));
        }
        #endregion
    }
}

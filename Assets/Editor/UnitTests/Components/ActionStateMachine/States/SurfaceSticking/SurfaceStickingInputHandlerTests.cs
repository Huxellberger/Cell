// Copyright (C) Threetee Gang All Rights Reserved

using Assets.Scripts.Components.ActionStateMachine.States.SurfaceSticking;
using Assets.Scripts.Input;
using Assets.Scripts.Test.Components.Movement;
using NUnit.Framework;
using UnityEngine;

namespace Assets.Editor.UnitTests.Components.ActionStateMachine.States.SurfaceSticking
{
    [TestFixture]
    public class SurfaceStickingInputHandlerTestFixture
    {

        private MockMovementComponent _movement;

        [SetUp]
        public void BeforeTest()
        {
            _movement = new GameObject().AddComponent<MockMovementComponent>();
        }

        [TearDown]
        public void AfterTest()
        {
            _movement = null;
        }

        [Test]
        public void ReceivesHorizontalAnalog_MovementInterface_AppliesSidewaysMotion()
        {
            var handler = new SurfaceStickingInputHandler(_movement);

            const float expectedAppliedInput = 0.5f;

            handler.HandleAnalogInput(EInputKey.HorizontalAnalog, expectedAppliedInput);

            Assert.AreEqual(expectedAppliedInput, _movement.ApplySidewaysMotionResult);
        }

        [Test]
        public void ReceivesHorizontalAnalog_MovementInterface_ReturnsHandled()
        {
            var handler = new SurfaceStickingInputHandler(_movement);

            Assert.AreEqual(EInputHandlerResult.Handled, handler.HandleAnalogInput(EInputKey.HorizontalAnalog, 1.0f));
        }

        [Test]
        public void ReceivesHorizontalAnalog_NoMovementInterface_DoesNotApplySidewaysMotion()
        {
            var handler = new SurfaceStickingInputHandler(null);

            const float expectedAppliedInput = 0.5f;

            handler.HandleAnalogInput(EInputKey.HorizontalAnalog, expectedAppliedInput);

            Assert.AreNotEqual(expectedAppliedInput, _movement.ApplySidewaysMotionResult);
        }

        [Test]
        public void ReceivesHorizontalAnalog_NoMovementInterface_ReturnsUnhandled()
        {
            var handler = new SurfaceStickingInputHandler(null);

            Assert.AreEqual(EInputHandlerResult.Unhandled, handler.HandleAnalogInput(EInputKey.HorizontalAnalog, 1.0f));
        }

        [Test]
        public void ReceivesVerticalAnalog_MovementInterface_AppliesForwardMotion()
        {
            var handler = new SurfaceStickingInputHandler(_movement);

            const float expectedAppliedInput = 0.5f;

            handler.HandleAnalogInput(EInputKey.VerticalAnalog, expectedAppliedInput);

            Assert.AreEqual(expectedAppliedInput, _movement.ApplyForwardMotionResult);
        }

        [Test]
        public void ReceivesVerticalAnalog_MovementInterface_ReturnsHandled()
        {
            var handler = new SurfaceStickingInputHandler(_movement);

            Assert.AreEqual(EInputHandlerResult.Handled, handler.HandleAnalogInput(EInputKey.VerticalAnalog, 1.0f));
        }

        [Test]
        public void ReceivesVerticalAnalog_NoMovementInterface_DoesNotApplyForwardMotion()
        {
            var handler = new SurfaceStickingInputHandler(null);

            const float expectedAppliedInput = 0.5f;

            handler.HandleAnalogInput(EInputKey.VerticalAnalog, expectedAppliedInput);

            Assert.AreNotEqual(expectedAppliedInput, _movement.ApplyForwardMotionResult);
        }

        [Test]
        public void ReceivesVerticalAnalog_NoMovementInterface_ReturnsUnhandled()
        {
            var handler = new SurfaceStickingInputHandler(null);

            Assert.AreEqual(EInputHandlerResult.Unhandled, handler.HandleAnalogInput(EInputKey.VerticalAnalog, 1.0f));
        }
    }
}

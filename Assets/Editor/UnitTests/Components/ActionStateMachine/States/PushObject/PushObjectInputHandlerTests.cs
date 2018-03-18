// Copyright (C) Threetee Gang All Rights Reserved

using Assets.Editor.UnitTests.Helpers;
using Assets.Scripts.Components.ActionStateMachine.States.PushObjectActionState;
using Assets.Scripts.Input;
using Assets.Scripts.Test.Components.Objects.Pushable;
using NUnit.Framework;
using UnityEngine;

namespace Assets.Editor.UnitTests.Components.ActionStateMachine.States.PushObject
{
    [TestFixture]
    public class PushObjectInputHandlerTestFixture
    {
        private MockPushableObjectComponent _pushable;
        private GameObject _owner;

        [SetUp]
        public void BeforeTest()
        {
            _pushable = new GameObject().AddComponent<MockPushableObjectComponent>();
            _owner = new GameObject();

            // Apply non-zero rotation to validate following tests function as expected
            _owner.transform.Rotate(new Vector3(12.0f, 13.0f, 15.0f));
        }

        [TearDown]
        public void AfterTest()
        {
            _owner = null;
            _pushable = null;
        }

        [Test]
        public void HandleVerticalAnalog_NoPushable_Unhandled()
        {
            var inputHandler = new PushObjectInputHandler(_owner, null);

            Assert.AreEqual(EInputHandlerResult.Unhandled, inputHandler.HandleAnalogInput(EInputKey.VerticalAnalog, 1.0f));
        }

        [Test]
        public void HandleVerticalAnalog_NoOwner_Unhandled()
        {
            var inputHandler = new PushObjectInputHandler(null, _pushable);

            Assert.AreEqual(EInputHandlerResult.Unhandled, inputHandler.HandleAnalogInput(EInputKey.VerticalAnalog, 1.0f));
        }

        [Test]
        public void HandleVerticalAnalog_OwnerAndPushable_Handled()
        {
            var inputHandler = new PushObjectInputHandler(_owner, _pushable);

            Assert.AreEqual(EInputHandlerResult.Handled, inputHandler.HandleAnalogInput(EInputKey.VerticalAnalog, 1.0f));
        }

        [Test]
        public void HandleVerticalAnalog_OwnerAndPushable_PushInUpVectorByModifier()
        {
            var inputHandler = new PushObjectInputHandler(_owner, _pushable);
            const float inputModifier = 0.7f;

            inputHandler.HandleAnalogInput(EInputKey.VerticalAnalog, inputModifier);

            ExtendedAssertions.AssertVectorsNearlyEqual(_owner.transform.up * inputModifier, _pushable.PushResult.Value);
        }

        [Test]
        public void HandleHorizontalAnalog_NoPushable_Unhandled()
        {
            var inputHandler = new PushObjectInputHandler(_owner, null);

            Assert.AreEqual(EInputHandlerResult.Unhandled, inputHandler.HandleAnalogInput(EInputKey.HorizontalAnalog, 1.0f));
        }

        [Test]
        public void HandleHorizontalAnalog_NoOwner_Unhandled()
        {
            var inputHandler = new PushObjectInputHandler(null, _pushable);

            Assert.AreEqual(EInputHandlerResult.Unhandled, inputHandler.HandleAnalogInput(EInputKey.HorizontalAnalog, 1.0f));
        }

        [Test]
        public void HandleHorizontalAnalog_OwnerAndPushable_Handled()
        {
            var inputHandler = new PushObjectInputHandler(_owner, _pushable);

            Assert.AreEqual(EInputHandlerResult.Handled, inputHandler.HandleAnalogInput(EInputKey.HorizontalAnalog, 1.0f));
        }

        [Test]
        public void HandleHorizontalAnalog_OwnerAndPushable_PushInRightVectorByModifier()
        {
            var inputHandler = new PushObjectInputHandler(_owner, _pushable);
            const float inputModifier = 0.7f;

            inputHandler.HandleAnalogInput(EInputKey.HorizontalAnalog, inputModifier);

            ExtendedAssertions.AssertVectorsNearlyEqual(_owner.transform.right * inputModifier, _pushable.PushResult.Value);
        }
    }
}

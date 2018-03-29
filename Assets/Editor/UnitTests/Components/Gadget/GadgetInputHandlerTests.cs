// Copyright (C) Threetee Gang All Rights Reserved

using Assets.Scripts.Components.Gadget;
using Assets.Scripts.Input;
using Assets.Scripts.Test.Components.Gadget;
using NUnit.Framework;
using UnityEngine;

namespace Assets.Editor.UnitTests.Components.Gadget
{
    [TestFixture]
    public class GadgetInputHandlerTestFixture
    {
        private MockGadgetSetComponent _gadgetSet;

        [SetUp]
        public void BeforeTest()
        {
            _gadgetSet = new GameObject().AddComponent<MockGadgetSetComponent>();
        }

        [TearDown]
        public void AfterTest()
        {
            _gadgetSet = null;
        }

        [Test]
        public void ReceivesUseActiveGadgetButton_GadgetSet_UsesActiveGadget()
        {
            var gadgetHandler = new GadgetInputHandler(_gadgetSet);

            gadgetHandler.HandleButtonInput(EInputKey.UseActiveGadget, true);

            Assert.IsTrue(_gadgetSet.UseActiveGadgetCalled);
        }

        [Test]
        public void ReceivesUseActiveGadgetButton_GadgetSet_DoesNotUseActiveGadget()
        {
            var gadgetHandler = new GadgetInputHandler(_gadgetSet);

            gadgetHandler.HandleButtonInput(EInputKey.UseActiveGadget, false);

            Assert.IsFalse(_gadgetSet.UseActiveGadgetCalled);
        }

        [Test]
        public void ReceivesUseActiveGadgetButton_GadgetSet_False_ReturnsHandled()
        {
            var gadgetHandler = new GadgetInputHandler(_gadgetSet);

            Assert.AreEqual(EInputHandlerResult.Handled, gadgetHandler.HandleButtonInput(EInputKey.UseActiveGadget, false));
        }

        [Test]
        public void ReceivesUseActiveGadgetButton_GadgetSet_ReturnsHandled()
        {
            var gadgetHandler = new GadgetInputHandler(_gadgetSet);

            Assert.AreEqual(EInputHandlerResult.Handled, gadgetHandler.HandleButtonInput(EInputKey.UseActiveGadget, true));
        }

        [Test]
        public void ReceivesUseActiveGadgetButton_NoGadgetSet_DoesNotUseUseActiveGadget()
        {
            var gadgetHandler = new GadgetInputHandler(null);

            gadgetHandler.HandleButtonInput(EInputKey.UseActiveGadget, false);

            Assert.IsFalse(_gadgetSet.UseActiveGadgetCalled);
        }

        [Test]
        public void ReceivesUseActiveGadgetButton_NoGadgetSet_ReturnsUnhandled()
        {
            var gadgetHandler = new GadgetInputHandler(null);

            Assert.AreEqual(EInputHandlerResult.Unhandled, gadgetHandler.HandleButtonInput(EInputKey.UseActiveGadget, true));
        }

        [Test]
        public void ReceivesCycleGadgetPositiveButton_GadgetSet_CyclesBy1()
        {
            var gadgetHandler = new GadgetInputHandler(_gadgetSet);

            gadgetHandler.HandleButtonInput(EInputKey.CycleGadgetPositive, true);

            Assert.AreEqual(1, _gadgetSet.CycleActiveGadgetResult);
        }

        [Test]
        public void ReceivesCycleGadgetPositiveButton_GadgetSet_DoesNotCycleGadgetPositive()
        {
            var gadgetHandler = new GadgetInputHandler(_gadgetSet);

            gadgetHandler.HandleButtonInput(EInputKey.CycleGadgetPositive, false);

            Assert.IsNull(_gadgetSet.CycleActiveGadgetResult);
        }

        [Test]
        public void ReceivesCycleGadgetPositiveButton_GadgetSet_False_ReturnsHandled()
        {
            var gadgetHandler = new GadgetInputHandler(_gadgetSet);

            Assert.AreEqual(EInputHandlerResult.Handled, gadgetHandler.HandleButtonInput(EInputKey.CycleGadgetPositive, false));
        }

        [Test]
        public void ReceivesCycleGadgetPositiveButton_GadgetSet_ReturnsHandled()
        {
            var gadgetHandler = new GadgetInputHandler(_gadgetSet);

            Assert.AreEqual(EInputHandlerResult.Handled, gadgetHandler.HandleButtonInput(EInputKey.CycleGadgetPositive, true));
        }

        [Test]
        public void ReceivesCycleGadgetPositiveButton_NoGadgetSet_DoesNotUseCycleGadgetPositive()
        {
            var gadgetHandler = new GadgetInputHandler(null);

            gadgetHandler.HandleButtonInput(EInputKey.CycleGadgetPositive, false);

            Assert.IsNull(_gadgetSet.CycleActiveGadgetResult);
        }

        [Test]
        public void ReceivesCycleGadgetPositiveButton_NoGadgetSet_ReturnsUnhandled()
        {
            var gadgetHandler = new GadgetInputHandler(null);

            Assert.AreEqual(EInputHandlerResult.Unhandled, gadgetHandler.HandleButtonInput(EInputKey.CycleGadgetPositive, true));
        }

        [Test]
        public void ReceivesCycleGadgetNegativeButton_GadgetSet_CyclesByNegative1()
        {
            var gadgetHandler = new GadgetInputHandler(_gadgetSet);

            gadgetHandler.HandleButtonInput(EInputKey.CycleGadgetNegative, true);

            Assert.AreEqual(-1, _gadgetSet.CycleActiveGadgetResult);
        }

        [Test]
        public void ReceivesCycleGadgetNegativeButton_GadgetSet_DoesNotCycleGadgetNegative()
        {
            var gadgetHandler = new GadgetInputHandler(_gadgetSet);

            gadgetHandler.HandleButtonInput(EInputKey.CycleGadgetNegative, false);

            Assert.IsNull(_gadgetSet.CycleActiveGadgetResult);
        }

        [Test]
        public void ReceivesCycleGadgetNegativeButton_GadgetSet_False_ReturnsHandled()
        {
            var gadgetHandler = new GadgetInputHandler(_gadgetSet);

            Assert.AreEqual(EInputHandlerResult.Handled, gadgetHandler.HandleButtonInput(EInputKey.CycleGadgetNegative, false));
        }

        [Test]
        public void ReceivesCycleGadgetNegativeButton_GadgetSet_ReturnsHandled()
        {
            var gadgetHandler = new GadgetInputHandler(_gadgetSet);

            Assert.AreEqual(EInputHandlerResult.Handled, gadgetHandler.HandleButtonInput(EInputKey.CycleGadgetNegative, true));
        }

        [Test]
        public void ReceivesCycleGadgetNegativeButton_NoGadgetSet_DoesNotUseCycleGadgetNegative()
        {
            var gadgetHandler = new GadgetInputHandler(null);

            gadgetHandler.HandleButtonInput(EInputKey.CycleGadgetNegative, false);

            Assert.IsNull(_gadgetSet.CycleActiveGadgetResult);
        }

        [Test]
        public void ReceivesCycleGadgetNegativeButton_NoGadgetSet_ReturnsUnhandled()
        {
            var gadgetHandler = new GadgetInputHandler(null);

            Assert.AreEqual(EInputHandlerResult.Unhandled, gadgetHandler.HandleButtonInput(EInputKey.CycleGadgetNegative, true));
        }
    }
}

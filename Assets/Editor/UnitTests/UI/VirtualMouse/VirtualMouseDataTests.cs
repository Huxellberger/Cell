// Copyright (C) Threetee Gang All Rights Reserved 

using Assets.Scripts.UI.VirtualMouse;
using NUnit.Framework;
using UnityEngine.EventSystems;

namespace Assets.Editor.UnitTests.UI.VirtualMouse
{
    [TestFixture]
    public class VirtualMouseDataTestFixture
    {
        [Test]
        public void Creation_EntriesForMouseButtons()
        {
            var data = new VirtualMouseData();

            Assert.AreEqual(3, data.ButtonEntries.Count);

            Assert.IsTrue(data.ButtonEntries.ContainsKey(PointerEventData.InputButton.Left));
            Assert.IsTrue(data.ButtonEntries.ContainsKey(PointerEventData.InputButton.Right));
            Assert.IsTrue(data.ButtonEntries.ContainsKey(PointerEventData.InputButton.Middle));
        }

        [Test]
        public void Reset_ButtonsAllReset()
        {
            var data = new VirtualMouseData();

            // Tweak some values
            foreach (var buttonEntry in data.ButtonEntries)
            {
                buttonEntry.Value.UpdateButton(false);
            }

            data.Reset();

            foreach (var buttonEntry in data.ButtonEntries)
            {
                Assert.IsFalse(buttonEntry.Value.Pressed);
                Assert.IsFalse(buttonEntry.Value.Released);
            }
        }

        [Test]
        public void Creation_PressedAndReleasedFalse()
        {
            var buttonEntry = new VirtualMouseButtonEntry();

            Assert.IsFalse(buttonEntry.Pressed);
            Assert.IsFalse(buttonEntry.Released);
        }

        [Test]
        public void Update_True_PressedTrue()
        {
            var buttonEntry = new VirtualMouseButtonEntry();

            buttonEntry.UpdateButton(true);

            Assert.IsTrue(buttonEntry.Pressed);
        }

        [Test]
        public void Update_False_ReleasedTrue()
        {
            var buttonEntry = new VirtualMouseButtonEntry();

            buttonEntry.UpdateButton(false);

            Assert.IsTrue(buttonEntry.Released);
        }

        [Test]
        public void StateForMouseButton_NotChanged_ReturnsNotChanged()
        {
            var buttonEntry = new VirtualMouseButtonEntry();

            Assert.AreEqual(PointerEventData.FramePressState.NotChanged, buttonEntry.StateForMouseButton());
        }

        [Test]
        public void StateForMouseButton_Pressed_ReturnsPressed()
        {
            var buttonEntry = new VirtualMouseButtonEntry();
            buttonEntry.UpdateButton(true);

            Assert.AreEqual(PointerEventData.FramePressState.Pressed, buttonEntry.StateForMouseButton());
        }

        [Test]
        public void StateForMouseButton_Released_ReturnsReleased()
        {
            var buttonEntry = new VirtualMouseButtonEntry();
            buttonEntry.UpdateButton(false);

            Assert.AreEqual(PointerEventData.FramePressState.Released, buttonEntry.StateForMouseButton());
        }

        [Test]
        public void StateForMouseButton_PressedAndReleased_ReturnsPressedAndReleased()
        {
            var buttonEntry = new VirtualMouseButtonEntry();
            buttonEntry.UpdateButton(true);
            buttonEntry.UpdateButton(false);

            Assert.AreEqual(PointerEventData.FramePressState.PressedAndReleased, buttonEntry.StateForMouseButton());
        }

        [Test]
        public void Reset_SetsPressedAndReleasedToFalse()
        {
            var buttonEntry = new VirtualMouseButtonEntry();
            buttonEntry.UpdateButton(true);
            buttonEntry.UpdateButton(false);

            buttonEntry.Reset();

            Assert.IsFalse(buttonEntry.Released);
            Assert.IsFalse(buttonEntry.Pressed);
        }
    }
}

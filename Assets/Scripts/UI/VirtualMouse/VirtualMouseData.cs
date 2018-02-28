// Copyright (C) Threetee Gang All Rights Reserved 

using System.Collections.Generic;
using UnityEngine.EventSystems;

namespace Assets.Scripts.UI.VirtualMouse
{
    public class VirtualMouseButtonEntry
    {
        public bool Pressed { get; private set; }
        public bool Released { get; private set; }

        public VirtualMouseButtonEntry()
        {
            Pressed = false;
            Released = false;
        }

        public void UpdateButton(bool status)
        {
            if (status)
            {
                Pressed = true;
            }
            else
            {
                Released = true;
            }
        }

        public void Reset()
        {
            Pressed = false;
            Released = false;
        }

        public PointerEventData.FramePressState StateForMouseButton()
        {
            if (Pressed && Released)
                return PointerEventData.FramePressState.PressedAndReleased;
            if (Pressed)
                return PointerEventData.FramePressState.Pressed;
            if (Released)
                return PointerEventData.FramePressState.Released;
            return PointerEventData.FramePressState.NotChanged;
        }
    }

    public class VirtualMouseData
    {
        public readonly IDictionary<PointerEventData.InputButton, VirtualMouseButtonEntry> ButtonEntries;

        public VirtualMouseData()
        {
            ButtonEntries = new Dictionary<PointerEventData.InputButton, VirtualMouseButtonEntry>
            {
                {PointerEventData.InputButton.Left, new VirtualMouseButtonEntry()},
                {PointerEventData.InputButton.Right, new VirtualMouseButtonEntry()},
                {PointerEventData.InputButton.Middle, new VirtualMouseButtonEntry()}
            };
        }

        public void Reset()
        {
            foreach (var buttonEntry in ButtonEntries)
            {
                buttonEntry.Value.Reset();
            }
        }
    }
}

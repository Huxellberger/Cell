// Copyright (C) Threetee Gang All Rights Reserved

#if UNITY_EDITOR

using Assets.Scripts.Components.Gadget;
using UnityEngine;

namespace Assets.Scripts.Test.Components.Gadget
{
    public class MockGadgetComponent 
        : MonoBehaviour 
        , IGadgetInterface
    {
        public GameObject UseGadgetGameObject { get; private set; }
        public EGadgetSlot GetGadgetSlotResult = EGadgetSlot.Mine;
        public Sprite GetGadgetIconReturnSprite { get; set; }

        public void UseGadget(GameObject user)
        {
            UseGadgetGameObject = user;
        }

        public EGadgetSlot GetGadgetSlot()
        {
            return GetGadgetSlotResult;
        }

        public Sprite GetGadgetIcon()
        {
            return GetGadgetIconReturnSprite;
        }
    }
}

#endif // UNITY_EDITOR

// Copyright (C) Threetee Gang All Rights Reserved

using Assets.Scripts.Components.Gadget;
using Assets.Scripts.Messaging;
using UnityEngine.UI;

namespace Assets.Scripts.UI.HUD
{
    public class GadgetHUDComponent 
        : UIComponent
    {
        public Image GadgetImage;
        public Text GadgetCount;

        private UnityMessageEventHandle<GadgetUpdatedUIMessage> _gadgetUpdatedHandle;

        protected override void OnStart()
        {
            GadgetCount.text = "0";
            _gadgetUpdatedHandle = Dispatcher.RegisterForMessageEvent<GadgetUpdatedUIMessage>(OnGadgetUpdated);
        }

        protected override void OnEnd()
        {
            Dispatcher.UnregisterForMessageEvent(_gadgetUpdatedHandle);
        }

        private void OnGadgetUpdated(GadgetUpdatedUIMessage inMessage)
        {
            GadgetImage.sprite = inMessage.GadgetGraphic;
            GadgetCount.text = inMessage.SlotCount.ToString();
        }
    }
}

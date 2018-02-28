// Copyright (C) Threetee Gang All Rights Reserved

using Assets.Scripts.Components.Stamina;
using Assets.Scripts.Messaging;
using UnityEngine.UI;

namespace Assets.Scripts.UI.HUD
{
    public class StaminaHUDComponent 
        : UIComponent
    {
        private Slider SliderComponent { get; set; }

        private UnityMessageEventHandle<StaminaChangedUIMessage> StaminaChangedMessageHandler { get; set; }
        private UnityMessageEventHandle<MaxStaminaChangedUIMessage> MaxStaminaChangedMessageHandler { get; set; }

        protected override void OnStart()
        {
            SliderComponent = gameObject.GetComponentInChildren<Slider>();

            ResetSlider();

            StaminaChangedMessageHandler = Dispatcher.RegisterForMessageEvent<StaminaChangedUIMessage>(OnStaminaChanged);
            MaxStaminaChangedMessageHandler = Dispatcher.RegisterForMessageEvent<MaxStaminaChangedUIMessage>(OnMaxStaminaChanged);
        }

        private void ResetSlider()
        {
            SliderComponent.wholeNumbers = true;
            SliderComponent.minValue = 0;
            SliderComponent.value = 0;
        }

        protected override void OnEnd()
        {
            SliderComponent = null;

            Dispatcher.UnregisterForMessageEvent(MaxStaminaChangedMessageHandler);
            Dispatcher.UnregisterForMessageEvent(StaminaChangedMessageHandler);
        }

        private void OnStaminaChanged(StaminaChangedUIMessage inMessage)
        {
            SliderComponent.value = inMessage.NewStamina;
        }

        private void OnMaxStaminaChanged(MaxStaminaChangedUIMessage inMessage)
        {
            SliderComponent.maxValue = inMessage.NewMaxStamina;
        }
    }
}

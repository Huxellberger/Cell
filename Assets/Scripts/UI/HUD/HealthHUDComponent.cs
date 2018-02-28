// Copyright (C) Threetee Gang All Rights Reserved

using Assets.Scripts.Components.Health;
using Assets.Scripts.Messaging;
using UnityEngine.UI;

namespace Assets.Scripts.UI.HUD
{
    public class HealthHUDComponent
        : UIComponent
    {
        private Slider HealthSlider { get; set; }

        private UnityMessageEventHandle<HealthChangedUIMessage> HealthChangedMessageHandler { get; set; }
        private UnityMessageEventHandle<MaxHealthChangedUIMessage> MaxHealthChangedMessageHandler { get; set; }

        protected override void OnStart()
        {
            HealthSlider = gameObject.GetComponentInChildren<Slider>();

            ResetSlider();

            HealthChangedMessageHandler = Dispatcher.RegisterForMessageEvent<HealthChangedUIMessage>(OnHealthChanged);
            MaxHealthChangedMessageHandler = Dispatcher.RegisterForMessageEvent<MaxHealthChangedUIMessage>(OnMaxHealthChanged);
        }

        private void ResetSlider()
        {
            HealthSlider.wholeNumbers = true;
            HealthSlider.minValue = 0;
            HealthSlider.value = 0;
        }

        protected override void OnEnd()
        {
            HealthSlider = null;

            Dispatcher.UnregisterForMessageEvent(MaxHealthChangedMessageHandler);
            Dispatcher.UnregisterForMessageEvent(HealthChangedMessageHandler);
        }

        private void OnHealthChanged(HealthChangedUIMessage inMessage)
        {
            HealthSlider.value = inMessage.NewHealth;
        }

        private void OnMaxHealthChanged(MaxHealthChangedUIMessage inMessage)
        {
            HealthSlider.maxValue = inMessage.MaxHealth;
        }
    }
}

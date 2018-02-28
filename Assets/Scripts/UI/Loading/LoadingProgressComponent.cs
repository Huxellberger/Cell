// Copyright (C) Threetee Gang All Rights Reserved

using Assets.Scripts.Instance.Loading;
using Assets.Scripts.Messaging;
using UnityEngine.UI;

namespace Assets.Scripts.UI.Loading
{
    public class LoadingProgressComponent
        : UIComponent
    {
        private Slider LoadingProgressSlider { get; set; }

        private UnityMessageEventHandle<LoadingProgressUpdatedUIMessage> LoadingProgressUpdatedMessageHandler { get; set; }

        protected override void OnStart()
        {
            LoadingProgressSlider = gameObject.GetComponentInChildren<Slider>();

            ResetSlider();

            LoadingProgressUpdatedMessageHandler = Dispatcher.RegisterForMessageEvent<LoadingProgressUpdatedUIMessage>(OnLoadProgressChanged);
        }

        private void ResetSlider()
        {
            LoadingProgressSlider.wholeNumbers = false;
            LoadingProgressSlider.minValue = 0f;
            LoadingProgressSlider.maxValue = 1f;
            LoadingProgressSlider.value = 0f;
        }

        protected override void OnEnd()
        {
            LoadingProgressSlider = null;

            Dispatcher.UnregisterForMessageEvent(LoadingProgressUpdatedMessageHandler);
        }

        private void OnLoadProgressChanged(LoadingProgressUpdatedUIMessage inMessage)
        {
            LoadingProgressSlider.value = inMessage.Progress;
        }
    }
}

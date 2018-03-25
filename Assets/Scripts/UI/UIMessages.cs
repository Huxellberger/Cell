// Copyright (C) Threetee Gang All Rights Reserved

using Assets.Scripts.Messaging;
using UnityEngine;

namespace Assets.Scripts.UI
{
    public class UpdateUIEnabledMessage 
        : UnityMessagePayload
    {
        public readonly bool IsEnabled;

        public UpdateUIEnabledMessage(bool inIsEnabled)
        {
            IsEnabled = inIsEnabled;
        }
    }

    public class DisplayToastUIMessage
        : UnityMessagePayload
    {
        public readonly string ToastText;
        public readonly AudioClip ToastAudio;

        public DisplayToastUIMessage(string inToastText, AudioClip inToastAudio)
        {
            ToastText = inToastText;
            ToastAudio = inToastAudio;
        }
    }
}

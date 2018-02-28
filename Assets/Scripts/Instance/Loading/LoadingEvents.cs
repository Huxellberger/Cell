// Copyright (C) Threetee Gang All Rights Reserved

using Assets.Scripts.Messaging;

namespace Assets.Scripts.Instance.Loading
{
    public class LoadingProgressUpdatedUIMessage 
        : UnityMessagePayload
    {
        public readonly float Progress;

        public LoadingProgressUpdatedUIMessage(float inProgress)
        {
            Progress = inProgress;
        }
    }
}

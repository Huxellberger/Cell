// Copyright (C) Threetee Gang All Rights Reserved

namespace Assets.Scripts.Components.Emote
{
    public interface IEmoteInterface
    {
        void SetEmoteState(EEmoteState inState);
        EEmoteState GetEmoteState();
    }
}

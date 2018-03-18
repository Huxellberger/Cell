// Copyright (C) Threetee Gang All Rights Reserved

using UnityEngine;

namespace Assets.Scripts.UI.HUD.Conversation
{
    public class DialogueLine
    {
        public readonly string DisplayText;
        public readonly string DisplayName;
        public readonly float TextSpeed;
        public readonly Sprite Portrait;
        public readonly AudioClip TalkNoise;

        public DialogueLine()
            : this(null, null, -1.0f, null, null)
        {
        }

        public DialogueLine(string inText, string inName, float inTextSpeed, Sprite inPortrait, AudioClip inTalkNoise)
        {
            DisplayText = inText;
            DisplayName = inName;
            TextSpeed = inTextSpeed;
            Portrait = inPortrait;
            TalkNoise = inTalkNoise;
        }
    }
}

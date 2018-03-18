// Copyright (C) Threetee Gang All Rights Reserved

using System;
using System.Collections.Generic;
using Assets.Scripts.Localisation;
using Assets.Scripts.UI.HUD.Conversation;
using UnityEngine;

namespace Assets.Scripts.AI.Chatter
{
    [Serializable]
    public class DialogueLineEntry
    {
        public LocalisationKey DialogueKey;
        public LocalisationKey NameKey;
        public float DialogueSpeed;
        public Sprite Portrait;
        public AudioClip TalkNoise;
    }

    [Serializable]
    public class DialogueEntry
    {
        public string DialogueEntryKey;
        public List<DialogueLineEntry> Lines;
        public int Priority;
        public DialogueRequestCompleteDelegate CompleteDelegate;
    }
}

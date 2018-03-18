// Copyright (C) Threetee Gang All Rights Reserved

using System;
using System.Collections.Generic;
using Assets.Scripts.Localisation;
using Assets.Scripts.Services.EventsOfInterest;
using UnityEngine;

namespace Assets.Scripts.AI.Chatter
{
    [Serializable]
    public class DialogueLineEntry
    {
        [Header("Localisation Key For Dialogue")]
        public LocalisationKey DialogueKey;
        [Header("Localisation Key For Character Name")]
        public LocalisationKey NameKey;
        [Header("Other Properties")]
        public float DialogueSpeed;
        public Sprite Portrait;
        public AudioClip TalkNoise;
    }

    [Serializable]
    public class DialogueEntry
    {
        [Tooltip("Key should match point of interest event!")]
        public string DialogueEntryKey;
        public List<DialogueLineEntry> Lines;
        [Tooltip("Higher priority = More likely to play")]
        public int Priority;
        [Tooltip("Decides whether the dialogue can have repeated plays")]
        public EEventOfInterestType ChatterType;
    }
}

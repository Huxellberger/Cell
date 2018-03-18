// Copyright (C) Threetee Gang All Rights Reserved

using System.Collections.Generic;
using Assets.Scripts.Messaging;

namespace Assets.Scripts.UI.HUD.Conversation
{
    public delegate void DialogueRequestCompleteDelegate();

    public class RequestDialogueUIMessage 
        : UnityMessagePayload
    {
        public readonly List<DialogueLine> Lines;
        public readonly int Priority;
        public readonly DialogueRequestCompleteDelegate DialogueCompleteDelegate;

        public RequestDialogueUIMessage(List<DialogueLine> inLines, int inPriority, DialogueRequestCompleteDelegate inDelegate)
        {
            Lines = inLines;
            Priority = inPriority;
            DialogueCompleteDelegate = inDelegate;
        }
    }
}

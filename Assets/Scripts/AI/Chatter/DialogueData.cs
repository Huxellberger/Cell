// Copyright (C) Threetee Gang All Rights Reserved

using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.AI.Chatter
{
    [CreateAssetMenu(fileName = "DialogueData", menuName = "Cell/AI/Dialogue", order = 1)]
    public class DialogueData 
        : ScriptableObject
    {
        public List<DialogueEntry> DialogueEntries;

        public static Dictionary<string, DialogueEntry> GenerateDialogueMappings(DialogueData inData)
        {
            var mappings = new Dictionary<string, DialogueEntry>();

            foreach (var dialogueEntry in inData.DialogueEntries)
            {
                mappings.Add(dialogueEntry.DialogueEntryKey, dialogueEntry);
            }
            return mappings;
        }
    }
}

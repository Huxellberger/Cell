// Copyright (C) Threetee Gang All Rights Reserved

using System.Collections.Generic;
using Assets.Scripts.AI.Chatter;
using NUnit.Framework;
using UnityEngine;

namespace Assets.Editor.UnitTests.AI.Chatter
{
    [TestFixture]
    public class DialogueDataTestFixture 
    {
        [Test]
        public void GenerateDialogueMappings_ConvertsAsExpected()
        {
            var entry1 = new DialogueEntry {DialogueEntryKey = "Key"};
            var entry2 = new DialogueEntry { DialogueEntryKey = "OtherKey" };

            var data = ScriptableObject.CreateInstance<DialogueData>();
            data.DialogueEntries = new List<DialogueEntry>{entry1, entry2};

            var mappings = DialogueData.GenerateDialogueMappings(data);

            Assert.AreSame(entry1, mappings[entry1.DialogueEntryKey]);
            Assert.AreSame(entry2, mappings[entry2.DialogueEntryKey]);
        }
    }
}

// Copyright (C) Threetee Gang All Rights Reserved

using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.AI.Chatter;
using Assets.Scripts.Components.Trigger;
using Assets.Scripts.Core;
using Assets.Scripts.Services.EventsOfInterest;
using UnityEditor;
using UnityEngine;

namespace Assets.Editor.CustomEditors.Services.EventsOfInterest
{
    public class EventsOfInterestCollectorEditor 
        : EditorWindow
    {
        private readonly List<string> _foundKeys = new List<string>();
        private readonly List<string> _expectedKeys = new List<string>();

        [MenuItem("Window/Events Of Interest Keys")]
        public static void ShowWindow()
        {
            GetWindow(typeof(EventsOfInterestCollectorEditor));
        }

        private void OnGUI()
        {
            GUILayout.Label("Current Keys In Scene", EditorStyles.boldLabel);

            if (_foundKeys != null)
            {
                foreach (var foundKey in _foundKeys)
                {
                    GUILayout.Label(foundKey, EditorStyles.label);
                }
            }

            GUILayout.Label("Expected Keys In Scene", EditorStyles.boldLabel);

            if (_expectedKeys != null)
            {
                foreach (var foundKey in _expectedKeys)
                {
                    GUILayout.Label(foundKey, EditorStyles.label);
                }
            }

            GUILayout.Label("Operations", EditorStyles.boldLabel);
            if (GUILayout.Button("Refresh Key Listing"))
            {
                _foundKeys.Clear();
                _expectedKeys.Clear();
                AddKeysForType<EventOfInterestTriggerResponseComponent>();
                AddKeysForType<PointOfInterestComponent>();

                AddExpectedKeysForType<ChatterComponent>();
            }
        }

        private void AddKeysForType<TComponentType>()
            where TComponentType : MonoBehaviour
        {
            var components = FindObjectsOfType<TComponentType>();

            foreach (var component in components)
            {
                var newlyFoundKeys = ReflectionFunctions.GetAttributeValuesOnTarget<EventOfInterestKeyAttribute, string>
                (
                    component
                ).ToList();

                for (var i = 0 ; i < newlyFoundKeys.Count() ; i++)
                {
                    newlyFoundKeys[i] = "Key: " + newlyFoundKeys[i] + "\tObject Name: " + component.gameObject.name;
                }

                _foundKeys.AddRange(newlyFoundKeys);
            }
        }

        private void AddExpectedKeysForType<TComponentType>()
            where TComponentType : MonoBehaviour
        {
            var components = FindObjectsOfType<TComponentType>();

            foreach (var component in components)
            {
                var newlyFoundKeys = ReflectionFunctions.GetAttributeValuesOnTarget<EventOfInterestRegistrationAttribute, DialogueData>
                (
                    component
                ).ToList();

                var newExpectedKeys = new List<string>();

                foreach (var newlyFoundKey in newlyFoundKeys)
                {
                    foreach (var dialogueEntry in newlyFoundKey.DialogueEntries)
                    {
                        newExpectedKeys.Add("Key: " + dialogueEntry.DialogueEntryKey + "\tObject Name: " + component.gameObject.name);
                    }
                }

                _expectedKeys.AddRange(newExpectedKeys);
            }
        }
    }
}

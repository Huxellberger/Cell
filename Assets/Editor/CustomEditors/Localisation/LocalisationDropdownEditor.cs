// Copyright (C) Threetee Gang All Rights Reserved

using Assets.Scripts.Localisation;
using UnityEditor;
using UnityEngine;

namespace Assets.Editor.CustomEditors.Localisation
{
    public static class LocalisationDropdownEditor
    {
        [MenuItem("GameObject/UI/Localisation Dropdown", false, 1)]
        static void CreateLocalisableDropdownInEditor(MenuCommand menuCommand)
        {
            var createdObject = new GameObject("LocalisationDropdown");

            // Update parenting
            GameObjectUtility.SetParentAndAlign(createdObject, menuCommand.context as GameObject);

            createdObject.AddComponent<LocalisationDropdown>();

            // Register for undo
            Undo.RegisterCreatedObjectUndo(createdObject, "Create " + createdObject.name);

            // Set as selected object
            Selection.activeObject = createdObject;
        }
    }
}

// Copyright (C) Threetee Gang All Rights Reserved

#if UNITY_EDITOR

using Assets.Scripts.Localisation;
using UnityEditor;
using UnityEngine;
using TextEditor = UnityEditor.UI.TextEditor;

namespace Assets.Editor.CustomEditors.Localisation
{
    [CustomPropertyDrawer(typeof(LocalisationKey))]
    class LocalisationKeyDrawer : PropertyDrawer
    {
        private const float PropertyHeight = 50.0f;
        private const float PropertyWidth = 300.0f;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            var labelHeight = GetPropertyHeight(property, label);

            // Draw label
            var namespacePosition = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), new GUIContent("Namespace"));
            var secondFieldPosition = new Rect(position.x, position.y + (labelHeight * 0.5f), PropertyWidth, position.height - (labelHeight * 0.5f));

            EditorGUI.PrefixLabel(secondFieldPosition, GUIUtility.GetControlID(FocusType.Passive),
                new GUIContent("Key"));

            // Don't make child fields be indented
            var indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;

            // Calculate rects
            var namespaceRect = new Rect(namespacePosition.x, position.y, PropertyWidth, position.height - (labelHeight * 0.5f));
            var keyRect = new Rect(namespacePosition.x, position.y + (labelHeight * 0.5f), PropertyWidth, position.height - (labelHeight * 0.5f));

            // add word wrap to style.
            var style = new GUIStyle(EditorStyles.textField);
            style.wordWrap = true;

            var namespaceProperty = property.FindPropertyRelative("LocalisationNamespace");
            var keyProperty = property.FindPropertyRelative("LocalisationKeyValue");

            // show the text area.
            EditorGUI.BeginChangeCheck();
            var namespaceinput = EditorGUI.TextArea(namespaceRect, namespaceProperty.stringValue, style);
            var keyInput = EditorGUI.TextArea(keyRect, keyProperty.stringValue, style);

            if (EditorGUI.EndChangeCheck())
            {
                namespaceProperty.stringValue = namespaceinput;
                keyProperty.stringValue = keyInput;
            }

            // Set indent back to what it was
            EditorGUI.indentLevel = indent;

            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return PropertyHeight;
        }
    }

    [CustomEditor(typeof(LocalisableUIText))]
    public class LocalisableUITextEditor 
        : TextEditor
    {
        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.PropertyField(serializedObject.FindProperty("LocalisedTextKey"), new GUIContent("LocalisationKey: Do not use text box!"));

            serializedObject.ApplyModifiedProperties();

            base.OnInspectorGUI();
        }

        [MenuItem("GameObject/UI/Localisable UI Text", false, 10)]
        static void CreateLocalisableTextElementInEditor(MenuCommand menuCommand)
        {
            var createdObject = new GameObject("LocalisableTextElement");
            
            // Update parenting
            GameObjectUtility.SetParentAndAlign(createdObject, menuCommand.context as GameObject);

            createdObject.AddComponent<LocalisableUIText>();

            // Register for undo
            Undo.RegisterCreatedObjectUndo(createdObject, "Create " + createdObject.name);

            // Set as selected object
            Selection.activeObject = createdObject;
        }
    }
}

#endif

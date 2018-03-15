// Copyright (C) Threetee Gang All Rights Reserved

using Assets.Scripts.AI.Pathfinding.Nav;
using UnityEditor;
using UnityEngine;

namespace Assets.Editor.CustomEditors.AI.Nav
{
    public class TilemapNavGenEditor 
        : EditorWindow
    {
        private string filePath = @"Assets\Data\AI\Nav\";
        private string fileName = "DefaultTilemapNav";

        [MenuItem("Window/Tilemap Nav Generation")]
        public static void ShowWindow()
        {
            GetWindow(typeof(TilemapNavGenEditor));
        }

        private void OnGUI()
        {
            GUILayout.Label("Nav Settings", EditorStyles.boldLabel);
            filePath = EditorGUILayout.TextField("File Path", filePath);
            fileName = EditorGUILayout.TextField("File Name", fileName);

            GUILayout.Label("Operations", EditorStyles.boldLabel);
            if (GUILayout.Button("Build Tilemap Nav"))
            {
                var returnedNodes = NavGenerationFunctions.GenerateNavDataForCurrentScene();

                var navData = CreateInstance<TilemapNavData>();
                navData.NodeData = returnedNodes;

                AssetDatabase.CreateAsset(navData, filePath + fileName + ".asset");
            }
        }
    }
}

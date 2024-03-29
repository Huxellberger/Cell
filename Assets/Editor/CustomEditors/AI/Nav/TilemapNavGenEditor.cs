﻿// Copyright (C) Threetee Gang All Rights Reserved

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
        private int tilesPerRegion = 64;

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
            tilesPerRegion = EditorGUILayout.IntField("Tiles Per Region", tilesPerRegion);

            GUILayout.Label("Operations", EditorStyles.boldLabel);
            if (GUILayout.Button("Build Tilemap Nav"))
            {
                var returnedNodes = NavGenerationFunctions.GenerateNavDataForCurrentScene();
                var returnedRegions = NavRegionGenerationFunctions.GenerateNavRegionsFromNodes(returnedNodes, tilesPerRegion);

                var navData = CreateInstance<TilemapNavData>();
                navData.NodeData = returnedNodes;
                navData.NavigationTable = new NavTable(returnedRegions);

                AssetDatabase.CreateAsset(navData, filePath + fileName + ".asset");
            }
        }
    }
}

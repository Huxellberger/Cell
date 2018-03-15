// Copyright (C) Threetee Gang All Rights Reserved

using Assets.Scripts.AI.Pathfinding.Nav;
using UnityEditor;

namespace Assets.Editor.CustomEditors.AI.Nav
{
    [CustomEditor(typeof(TilemapNavData))]
    public class TilemapNavDataEditor 
        : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            var data = (TilemapNavData)target;

            EditorGUILayout.LabelField("Node Count: ", data.NodeData.Count.ToString());
        }
    }
}

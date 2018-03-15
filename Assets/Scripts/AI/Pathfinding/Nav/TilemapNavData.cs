// Copyright (C) Threetee Gang All Rights Reserved

using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.AI.Pathfinding.Nav
{
    public class TilemapNavData 
        : ScriptableObject
    {
        [HideInInspector]
        public List<NavNode> NodeData;
    }
}

// Copyright (C) Threetee Gang All Rights Reserved

using UnityEngine;

namespace Assets.Scripts.AI.Pathfinding.Nav
{
    [System.Serializable]
    public class NavNode
    {
        public NavNode[] Neighbours;
        public Vector2 Position;
        public int Weight;
    }
}

// Copyright (C) Threetee Gang All Rights Reserved

using System;
using UnityEngine;

namespace Assets.Scripts.AI.Pathfinding.Nav
{
    [Serializable]
    public class NavNode
    {
        [NonSerialized]
        public NavNode[] NeighbourRefs;
        public int[] Neighbours;
        public Vector2 Position;
        public int Weight;
    }
}

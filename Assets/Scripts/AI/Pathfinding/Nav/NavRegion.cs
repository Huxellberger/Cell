// Copyright (C) Threetee Gang All Rights Reserved

using System;
using UnityEngine;

namespace Assets.Scripts.AI.Pathfinding.Nav
{
    [Serializable]
    public class NavRegion
    {
        public readonly NavNode[] Nodes;
        public Bounds RegionBounds { get; private set; }

        public NavRegion(NavNode[] inNodes)
        {
            Nodes = inNodes;
           InitializeBounds();
        }

        private void InitializeBounds()
        {
            if (Nodes.Length >= 2)
            {
                var minPoint = Nodes[0].Position;

                var maxPoint = Nodes[0].Position;

                for (var currentNodeIndex = 1; currentNodeIndex < Nodes.Length; currentNodeIndex++)
                {
                    if (IsSmallestPoint(Nodes[currentNodeIndex].Position, minPoint))
                    {
                        minPoint = Nodes[currentNodeIndex].Position;
                    }
                    else if (IsLargestPoint(Nodes[currentNodeIndex].Position, maxPoint))
                    {
                        maxPoint = Nodes[currentNodeIndex].Position;
                    }
                }

                RegionBounds = new Bounds();
                RegionBounds.SetMinMax(new Vector3(minPoint.x, minPoint.y, 0.0f), new Vector3(maxPoint.x, maxPoint.y, 0.0f));
            }
        }

        private static bool IsSmallestPoint(Vector2 first, Vector2 second)
        {
            return first.y < second.y || (first.y.Equals(second.y) && first.x < second.x);
        }

        private static bool IsLargestPoint(Vector2 first, Vector2 second)
        {
            return first.y > second.y || (first.y.Equals(second.y) && first.x > second.x);
        }
    }
}

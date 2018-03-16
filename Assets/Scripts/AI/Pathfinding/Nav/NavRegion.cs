// Copyright (C) Threetee Gang All Rights Reserved

using System;
using UnityEngine;

namespace Assets.Scripts.AI.Pathfinding.Nav
{
    public static class NavRegionConstants
    {
        public const float MaxInclusionExtension = 0.01f;
    }

    [Serializable]
    public class NavRegion
    {
        public NavNode[] Nodes;
        public Rect RegionBounds;

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

                RegionBounds = Rect.MinMaxRect(minPoint.x, minPoint.y, maxPoint.x + NavRegionConstants.MaxInclusionExtension, maxPoint.y + NavRegionConstants.MaxInclusionExtension);
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

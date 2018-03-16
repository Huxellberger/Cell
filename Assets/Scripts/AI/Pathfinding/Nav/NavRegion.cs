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
            RegionBounds = GenerateRectFromNodes(Nodes);
        }

        public static Rect GenerateRectFromNodes(NavNode[] inNodes)
        {
            if (inNodes.Length >= 2)
            {
                var minX = inNodes[0].Position.x;
                var maxX = inNodes[0].Position.x;

                var minY = inNodes[0].Position.y;
                var maxY = inNodes[0].Position.y;

                for (var currentNodeIndex = 1; currentNodeIndex < inNodes.Length; currentNodeIndex++)
                {
                    var currentNodePosition = inNodes[currentNodeIndex].Position;

                    if (currentNodePosition.x < minX)
                    {
                        minX = currentNodePosition.x;
                    }
                    else if (currentNodePosition.x > maxX)
                    {
                        maxX = currentNodePosition.x;
                    }

                    if (currentNodePosition.y < minY)
                    {
                        minY = currentNodePosition.y;
                    }
                    else if (currentNodePosition.y > maxY)
                    {
                        maxY = currentNodePosition.y;
                    }
                }

                return Rect.MinMaxRect(minX, minY, maxX + NavRegionConstants.MaxInclusionExtension, maxY + NavRegionConstants.MaxInclusionExtension);
            }

            return new Rect();
        }
    }
}

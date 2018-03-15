// Copyright (C) Threetee Gang All Rights Reserved

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Assets.Scripts.AI.Pathfinding.Nav
{
    public static class NavGenerationFunctions
    {
        public class TileNavInfo
        {
            public readonly TileBase CurrentTile;
            public readonly Vector2 CurrentTilePosition;
            public readonly Vector3Int CurrentCell;

            public TileNavInfo(TileBase inTile, Vector2 inTilePosition, Vector3Int inCell)
            {
                CurrentTile = inTile;
                CurrentTilePosition = inTilePosition;
                CurrentCell = inCell;
            }
        }

        public class NodeReturnResult
        {
            public readonly int NodeIndex;
            public readonly NavNode ReturnedNode;

            public NodeReturnResult(int inNodeIndex, NavNode inReturnedNode)
            {
                NodeIndex = inNodeIndex;
                ReturnedNode = inReturnedNode;
            }
        }

        public static List<NavNode> GenerateNavDataForCurrentScene()
        {
            var allTilemaps = Object.FindObjectsOfType<Tilemap>();

            var collidingTilemaps = new List<TilemapCollider2D>(allTilemaps.Length);
            var walkableTilemaps = new List<Tilemap>(allTilemaps.Length);

            foreach (var tilemap in allTilemaps)
            {
                var collider = tilemap.gameObject.GetComponent<TilemapCollider2D>();
                if (collider != null)
                {
                    collidingTilemaps.Add(collider);
                }
                else
                {
                    walkableTilemaps.Add(tilemap);
                }
            }

            return GenerateNavDataForTilemaps(walkableTilemaps, collidingTilemaps);
        }

        private static List<NavNode> GenerateNavDataForTilemaps
        (
            IEnumerable<Tilemap> inTilemaps,
            IEnumerable<TilemapCollider2D> inBlockingTilemaps
        )
        {
            var tileInfos = new List<TileNavInfo>();
            foreach (var tilemap in inTilemaps)
            {
                tilemap.CompressBounds();

                // Go through each of the cells (each x and y corresponds to a tile in the cell)
                for (var y = tilemap.origin.y; y < (tilemap.origin.y + tilemap.size.y); y++)
                {
                    for (var x = tilemap.origin.x; x < (tilemap.origin.x + tilemap.size.x); x++)
                    {
                        var tileCell = new Vector3Int(x, y, 0);
                        var tile = tilemap.GetTile(tileCell);

                        if (tile != null)
                        {
                            Vector2 tileWorldPosition = tilemap.CellToWorld(tileCell);
                            if (!TileIsBlocked(tileWorldPosition, inBlockingTilemaps))
                            {
                                tileInfos.Add(new TileNavInfo(tile, tileWorldPosition, tileCell));
                            }
                        }
                    }
                }
            }

            return GenerateNavDataFromPoints(tileInfos);
        }

        private static bool TileIsBlocked(Vector2 tileWorldPos, IEnumerable<TilemapCollider2D> blockingTiles)
        {
            foreach (var blockingTile in blockingTiles)
            {
                if (blockingTile.OverlapPoint(tileWorldPos))
                {
                    return true;
                }
            }

            return false;
        }

        private static List<NavNode> GenerateNavDataFromPoints(List<TileNavInfo> nodePositions)
        {
            var generatedNodes = new List<NavNode>();
            var foundNeighbours = new List<int>(4);
            foreach (var nodePosition in nodePositions)
            {
                foreach (var otherNode in nodePositions)
                {
                    if (NodeIsNeighbour(nodePosition.CurrentCell, otherNode.CurrentCell))
                    {
                        var neighbourNode = GetNode(otherNode, generatedNodes);
                        if (neighbourNode == null)
                        {
                            var generatedNeighbourNode = new NavNode { Position = otherNode.CurrentTilePosition, Weight = 1 };
                            generatedNodes.Add(generatedNeighbourNode);
                            neighbourNode = new NodeReturnResult(generatedNodes.Count - 1, generatedNeighbourNode);
                        }

                        foundNeighbours.Add(neighbourNode.NodeIndex);
                    }
                }

                var nodeToUpdate = GetNode(nodePosition, generatedNodes);

                if (nodeToUpdate == null)
                {
                    var generatedNodeToUpdate = new NavNode
                    {
                        Position = nodePosition.CurrentTilePosition,
                        Weight = 1,
                        Neighbours = foundNeighbours.ToArray()
                    };

                    generatedNodes.Add(generatedNodeToUpdate);
                }
                else
                {
                    nodeToUpdate.ReturnedNode.Neighbours = foundNeighbours.ToArray();
                }

                foundNeighbours.Clear();
            }

            return generatedNodes;
        }

        private static bool NodeIsNeighbour(Vector3Int inNode, Vector3Int inOtherNode)
        {
            // Same Node
            if (inNode.Equals(inOtherNode))
            {
                return false;
            }

            return Mathf.Abs(inNode.x - inOtherNode.x) + Mathf.Abs(inNode.y - inOtherNode.y) == 1;
        }

        private static NodeReturnResult GetNode(TileNavInfo inInfo, List<NavNode> existingNodes)
        {
            for (var currentNodeIndex = 0; currentNodeIndex < existingNodes.Count; currentNodeIndex++) 
            {
                if (inInfo.CurrentTilePosition.Equals(existingNodes[currentNodeIndex].Position))
                {
                    return new NodeReturnResult(currentNodeIndex, existingNodes[currentNodeIndex]);
                }
            }

            return null;
        }
    }
}

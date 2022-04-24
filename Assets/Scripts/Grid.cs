using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
namespace WEditor.Game
{
    public class Grid : MonoBehaviour
    {
        public static Grid instance;
        public Tilemap levelTilemap;
        public const float levelTileSize = .64f;
        Node[,] grid;

        void Start()
        {
            instance = this;
        }
        public void CreateGrid()
        {
            if (grid != null) return;

            grid = new Node[levelTilemap.size.x, levelTilemap.size.y];
            for (int x = 0; x < levelTilemap.size.x; x++)
            {
                for (int y = 0; y < levelTilemap.size.y; y++)
                {
                    grid[x, y] = new Node(x, y, IsWalkable(x, y));
                }
            }
        }
        public List<Node> GetNeighbours(Node node)
        {
            List<Node> neighbours = new List<Node>();

            Vector3Int cellPos = Vector3Int.FloorToInt(node.gridPosition);

            Vector3Int topTile = cellPos.GetTopTile();
            Vector3Int bottomTile = cellPos.GetBottomTile();
            Vector3Int rightTile = cellPos.GetRightTile();
            Vector3Int leftTile = cellPos.GetLeftTile();

            if (IsInsideGrid(topTile))
                neighbours.Add(grid[topTile.x, topTile.y]);

            if (IsInsideGrid(bottomTile))
                neighbours.Add(grid[bottomTile.x, bottomTile.y]);

            if (IsInsideGrid(leftTile))
                neighbours.Add(grid[leftTile.x, leftTile.y]);

            if (IsInsideGrid(rightTile))
                neighbours.Add(grid[rightTile.x, rightTile.y]);

            return neighbours;
        }
        private bool IsInsideGrid(Vector3Int cellPos)
        {
            int x = cellPos.x;
            int y = cellPos.y;
            return x >= 0 && x < grid.GetLength(0) && y >= 0 && y < grid.GetLength(1);
        }
        private bool IsWalkable(int x, int y)
        {
            Vector3Int cellPos = new Vector3Int(x, y, 0);
            string tileName = "";
            if (levelTilemap.HasTile(cellPos))
                tileName = levelTilemap.GetTile(cellPos).name.ToLower();

            if (tileName == "")
                return true;

            if (!tileName.Contains("wall") && !tileName.Contains("prop"))
                return true;
            else
                return false;
        }
        private int GetDistance(Node nodeA, Node nodeB)
        {
            int dstX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
            int dstY = Mathf.Abs(nodeA.gridY - nodeB.gridY);

            if (dstX > dstY)
                return 14 * dstY + 10 * (dstX - dstY);
            return 14 * dstX + 10 * (dstY - dstX);
        }
        public Node GetNodeFromGridPosition(Vector3 nodePosition)
        {
            Vector3Int nodeInt = Vector3Int.FloorToInt(nodePosition);
            return grid[nodeInt.x, nodeInt.z];
        }
        // private void OnDrawGizmos()
        // {
        //     // Gizmos.DrawWireCube(center, new Vector3(levelTilemap.size.x + .5f, levelTilemap.size.y, levelTilemap.size.y));
        //     foreach (Node node in grid)
        //     {
        //         Gizmos.color = (node.isWalkable) ? Color.white : Color.red;
        //         Gizmos.DrawCube(new Vector3(node.gridPosition.x + .5f, .5f, node.gridPosition.y + .5f), Vector3.one * levelTileSize);
        //     }

        // }
    }
}
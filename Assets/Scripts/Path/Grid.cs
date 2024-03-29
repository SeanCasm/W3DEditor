using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WEditor.Utils;
namespace WEditor.Game
{
    public class Grid : MonoBehaviour
    {
        public static Grid instance;
        private Vector2Int levelSize { get => DataHandler.levelSize; }
        Node[,] grid;

        void Start()
        {
            instance = this;
        }
        public void CreateGrid()
        {
            if (grid != null) return;

            grid = new Node[levelSize.x, levelSize.y];
            for (int x = 0; x < levelSize.x; x++)
            {
                for (int y = 0; y < levelSize.y; y++)
                {
                    grid[x, y] = new Node(x, y, IsWalkable(x, y));
                }
            }
        }
        public List<Node> GetNeighbours(Node node)
        {
            List<Node> neighbours = new List<Node>();

            Vector3Int cellPos = new Vector3Int(node.gridX, node.gridY, 0);

            Vector3Int topTile = cellPos.GetTop();
            Vector3Int bottomTile = cellPos.GetBottom();
            Vector3Int rightTile = cellPos.GetRight();
            Vector3Int leftTile = cellPos.GetLeft();

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
            if (DataHandler.grid[x, y] == null)
                return true;

            string tileName = DataHandler.grid[x, y].tileName.ToLower();

            if (!tileName.Contains("wall") && !tileName.Contains("prop"))
                return true;
            else
                return false;
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
        //         Gizmos.DrawCube(new Vector3(node.gridPosition.x, .5f, node.gridPosition.z), Vector3.one * levelTileSize);
        //     }
        // }
    }
}
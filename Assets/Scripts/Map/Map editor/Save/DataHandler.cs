using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace WEditor
{
    public static class DataHandler
    {
        public static object tileMap;
        public static EditorGridLevelData[,] grid { get; private set; }
        public static Vector3 currentLevelPosition { get; set; }
        public static string currentLevelName { get; set; }
        public static DifficultyTier difficultyTier = DifficultyTier.Easy;
        public static Vector2Int levelSize { get => new Vector2Int(grid.GetLength(0), grid.GetLength(1)); }
        public static Vector3Int spawnPosition;
        public static void SetGrid(Vector3Int dimension, EditorGridLevelData data)
        {
            grid[dimension.x, dimension.y] = data;
        }
        public static void ClearGrid()
        {
            grid = new EditorGridLevelData[0, 0];
        }
        public static void GridSize(Vector3Int dimension)
        {
            grid = new EditorGridLevelData[dimension.x, dimension.y];
        }
    }
    public class EditorGridLevelData
    {
        public EditorGridLevelData(Vector3Int position, string tileName)
        {
            this.position = position;
            this.tileName = tileName;
        }
        public string tileName { get; private set; }
        public Vector3Int position { get; private set; }
    }
}
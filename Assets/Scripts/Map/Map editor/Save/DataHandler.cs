using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace WEditor
{
    public static class DataHandler
    {
        public static Tilemap tileMap;
        public static EditorGridLevelData[,] grid { get; private set; }
        public static GameData currentLevel { get; set; }
        public static Vector3 currentLevelPosition { get; set; }
        public static string currentLevelName { get; set; }
        public static Vector2Int levelSize { get => new Vector2Int(grid.GetLength(0), grid.GetLength(1)); }
        public static Vector3Int spawnPosition;
        public static int[] levelGuns = new int[] { 0, 1, 2, 3 }; //default value
        public static void SetCurrentLevel(GameData curr)
        {
            currentLevel = curr;
            currentLevelName = currentLevel.levelName;
            levelGuns = currentLevel.levelGunsToArray;
            DataHandler.currentLevelPosition = new Vector3(currentLevel.levelSpawnX, .5f, currentLevel.levelSpawnZ);
        }
        public static bool CheckForWall(Vector3Int position)
        {
            EditorGridLevelData eg = grid[position.x, position.y];
            if (eg != null)
                return eg.tileName.Contains("Wall");
            return false;
        }
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
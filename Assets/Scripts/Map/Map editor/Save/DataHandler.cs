using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace WEditor
{
    public static class DataHandler
    {
        public static EditorGridLevelData[,] grid { get; private set; }
        public static Vector3 currentLevelPosition { get; set; }
        public static string currentLevelName { get; set; }
        public static Vector2Int levelSize { get => new Vector2Int(grid.GetLength(0), grid.GetLength(1)); }
        public static void SetGrid(int dimension, int dimension2, EditorGridLevelData data)
        {
            grid[dimension, dimension2] = data;
        }
        public static void ClearGrid()
        {
            grid = new EditorGridLevelData[0, 0];
        }
        public static void GridSize(int dimension, int dimension2)
        {
            grid = new EditorGridLevelData[dimension, dimension2];
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
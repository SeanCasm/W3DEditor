using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WEditor.Scenario.Editor;
namespace WEditor
{
    public static class DataHandler
    {
        public static TileLevelData[,] levelTiles { get; private set; }
        public static Vector3 currentLevelPosition { get; set; }
        public static string currentLevelName { get; set; }
        public static Vector2Int levelSize { get => new Vector2Int(levelTiles.GetLength(0), levelTiles.GetLength(1)); }
        public static void SetLevelTiles(int dimension, int dimension2, TileLevelData data)
        {
            levelTiles[dimension, dimension2] = data;
        }
        public static void ClearLevelTiles()
        {
            levelTiles = new TileLevelData[0, 0];
        }
        public static void LevelTileSize(int dimension, int dimension2)
        {
            levelTiles = new TileLevelData[dimension, dimension2];
        }
    }
}
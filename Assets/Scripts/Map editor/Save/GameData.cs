using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WEditor.Scenario.Editor;

namespace WEditor
{
    [System.Serializable]
    public class GameData
    {
        public (int x, int z) levelSpawn;
        public List<(int assetListIndex, int xpos, int ypos, string tileName)> levelTiles { get; set; } = new List<(int assetListIndex, int xpos, int ypos, string tileName)>();
        public (int x, int y) levelSize;
        public string levelName;
        public int levelID;
        public GameData(string levelName, Vector3Int levelSpawn, (int w, int h) size, TileLevelData[,] tileData)
        {
            this.levelName = levelName;
            this.levelSpawn.x = levelSpawn.x;
            this.levelSpawn.z = levelSpawn.y;
            this.levelSize = size;
            foreach (var item in tileData)
            {
                if (item.tileName != null)
                {
                    levelTiles.Add((item.assetListIndex, item.position.x, item.position.y, item.tileName));
                }
            }
        }
    }
}
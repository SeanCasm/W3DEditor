using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WEditor
{
    [System.Serializable]
    public class GameData
    {
        public (int x, int z) levelSpawn;
        public List<(int assetListIndex, int xpos, int ypos, string tileName)> levelTiles { get; set; } = new List<(int assetListIndex, int xpos, int ypos, string tileName)>();
        public (int x, int y) levelSize;
        public string levelName;
        public GameData(string levelName, Vector3Int levelSpawn, (int w, int h) size, TileData[,] tileData)
        {
            this.levelName = levelName;
            this.levelSpawn.x = levelSpawn.x;
            this.levelSpawn.z = levelSpawn.y;
            this.levelSize = size;
            foreach (var item in tileData)
            {
                if (item.tileName != null)
                {
                    Debug.Log(item.tileName);
                    levelTiles.Add((item.assetListIndex, item.position.x, item.position.y, item.tileName));
                }
            }
        }
    }

    public struct TileData
    {
        public TileData(int assetListIndex, Vector3Int position, string tileName)
        {
            this.assetListIndex = assetListIndex;
            this.position = position;
            this.tileName = tileName;
        }
        public string tileName { get; private set; }
        public int assetListIndex { get; private set; }
        public Vector3Int position { get; private set; }

    }
}

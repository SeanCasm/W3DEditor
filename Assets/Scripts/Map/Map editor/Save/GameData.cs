using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WEditor
{
    [System.Serializable]
    public class GameData
    {
        public (int x, int z) levelSpawn;
        public string[,] levelTiles { get; private set; }
        public (int x, int y) levelSize;
        public string levelName;
        public int levelID;
        public GameData(string levelName, Vector3Int levelSpawn, (int w, int h) size, EditorGridLevelData[,] tileData)
        {
            this.levelName = levelName;
            this.levelSpawn.x = levelSpawn.x;
            this.levelSpawn.z = levelSpawn.y;
            this.levelSize = size;
            levelTiles = new string[size.w, size.h];
            foreach (var item in tileData)
            {
                if (item != null)
                {
                    Debug.Log(item.tileName);
                    levelTiles[item.position.x, item.position.y] = item.tileName;
                }
            }
        }
    }
}
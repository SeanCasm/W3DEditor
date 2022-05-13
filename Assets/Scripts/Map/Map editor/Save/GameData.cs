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
        public string difficultTier = "";
        public int levelID;
        public GameData()
        {
            this.levelName = DataHandler.currentLevelName;
            this.levelSpawn.x = DataHandler.spawnPosition.x;
            this.levelSpawn.z = DataHandler.spawnPosition.z;
            this.levelSize.x = DataHandler.levelSize.x;
            this.levelSize.y = DataHandler.levelSize.y;
            this.levelTiles = new string[levelSize.x, levelSize.y];
            foreach (var item in DataHandler.grid)
            {
                if (item != null)
                {
                    levelTiles[item.position.x, item.position.y] = item.tileName;
                }
            }
        }
    }
}
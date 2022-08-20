using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace WEditor
{
    public class GameData
    {
        public int levelSpawnX, levelSpawnZ;
        public List<string> levelTiles = new List<string>();
        public List<int> levelTilesX = new List<int>();
        public List<int> levelTilesY = new List<int>();
        public int levelSizeX, levelSizeY;
        public string levelName;
        public List<int> levelGuns = new List<int>();
        public int[] levelGunsToArray => levelGuns.ToArray();

        public void SetData()
        {
            this.levelName = DataHandler.currentLevelName;
            this.levelSpawnX = DataHandler.spawnPosition.x;
            this.levelSpawnZ = DataHandler.spawnPosition.z;
            this.levelSizeX = DataHandler.levelSize.x;
            this.levelSizeY = DataHandler.levelSize.y;
            this.levelGuns = DataHandler.levelGuns.ToList();
            int pos = 0;
            for (int x = 0; x < DataHandler.grid.GetLength(0); x++)
            {
                for (int y = 0; y < DataHandler.grid.GetLength(1); y++)
                {
                    EditorGridLevelData grid = DataHandler.grid[x, y];
                    if (grid != null)
                    {
                        levelTiles.Add(grid.tileName);
                        levelTilesX.Add(x);
                        levelTilesY.Add(y);
                        pos++;
                    }
                }
            }
        }
    }
}
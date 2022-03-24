using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WEditor
{
    [System.Serializable]
    public class GameData
    {
        public (int x, int z) levelSpawn;
        public List<(string assetPath, int xpos, int ypos)> groundWallTiles { get; set; } = new List<(string assetPath, int xpos, int ypos)>();
        public List<(string assetPath, int xpos, int ypos)> propDoorTiles { get; set; } = new List<(string assetPath, int xpos, int ypos)>();
        public string levelName;
        public GameData(string levelName, Vector3Int levelSpawn)
        {
            this.levelName = levelName;
            this.levelSpawn.x = levelSpawn.x;
            this.levelSpawn.z = levelSpawn.y;
            foreach (var item in DataHandler.groundWallTiles)
            {
                groundWallTiles.Add((item.assetPath, item.position.x, item.position.y));
            }
            foreach (var item in DataHandler.propDoorTiles)
            {
                propDoorTiles.Add((item.assetPath, item.position.x, item.position.y));
            }
        }
    }

    public struct TileData
    {
        public TileData(string assetPath, Vector3Int position)
        {
            this.assetPath = assetPath;
            this.position = position;
        }
        public string assetPath { get; private set; }
        public Vector3Int position { get; private set; }

    }
}

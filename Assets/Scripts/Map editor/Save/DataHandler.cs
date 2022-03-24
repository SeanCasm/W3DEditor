using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WEditor
{
    public static class DataHandler
    {
        public static TileData[,] groundWallTiles { get; private set; }
        public static TileData[,] propDoorTiles { get; private set; }
      
        public static void SetPropDoorTileData(int dimension, int dimension2, TileData data)
        {
            propDoorTiles[dimension, dimension2] = data;
        }
        public static void SetGroundWallTileData(int dimension, int dimension2, TileData data)
        {
            groundWallTiles[dimension, dimension2] = data;
        }
        public static void TileDataSize(int dimension, int dimension2)
        {
            groundWallTiles = new TileData[dimension, dimension2];
            propDoorTiles = new TileData[dimension, dimension2];
        }
        public static void Save(string levelName, Vector3Int levelSpawn)
        {
            SaveData.SaveToLocal(levelName, levelSpawn);
        }

    }
}

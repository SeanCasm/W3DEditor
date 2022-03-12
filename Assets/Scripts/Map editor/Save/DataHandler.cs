using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WEditor
{
    public static class DataHandler
    {
        public static TileData[,] tileData { get; private set; }
        public static void SetTileData(int dimension, int dimension2, TileData data)
        {
            tileData[dimension, dimension2] = data;
        }
        public static void TileDataSize(int dimension, int dimension2)
        {
            tileData = new TileData[dimension, dimension2];
        }

    }
}

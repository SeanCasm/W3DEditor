using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WEditor
{
    [System.Serializable]
    public class GameData
    {
        public List<(string assetPath, int xpos, int ypos)> tileInfo { get; set; } = new List<(string assetPath, int xpos, int ypos)>();
        public GameData()
        {
            foreach (var item in DataHandler.tileData)
            {
                tileInfo.Add((item.assetPath, item.position.x, item.position.y));
            }
        }
    }

    [System.Serializable]
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

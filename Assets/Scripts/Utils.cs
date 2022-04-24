using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace WEditor
{
    public static class Utils
    {

        public static Vector3 FixTilePivot(this Vector3 value1)
        {
            return new Vector3(value1.x + .5f, value1.y, value1.z + .5f);
        }
        public static Vector3Int GetTopTile(this Vector3Int cellPos)
        {
            return new Vector3Int(cellPos.x, cellPos.y + 1, 0);
        }
        public static Vector3Int GetBottomTile(this Vector3Int cellPos)
        {
            return new Vector3Int(cellPos.x, cellPos.y - 1, 0);
        }
        public static Vector3Int GetLeftTile(this Vector3Int cellPos)
        {
            return new Vector3Int(cellPos.x - 1, cellPos.y, 0);
        }
        public static Vector3Int GetRightTile(this Vector3Int cellPos)
        {
            return new Vector3Int(cellPos.x + 1, cellPos.y, 0);
        }
        public static int GetIndexFromAssetName(this string assetName)
        {
            return int.Parse(assetName.Split('_')[1].ToString());
        }
    }
}

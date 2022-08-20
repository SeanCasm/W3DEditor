using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace WEditor.Utils
{
    public static class GridUtils
    {
        /// <summary>
        /// Fix the position of the current object to match with the tile center.
        /// </summary>
        /// <param name="value1"></param>
        /// <returns></returns>
        public static Vector3 FixTilePivot(this Vector3 value1)
        {
            return new Vector3(value1.x + .5f, value1.y, value1.z + .5f);
        }
        public static Vector3Int GetTop(this Vector3Int cellPos)
        {
            return new Vector3Int(cellPos.x, cellPos.y + 1, 0);
        }
        public static Vector3Int GetBottom(this Vector3Int cellPos)
        {
            return new Vector3Int(cellPos.x, cellPos.y - 1, 0);
        }
        public static Vector3Int GetLeft(this Vector3Int cellPos)
        {
            return new Vector3Int(cellPos.x - 1, cellPos.y, 0);
        }
        public static Vector3Int GetRight(this Vector3Int cellPos)
        {
            return new Vector3Int(cellPos.x + 1, cellPos.y, 0);
        }
        public static Vector3Int SwapZToY(this Vector3Int cellPos)
        {
            return new Vector3Int(cellPos.x, cellPos.z, 0);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace WEditor.Scenario
{
    public class Door
    {
        public Door(bool topBottomSide, string name, Vector2Int[] walls)
        {
            this.topBottomSide = topBottomSide;
            this.name = name;
            this.walls = walls;
        }
        public Door() { }
        public Vector3Int position;
        public Vector2Int[] walls { get; set; }
        public bool topBottomSide { get; private set; }
        public string name;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace WEditor.Scenario
{
    public class Door : LevelBase
    {
        public Door(string name, WallSide wallSide)
        {
            this.topBottomSide = wallSide;
            this.tileName = name;
        }
        public Door() { }
        public Vector3Int elevatorPosition { get; set; }
        public WallSide topBottomSide { get; set; }
    }
    public enum WallSide
    {
        TopBottom, LeftRight
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WEditor.Scenario.Playable;

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
        public WallSide topBottomSide { get; set; } = WallSide.None;
    }

    public enum WallSide
    {
        TopBottom, LeftRight, None
    }
}

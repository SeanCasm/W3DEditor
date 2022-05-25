using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace WEditor.Scenario
{
    public class LevelBase
    {
        public Vector3Int position;
        public string tileName;
        public LevelBase() { }
        public LevelBase(string tileName, Vector3Int position)
        {
            this.position = position;
            this.tileName = tileName;
        }
    }
}

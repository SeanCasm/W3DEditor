using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WEditor.Scenario
{
    public class Wall
    {
        public string wallName;
        public Vector3Int position;
        public Wall(string wallName, Vector3Int position)
        {
            this.position = position;
            this.wallName = wallName;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace WEditor.Scenario.Playable
{
    public class ScenarioGenerator : ScenarioGeneratorBase
    {
        public void InitGeneration(GameData levelData)
        {
            (int x, int y) size = levelData.levelSize;
            DataHandler.GridSize(new Vector3Int(size.x, size.y, 0));
            base.InitGeneration();
            List<Door> doors = new List<Door>();
            List<Wall> walls = new List<Wall>();
            for (int x = 0; x < size.x; x++)
            {
                for (int y = 0; y < size.y; y++)
                {
                    if (levelData.levelTiles[x, y] == null)
                        continue;
                        
                    string tileName = levelData.levelTiles[x, y];
                    Vector3Int cellPos = new Vector3Int(x, y, 0);
                    base.HandleTilesLocation(tileName, cellPos, doors, walls);
                    DataHandler.SetGrid(new Vector3Int(x, y, 0), new EditorGridLevelData(cellPos, tileName));
                }
            }

            base.HandleWallGeneration(walls);
            base.HandleDoorsGeneration(doors);
            PlayerGlobalReference.instance.position = DataHandler.currentLevelPosition;
        }
    }
}

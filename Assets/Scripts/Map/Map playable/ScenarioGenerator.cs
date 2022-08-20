using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WEditor.Scenario.Playable
{
    public class ScenarioGenerator : ScenarioGeneratorBase
    {
        public void InitGeneration(GameData levelData)
        {
            (int x, int y) size;
            size.x = levelData.levelSizeX;
            size.y = levelData.levelSizeY;
            DataHandler.GridSize(new Vector3Int(size.x, size.y, 0));
            base.InitGeneration();
            List<Door> doors = new List<Door>();
            List<Wall> walls = new List<Wall>();
            for (int pos = 0; pos < levelData.levelTiles.Count; pos++)
            {
                int y = levelData.levelTilesY[pos];
                int x = levelData.levelTilesX[pos];

                string tileName = levelData.levelTiles[pos];
                Vector3Int cellPos = new Vector3Int(x, y, 0);
                base.HandleTilesLocation(tileName, cellPos, doors, walls);
                DataHandler.SetGrid(new Vector3Int(x, y, 0), new EditorGridLevelData(cellPos, tileName));
            }

            base.HandleWallGeneration(walls);
            base.HandleDoorsGeneration(doors);
            PlayerGlobalReference.instance.position = DataHandler.currentLevelPosition;
        }
    }
}

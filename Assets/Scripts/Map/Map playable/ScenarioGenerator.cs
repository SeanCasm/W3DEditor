using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using WEditor.Game.Scriptables;

namespace WEditor.Scenario.Playable
{
    public class ScenarioGenerator : ScenarioGeneratorBase
    {
        public void InitGeneration(GameData levelData)
        {
            (int x, int y) size = levelData.levelSize;
            base.InitGeneration();
            List<Door> doors = new List<Door>();
            List<Wall> walls = new List<Wall>();

            DataHandler.GridSize(new Vector3Int(size.x, size.y, 0));
            for (int x = 0; x < size.x; x++)
            {
                for (int y = 0; y < size.y; y++)
                {
                    if (levelData.levelTiles[x, y] == null)
                        continue;
                    string tileName = levelData.levelTiles[x, y];
                    Vector3Int cellPos = new Vector3Int(x, y, 0);
                    mainGrid[x, y] = true;
                    base.HandleTilesLocation(tileName, cellPos, doors, walls);
                    DataHandler.SetGrid(new Vector3Int(x, y, 0), new EditorGridLevelData(cellPos, tileName));
                }
            }

            base.HandleWallGeneration(walls);
            this.HandleDoorGeneration(doors);
            PlayerGlobalReference.instance.position = DataHandler.currentLevelPosition;
        }
        private void HandleDoorGeneration(List<Door> doors)
        {
            //Doors needs to be located after all of the rest of tiles
            //to avoid fails on the generation
            doors.ForEach(item =>
            {
                Tile itemTile = ScriptableObject.CreateInstance("Tile") as Tile;
                Vector3Int cellPos = item.position;
                Texture2D doorTex = doorScriptable.GetTexture(item.tileName);
                itemTile.sprite = Sprite.Create(doorTex, new Rect(0, 0, doorTex.width, doorTex.height), new Vector2(.5f, .5f));

                mainGrid[cellPos.x, cellPos.y] = true;

                AddDoorToList(item);
            });
            base.HandleDoorsGeneration();
        }
    }
}

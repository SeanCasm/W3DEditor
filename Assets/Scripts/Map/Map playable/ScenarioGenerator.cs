using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using WEditor.Game.Scriptables;

namespace WEditor.Scenario.Playable
{
    public class ScenarioGenerator : ScenarioGeneratorBase
    {
        [SerializeField] ScenarioScriptable wallScenarioScriptable;
        public void InitGeneration(GameData levelData)
        {
            (int x, int y) size = levelData.levelSize;
            base.InitGeneration(new Vector3Int(size.x, size.y, 0));
            List<(Vector3Int, string)> doors = new List<(Vector3Int, string)>();
            List<(string tileName, Vector3Int cellPos)> walls = new List<(string, Vector3Int)>();

            DataHandler.GridSize(size.x, size.y);
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
                    DataHandler.SetGrid(x, y, new EditorGridLevelData(cellPos, tileName));
                }
            }

            base.HandleWallGeneration(walls);
            this.HandleDoorGeneration(doors);
            PlayerGlobalReference.instance.position = DataHandler.currentLevelPosition;
        }
        private void HandleDoorGeneration(List<(Vector3Int, string)> doors)
        {
            //Doors needs to be located after all of the rest of tiles
            //to avoid fails on the generation
            doors.ForEach(item =>
            {
                Tile itemTile = new Tile();
                Vector3Int cellPos = item.Item1;
                Texture2D doorTex = doorScriptable.GetTexture(item.Item2);
                itemTile.sprite = Sprite.Create(doorTex, new Rect(0, 0, doorTex.width, doorTex.height), new Vector2(.5f, .5f));

                mainGrid[cellPos.x, cellPos.y] = true;

                AddDoorToList(cellPos, item.Item2);
            });
            base.HandleDoorsGeneration();
        }
    }
}

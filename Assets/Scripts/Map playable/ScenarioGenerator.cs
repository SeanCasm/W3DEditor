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

            levelData.levelTiles.ForEach(item =>
                {
                    Tile itemTile = new Tile();
                    (int x, int y, string tileName) = item;
                    Vector3Int cellPos = new Vector3Int(x, y, 0);
                    itemTile.name = tileName;
                    mainGrid[x, y] = true;
                    if (tileName.Contains("top"))
                    {
                        itemTile.sprite = propsTopSprites.GetSprite(tileName);
                        HandlePropGeneration(tileName, cellPos);
                    }

                    else if (tileName.Contains("wall"))
                    {
                        walls.Add((tileName, cellPos));
                    }
                    else if (tileName.Contains("prop"))
                    {
                        itemTile.sprite = propsDefaultSprites.GetSprite(tileName);
                        HandlePropGeneration(tileName, cellPos);
                    }
                    else if (tileName.Contains("door"))
                    {
                        doors.Add((cellPos, tileName));
                    }
                }
            );
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

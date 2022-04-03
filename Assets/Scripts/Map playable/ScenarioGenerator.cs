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
            List<(int, int, int, string)> doors = new List<(int, int, int, string)>();

            levelData.levelTiles.ForEach(item =>
            {
                Tile itemTile = new Tile();
                (int index, int x, int y, string tileName) = item;
                Vector3Int cellPos = new Vector3Int(x, y, 0);
                itemTile.name = tileName;

                if (tileName.Contains("top"))
                {
                    itemTile.sprite = propsTopSprites.spritesCollection[index];
                    mainTilemap.SetTile(cellPos, itemTile);

                    HandlePropGeneration(tileName, cellPos);
                }

                else if (tileName.Contains("wall"))
                {
                    itemTile.sprite = wallTextures.spritesCollection[index];
                    mainTilemap.SetTile(cellPos, itemTile);

                    HandleWallGeneration(tileName, cellPos);
                }
                else if (tileName.Contains("prop"))
                {
                    itemTile.sprite = propsDefaultSprites.spritesCollection[index];
                    mainTilemap.SetTile(cellPos, itemTile);

                    HandlePropGeneration(tileName, cellPos);
                }
                else if (tileName.Contains("door"))
                {
                    doors.Add(item);
                }

            });
            //Doors needs to be located after all of the rest of tiles
            //to avoid fails on the generation
            doors.ForEach(item =>
            {
                Tile itemTile = new Tile();
                (int index, int x, int y, string tileName) = item;
                Vector3Int cellPos = new Vector3Int(x, y, 0);

                itemTile.sprite = doorSprites.spritesCollection[index];
                mainTilemap.SetTile(cellPos, itemTile);

                AddDoorToList(cellPos, tileName);
            });
            HandleDoorsGeneration();
            PlayerGlobalReference.instance.position = DataHandler.currentLevelPosition;
            mainTilemap.gameObject.SetActive(false);
        }
    }
}

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
            levelData.levelTiles.ForEach(item =>
            {
                Tile itemTile = new Tile();
                Vector3Int cellPos = new Vector3Int(item.xpos, item.ypos, 0);
                string tileName = item.tileName.ToLower();
                itemTile.name = tileName;

                if (itemTile.name.Contains("top"))
                {
                    itemTile.sprite = propsTopSprites.spritesCollection[item.assetListIndex];
                    mainTilemap.SetTile(cellPos, itemTile);

                    HandlePropGeneration(tileName, cellPos);
                }

                else if (tileName.Contains("wall"))
                {
                    itemTile.sprite = wallTextures.spritesCollection[item.assetListIndex];
                    mainTilemap.SetTile(cellPos, itemTile);

                    HandleWallGeneration(tileName, cellPos);
                }
                else if (tileName.Contains("prop"))
                {
                    itemTile.sprite = propsDefaultSprites.spritesCollection[item.assetListIndex];
                    mainTilemap.SetTile(cellPos, itemTile);

                    HandlePropGeneration(tileName, cellPos);
                }

            });
            //Doors needs to be located after all of the rest of tiles
            //for issues with the door generation
            levelData.levelTiles.ForEach(item =>
            {
                Tile itemTile = new Tile();
                Vector3Int cellPos = new Vector3Int(item.xpos, item.ypos, 0);
                string tileName = item.tileName.ToLower();
                itemTile.name = tileName;
                if (tileName.Contains("door"))
                {
                    itemTile.sprite = doorSprites.spritesCollection[item.assetListIndex];
                    mainTilemap.SetTile(cellPos, itemTile);

                    AddDoorToList(cellPos, tileName);
                }
            });
            HandleDoorsGeneration();
            PlayerGlobalReference.instance.position = DataHandler.currentLevelPosition;
            mainTilemap.gameObject.SetActive(false);
        }
    }
}

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
            List<(int, int, string)> doors = new List<(int, int, string)>();

            levelData.levelTiles.ForEach(item =>
            {
                Tile itemTile = new Tile();
                (int x, int y, string tileName) = item;
                Vector3Int cellPos = new Vector3Int(x, y, 0);
                itemTile.name = tileName;

                if (tileName.Contains("top"))
                {
                    itemTile.sprite = propsTopSprites.GetSprite(tileName);
                    mainTilemap.SetTile(cellPos, itemTile);

                    HandlePropGeneration(tileName, cellPos);
                }

                else if (tileName.Contains("wall"))
                {
                    itemTile.sprite =  wallScenarioScriptable.GetSprite(tileName);
                    mainTilemap.SetTile(cellPos, itemTile);

                    HandleWallGeneration(tileName, cellPos);
                }
                else if (tileName.Contains("prop"))
                {
                    itemTile.sprite = propsDefaultSprites.GetSprite(tileName);
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
                (int x, int y, string tileName) = item;
                Vector3Int cellPos = new Vector3Int(x, y, 0);
                Texture2D doorTex = doorScriptable.GetTexture(tileName);
                itemTile.sprite = Sprite.Create(doorTex, new Rect(0, 0, doorTex.width, doorTex.height), new Vector2(.5f, .5f));
                mainTilemap.SetTile(cellPos, itemTile);

                AddDoorToList(cellPos, tileName);
            });
            HandleDoorsGeneration();
            PlayerGlobalReference.instance.position = DataHandler.currentLevelPosition;
            mainTilemap.gameObject.SetActive(false);
        }
    }
}

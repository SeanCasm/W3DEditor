using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using WEditor.Events;

namespace WEditor.Scenario.Editor
{
    public class ScenarioGenerator : ScenarioGeneratorBase
    {
        private void OnEnable()
        {
            EditorEvent.instance.onPreviewModeExit += OnPreviewModeExit;
        }
        private void OnDisable()
        {
            EditorEvent.instance.onPreviewModeExit -= OnPreviewModeExit;
        }
        public override void InitGeneration()
        {
            Tilemap mainTilemap = DataHandler.tileMap as Tilemap;
            Vector3Int size = mainTilemap.size;
            base.InitGeneration();
            List<Door> doors = new List<Door>();
            List<Wall> walls = new List<Wall>();
            for (int x = 0; x < size.x; x++)
            {
                for (int y = 0; y < size.y; y++)
                {
                    Vector3Int pos = new Vector3Int(x, y, 0);
                    Vector3 position = mainTilemap.CellToWorld(pos);
                    if (mainTilemap.HasTile(pos))
                    {
                        mainGrid[x, y] = true;
                        TileBase tile = mainTilemap.GetTile(pos);
                        base.HandleTilesLocation(tile.name, pos, doors, walls);
                    }
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
                Texture2D doorTex = doorScriptable.GetTexture(item.tileName);
                itemTile.sprite = Sprite.Create(doorTex, new Rect(0, 0, doorTex.width, doorTex.height), new Vector2(.5f, .5f));

                mainGrid[item.position.x, item.position.y] = true;

                AddDoorToList(item);
            });
            base.HandleDoorsGeneration();
        }
        private new void OnPreviewModeExit()
        {
            base.OnPreviewModeExit();
            doorGrid = new Door[0, 0];
            wallGrid = new Wall[0, 0];
            EditorGrid.instance.currentSpawn.SetActive(true);
            Destroy(groundPlane);

            objectsGenerated.ForEach(wall =>
            {
                Destroy(wall);
            });
            objectsGenerated.Clear();
        }
    }
}
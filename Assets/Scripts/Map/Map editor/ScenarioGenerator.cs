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
            Tilemap mainTilemap = DataHandler.tileMap;
            Vector3Int size = mainTilemap.size;
            base.InitGeneration();
            List<Door> doors = new List<Door>();
            List<Wall> walls = new List<Wall>();
            for (int x = 0; x < size.x; x++)
            {
                for (int y = 0; y < size.y; y++)
                {
                    Vector3Int pos = new Vector3Int(x, y, 0);
                    if (!mainTilemap.HasTile(pos))
                        continue;

                    TileBase tile = mainTilemap.GetTile(pos);
                    base.HandleTilesLocation(tile.name, pos, doors, walls);
                }
            }
            base.HandleWallGeneration(walls);
            base.HandleDoorsGeneration(doors);
            PlayerGlobalReference.instance.position = DataHandler.currentLevelPosition;
        }
        private new void OnPreviewModeExit()
        {
            base.OnPreviewModeExit();
            doorGrid = new Door[0, 0];
            wallGrid = new Wall[0, 0];
            EditorGrid.instance.currentSpawn.SetActive(true);
            Destroy(groundPlane);

            prefabInstances.ForEach(wall =>
            {
                Destroy(wall);
            });
            prefabInstances.Clear();
        }
    }
}
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
            GameEvent.instance.onPreviewModeExit += OnPreviewModeExit;
        }
        private void OnDisable()
        {
            GameEvent.instance.onPreviewModeExit -= OnPreviewModeExit;
        }
        public override void InitGeneration()
        {
            for (int x = 0; x < mainTilemap.size.x; x++)
            {
                for (int y = 0; y < mainTilemap.size.y; y++)
                {
                    Vector3Int pos = new Vector3Int(x, y, 0);
                    if (mainTilemap.HasTile(pos))
                    {
                        TileBase tile = mainTilemap.GetTile(pos);
                        string tileName = tile.name.ToLower();

                        if (tileName.StartsWith("door"))
                        {
                            AddDoorToList(pos, tileName);
                        }
                        else if (tileName.StartsWith("wall"))
                        {
                            HandleWallGeneration(tileName, pos);
                        }
                        else if (tileName.Contains("prop"))
                        {
                            HandlePropGeneration(tileName, pos);
                        }
                        else if (tileName.Contains("health"))
                        {
                            HandleHealthGeneration(tileName, pos);
                        }
                        else if (tileName.Contains("ammo"))
                        {
                            HandleAmmoGeneration(tileName, pos);
                        }
                        else if (tileName.StartsWith("guard") || tileName.StartsWith("ss"))
                        {
                            HandleEnemyGeneration(tileName, pos);
                        }
                    }
                }
            }
            HandleDoorsGeneration();
            PlayerGlobalReference.instance.position = DataHandler.currentLevelPosition;
        }
        private void OnPreviewModeExit()
        {
            doorsLocation.Clear();
            walls.Clear();
            EditorGrid.instance.currentSpawn.SetActive(true);

            objectsGenerated.ForEach(wall =>
            {
                Destroy(wall);
            });
            objectsGenerated.Clear();
        }
    }
}
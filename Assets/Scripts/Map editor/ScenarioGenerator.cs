using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace WEditor.Scenario
{
    public class ScenarioGenerator : MonoBehaviour
    {
        [SerializeField] GameObject grid;
        [SerializeField] Tilemap generatedTilemap;
        public bool isTesting { get; set; }

        public void InitGeneration(GeneratorInfo[,] generatorInfo, Tilemap tilemap)
        {
            generatedTilemap.size = tilemap.size;

            generatedTilemap.transform.position = tilemap.transform.position;

            HandleGroundGeneration(generatedTilemap, tilemap);
        }
        private void HandleGroundGeneration(Tilemap generatedTilemap, Tilemap tilemap)
        {
            for (int x = 0; x < generatedTilemap.size.x; x++)
            {
                for (int y = 0; y < generatedTilemap.size.y; y++)
                {
                    Vector3Int pos = new Vector3Int(x, y, 0);
                    TileBase tile = tilemap.GetTile(pos);
                    generatedTilemap.SetTile(pos, tile);
                }
            }
            generatedTilemap.transform.SetParent(grid.transform);
        }
    }
}
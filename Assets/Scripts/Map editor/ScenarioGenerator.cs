using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using WEditor.Events;
namespace WEditor.Scenario
{
    public class ScenarioGenerator : MonoBehaviour
    {
        [SerializeField] GameObject grid;
        [SerializeField] Tilemap generatedTilemap;
        [SerializeField] List<Sprite> wallTextures;
        [SerializeField] Material wallMaterial;
        [SerializeField] GameObject wallGameObject;
        [SerializeField] float xOffset, yOffset, zOffset;

        private List<GameObject> wallsGenerated = new List<GameObject>();
        private void OnEnable()
        {
            GameEvent.instance.onPreviewModeExit += OnPreviewModeExit;
        }
        private void OnDisable()
        {
            GameEvent.instance.onPreviewModeExit -= OnPreviewModeExit;
        }
        public void InitGeneration(GeneratorInfo[,] generatorInfo, Tilemap tilemap)
        {
            generatedTilemap.size = tilemap.size;

            generatedTilemap.transform.position = tilemap.transform.position;

            HandleGeneration(generatedTilemap, tilemap);
        }
        private void HandleGeneration(Tilemap generatedTilemap, Tilemap tilemap)
        {
            for (int x = 0; x < generatedTilemap.size.x; x++)
            {
                for (int y = 0; y < generatedTilemap.size.y; y++)
                {

                    Vector3Int pos = new Vector3Int(x, y, 0);
                    TileBase tile = tilemap.GetTile(pos);
                    string tileName = tile.name.ToLower();
                    if (tileName.StartsWith("ground"))
                    {
                        generatedTilemap.SetTile(pos, tile);
                    }
                    else
                    {
                        HandleWallGeneration(tileName, generatedTilemap.CellToWorld(pos));
                    }
                }
            }
            generatedTilemap.transform.SetParent(grid.transform);
        }
        private void HandleWallGeneration(string tileName, Vector3 pos)
        {
            Sprite wallSprite = wallTextures.Find(item => item.name.ToLower().StartsWith(tileName));

            GameObject wallObject = Instantiate(wallGameObject);

            SpriteRenderer[] wallObjectSprites = wallObject.GetComponentsInChildren<SpriteRenderer>();
            foreach (var item in wallObjectSprites)
            {
                item.sprite = wallSprite;
            }

            pos = new Vector3(pos.x + xOffset, pos.y + yOffset, pos.z + zOffset);
            wallObject.transform.position = pos;

            wallsGenerated.Add(wallObject);
            wallObject.SetActive(true);
        }

        private void OnPreviewModeExit()
        {
            wallsGenerated.ForEach(wall =>
            {
                Destroy(wall);
            });
            wallsGenerated.Clear();
        }
    }
}
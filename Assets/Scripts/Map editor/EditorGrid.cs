using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace WEditor.Scenario
{
    public class EditorGrid : MonoBehaviour
    {
        public static EditorGrid instance;
        [SerializeField] Tilemap tilemap, previewTilemap, miscTilemap;
        [SerializeField] Tile gridTile, helperTile;
        [SerializeField] int width, height;
        [SerializeField] string folderAssetPath;
        [SerializeField] ScenarioGenerator scenarioGenerator;
        GeneratorInfo[,] grid;
        private Vector3Int currentWorldPos, currentWorldPos2;
        public Vector2 center { get => new Vector2((float)width / 2, (float)height / 2); }
        private void Start()
        {
            if (!instance) instance = this;
            else Destroy(this);

            grid = new GeneratorInfo[width, height];
            previewTilemap.size = new Vector3Int(width, height, 0);
            tilemap.size = new Vector3Int(width, height, 0);
            miscTilemap.size = new Vector3Int(width, height, 0);
            tilemap.BoxFill(Vector3Int.zero, gridTile, 0, 0, width, height);
        }
        public void SetTile(Vector2 pos, Tile tile)
        {
            Vector3Int cellPos = tilemap.WorldToCell(pos);

            if (tilemap.HasTile(cellPos))
            {
                if (!tile.name.ToLower().Contains("hud"))
                {
                    bool isGround = tile.name.ToLower().Contains("ground");

                    grid[cellPos.x, cellPos.y] = new GeneratorInfo(isGround, folderAssetPath + tile.name);
                    tilemap.SetTile(cellPos, tile);
                }
                else
                {
                    miscTilemap.SetTile(currentWorldPos2, null);
                    miscTilemap.SetTile(cellPos, tile);
                    currentWorldPos2 = cellPos;
                }
            }
        }
        public void SetPreviewTileOnAim(Vector2 pos)
        {
            Vector3Int cellPos = previewTilemap.WorldToCell(pos);
            if (tilemap.HasTile(cellPos))
            {
                previewTilemap.SetTile(currentWorldPos, gridTile);
                previewTilemap.SetTile(cellPos, helperTile);
                currentWorldPos = cellPos;
            }

        }
        public void Button_InitGeneration()
        {
            scenarioGenerator.InitGeneration(grid, tilemap);
        }
    }
    [System.Serializable]
    public struct GeneratorInfo
    {
        public GeneratorInfo(bool isGround, string assetPath)
        {
            this.isGround = isGround;
            this.assetPath = assetPath;
        }
        public bool isGround { get; set; }
        public string assetPath { get; set; }
    }
}
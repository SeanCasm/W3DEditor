using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace WEditor.Scenario
{
    public class EditorGrid : MonoBehaviour
    {
        public static EditorGrid instance;
        [SerializeField] Tilemap tilemap, previewTilemap;
        [SerializeField] Tile gridTile, helperTile;
        [SerializeField] int width, height;
        int[,] grid;
        private Vector3Int currentWorldPos;
        public Vector2 center { get => new Vector2(width / 2, height / 2); }
        private void Start()
        {
            if (!instance) instance = this;
            else Destroy(this);

            grid = new int[width, height];
            previewTilemap.size = new Vector3Int(width, height, 0);
            tilemap.size = new Vector3Int(width, height, 0);
            tilemap.BoxFill(Vector3Int.zero, gridTile, 0, 0, width, height);
        }
        public void SetTile(Vector2 pos, Tile tile)
        {
            if (tile != null)
            {
                Vector3Int cellPos = tilemap.WorldToCell(pos);
                tilemap.SetTile(cellPos, tile);
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

    }
}
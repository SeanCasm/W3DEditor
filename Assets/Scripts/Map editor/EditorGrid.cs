using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using WEditor.UI;
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
        [SerializeField] BoxCollider confinerCollider;
        public bool isSpawnLocated { get; private set; }
        private Vector3Int currentWorldPos, currentWorldPos2;
        public Vector2 center { get => new Vector2((float)width / 2, (float)height / 2); }
        private void Start()
        {
            if (!instance) instance = this;
            else Destroy(this);

            previewTilemap.size = new Vector3Int(width, height, 0);
            tilemap.size = new Vector3Int(width, height, 0);
            miscTilemap.size = new Vector3Int(width, height, 0);

            confinerCollider.size = new Vector3(tilemap.size.x, tilemap.size.y, 60);
            confinerCollider.transform.position = center;

            tilemap.BoxFill(Vector3Int.zero, gridTile, 0, 0, width, height);
            DataHandler.TileDataSize(width, height);
        }
        public void EraseTile(Vector3 pos)
        {
            Vector3Int cellPos = tilemap.WorldToCell(pos);
            tilemap.SetTile(cellPos, gridTile);
        }
        public void SetTile(Vector3 pos, Tile tile)
        {
            Vector3Int cellPos = tilemap.WorldToCell(pos);
            if (tilemap.HasTile(cellPos))
            {
                string nameToLower = tile.name.ToLower();
                if (!tile.name.ToLower().Contains("hud"))
                {
                    if (nameToLower.StartsWith("door"))
                    {
                        CheckWallsAroundDoorLocation(cellPos, tile);
                    }
                    else
                    {
                        tilemap.SetTile(cellPos, tile);
                    }
                }
                else
                {
                    string nameToLower2 = tilemap.GetTile(cellPos).name.ToLower();
                    //spawn point
                    if (nameToLower2 == "wall" || nameToLower2 == "props")
                    {
                        TextMessageHandler.instance.SP_PL();
                        return;
                    }
                    miscTilemap.SetTile(currentWorldPos2, null);
                    miscTilemap.SetTile(cellPos, tile);
                    currentWorldPos2 = cellPos;
                    isSpawnLocated = true;
                }
                DataHandler.SetTileData(cellPos.x, cellPos.y, new TileData(folderAssetPath + tile.name, cellPos));
            }
        }
        private void CheckWallsAroundDoorLocation(Vector3Int cellPos, Tile tile)
        {
            bool error = false;

            error = SetDoor(cellPos, tile);

            if (error) TextMessageHandler.instance.DB_GE();
        }
        public void SetPreviewTileOnAim(Vector3 pos)
        {
            Vector3Int cellPos = previewTilemap.WorldToCell(pos);
            if (tilemap.HasTile(cellPos))
            {
                previewTilemap.SetTile(currentWorldPos, gridTile);
                previewTilemap.SetTile(cellPos, helperTile);
                currentWorldPos = cellPos;
            }
        }
        private bool SetDoor(Vector3Int cellPos, Tile tile)
        {
            //Gets top and bottom tiles position in tilemap
            Vector3Int topPos = new Vector3Int(cellPos.x, cellPos.y + 1, cellPos.z);
            Vector3Int bottomPos = new Vector3Int(cellPos.x, cellPos.y - 1, cellPos.z);
            //Checks top and bottom tiles
            TileBase topTile = tilemap.GetTile(topPos);
            TileBase bottomTile = tilemap.GetTile(bottomPos);

            if (topTile.name.ToLower().StartsWith("wall") && bottomTile.name.ToLower().StartsWith("wall"))
            {
                tilemap.SetTile(cellPos, tile);
                return false;
            }

            Vector3Int leftPos = new Vector3Int(cellPos.x - 1, cellPos.y, cellPos.z);
            Vector3Int rightPos = new Vector3Int(cellPos.x + 1, cellPos.y, cellPos.z);
            //Checks left and right tiles
            TileBase leftTile = tilemap.GetTile(leftPos);
            TileBase rightTile = tilemap.GetTile(rightPos);

            if (leftTile.name.ToLower().StartsWith("wall") && rightTile.name.ToLower().StartsWith("wall"))
            {
                tilemap.SetTile(cellPos, tile);
                return false;
            }
            return true;
        }
        public void InitGeneration()
        {
            scenarioGenerator.InitGeneration(tilemap);
        }
    }

}
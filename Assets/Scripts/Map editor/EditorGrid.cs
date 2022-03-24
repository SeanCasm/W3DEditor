using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using WEditor.UI;
using WEditor.Events;
namespace WEditor.Scenario.Editor
{
    public class EditorGrid : MonoBehaviour
    {
        public static EditorGrid instance;
        [SerializeField] Tilemap groundWall, pointerPreview, prop, whiteSquare;
        [SerializeField] Tile gridTile, helperTile;
        [SerializeField] string folderAssetPath;
        [SerializeField] ScenarioGenerator scenarioGenerator;
        [SerializeField] BoxCollider confinerCollider;
        public bool isSpawnLocated { get; private set; }
        private Vector3Int currentWorldPos;
        private GameObject currentSpawn;
        private Vector3 spawnPosition;
        private int width, height;
        public string levelName { get; set; } = "";
        public string widthString { get; set; }
        public string heightString { get; set; }
        public Vector2 center { get => new Vector2((float)width / 2, (float)height / 2); }
        private void Start()
        {
            if (!instance) instance = this;
            else Destroy(this);
        }
        public void Button_Save()
        {
            if (levelName.Length == 0)
            {
                TextMessageHandler.instance.SetError("sl_ln");
                return;
            }
            else if (levelName.Contains("_"))
            {
                TextMessageHandler.instance.SetError("el_ic");
                return;
            }
            DataHandler.Save(levelName, groundWall.WorldToCell(spawnPosition));
        }
        public void Create(GameObject sizePanel)
        {
            width = int.Parse(widthString);
            height = int.Parse(heightString);

            if (width <= 0 || height <= 0)
            {
                TextMessageHandler.instance.SetError("cc_zl");
                return;
            }
            else if (width > 50 || height > 50)
            {
                TextMessageHandler.instance.SetError("cc_le");
                return;
            }

            pointerPreview.size = new Vector3Int(width, height, 0);
            groundWall.size = new Vector3Int(width, height, 0);
            prop.size = new Vector3Int(width, height, 0);
            whiteSquare.size = new Vector3Int(width, height, 0);

            confinerCollider.size = new Vector3(groundWall.size.x, groundWall.size.y, 60);
            confinerCollider.transform.position = center;

            whiteSquare.BoxFill(Vector3Int.zero, gridTile, 0, 0, width, height);
            DataHandler.TileDataSize(width, height);
            sizePanel.SetActive(false);

            GameEvent.instance.Create();
        }
        public void EraseTile(Vector3 pos)
        {
            Vector3Int cellPos = groundWall.WorldToCell(pos);
            groundWall.SetTile(cellPos, gridTile);
        }
        public void SetTile(Vector3 pos, Tile tile)
        {
            Vector3Int cellPos = groundWall.WorldToCell(pos);
            if (TileIsInsideTilemap(cellPos))
            {
                string nameToLower = tile.name.ToLower();
                if (!nameToLower.Contains("hud"))
                {
                    if (nameToLower.StartsWith("door"))
                    {
                        HandleDoorLocation(cellPos, tile);
                    }
                    else if (nameToLower.StartsWith("prop"))
                    {
                        HandlePropLocation(cellPos, tile);
                    }
                    else
                    {
                        groundWall.SetTile(cellPos, tile);
                    }
                }
                else
                {

                }
            }
        }
        public void SetSpawnObject(Vector3 pos, GameObject spawnPrefab)
        {
            Vector3Int cellPos = groundWall.WorldToCell(pos);
            string nameToLower2 = groundWall.HasTile(cellPos) ? groundWall.GetTile(cellPos).name.ToLower() : "non";
            //spawn point
            if (nameToLower2 != "non" && (nameToLower2 == "wall" || nameToLower2 == "props"))
            {
                TextMessageHandler.instance.SetError("sp_pl");
                return;
            }

            Destroy(currentSpawn);

            spawnPosition = pos;
            spawnPosition.Set(spawnPosition.x, spawnPosition.y + .5f, spawnPosition.z);

            pos = groundWall.CellToWorld(cellPos);
            //fix tile pivot
            pos = new Vector3(pos.x + .5f, pos.y, pos.z + .5f);
            currentSpawn = Instantiate(spawnPrefab, pos, Quaternion.Euler(90, 0, 0));
            isSpawnLocated = true;
        }
        private bool TileIsInsideTilemap(Vector3Int cellPos)
        {
            return cellPos.x >= 0 && cellPos.x <= width && cellPos.y >= 0 && cellPos.y <= height;
        }
        public void SetPreviewTileOnAim(Vector3 pos)
        {
            Vector3Int cellPos = pointerPreview.WorldToCell(pos);
            if (TileIsInsideTilemap(cellPos))
            {
                pointerPreview.SetTile(currentWorldPos, gridTile);
                pointerPreview.SetTile(cellPos, helperTile);
                currentWorldPos = cellPos;
            }
        }
        private void HandlePropLocation(Vector3Int cellPos, Tile tile)
        {
            if (groundWall.HasTile(cellPos))
            {
                TextMessageHandler.instance.SetError("pp_ll");
                return;
            }

            DataHandler.SetGroundWallTileData(cellPos.x, cellPos.y, new TileData(folderAssetPath + tile.name, cellPos));
        }
        private void HandleDoorLocation(Vector3Int cellPos, Tile tile)
        {
            //Gets top and bottom tiles position in tilemap
            Vector3Int topPos = new Vector3Int(cellPos.x, cellPos.y + 1, cellPos.z);
            Vector3Int bottomPos = new Vector3Int(cellPos.x, cellPos.y - 1, cellPos.z);

            //Checks top and bottom tiles
            TileBase topTile = groundWall.GetTile(topPos);
            TileBase bottomTile = groundWall.GetTile(bottomPos);

            if (topTile != null && bottomTile != null &&
                 topTile.name.ToLower().StartsWith("wall") && bottomTile.name.ToLower().StartsWith("wall"))
            {
                groundWall.SetTile(cellPos, tile);
            }

            Vector3Int leftPos = new Vector3Int(cellPos.x - 1, cellPos.y, cellPos.z);
            Vector3Int rightPos = new Vector3Int(cellPos.x + 1, cellPos.y, cellPos.z);

            //Checks left and right tiles
            TileBase leftTile = groundWall.GetTile(leftPos);
            TileBase rightTile = groundWall.GetTile(rightPos);

            if (leftTile != null && rightTile != null &&
                 leftTile.name.ToLower().StartsWith("wall") && rightTile.name.ToLower().StartsWith("wall"))
            {
                groundWall.SetTile(cellPos, tile);

            }
            DataHandler.SetPropDoorTileData(cellPos.x, cellPos.y, new TileData(folderAssetPath + tile.name, cellPos));
            TextMessageHandler.instance.SetError("db_ge");
        }
        public void InitGeneration()
        {
            scenarioGenerator.InitGeneration(spawnPosition);
        }
    }
}
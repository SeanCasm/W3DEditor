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
        [SerializeField] Tilemap mainGround, pointerPreview, prop, ground;
        [SerializeField] Tile gridTile, helperTile;
        [SerializeField] int width, height;
        [SerializeField] string folderAssetPath;
        [SerializeField] ScenarioGenerator scenarioGenerator;
        [SerializeField] BoxCollider confinerCollider;
        public bool isSpawnLocated { get; private set; }
        private Vector3Int currentWorldPos;
        private GameObject currentSpawn;
        private Vector3 spawnPosition;
        public Vector2 center { get => new Vector2((float)width / 2, (float)height / 2); }
        private void Start()
        {
            if (!instance) instance = this;
            else Destroy(this);

            pointerPreview.size = new Vector3Int(width, height, 0);
            mainGround.size = new Vector3Int(width, height, 0);
            prop.size = new Vector3Int(width, height, 0);
            ground.size = new Vector3Int(width, height, 0);

            confinerCollider.size = new Vector3(mainGround.size.x, mainGround.size.y, 60);
            confinerCollider.transform.position = center;

            ground.BoxFill(Vector3Int.zero, gridTile, 0, 0, width, height);
            DataHandler.TileDataSize(width, height);
        }
        public void EraseTile(Vector3 pos)
        {
            Vector3Int cellPos = mainGround.WorldToCell(pos);
            mainGround.SetTile(cellPos, gridTile);
        }
        public void SetTile(Vector3 pos, Tile tile)
        {
            Vector3Int cellPos = mainGround.WorldToCell(pos);
            if (TileIsInsideTilemap(cellPos))
            {
                string nameToLower = tile.name.ToLower();
                if (!nameToLower.Contains("hud"))
                {
                    if (nameToLower.StartsWith("door"))
                    {
                        CheckWallsAroundDoorLocation(cellPos, tile);
                    }
                    else if (nameToLower.StartsWith("prop"))
                    {
                        prop.SetTile(cellPos, tile);
                    }
                    else
                    {
                        mainGround.SetTile(cellPos, tile);
                    }
                }
                else
                {

                }
                DataHandler.SetTileData(cellPos.x, cellPos.y, new TileData(folderAssetPath + tile.name, cellPos));
            }
        }
        public void SetSpawnObject(Vector3 pos, GameObject spawnPrefab)
        {
            Vector3Int cellPos = mainGround.WorldToCell(pos);
            string nameToLower2 = mainGround.HasTile(cellPos) ? mainGround.GetTile(cellPos).name.ToLower() : "non";
            //spawn point
            if (nameToLower2 != "non" && (nameToLower2 == "wall" || nameToLower2 == "props"))
            {
                TextMessageHandler.instance.SP_PL();
                return;
            }

            Destroy(currentSpawn);

            spawnPosition = pos;
            spawnPosition.Set(spawnPosition.x, spawnPosition.y + .5f, spawnPosition.z);

            pos = mainGround.CellToWorld(cellPos);
            //fix tile pivot
            pos = new Vector3(pos.x + .5f, pos.y, pos.z + .5f);
            currentSpawn = Instantiate(spawnPrefab, pos, Quaternion.Euler(90, 0, 0));
            isSpawnLocated = true;
        }
        private bool TileIsInsideTilemap(Vector3Int cellPos)
        {
            return cellPos.x >= 0 && cellPos.x <= width && cellPos.y >= 0 && cellPos.y <= height;
        }
        private void CheckWallsAroundDoorLocation(Vector3Int cellPos, Tile tile)
        {
            bool error = false;

            error = SetDoor(cellPos, tile);

            if (error) TextMessageHandler.instance.DB_GE();
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
        private bool SetDoor(Vector3Int cellPos, Tile tile)
        {
            //Gets top and bottom tiles position in tilemap
            Vector3Int topPos = new Vector3Int(cellPos.x, cellPos.y + 1, cellPos.z);
            Vector3Int bottomPos = new Vector3Int(cellPos.x, cellPos.y - 1, cellPos.z);

            //Checks top and bottom tiles
            TileBase topTile = mainGround.GetTile(topPos);
            TileBase bottomTile = mainGround.GetTile(bottomPos);

            if (topTile != null && bottomTile != null &&
                 topTile.name.ToLower().StartsWith("wall") && bottomTile.name.ToLower().StartsWith("wall"))
            {
                mainGround.SetTile(cellPos, tile);
                return false;
            }

            Vector3Int leftPos = new Vector3Int(cellPos.x - 1, cellPos.y, cellPos.z);
            Vector3Int rightPos = new Vector3Int(cellPos.x + 1, cellPos.y, cellPos.z);

            //Checks left and right tiles
            TileBase leftTile = mainGround.GetTile(leftPos);
            TileBase rightTile = mainGround.GetTile(rightPos);

            if (leftTile != null && rightTile != null &&
                 leftTile.name.ToLower().StartsWith("wall") && rightTile.name.ToLower().StartsWith("wall"))
            {
                mainGround.SetTile(cellPos, tile);
                return false;
            }
            return true;
        }
        public void InitGeneration()
        {
            scenarioGenerator.InitGeneration(mainGround, prop, spawnPosition);
        }
    }
}
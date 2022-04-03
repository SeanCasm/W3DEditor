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
        [SerializeField] Tilemap mainTilemap, pointerPreview, whiteSquare;
        [SerializeField] Tile gridTile, helperTile;
        [SerializeField] ScenarioGenerator scenarioGenerator;
        [SerializeField] BoxCollider confinerCollider;
        [SerializeField] Transform editorCamera;
        [SerializeField] GameObject spawnPrefab;
        [Header("Level load settings")]
        [SerializeField] TMPro.TMP_InputField levelNameInputField;
        [SerializeField] GameObject loadScreen;
        [SerializeField] WEditor.Game.Scriptables.ScenarioScriptable wall, door, props, propsTop, health, score, ammo;
        public bool isSpawnLocated { get; private set; }
        private Vector3Int currentWorldPos;
        public GameObject currentSpawn { get; set; }
        private Vector3 spawnPosition;
        private int width, height;
        public string levelName { get; set; } = "";
        public string widthString { get; set; }
        public string heightString { get; set; }
        public Vector3 center { get => new Vector3((float)width / 2, 0, (float)height / 2); }
        private void Start()
        {
            if (!instance) instance = this;
            else Destroy(this);
        }
        private void OnEnable()
        {
            GameEvent.instance.onEditorExit += Clear;
        }
        private void OnDisable()
        {
            GameEvent.instance.onEditorExit -= Clear;
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
                TextMessageHandler.instance.SetError("sl_ic");
                return;
            }
            else if (!isSpawnLocated)
            {
                TextMessageHandler.instance.SetError("sl_sp");
                return;
            }

            SaveData.SaveToLocal(levelName, mainTilemap.WorldToCell(spawnPosition), (width, height));
        }
        public void Load(GameData gameData)
        {
            DataHandler.LevelTileSize(gameData.levelSize.x, gameData.levelSize.y);
            levelName = gameData.levelName;
            levelNameInputField.text = levelName;
            List<(int, int, int, string)> doors = new List<(int, int, int, string)>();

            width = gameData.levelSize.x;
            height = gameData.levelSize.y;

            SetSpawnObject(new Vector3(gameData.levelSpawn.x, 0, gameData.levelSpawn.z));
            InitLevel();

            gameData.levelTiles.ForEach(item =>
            {
                Tile tile = new Tile();
                (int index, int x, int y, string tileName) = item;
                Vector3Int cellPos = new Vector3Int(x, y, 0);
                tile.name = tileName;

                if (tileName.Contains("wall"))
                {
                    tile.sprite = wall.spritesCollection[index];
                    SetTile(cellPos, tile);
                }
                else if (tileName.Contains("prop"))
                {
                    tile.sprite = props.spritesCollection[index];
                    SetTile(cellPos, tile);
                }
                else if (tileName.Contains("door"))
                {
                    doors.Add(item);
                }
                else if (tileName.Contains("health"))
                {
                    tile.sprite = health.spritesCollection[index];
                    SetTile(cellPos, tile);
                }
            });
            doors.ForEach(item =>
            {
                Tile tile = new Tile();
                (int index, int x, int y, string tileName) = item;
                tile.name = tileName;
                Vector3Int cellPos = new Vector3Int(x, y, 0);

                tile.sprite = door.spritesCollection[index];
                SetTile(cellPos, tile);
            });
            loadScreen.SetActive(false);

        }
        public void Create()
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
            InitLevel();
        }
        private void InitLevel()
        {
            Vector3Int size = new Vector3Int(width, height, 0);
            whiteSquare.size = mainTilemap.size = pointerPreview.size = size;

            confinerCollider.size = new Vector3(mainTilemap.size.x, 100, mainTilemap.size.y);
            confinerCollider.transform.position = new Vector3(center.x, center.y + confinerCollider.size.y / 2, center.z);

            editorCamera.position = new Vector3(center.x, editorCamera.position.y, center.z);

            whiteSquare.BoxFill(Vector3Int.zero, gridTile, 0, 0, width, height);
            DataHandler.LevelTileSize(width, height);
            GameEvent.instance.EditorEnter();
        }
        public void EraseTile(Vector3 pos)
        {
            Vector3Int cellPos = mainTilemap.WorldToCell(pos);
            mainTilemap.SetTile(cellPos, gridTile);
        }
        public void SetTile(Vector3Int cellPos, Tile tile)
        {
            HandleSetTile(cellPos, tile);
        }
        public void SetTile(Vector3 pos, Tile tile)
        {
            Vector3Int cellPos = mainTilemap.WorldToCell(pos);
            HandleSetTile(cellPos, tile);
        }
        private void HandleSetTile(Vector3Int cellPos, Tile tile)
        {
            if (IsTileInsideTilemap(cellPos))
            {
                string nameToLower = tile.name.ToLower();
                if (nameToLower.Contains("door"))
                {
                    HandleDoorLocation(cellPos, tile);
                }
                else if (nameToLower.Contains("prop"))
                {
                    HandlePropLocation(cellPos, tile);
                }
                else if (nameToLower.Contains("wall"))
                {
                    mainTilemap.SetTile(cellPos, tile);
                }
                else
                {
                    HandleCollectibleLocation(cellPos, tile);
                }
                string[] index = nameToLower.Split('_');
                DataHandler.SetLevelTiles(cellPos.x, cellPos.y, new TileLevelData(int.Parse(index[index.Length - 1]), cellPos, nameToLower));
            }
        }
        private void HandleCollectibleLocation(Vector3Int cellPos, Tile tile)
        {
            bool hasTile = mainTilemap.HasTile(cellPos);
            string tilePlacedName = hasTile ? mainTilemap.GetTile(cellPos).name : "n";
            if (hasTile && tilePlacedName.Contains("door") || tilePlacedName.Contains("wall"))
            {
                TextMessageHandler.instance.SetError("ci_ll");
                return;
            }
            mainTilemap.SetTile(cellPos, tile);
        }
        public void SetSpawnObject(Vector3 pos)
        {
            Vector3Int cellPos = mainTilemap.WorldToCell(pos);
            string nameToLower2 = mainTilemap.HasTile(cellPos) ? mainTilemap.GetTile(cellPos).name.ToLower() : "non";
            //spawn point
            if (nameToLower2 != "non")
            {
                TextMessageHandler.instance.SetError("sp_pl");
                return;
            }
            else if (!IsTileInsideTilemap(cellPos))
            {
                return;
            }

            Destroy(currentSpawn);

            spawnPosition = pos;
            spawnPosition.Set(spawnPosition.x, spawnPosition.y + .5f, spawnPosition.z);

            pos = mainTilemap.CellToWorld(cellPos);
            //fix tile pivot
            currentSpawn = Instantiate(spawnPrefab, pos.FixTilePivot(), Quaternion.Euler(90, 0, 0));
            isSpawnLocated = true;
        }
        private bool IsTileInsideTilemap(Vector3Int cellPos)
        {
            return cellPos.x >= 0 && cellPos.x < width && cellPos.y >= 0 && cellPos.y < height;
        }
        public void SetPreviewTileOnAim(Vector3 pos)
        {
            Vector3Int cellPos = pointerPreview.WorldToCell(pos);
            if (IsTileInsideTilemap(cellPos))
            {
                pointerPreview.SetTile(currentWorldPos, gridTile);
                pointerPreview.SetTile(cellPos, helperTile);
                currentWorldPos = cellPos;
            }
        }
        private void HandlePropLocation(Vector3Int cellPos, Tile tile)
        {
            if (mainTilemap.HasTile(cellPos))
            {
                TextMessageHandler.instance.SetError("pp_ll");
                return;
            }
            mainTilemap.SetTile(cellPos, tile);
        }
        private void HandleDoorLocation(Vector3Int cellPos, Tile tile)
        {
            bool tilesAround = false;
            //Gets top and bottom tiles position in tilemap
            Vector3Int topPos = new Vector3Int(cellPos.x, cellPos.y + 1, cellPos.z);
            Vector3Int bottomPos = new Vector3Int(cellPos.x, cellPos.y - 1, cellPos.z);

            //Checks top and bottom tiles
            TileBase topTile = mainTilemap.GetTile(topPos);
            TileBase bottomTile = mainTilemap.GetTile(bottomPos);

            Vector3Int leftPos = new Vector3Int(cellPos.x - 1, cellPos.y, cellPos.z);
            Vector3Int rightPos = new Vector3Int(cellPos.x + 1, cellPos.y, cellPos.z);

            //Checks left and right tiles
            TileBase leftTile = mainTilemap.GetTile(leftPos);
            TileBase rightTile = mainTilemap.GetTile(rightPos);

            if ((topTile != null && bottomTile != null &&
                 topTile.name.ToLower().StartsWith("wall") && bottomTile.name.ToLower().StartsWith("wall")) ||
                 (leftTile != null && rightTile != null &&
                 leftTile.name.ToLower().StartsWith("wall") && rightTile.name.ToLower().StartsWith("wall")))
            {
                mainTilemap.SetTile(cellPos, tile);
                tilesAround = true;
            }

            if (!tilesAround)
            {
                TextMessageHandler.instance.SetError("dg_ge");
                return;
            }
        }
        private void Clear()
        {
            mainTilemap.ClearAllTiles();
            whiteSquare.ClearAllTiles();
            pointerPreview.ClearAllTiles();
            DataHandler.ClearLevelTiles();
            Destroy(currentSpawn);
            isSpawnLocated = false;
            spawnPosition = Vector3.zero;
            levelName = "";
        }
        public void InitGeneration()
        {
            DataHandler.currentLevelPosition = spawnPosition;
            currentSpawn.SetActive(false);
            scenarioGenerator.InitGeneration();
        }
    }
    public struct TileLevelData
    {
        public TileLevelData(int assetListIndex, Vector3Int position, string tileName)
        {
            this.assetListIndex = assetListIndex;
            this.position = position;
            this.tileName = tileName;
        }
        public string tileName { get; private set; }
        public int assetListIndex { get; private set; }
        public Vector3Int position { get; private set; }

    }
}
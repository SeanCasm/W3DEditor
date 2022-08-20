using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using WEditor.UI;
using WEditor.Events;
using WEditor.Game.Scriptables;
using WEditor.Utils;
using WEditor.Input;

namespace WEditor.Scenario.Editor
{
    public class EditorGrid : MonoBehaviour
    {
        public static EditorGrid instance;
        [SerializeField] ElevatorPosition elevatorGeneration;
        [SerializeField] Tilemap mainTilemap, pointerPreview, whiteSquare;
        [SerializeField] Sprite gridSprite, helperSprite, eraserSprite;
        [SerializeField] ScenarioGenerator scenarioGenerator;
        [SerializeField] BoxCollider confinerCollider;
        [SerializeField] Transform editorCamera;
        [SerializeField] GameObject spawnPrefab;
        [Header("Level load settings")]
        [SerializeField] TMPro.TMP_InputField levelNameInputField;
        [SerializeField] GameObject loadScreen;
        [SerializeField] ScenarioScriptable wall, door, props;
        [Header("Enemy scriptables")]
        [SerializeField] List<EnemyScriptable> enemies;
        public bool isSpawnLocated { get; private set; }
        private Vector3Int currentWorldPos;
        public GameObject currentSpawn { get; set; }
        private Vector3 spawnPosition;
        private int width, height;
        public string levelName { get; set; } = "";
        private bool HasTile(Vector3Int cellPos) => mainTilemap.HasTile(cellPos);
        public Vector3 center => new Vector3((float)width / 2, 0, (float)height / 2);
        private void Start()
        {
            if (!instance) instance = this;
            else Destroy(this);
        }
        private void OnEnable()
        {
            EditorEvent.instance.onEditorExit += Clear;
            EditorEvent.instance.onElevatorPlacementFailed += EraseTile;
            EditorEvent.instance.onPreviewModeEnter += PreviewEnter;
            EditorEvent.instance.onPreviewModeExit += PreviewExit;
        }
        private void OnDisable()
        {
            EditorEvent.instance.onEditorExit -= Clear;
            EditorEvent.instance.onElevatorPlacementFailed -= EraseTile;
            EditorEvent.instance.onPreviewModeEnter -= PreviewEnter;
            EditorEvent.instance.onPreviewModeExit -= PreviewExit;
        }
        public void Button_Save()
        {
            if (levelName.Length == 0)
            {
                MessageHandler.instance.SetError("level_name");
                return;
            }
            else if (levelName.Contains("_"))
            {
                MessageHandler.instance.SetError("level_name_uc");
                return;
            }
            else if (!isSpawnLocated)
            {
                MessageHandler.instance.SetError("level_spawn_r");
                return;
            }
            DataHandler.currentLevelName = levelName;
            SaveData.SaveToLocal();
        }
        public void Load(GameData gameData)
        {
            width = gameData.levelSizeX;
            height = gameData.levelSizeY;

            DataHandler.GridSize(new Vector3Int(width, height, 0));
            DataHandler.levelGuns = gameData.levelGunsToArray;

            levelName = gameData.levelName;
            levelNameInputField.text = levelName;
            List<(int, int, string)> doors = new List<(int, int, string)>();


            SetSpawnObject(new Vector3(gameData.levelSpawnX, 0, gameData.levelSpawnZ));
            InitLevel();
            for (int pos = 0; pos < gameData.levelTiles.Count; pos++)
            {
                int x = gameData.levelTilesX[pos];
                int y = gameData.levelTilesY[pos];
                Tile tile = ScriptableObject.CreateInstance("Tile") as Tile;
                Vector3Int cellPos = new Vector3Int(x, y, 0);
                tile.name = gameData.levelTiles[pos];

                if (tile.name.Contains("Wall"))
                {
                    tile.sprite = wall.GetSprite(tile.name);
                    mainTilemap.SetTile(cellPos, tile);
                    DataHandler.SetGrid(cellPos, new EditorGridLevelData(cellPos, tile.name));
                }
                else if (tile.name.Contains("Ground"))
                {
                    tile.sprite = props.GetSprite(tile.name);
                    mainTilemap.SetTile(cellPos, tile);
                    DataHandler.SetGrid(cellPos, new EditorGridLevelData(cellPos, tile.name));
                }
                else if (tile.name.Contains("Door"))
                {
                    tile.sprite = door.GetSprite(tile.name);
                    mainTilemap.SetTile(cellPos, tile);
                    DataHandler.SetGrid(cellPos, new EditorGridLevelData(cellPos, tile.name));
                }
                else
                {
                    enemies.ForEach(item =>
                    {
                        if (tile.name.Contains(item.enemyName))
                        {
                            tile.sprite = item.enemySprite;
                            mainTilemap.SetTile(cellPos, tile);
                            DataHandler.SetGrid(cellPos, new EditorGridLevelData(cellPos, tile.name));
                        }
                    });
                }

                loadScreen.SetActive(false);
            }
        }
        public void Create(int width, int height)
        {
            this.width = width;
            this.height = height;

            if (width <= 0 || height <= 0)
            {
                MessageHandler.instance.SetError("level_size_l");
                return;
            }
            else if (width > 50 || height > 50)
            {
                MessageHandler.instance.SetError("level_size_u");
                return;
            }
            InitLevel();
        }
        private void InitLevel()
        {
            Vector3Int size = new Vector3Int(width, height, 0);
            whiteSquare.size = mainTilemap.size = pointerPreview.size = size;
            Vector3 center = new Vector3(mainTilemap.size.x / 2, 0, mainTilemap.size.y / 2);
            confinerCollider.size = new Vector3(mainTilemap.size.x, 100, mainTilemap.size.y);
            confinerCollider.transform.position = new Vector3(center.x, center.y + confinerCollider.size.y / 2, center.z);

            editorCamera.position = new Vector3(center.x, editorCamera.position.y, center.z);

            whiteSquare.BoxFill(Vector3Int.zero, CreateTile(gridSprite), 0, 0, width, height);
            DataHandler.GridSize(new Vector3Int(width, height, 0));
            EditorEvent.instance.EditorEnter();
        }

        public void EraseTile(Vector3Int cellPos)
        {
            if (!HasTile(cellPos)) return;

            DataHandler.SetGrid(cellPos, null);
            TileBase tileToErase = mainTilemap.GetTile(cellPos);

            if (tileToErase.name.Contains("_elv") || tileToErase.name.Contains("_end"))
            {
                elevatorGeneration.Delete();
            }

            if (tileToErase.name.Contains("Wall") || tileToErase.name.Contains("_elv") ||
            tileToErase.name.Contains("_end"))
            {
                //Delete any door near
                EraseDoorTile(cellPos.GetLeft());
                EraseDoorTile(cellPos.GetRight());
                EraseDoorTile(cellPos.GetBottom());
                EraseDoorTile(cellPos.GetTop());
            }

            mainTilemap.SetTile(cellPos, null);

            void EraseDoorTile(Vector3Int cellPos)
            {
                if (HasTile(cellPos) && (mainTilemap.GetTile(cellPos).name.Contains("Door")
                || mainTilemap.GetTile(cellPos).name.Contains("_elv")))
                {
                    mainTilemap.SetTile(cellPos, null);
                    DataHandler.SetGrid(cellPos, null);
                    return;
                }
            }
        }
        public void SetTile(Vector3Int cellPos, Tile tile)
        {

            if (!InsideTilemap(cellPos) || tile == null)
                return;

            if (tile.name.StartsWith("Door"))
            {
                if (tile.name.EndsWith("_end"))
                    ElevatorPlacement(cellPos, tile);
                else
                    DoorPlacement(cellPos, tile);
            }
            else if (tile.name.StartsWith("Ground"))
            {
                if (tile.name.Contains("health") || tile.name.Contains("ammo") || tile.name.Contains("score"))
                    CollectiblePlacement(cellPos, tile);
                else
                    PropPlacement(cellPos, tile);

            }
            else
            {
                mainTilemap.SetTile(cellPos, tile);
                DataHandler.SetGrid(cellPos, new EditorGridLevelData(cellPos, tile.name));
            }
        }

        private void CollectiblePlacement(Vector3Int cellPos, Tile tile)
        {
            bool hasTile = HasTile(cellPos);
            string tilePlacedName = hasTile ? mainTilemap.GetTile(cellPos).name : "n";
            if (hasTile && tilePlacedName.Contains("Door") || tilePlacedName.Contains("Wall"))
            {
                MessageHandler.instance.SetError("grid_prop_l");
                return;
            }
            mainTilemap.SetTile(cellPos, tile);
            DataHandler.SetGrid(cellPos, new EditorGridLevelData(cellPos, tile.name));
        }
        public void SetSpawnObject(Vector3 pos)
        {
            Vector3Int cellPos = mainTilemap.WorldToCell(pos);
            string tileName = HasTile(cellPos) ? mainTilemap.GetTile(cellPos).name : "non";
            //spawn point
            if (tileName != "non")
            {
                MessageHandler.instance.SetError("level_spawn_l");
                return;
            }
            else if (!InsideTilemap(cellPos))
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
            DataHandler.spawnPosition = Vector3Int.FloorToInt(spawnPosition);
        }
        private bool InsideTilemap(Vector3Int cellPos) => (cellPos.x >= 0 && cellPos.x < width && cellPos.y >= 0 && cellPos.y < height);
        public void SetEraserTileOnAim(Vector3 pos)
        {
            Vector3Int cellPos = pointerPreview.WorldToCell(pos);
            if (InsideTilemap(cellPos))
            {
                pointerPreview.SetTile(currentWorldPos, CreateTile(gridSprite));
                pointerPreview.SetTile(cellPos, CreateTile(eraserSprite));
                currentWorldPos = cellPos;
            }
        }
        public void SetPreviewTileOnAim(Vector3 pos)
        {
            Vector3Int cellPos = pointerPreview.WorldToCell(pos);
            if (InsideTilemap(cellPos))
            {
                pointerPreview.SetTile(currentWorldPos, CreateTile(gridSprite));
                pointerPreview.SetTile(cellPos, CreateTile(helperSprite));
                currentWorldPos = cellPos;
            }
        }
        private Tile CreateTile(Sprite sprite)
        {
            Tile tile = ScriptableObject.CreateInstance("Tile") as Tile;
            tile.sprite = sprite;
            return tile;
        }

        private void PropPlacement(Vector3Int cellPos, Tile tile)
        {
            if (HasTile(cellPos) && (tile.name.Contains("Wall") || tile.name.Contains("Door")))
            {
                MessageHandler.instance.SetError("grid_prop_l");
                return;
            }
            mainTilemap.SetTile(cellPos, tile);
            DataHandler.SetGrid(cellPos, new EditorGridLevelData(cellPos, tile.name));
        }

        private void ElevatorPlacement(Vector3Int cellPos, Tile tile)
        {
            bool doorPlaced = DoorPlacement(cellPos, tile);

            if (doorPlaced)
                elevatorGeneration.EnablePanel(mainTilemap, cellPos);
        }

        private bool DoorPlacement(Vector3Int cellPos, Tile tile)
        {
            bool tilePlaced = false;

            if ((DataHandler.CheckForWall(cellPos.GetTop()) && DataHandler.CheckForWall(cellPos.GetBottom())) ||
                (DataHandler.CheckForWall(cellPos.GetRight()) && DataHandler.CheckForWall(cellPos.GetLeft())))
            {
                mainTilemap.SetTile(cellPos, tile);
                DataHandler.SetGrid(cellPos, new EditorGridLevelData(cellPos, tile.name));
                tilePlaced = true;
            }

            if (!tilePlaced)
                MessageHandler.instance.SetError("grid_door");

            return tilePlaced;
        }
        private void PreviewEnter()
        {
            whiteSquare.gameObject.SetActive(false);
            currentSpawn.SetActive(false);
        }
        private void PreviewExit()
        {
            whiteSquare.gameObject.SetActive(true);
            currentSpawn.SetActive(true);
        }
        private void Clear()
        {
            mainTilemap.ClearAllTiles();
            whiteSquare.ClearAllTiles();
            pointerPreview.ClearAllTiles();
            DataHandler.ClearGrid();
            Destroy(currentSpawn);
            isSpawnLocated = false;
            whiteSquare.gameObject.SetActive(true);
            spawnPosition = Vector3.zero;
            levelName = "";
        }
        public void InitGeneration()
        {
            DataHandler.currentLevelPosition = spawnPosition;
            DataHandler.tileMap = mainTilemap;
            scenarioGenerator.InitGeneration();
        }
    }
}
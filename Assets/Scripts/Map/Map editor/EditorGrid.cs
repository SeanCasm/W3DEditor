using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using WEditor.UI;
using WEditor.Events;
using WEditor.Game.Scriptables;
namespace WEditor.Scenario.Editor
{
    public class EditorGrid : MonoBehaviour
    {
        public static EditorGrid instance;
        [SerializeField] Tilemap mainTilemap, pointerPreview, whiteSquare;
        [SerializeField] Sprite gridSprite, helperSprite, eraserSprite;
        [SerializeField] ScenarioGenerator scenarioGenerator;
        [SerializeField] BoxCollider confinerCollider;
        [SerializeField] Transform editorCamera;
        [SerializeField] GameObject spawnPrefab;
        [SerializeField] Behaviour lightComponent;
        [Header("Level load settings")]
        [SerializeField] TMPro.TMP_InputField levelNameInputField;
        [SerializeField] GameObject loadScreen;
        [SerializeField] ScenarioScriptable wall, door, props, propsTop, health, score, ammo;
        [Header("Enemy scriptables")]
        [SerializeField] List<EnemyScriptable> enemies;
        public bool isSpawnLocated { get; private set; }
        private Vector3Int currentWorldPos;
        public GameObject currentSpawn { get; set; }
        private Vector3 spawnPosition;
        private int width, height;
        public string levelName { get; set; } = "";
        public Vector3 center { get => new Vector3((float)width / 2, 0, (float)height / 2); }
        private void Start()
        {
            if (!instance) instance = this;
            else Destroy(this);
        }
        private void OnEnable()
        {
            GameEvent.instance.onEditorExit += Clear;
            GameEvent.instance.onPreviewModeEnter += PreviewEnter;
            GameEvent.instance.onPreviewModeExit += PreviewExit;
        }
        private void OnDisable()
        {
            GameEvent.instance.onEditorExit -= Clear;
            GameEvent.instance.onPreviewModeEnter -= PreviewEnter;
            GameEvent.instance.onPreviewModeExit -= PreviewExit;
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
            width = gameData.levelSize.x;
            height = gameData.levelSize.y;

            DataHandler.GridSize(width, height);
            levelName = gameData.levelName;
            levelNameInputField.text = levelName;
            List<(int, int, string)> doors = new List<(int, int, string)>();


            SetSpawnObject(new Vector3(gameData.levelSpawn.x, 0, gameData.levelSpawn.z));
            InitLevel();
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    if (gameData.levelTiles[x, y] == null)
                        continue;

                    Tile tile = new Tile();
                    Vector3Int cellPos = new Vector3Int(x, y, 0);
                    tile.name = gameData.levelTiles[x, y];

                    if (tile.name.Contains("wall"))
                    {
                        tile.sprite = wall.GetSprite(tile.name);
                        SetTile(cellPos, tile);
                    }
                    else if (tile.name.Contains("prop"))
                    {
                        tile.sprite = props.GetSprite(tile.name);
                        SetTile(cellPos, tile);
                    }
                    else if (tile.name.Contains("door"))
                    {
                        doors.Add((x, y, tile.name));
                    }
                    else if (tile.name.Contains("health"))
                    {
                        tile.sprite = health.GetSprite(tile.name);
                        SetTile(cellPos, tile);
                    }
                    else
                    {
                        enemies.ForEach(item =>
                        {
                            if (tile.name.Contains(item.enemyName))
                            {
                                tile.sprite = item.enemySprite;
                                SetTile(cellPos, tile);
                            }
                        });
                    }
                }
            }

            doors.ForEach(item =>
            {
                Tile tile = new Tile();
                (int x, int y, string tileName) = item;
                tile.name = tileName;
                Vector3Int cellPos = new Vector3Int(x, y, 0);

                tile.sprite = door.GetSprite(tileName);
                SetTile(cellPos, tile);
            });
            loadScreen.SetActive(false);

        }
        public void Create(int width, int height)
        {
            this.width = width;
            this.height = height;

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
            Vector3 center = new Vector3(mainTilemap.size.x / 2, 0, mainTilemap.size.y / 2);
            confinerCollider.size = new Vector3(mainTilemap.size.x, 100, mainTilemap.size.y);
            confinerCollider.transform.position = new Vector3(center.x, center.y + confinerCollider.size.y / 2, center.z);

            editorCamera.position = new Vector3(center.x, editorCamera.position.y, center.z);

            whiteSquare.BoxFill(Vector3Int.zero, CreateTile(gridSprite), 0, 0, width, height);
            DataHandler.GridSize(width, height);
            GameEvent.instance.EditorEnter();
        }
        public void EraseTile(Vector3 pos)
        {
            Vector3Int cellPos = mainTilemap.WorldToCell(pos);
            mainTilemap.SetTile(cellPos, null);
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
                else if (nameToLower.Contains("health") || nameToLower.Contains("ammo"))
                {
                    HandleCollectibleLocation(cellPos, tile);
                }
                else
                {
                    mainTilemap.SetTile(cellPos, tile);
                }
                DataHandler.SetGrid(cellPos.x, cellPos.y, new EditorGridLevelData(cellPos, nameToLower));
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
        public void SetEraserTileOnAim(Vector3 pos)
        {
            Vector3Int cellPos = pointerPreview.WorldToCell(pos);
            if (IsTileInsideTilemap(cellPos))
            {
                pointerPreview.SetTile(currentWorldPos, CreateTile(gridSprite));
                pointerPreview.SetTile(cellPos, CreateTile(eraserSprite));
                currentWorldPos = cellPos;
            }
        }
        public void SetPreviewTileOnAim(Vector3 pos)
        {
            Vector3Int cellPos = pointerPreview.WorldToCell(pos);
            if (IsTileInsideTilemap(cellPos))
            {
                pointerPreview.SetTile(currentWorldPos, CreateTile(gridSprite));
                pointerPreview.SetTile(cellPos, CreateTile(helperSprite));
                currentWorldPos = cellPos;
            }
        }
        private Tile CreateTile(Sprite sprite)
        {
            Tile tile = new Tile();
            tile.sprite = sprite;
            return tile;
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

            //Checks top and bottom tiles
            TileBase topTile = mainTilemap.GetTile(cellPos.GetTopTile());
            TileBase bottomTile = mainTilemap.GetTile(cellPos.GetBottomTile());

            //Checks left and right tiles
            TileBase leftTile = mainTilemap.GetTile(cellPos.GetLeftTile());
            TileBase rightTile = mainTilemap.GetTile(cellPos.GetRightTile());

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
        private void PreviewEnter()
        {
            lightComponent.enabled = true;
            whiteSquare.gameObject.SetActive(false);
            currentSpawn.SetActive(false);
        }
        private void PreviewExit()
        {
            lightComponent.enabled = false;
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
            scenarioGenerator.InitGeneration(mainTilemap);
        }
    }
}
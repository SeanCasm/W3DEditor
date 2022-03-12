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
        [SerializeField] List<Sprite> propsSprites;
        [Header("Wall generation")]
        [SerializeField] float xOffset, yOffset, zOffset;
        [SerializeField] GameObject wallGameObject;
        [SerializeField] List<Sprite> wallTextures;
        [Header("Door generation")]
        [SerializeField] List<Sprite> doorSprites;
        [SerializeField] GameObject doorPrefab;
        [SerializeField] float xDoorOffset1 = .5f, xDoorOffset2, zDoorOffset1 = 1, zDoorOffset2;

        private List<Door> doorsLocation = new List<Door>();
        private List<GameObject> objectsGenerated = new List<GameObject>();
        private void OnEnable()
        {
            GameEvent.instance.onPreviewModeExit += OnPreviewModeExit;
        }
        private void OnDisable()
        {
            GameEvent.instance.onPreviewModeExit -= OnPreviewModeExit;
        }
        public void InitGeneration(Tilemap tilemap)
        {
            generatedTilemap.size = tilemap.size;

            generatedTilemap.transform.position = tilemap.transform.position;

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
                    else if (tileName.StartsWith("door"))
                    {
                        AddDoorToList(pos, tilemap, tileName);
                    }
                    else if (tileName.StartsWith("wall"))
                    {
                        HandleWallGeneration(tileName, generatedTilemap.CellToWorld(pos));
                    }
                }
            }
            HandleDoorsGeneration(tilemap);
            generatedTilemap.transform.SetParent(grid.transform);
        }
        private void AddDoorToList(Vector3Int cellPos, Tilemap tilemap, string tileName)
        {
            Vector3Int topPos = new Vector3Int(cellPos.x, cellPos.y + 1, cellPos.z);
            Vector3Int bottomPos = new Vector3Int(cellPos.x, cellPos.y - 1, cellPos.z);
            if (tilemap.HasTile(topPos) && tilemap.HasTile(bottomPos))
            {
                doorsLocation.Add(new Door(cellPos, true, tileName));
                return;
            }

            Vector3Int leftPos = new Vector3Int(cellPos.x - 1, cellPos.y, cellPos.z);
            Vector3Int rightPos = new Vector3Int(cellPos.x + 1, cellPos.y, cellPos.z);
            if (tilemap.HasTile(leftPos) && tilemap.HasTile(rightPos))
            {
                doorsLocation.Add(new Door(cellPos, false, tileName));
            }
        }

        private void HandleDoorsGeneration(Tilemap tilemap)
        {
            foreach (var door in doorsLocation)
            {
                if (tilemap.GetTile(door.position))
                {
                    Sprite doorSprite = doorSprites.Find(item => item.name.ToLower().StartsWith(door.name));
                    GameObject doorObject = Instantiate(doorPrefab);
                    doorObject.GetComponent<SpriteRenderer>().sprite = doorSprite;
                    Vector3 position = Vector3.zero;
                    if (door.topBottomSide)
                    {
                        position = new Vector3(door.position.x + xDoorOffset2, 0, door.position.y + zDoorOffset2);
                    }
                    else
                    {
                        position = new Vector3(door.position.x + xDoorOffset1, 0, door.position.y + zDoorOffset1);
                    }
                    doorObject.transform.position = position;
                    objectsGenerated.Add(doorObject);
                }
            }
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

            objectsGenerated.Add(wallObject);
            wallObject.SetActive(true);
        }

        private void OnPreviewModeExit()
        {
            doorsLocation.Clear();

            objectsGenerated.ForEach(wall =>
            {
                Destroy(wall);
            });
            objectsGenerated.Clear();
        }
    }
    public struct Door
    {
        public Door(Vector3Int position, bool topBottomSide, string name)
        {
            this.position = position;
            this.topBottomSide = topBottomSide;
            this.name = name;
        }
        public Vector3Int position { get; private set; }
        public bool topBottomSide { get; private set; }
        public string name { get; private set; }
    }
}
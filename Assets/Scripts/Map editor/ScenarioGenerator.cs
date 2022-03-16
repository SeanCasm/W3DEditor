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
        [SerializeField] Tilemap wallGroundTilemap;
        [SerializeField] float xOffset, yOffset, zOffset;
        [Header("Wall generation")]
        [SerializeField] GameObject wallGameObject;
        [SerializeField] List<Sprite> wallTextures;
        [SerializeField] Sprite wallFacingDoor;
        [Header("Door generation")]
        [SerializeField] List<Sprite> doorSprites;
        [SerializeField] GameObject doorPrefab;
        [SerializeField] float xDoorOffset1 = .5f, xDoorOffset2, zDoorOffset1 = 1, zDoorOffset2;
        [Header("Prop generation")]
        [SerializeField] GameObject propPrefab;
        [SerializeField] List<Sprite> propsDefaultSprites, propsTopSprites;
        [SerializeField] float propXOffset, propYOffset, propZOffset;

        private List<Door> doorsLocation = new List<Door>();
        private List<GameObject> objectsGenerated = new List<GameObject>();
        private List<Wall> walls = new List<Wall>();
        private void OnEnable()
        {
            GameEvent.instance.onPreviewModeExit += OnPreviewModeExit;
        }
        private void OnDisable()
        {
            GameEvent.instance.onPreviewModeExit -= OnPreviewModeExit;
        }
        public void InitGeneration(Tilemap tilemap, Tilemap props)
        {

            wallGroundTilemap.size = tilemap.size;

            wallGroundTilemap.transform.position = tilemap.transform.position;

            for (int x = 0; x < wallGroundTilemap.size.x; x++)
            {
                for (int y = 0; y < wallGroundTilemap.size.y; y++)
                {

                    Vector3Int pos = new Vector3Int(x, y, 0);

                    if (tilemap.HasTile(pos))
                    {
                        TileBase tile = tilemap.GetTile(pos);
                        string tileName = tile.name.ToLower();

                        if (tileName.StartsWith("ground"))
                        {
                            wallGroundTilemap.SetTile(pos, tile);
                        }
                        else if (tileName.StartsWith("door"))
                        {
                            AddDoorToList(pos, tilemap, tileName);
                        }
                        else if (tileName.StartsWith("wall"))
                        {
                            HandleWallGeneration(tileName, pos);
                        }
                    }

                    if (props.HasTile(pos))
                    {
                        TileBase tileProp = props.GetTile(pos);
                        string propTileName = tileProp.name.ToLower();

                        if (propTileName.StartsWith("props"))
                            HandlePropGeneration(propTileName, pos);
                    }
                }
            }
            HandleDoorsGeneration(tilemap);
            wallGroundTilemap.transform.SetParent(grid.transform);
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
                        position = new Vector3(door.position.x + xDoorOffset2, yOffset, door.position.y + zDoorOffset2);
                        doorObject.transform.eulerAngles = new Vector3(0, 90, 0);
                    }
                    else
                    {
                        position = new Vector3(door.position.x + xDoorOffset1, yOffset, door.position.y + zDoorOffset1);
                    }
                    SetWallFace(door);
                    doorObject.transform.position = position;
                    objectsGenerated.Add(doorObject);
                }
            }
        }
        private void HandlePropGeneration(string tileName, Vector3Int pos)
        {
            //world position
            Vector3 position = wallGroundTilemap.CellToWorld(pos);

            GameObject propObject = Instantiate(propPrefab);

            SpriteRenderer spriteRenderer = propObject.GetComponentInChildren<SpriteRenderer>();

            spriteRenderer.sprite = tileName.Contains("top")
            ?
            propsTopSprites.Find(sprite => sprite.name.ToLower() == tileName)
            :
            propsDefaultSprites.Find(sprite => sprite.name.ToLower() == tileName);

            //setup the collision for this prop
            if (tileName.EndsWith("c"))
            {
                Rigidbody propRigid = propObject.AddComponent<Rigidbody>();
                propRigid.useGravity = false;
                propRigid.constraints = RigidbodyConstraints.FreezeAll;
                propObject.AddComponent<BoxCollider>();
            }

            if (tileName.Contains("top"))
            {
                position = new Vector3(position.x + propXOffset, .5f, position.z + propZOffset);
            }
            else
            {
                position = new Vector3(position.x + propXOffset, position.y + propYOffset, position.z + propZOffset);
            }

            //fix the tile center pivot
            propObject.transform.position = position;

            objectsGenerated.Add(propObject);
        }
        private void HandleWallGeneration(string tileName, Vector3Int pos)
        {
            //world position
            Vector3 position = wallGroundTilemap.CellToWorld(pos);

            Sprite wallSprite = wallTextures.Find(item => item.name.ToLower().StartsWith(tileName));

            GameObject wallObject = Instantiate(wallGameObject);

            SpriteRenderer[] wallObjectSprites = wallObject.GetComponentsInChildren<SpriteRenderer>();
            foreach (var item in wallObjectSprites)
            {
                item.sprite = wallSprite;
            }
            walls.Add(new Wall(pos, wallObject));
            //fix the tile center pivot
            position = new Vector3(position.x + xOffset, position.y + yOffset, position.z + zOffset);
            wallObject.transform.position = position;

            objectsGenerated.Add(wallObject);
        }
        private void SetWallFace(Door door)
        {
            Vector3Int cellPos = door.position;
            if (door.topBottomSide)
            {
                Vector3Int topPos = new Vector3Int(cellPos.x, cellPos.y + 1, cellPos.z);
                Vector3Int bottomPos = new Vector3Int(cellPos.x, cellPos.y - 1, cellPos.z);

                string[] childsToFind = new string[2] { "front", "back" };
                SetWallSideface(topPos, bottomPos, childsToFind);
            }
            else
            {
                Vector3Int leftPos = new Vector3Int(cellPos.x - 1, cellPos.y, cellPos.z);
                Vector3Int rightPos = new Vector3Int(cellPos.x + 1, cellPos.y, cellPos.z);

                string[] childsToFind = new string[2] { "right", "left" };
                SetWallSideface(leftPos, rightPos, childsToFind);
            }
        }
        private void SetWallSideface(Vector3Int pos1, Vector3Int pos2, string[] transformChilds)
        {
            Wall wall1 = walls.Find(wall => wall.position == pos1);
            Wall wall2 = walls.Find(wall => wall.position == pos2);

            Transform side1 = wall1.objectReference.transform.Find(transformChilds[0]);
            Transform side2 = wall2.objectReference.transform.Find(transformChilds[1]);

            side1.GetComponent<SpriteRenderer>().sprite = wallFacingDoor;
            side2.GetComponent<SpriteRenderer>().sprite = wallFacingDoor;
        }
        private void OnPreviewModeExit()
        {
            doorsLocation.Clear();
            walls.Clear();

            objectsGenerated.ForEach(wall =>
            {
                Destroy(wall);
            });
            objectsGenerated.Clear();
        }
    }
    public struct Wall
    {
        public Wall(Vector3Int position, GameObject objectReference)
        {
            this.position = position;
            this.objectReference = objectReference;
        }
        public Vector3Int position { get; private set; }
        public GameObject objectReference { get; private set; }
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
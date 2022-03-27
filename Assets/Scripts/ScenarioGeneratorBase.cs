using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace WEditor.Scenario
{
    public class ScenarioGeneratorBase : MonoBehaviour
    {
        [SerializeField] protected Tilemap mainTilemap;
        [SerializeField] protected float xOffset, yOffset, zOffset;
        [Header("Wall generation")]
        [SerializeField] protected GameObject wallGameObject;
        [SerializeField] protected List<Sprite> wallTextures;
        [SerializeField] protected Sprite wallFacingDoor;
        [Header("Door generation")]
        [SerializeField] protected List<Sprite> doorSprites;
        [SerializeField] protected GameObject doorPrefab;
        [SerializeField] protected float xDoorOffset1 = .5f, xDoorOffset2, zDoorOffset1 = 1, zDoorOffset2;
        [Header("Prop generation")]
        [SerializeField] protected GameObject propPrefab;
        [SerializeField] protected List<Sprite> propsDefaultSprites, propsTopSprites;
        [SerializeField] protected float propXOffset, propYOffset, propZOffset;
        protected List<Door> doorsLocation = new List<Door>();
        protected List<GameObject> objectsGenerated = new List<GameObject>();
        protected List<Wall> walls = new List<Wall>();
        public virtual void InitGeneration()
        {

        }
        protected void HandleWallGeneration(string tileName, Vector3Int pos)
        {
            //world position
            Vector3 position = mainTilemap.CellToWorld(pos);

            int index = wallTextures.FindIndex(item => item.name.ToLower().StartsWith(tileName));
            Sprite wallSprite = wallTextures[index];

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

        protected void AddDoorToList(Vector3Int cellPos, string tileName)
        {
            Vector3Int topPos = new Vector3Int(cellPos.x, cellPos.y + 1, cellPos.z);
            Vector3Int bottomPos = new Vector3Int(cellPos.x, cellPos.y - 1, cellPos.z);
            if (mainTilemap.HasTile(topPos) && mainTilemap.HasTile(bottomPos))
            {
                doorsLocation.Add(new Door(cellPos, true, tileName));
                return;
            }

            Vector3Int leftPos = new Vector3Int(cellPos.x - 1, cellPos.y, cellPos.z);
            Vector3Int rightPos = new Vector3Int(cellPos.x + 1, cellPos.y, cellPos.z);
            if (mainTilemap.HasTile(leftPos) && mainTilemap.HasTile(rightPos))
            {
                doorsLocation.Add(new Door(cellPos, false, tileName));
            }
        }
        protected void HandleDoorsGeneration()
        {
            foreach (var door in doorsLocation)
            {
                if (mainTilemap.GetTile(door.position))
                {
                    int index = doorSprites.FindIndex(item => item.name.ToLower().StartsWith(door.name));
                    Sprite doorSprite = doorSprites[index];

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
        protected void HandlePropGeneration(string tileName, Vector3Int pos)
        {
            //world position
            Vector3 position = mainTilemap.CellToWorld(pos);

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
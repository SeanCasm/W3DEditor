using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using WEditor.Game.Scriptables;
using WEditor.Game.Collectibles;

namespace WEditor.Scenario
{
    public class ScenarioGeneratorBase : MonoBehaviour
    {
        [SerializeField] protected Tilemap mainTilemap;
        [SerializeField] protected float xOffset, yOffset, zOffset;
        [Header("Wall generation")]
        [SerializeField] protected GameObject wallGameObject;
        [SerializeField] protected ScenarioScriptable wallTextures;
        [SerializeField] protected Sprite wallFacingDoor;
        [Header("Door generation")]
        [SerializeField] protected ScenarioScriptable doorSprites;
        [SerializeField] protected GameObject doorPrefab;
        [SerializeField] protected float xDoorOffset1 = .5f, xDoorOffset2, zDoorOffset1 = 1, zDoorOffset2;
        [Header("Prop generation")]
        [SerializeField] protected GameObject propPrefab;
        [SerializeField] protected ScenarioScriptable propsDefaultSprites, propsTopSprites;
        [SerializeField] protected float propXOffset, propYOffset, propZOffset;
        [Header("Collectible generation")]
        [SerializeField] List<CollectibleScriptable> healthScriptables, ammoScriptables, scoreScriptables;
        [Header("Enemy generation")]
        // [SerializeField] List<Sprite> guard, ss;
        [SerializeField] GameObject guardPrefab, ssPrefab;
        protected List<Door> doorsLocation = new List<Door>();
        protected List<GameObject> objectsGenerated = new List<GameObject>();
        protected List<Wall> walls = new List<Wall>();
        public virtual void InitGeneration()
        {

        }
        protected void HandlePointsGeneration(string tileName, Vector3Int cellPos)
        {
            var (itemGameObject, lastIndex, position) = GetItem(tileName, cellPos);

            Score score = itemGameObject.AddComponent<Score>();
            score.collectibleScriptable = scoreScriptables[lastIndex];

            SetItemPosition(itemGameObject, position);
        }
        private (GameObject, int, Vector3) GetItem(string tileName, Vector3Int cellPos)
        {
            Vector3 position = mainTilemap.CellToWorld(cellPos);
            position = new Vector3(position.x + .5f, position.y, position.z + .5f);

            GameObject itemGameObject = new GameObject(tileName);
            itemGameObject.SetActive(false);
            itemGameObject.AddComponent<SpriteRenderer>();
            BoxCollider box = itemGameObject.AddComponent<BoxCollider>();
            box.isTrigger = true;
            objectsGenerated.Add(itemGameObject);

            string[] split = tileName.Split('_');
            int lastIndex = int.Parse(split[1].ToString());
            return (itemGameObject, lastIndex, position);
        }
        private void SetItemPosition(GameObject itemGameObject, Vector3 position)
        {
            Instantiate(itemGameObject);
            itemGameObject.transform.position = position;
            itemGameObject.SetActive(true);
        }
        protected void HandleAmmoGeneration(string tileName, Vector3Int cellPos)
        {
            var (itemGameObject, lastIndex, position) = GetItem(tileName, cellPos);

            Ammo ammo = itemGameObject.AddComponent<Ammo>();
            ammo.collectibleScriptable = ammoScriptables[lastIndex];

            SetItemPosition(itemGameObject, position);
        }
        protected void HandleHealthGeneration(string tileName, Vector3Int cellPos)
        {
            var (itemGameObject, lastIndex, position) = GetItem(tileName, cellPos);

            Health health = itemGameObject.AddComponent<Health>();
            health.collectibleScriptable = healthScriptables[lastIndex];

            SetItemPosition(itemGameObject, position);
        }
        protected void HandleWallGeneration(string tileName, Vector3Int pos)
        {
            //world position
            Vector3 position = mainTilemap.CellToWorld(pos);

            Sprite wallSprite = wallTextures.spritesCollection.Find(item => item.name.ToLower().StartsWith(tileName));

            GameObject wallObject = Instantiate(wallGameObject);

            SpriteRenderer[] wallObjectSprites = wallObject.GetComponentsInChildren<SpriteRenderer>();
            foreach (var item in wallObjectSprites) item.sprite = wallSprite;
            walls.Add(new Wall(pos, wallObject));
            //fix the tile center pivot
            position = new Vector3(position.x + xOffset, position.y + yOffset, position.z + zOffset);
            wallObject.transform.position = position;

            objectsGenerated.Add(wallObject);
        }
        protected void HandleEnemyGeneration(string tileName, Vector3Int pos)
        {
            Vector3 position = mainTilemap.CellToWorld(pos);
            position = new Vector3(position.x + .5f, position.y, position.z + .5f);
            GameObject enemy = tileName.StartsWith("guard") ? Instantiate(guardPrefab) : Instantiate(ssPrefab);
            enemy.transform.position = position;
            objectsGenerated.Add(enemy);
        }
        private void SetWallFace(Door door)
        {
            Vector3Int cellPos = door.position;
            if (door.topBottomSide)
                SetWallSideface(cellPos.GetTopTile(), cellPos.GetBottomTile(), new string[2] { "front", "back" });
            else
                SetWallSideface(cellPos.GetLeftTile(), cellPos.GetRightTile(), new string[2] { "right", "left" });
        }
        private void SetWallSideface(Vector3Int pos1, Vector3Int pos2, string[] transformChilds)
        {
            Wall wall1 = walls.Find(wall => wall.position.x == pos1.x && wall.position.y == pos1.y);
            Wall wall2 = walls.Find(wall => wall.position.x == pos2.x && wall.position.y == pos2.y);
            Transform side1 = wall1.objectReference.transform.Find(transformChilds[0]);
            Transform side2 = wall2.objectReference.transform.Find(transformChilds[1]);

            side1.GetComponent<SpriteRenderer>().sprite = wallFacingDoor;
            side2.GetComponent<SpriteRenderer>().sprite = wallFacingDoor;
        }

        protected void AddDoorToList(Vector3Int cellPos, string tileName)
        {

            if (mainTilemap.HasTile(cellPos.GetTopTile()) && mainTilemap.HasTile(cellPos.GetBottomTile()))
                doorsLocation.Add(new Door(cellPos, true, tileName));
            else if (mainTilemap.HasTile(cellPos.GetLeftTile()) && mainTilemap.HasTile(cellPos.GetRightTile()))
                doorsLocation.Add(new Door(cellPos, false, tileName));
        }
        protected void HandleDoorsGeneration()
        {
            foreach (var door in doorsLocation)
            {
                if (mainTilemap.GetTile(door.position))
                {
                    int index = doorSprites.spritesCollection.FindIndex(item => item.name.ToLower().StartsWith(door.name));
                    Sprite doorSprite = doorSprites.spritesCollection[index];

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

            spriteRenderer.sprite = tileName.Contains("top") ?
            propsTopSprites.spritesCollection.Find(sprite => sprite.name.ToLower() == tileName) :
            propsDefaultSprites.spritesCollection.Find(sprite => sprite.name.ToLower() == tileName);

            //setup the collision for this prop
            if (tileName.EndsWith("c"))
            {
                Rigidbody propRigid = propObject.AddComponent<Rigidbody>();
                propRigid.useGravity = false;
                propRigid.constraints = RigidbodyConstraints.FreezeAll;
                propObject.AddComponent<BoxCollider>();
            }
            position = tileName.Contains("top") ?
            new Vector3(position.x + propXOffset, .5f, position.z + propZOffset) :
            new Vector3(position.x + propXOffset, position.y + propYOffset, position.z + propZOffset);

            //fix the tile center pivot
            propObject.transform.position = position;

            objectsGenerated.Add(propObject);
        }
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
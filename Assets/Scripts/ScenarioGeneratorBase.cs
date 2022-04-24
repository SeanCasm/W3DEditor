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
        [SerializeField] Material groundMaterial;
        [Header("Wall generation")]
        [SerializeField] protected GameObject wallPrefab;
        [SerializeField] protected TextureScenarioScriptable wallScriptable;
        [SerializeField] GameObject wallFacingPrefab;
        [Header("Door generation")]
        [SerializeField] protected TextureScenarioScriptable doorScriptable;
        [SerializeField] GameObject doorPrefab;
        [Header("Prop generation")]
        [SerializeField] GameObject propPrefab;
        [SerializeField] protected ScenarioScriptable propsDefaultSprites, propsTopSprites;
        [SerializeField] protected float propXOffset, propYOffset, propZOffset;
        [Header("Collectible generation")]
        [SerializeField] List<CollectibleScriptable> healthScriptables, ammoScriptables, scoreScriptables;
        [Header("Enemy generation")]
        [SerializeField] GameObject guardPrefab, ssPrefab;
        protected List<GameObject> objectsGenerated = new List<GameObject>();
        protected Wall[,] wallGrid;
        protected Door[,] doorGrid;
        protected bool[,] mainGrid;
        protected GameObject groundPlane;
        public void InitGeneration(Vector3Int size)
        {
            int mapWidth = size.x;
            int mapHeight = size.y;
            mainGrid = new bool[mapWidth, mapHeight];
            wallGrid = new Wall[mapWidth, mapHeight];
            doorGrid = new Door[mapWidth, mapHeight];
            groundPlane = GameObject.CreatePrimitive(PrimitiveType.Plane);
            MeshRenderer planeMesh = groundPlane.GetComponent<MeshRenderer>();
            planeMesh.material = groundMaterial;
            float scaleX = mapWidth / .64f;
            float sclaeZ = mapHeight / .64f;
            groundPlane.transform.localScale = new Vector3(scaleX, 1, sclaeZ);
            groundPlane.transform.position = new Vector3(mainGrid.GetLength(0) / 2, 0, mainGrid.GetLength(1) / 2);
        }
        protected void HandlePointsGeneration(string tileName, Vector3Int cellPos)
        {
            var (itemGameObject, lastIndex, position) = GetItem(tileName, cellPos);

            Score score = itemGameObject.AddComponent<Score>();
            score.collectibleScriptable = scoreScriptables[lastIndex];

            SetItemPosition(itemGameObject, position);
        }
        private (GameObject, int, Vector3) GetItem(string tileName, Vector3 position)
        {
            position = new Vector3(position.x + .5f, position.y, position.z + .5f);

            GameObject itemGameObject = new GameObject(tileName);
            itemGameObject.SetActive(false);
            itemGameObject.AddComponent<SpriteRenderer>();
            BoxCollider box = itemGameObject.AddComponent<BoxCollider>();
            box.isTrigger = true;
            objectsGenerated.Add(itemGameObject);

            int lastIndex = tileName.GetIndexFromAssetName();
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
        protected void HandleWallGeneration(List<(string tileName, Vector3Int cellPos)> walls)
        {
            foreach (var wall in walls)
            {
                Texture2D wallTex = wallScriptable.GetTexture(wall.tileName);
                int x = wall.cellPos.x;
                int y = wall.cellPos.y;
                GameObject wallObject = Instantiate(wallPrefab);
                wallObject.GetComponent<MeshRenderer>().material.mainTexture = wallTex;
                wallGrid[x, y] = new Wall(wallObject);
                //fix the tile center pivot
                wallObject.transform.position = new Vector3(x, 0, y);
                objectsGenerated.Add(wallObject);
            }
        }
        protected void HandleEnemyGeneration(string tileName, Vector3 position)
        {
            position = new Vector3(position.x + .5f, position.y, position.z + .5f);
            GameObject enemy = tileName.StartsWith("guard") ? Instantiate(guardPrefab) : Instantiate(ssPrefab);
            enemy.transform.position = position;
            objectsGenerated.Add(enemy);
        }


        protected void AddDoorToList(Vector3Int cellPos, string tileName)
        {
            int x = cellPos.x;
            int y = cellPos.y;

            if (mainGrid[x, y + 1] && mainGrid[x, y - 1])
            {
                doorGrid[x, y] = new Door(
                    true, tileName,
                    new Vector2Int[] {
                        new Vector2Int(x, y + 1),
                        new Vector2Int(x, y - 1),
                    }
                );
            }
            else if (mainGrid[x - 1, y] && mainGrid[x + 1, y])
            {
                doorGrid[x, y] = new Door(
                    false, tileName,
                    new Vector2Int[] {
                        new Vector2Int(x+1, y),
                        new Vector2Int(x-1, y),
                    }
                );
            }
        }
        protected void HandleDoorsGeneration()
        {
            for (int x = 0; x < doorGrid.GetLength(0); x++)
            {
                for (int y = 0; y < doorGrid.GetLength(1); y++)
                {
                    Door door = doorGrid[x, y];
                    if (door == null || !mainGrid[x, y]) continue;

                    Texture doorTex = doorScriptable.GetTexture(door.name);

                    GameObject doorObject = Instantiate(doorPrefab);
                    doorObject.GetComponent<MeshRenderer>().material.mainTexture = doorTex;
                    Vector3 position = Vector3.zero;
                    if (door.topBottomSide)
                    {
                        position = new Vector3(x + .5f, .5f, y + .5f);
                        doorObject.transform.eulerAngles = new Vector3(0, 90, 0);
                        SetWallFace(door, new Vector3Int(x, y, 0));
                    }
                    else
                    {
                        position = new Vector3(x + .5f, .5f, y + .5f);
                        SetWallFace(door, new Vector3Int(x, y, 0));
                    }
                    doorObject.transform.position = position;
                    objectsGenerated.Add(doorObject);
                }
            }
        }
        private void SetWallFace(Door door, Vector3Int position)
        {
            int x = position.x;
            int y = position.y;

            GameObject wallF = Instantiate(wallFacingPrefab);
            wallF.transform.position = new Vector3(position.x + .5f, .5f, position.y + .5f);
            objectsGenerated.Add(wallF);
            if (door.topBottomSide)
                wallF.transform.eulerAngles = new Vector3(0, 90, 0);
        }
        protected void HandlePropGeneration(string tileName, Vector3 position)
        {
            //world position
            GameObject propObject = Instantiate(propPrefab);

            SpriteRenderer spriteRenderer = propObject.GetComponentInChildren<SpriteRenderer>();

            spriteRenderer.sprite = tileName.Contains("top") ?
            propsTopSprites.GetSprite(tileName) :
            propsDefaultSprites.GetSprite(tileName);

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
    public Wall(GameObject objectReference)
    {
        this.objectReference = objectReference;
    }
    public GameObject objectReference { get; private set; }
}
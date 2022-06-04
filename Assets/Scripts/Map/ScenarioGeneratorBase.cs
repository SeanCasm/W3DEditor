using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WEditor.Game.Scriptables;
using WEditor.Game.Collectibles;
using WEditor.Game;

namespace WEditor.Scenario
{
    public class ScenarioGeneratorBase : MonoBehaviour
    {
        public static ScenarioGeneratorBase instance;
        [SerializeField] Material groundMaterial;
        [SerializeField] MeshCombiner meshCombiner;
        [Header("Items generation")]
        [SerializeField] GameObject scorePrefab, keyPrefab;
        [SerializeField] GameObject ammoPrefab, healthPrefab, gunPrefab;
        [Header("Wall generation")]
        [SerializeField] protected GameObject wallPrefab;
        [SerializeField] protected ScenarioScriptable wallScriptable;
        [Header("Door generation")]
        [SerializeField] DoorGeneration doorGeneration;

        [Header("Prop generation")]
        [SerializeField] GameObject propPrefab;
        [SerializeField] protected ScenarioScriptable propsDefaultSprites;
        [Header("Collectible generation")]
        [SerializeField] List<CollectibleScriptable> keyScriptables;
        [SerializeField] List<CollectibleScriptable> healthScriptables, ammoScriptables;
        [SerializeField] List<CollectibleScriptable> scoreScriptables, gunScriptables;
        [Header("Enemy generation")]
        [SerializeField] GameObject guardPrefab, ssPrefab;
        protected List<GameObject> objectsGenerated = new List<GameObject>();
        protected Wall[,] wallGrid;
        protected Door[,] doorGrid;
        protected bool[,] mainGrid;
        protected GameObject groundPlane;
        private void Start() => instance = this;
        protected void HandleTilesLocation(string tileName, Vector3Int cellPos, List<Door> doors, List<Wall> walls)
        {
            Vector3 position = new Vector3(cellPos.x, 0, cellPos.y);
            if (tileName.Contains("Wall"))
            {
                walls.Add(new Wall(tileName, cellPos));
            }
            else if (tileName.StartsWith("Ground"))
            {
                if (tileName.Contains("health"))
                    HandleHealthGeneration(tileName, cellPos);
                else if (tileName.Contains("ammo"))
                    HandleAmmoGeneration(tileName, cellPos);
                else if (tileName.Contains("score"))
                    HandleScoreGeneration(tileName, cellPos);
                else if (tileName.Contains("key"))
                    HandleKeyGeneration(tileName, cellPos);
                else if (tileName.Contains("gun"))
                    HandleGunGeneration(tileName, cellPos);
                else
                    HandlePropGeneration(tileName, position);
            }
            else if (tileName.Contains("Door"))
            {
                print(tileName);
                doors.Add(new Door { tileName = tileName, position = cellPos });
            }
            else if (tileName.StartsWith("guard") || tileName.StartsWith("ss"))
            {
                HandleEnemyGeneration(tileName, position);
            }
            mainGrid[cellPos.x, cellPos.y] = true;
        }
        public void ResetLevel()
        {
            objectsGenerated.ForEach(item => Destroy(item));
            InitGeneration();
        }
        public virtual void InitGeneration()
        {
            int mapWidth = DataHandler.levelSize.x;
            int mapHeight = DataHandler.levelSize.y;
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

            CreateScenarioLimiter();
        }
        private void CreateScenarioLimiter()
        {
            float mapWidth = mainGrid.GetLength(0);
            float mapHeight = mainGrid.GetLength(1);
            List<MeshFilter> fences = new List<MeshFilter>();
            //bottom-right to top-left
            for (float y = 0; y < mapHeight; y += 1)
            {
                CreateFence(new Vector3(-1, 0, y));
            }
            //top-left to top-right
            for (float x = 0; x < mapWidth; x += 1)
            {
                CreateFence(new Vector3(x, 0, mapHeight));
            }
            //top-right to bottom-right
            for (float y = mapHeight - 1; y >= 0; y -= 1)
            {
                CreateFence(new Vector3(mapWidth, 0, y));
            }
            //bottom-right to bottom-left
            for (float x = mapWidth - 1; x >= 0; x -= 1)
            {
                CreateFence(new Vector3(x, 0, -1));
            }

            void CreateFence(Vector3 position)
            {
                GameObject fence = Instantiate(wallPrefab);
                fence.transform.position = position;
                fences.Add(fence.GetComponent<MeshFilter>());
                objectsGenerated.Add(fence);
            }
            meshCombiner.CombineMeshes(fences);
        }
        protected void HandleKeyGeneration(string tileName, Vector3Int cellPos)
        {
            GameObject keyObject = Instantiate(keyPrefab);
            Key key = keyObject.GetComponent<Key>();
            key.CollectibleScriptable = keyScriptables.Find(x => x.spriteName == tileName);
            key.keyType = key.CollectibleScriptable.name.Contains("Golden") ? KeyType.Golden : KeyType.Platinum;
            Vector3 position = GetWorldPosition(key.getSprite, cellPos);
            SetItemPosition(keyObject, position);
        }
        protected void HandleGunGeneration(string tileName, Vector3Int cellPos)
        {
            GameObject gunObject = Instantiate(gunPrefab);
            Gun gun = gunObject.GetComponent<Gun>();
            int index = gunScriptables.FindIndex(x => x.spriteName == tileName);
            gun.CollectibleScriptable = gunScriptables[index];
            gun.gunIndex = index + 2;
            Vector3 position = GetWorldPosition(gun.getSprite, cellPos);
            SetItemPosition(gunObject, position);
        }
        protected void HandleScoreGeneration(string tileName, Vector3Int cellPos)
        {
            GameObject scoreObject = Instantiate(scorePrefab);

            Score score = scoreObject.GetComponent<Score>();
            score.CollectibleScriptable = scoreScriptables.Find(x => x.spriteName == tileName);

            Vector3 position = GetWorldPosition(score.getSprite, cellPos);
            SetItemPosition(scoreObject, position);
        }
        private Vector3 GetWorldPosition(Sprite tileSprite, Vector3Int pos)
        {
            float newY = tileSprite.bounds.max.y;
            return new Vector3(pos.x + .5f, newY, pos.y + .5f);
        }
        private void SetItemPosition(GameObject itemGameObject, Vector3 position)
        {
            itemGameObject.transform.position = position;
            itemGameObject.SetActive(true);
            objectsGenerated.Add(itemGameObject);
        }
        protected void HandleAmmoGeneration(string tileName, Vector3Int cellPos)
        {
            GameObject ammoObject = Instantiate(ammoPrefab);
            Ammo ammo = ammoObject.GetComponent<Ammo>();
            ammo.CollectibleScriptable = ammoScriptables[Random.Range(0, ammoScriptables.Count)];
            Vector3 position = GetWorldPosition(ammo.getSprite, cellPos);
            SetItemPosition(ammoObject, position);
        }
        protected void HandleHealthGeneration(string tileName, Vector3Int cellPos)
        {
            GameObject healthObject = Instantiate(healthPrefab);
            Health health = healthObject.GetComponent<Health>();
            health.CollectibleScriptable = healthScriptables.Find(x => x.spriteName == tileName);
            Vector3 position = GetWorldPosition(health.getSprite, cellPos);

            SetItemPosition(healthObject, position);
        }
        protected void HandleWallGeneration(List<Wall> walls)
        {
            Dictionary<string, List<GameObject>> wallsToFilter = new Dictionary<string, List<GameObject>>();
            foreach (var wall in walls)
            {
                string tileName = wall.tileName;
                Texture2D wallTex = wallScriptable.GetTexture(tileName);
                int x = wall.position.x;
                int y = wall.position.y;
                GameObject wallObject = Instantiate(wallPrefab);
                if (wallsToFilter.ContainsKey(tileName))
                {
                    List<GameObject> wallsFiltered = wallsToFilter[tileName];
                    wallsFiltered.Add(wallObject);
                }
                else
                {
                    List<GameObject> l = new List<GameObject>(new GameObject[] { wallObject });
                    wallsToFilter.Add(tileName, l);
                }
                wallObject.GetComponent<MeshRenderer>().material.mainTexture = wallTex;
                wallGrid[x, y] = new Wall(tileName, wall.position);
                //fix the tile center pivot
                wallObject.transform.position = new Vector3(x, 0, y);
                objectsGenerated.Add(wallObject);
            }

            meshCombiner.CombineMultipleMeshes(wallsToFilter);
        }
        protected void HandleEnemyGeneration(string tileName, Vector3 position)
        {
            position = new Vector3(position.x + .5f, position.y, position.z + .5f);
            GameObject enemy = tileName.StartsWith("guard") ? Instantiate(guardPrefab) : Instantiate(ssPrefab);
            enemy.transform.position = position;
            objectsGenerated.Add(enemy);
        }
        protected void AddDoorToGrid(Door door)
        {
            Door initilizedDoor = new Door { tileName = door.tileName, position = door.position };
            int x = door.position.x;
            int y = door.position.y;
            doorGrid[x, y] = initilizedDoor;

            if (door.tileName.Contains("_elv"))
                return;

            if (mainGrid[x, y + 1] && mainGrid[x, y - 1])
                doorGrid[x, y].topBottomSide = WallSide.TopBottom;
            else if (mainGrid[x - 1, y] && mainGrid[x + 1, y])
                doorGrid[x, y].topBottomSide = WallSide.LeftRight;

        }
        protected void HandleDoorsGeneration(List<Door> doors)
        {
            doors.ForEach(item => AddDoorToGrid(item));
            for (int x = 0; x < doorGrid.GetLength(0); x++)
            {
                for (int y = 0; y < doorGrid.GetLength(1); y++)
                {
                    Door door = doorGrid[x, y];
                    if (door == null || !mainGrid[x, y] || door.tileName.Contains("_elv")) continue;
                    List<GameObject> newDoor = doorGeneration.StartGeneration(door, doorGrid);
                    objectsGenerated.AddRange(newDoor);
                }
            }
        }

        protected void HandlePropGeneration(string tileName, Vector3 position)
        {
            //world position
            GameObject propObject = Instantiate(propPrefab);

            SpriteRenderer spriteRenderer = propObject.GetComponentInChildren<SpriteRenderer>();

            //setup the collision for this prop
            if (!tileName.Contains("_n"))
            {
                Rigidbody propRigid = propObject.AddComponent<Rigidbody>();
                propRigid.useGravity = false;
                propRigid.constraints = RigidbodyConstraints.FreezeAll;
                propObject.AddComponent<BoxCollider>();
            }
            spriteRenderer.sprite = propsDefaultSprites.GetSprite(tileName);
            position = new Vector3(position.x + .5f, .5f, position.z + .5f);

            //fix the tile center pivot
            propObject.transform.position = position;

            objectsGenerated.Add(propObject);
        }
        protected void OnPreviewModeExit() => meshCombiner.DisableTargetCombiner();
    }
}
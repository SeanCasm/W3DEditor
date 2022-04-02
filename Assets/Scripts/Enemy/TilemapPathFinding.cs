using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace WEditor.Game.Enemy
{
    public class TilemapPathFinding : MonoBehaviour
    {
        [SerializeField] float checkUpdate;
        private Vector3Int currentPosition;
        private PathPosition pathPosition;
        private bool completed;
        private List<PathNode> openSet = new List<PathNode>();
        private List<PathNode> closedSet = new List<PathNode>();
        public void Init(Vector3 startPosition)
        {
            pathPosition = new PathPosition(
                currentPosition = LevelGlobalReferences.instance.TilemapPosition(startPosition),
                LevelGlobalReferences.instance.playerTilemapPosition
            );

            CreateNodeGrid();
            HandlePath();

            StartCoroutine(nameof(UpdateChecks));
        }
        private void CreateNodeGrid()
        {
            Tilemap levelTilemap = LevelGlobalReferences.instance.levelTilemap;
            int x = levelTilemap.size.x;
            int y = levelTilemap.size.y;
            PathNode[,] grid = new PathNode[x, y];
            for (int i = 0; i < grid.GetLength(0); i++)
            {
                for (int j = 0; j < grid.GetLength(1); j++)
                {
                    grid[i, j] = new PathNode(
                        Mathf.Abs(i - pathPosition.start.x) + Mathf.Abs(j - i - pathPosition.start.y) * 10,
                        Mathf.Abs(i - pathPosition.target.x) + Mathf.Abs(j - pathPosition.target.y),
                        new Vector3Int(i, j, 0)
                    );
                }
            }
        }
        public void Stop()
        {
            StopAllCoroutines();
        }
        private void HandlePath()
        {
            while (!completed)
            {
                int cellIndex = openSet.Min(node => node.f);
                PathNode cell = openSet[cellIndex];
                closedSet.Add(cell);


            }
        }
        private Vector3Int[] GetTileNeighboors(Vector3Int position)
        {
            Tilemap levelTilemap = LevelGlobalReferences.instance.levelTilemap;
            List<Vector3Int> cells = new List<Vector3Int>();

            for (int x = 0; x < 3; x++)
            {
                for (int y = 0; y < 3; y++)
                {
                    Vector3Int cellPos = new Vector3Int(x, y, 0);
                    if (levelTilemap.HasTile(cellPos) && x != y)
                    {
                        cells.Add(cellPos);
                    }
                }
            }

            return cells.ToArray();
        }
        private void SelectNeighboors()
        {

        }
        IEnumerator UpdateChecks()
        {
            while (!pathPosition.TargetChanged(LevelGlobalReferences.instance.playerTilemapPosition))
            {

                yield return new WaitForSeconds(checkUpdate);
            }
        }
    }

    public struct PathPosition
    {
        public PathPosition(Vector3Int start, Vector3Int target)
        {
            this.start = start;
            this.target = target;
        }
        public Vector3Int start { get; private set; }
        public Vector3Int target { get; private set; }
        public bool TargetChanged(Vector3Int position)
        {
            return LevelGlobalReferences.instance.ComparePositionAndPlayer(target);
        }

    }
}
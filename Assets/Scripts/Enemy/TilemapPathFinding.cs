using System;
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
        private bool completed;
        private int checks = 0;
        private List<PathNode> openSet = new List<PathNode>();
        private List<PathNode> closedSet = new List<PathNode>();
        public Vector3Int start { get; private set; }
        public Vector3Int target { get; private set; }
        PathNode[,] grid;
        public void Init(Vector3 startPosition)
        {
            start = currentPosition = TilemapPathfindingLevel.instance.TilemapPosition(startPosition);
            target = TilemapPathfindingLevel.instance.playerTilemapPosition;

            //CreateBinaryNodeGrid();
            openSet.Add(grid[start.x, start.y]);
            HandlePath();

            StartCoroutine(nameof(UpdateChecks));
        }
        private void CreateBinaryNodeGrid()
        {
            Tilemap levelTilemap = TilemapPathfindingLevel.instance.levelTilemap;
            int x = levelTilemap.size.x;
            int y = levelTilemap.size.y;
            grid = new PathNode[x, y];
            for (int i = 0; i < grid.GetLength(0); i++)
            {
                for (int j = 0; j < grid.GetLength(1); j++)
                {
                    if (!TilemapPathfindingLevel.instance.HasValidTile(new Vector3Int(i, j, 0)))
                        continue;


                    grid[i, j] = new PathNode(
                        Mathf.Abs(i - start.x) + Mathf.Abs(j - start.y) * 10,
                        Mathf.Abs(i - target.x) + Mathf.Abs(j - target.y),
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
                PathNode fLower = openSet.Min();
                openSet.Remove(fLower);
                closedSet.Add(fLower);
                var neighboors = GetNeighbors(fLower);

            }
        }
        private PathNode[] GetNeighbors(PathNode node)
        {
            Tilemap levelTilemap = TilemapPathfindingLevel.instance.levelTilemap;
            List<PathNode> cells = new List<PathNode>();
            List<Vector3Int> neighbors = new List<Vector3Int>();

            Vector3Int position = node.position;

            var corners = GetCorners(position);
            var lines = GetInlines(position);

            List<Vector3Int> cornersWithoutWalls = Array.FindAll(corners, node => TilemapPathfindingLevel.instance.HasValidTile(node)).ToList();
            List<Vector3Int> linesWithoutWalls = Array.FindAll(lines, node => TilemapPathfindingLevel.instance.HasValidTile(node)).ToList();

            neighbors = cornersWithoutWalls.Concat(linesWithoutWalls).ToList();

            CreatePathNodes(neighbors, cells);

            return cells.ToArray();
        }
        private void CreatePathNodes(List<Vector3Int> cellPositions, List<PathNode> nodes)
        {
            cellPositions.ForEach(node =>
                        {
                            int x = node.x;
                            int y = node.y;
                            PathNode neighborNode = new PathNode(
                                Mathf.Abs(x - start.x) * 10 + Mathf.Abs(y - start.y) * 10,
                                Mathf.Abs(x - target.x) * 10 + Mathf.Abs(y - target.y) * 10,
                                node
                            );
                            nodes.Add(neighborNode);
                        });
        }
        private Vector3Int[] GetInlines(Vector3Int value1)
        {
            Vector3Int[] lines = {
                new Vector3Int(value1.x, value1.y + 1, 0),
                new Vector3Int(value1.x, value1.y - 1, 0),
                new Vector3Int(value1.x - 1, value1.y, 0),
                new Vector3Int(value1.x + 1, value1.y, 0),
            };
            return lines;
        }
        private Vector3Int[] GetCorners(Vector3Int value1)
        {
            Vector3Int[] corners = {
                new Vector3Int(value1.x - 1, value1.y + 1, 0),
                new Vector3Int(value1.x + 1, value1.y + 1, 0),
                new Vector3Int(value1.x - 1, value1.y - 1, 0),
                new Vector3Int(value1.x + 1, value1.y - 1, 0),
            };
            return corners;
        }
        private void SelectNeighboors()
        {

        }
        IEnumerator UpdateChecks()
        {
            while (!TargetChanged(TilemapPathfindingLevel.instance.playerTilemapPosition))
            {

                yield return new WaitForSeconds(checkUpdate);
            }
        }
        private bool TargetChanged(Vector3Int position)
        {
            return TilemapPathfindingLevel.instance.ComparePositionAndPlayer(target);
        }
    }

}
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WEditor.Game.Enemy
{
    public class PathFinding : MonoBehaviour
    {
        [SerializeField] float checkUpdate;
        public List<Node> finalPath { get; set; } = new List<Node>();

        public void FindPath(Vector3 startPosition, Vector3 targetPosition)
        {
            List<Node> openSet = new List<Node>();
            HashSet<Node> closedSet = new HashSet<Node>();

            Grid.instance.CreateGrid();
            Node targetNode = Grid.instance.GetNodeFromGridPosition(targetPosition);
            Node startNode = Grid.instance.GetNodeFromGridPosition(startPosition);
            openSet.Add(startNode);
            while (openSet.Count > 0)
            {
                Node currentNode = openSet[0];
                for (int i = 1; i < openSet.Count; i++)
                {
                    if (openSet[i].fCost < currentNode.fCost || openSet[i].fCost == currentNode.fCost && openSet[i].hCost < currentNode.hCost)
                    {
                        currentNode = openSet[i];
                    }
                }

                openSet.Remove(currentNode);
                closedSet.Add(currentNode);

                if (currentNode == targetNode)
                {
                    RetracePath(startNode, targetNode);
                    return;
                }

                foreach (Node neighbour in Grid.instance.GetNeighbours(currentNode))
                {
                    if (!neighbour.isWalkable || closedSet.Contains(neighbour)) continue;

                    int newMovementCostToNeighbour = currentNode.gCost + GetDistance(currentNode, neighbour);
                    if (newMovementCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour))
                    {
                        neighbour.gCost = newMovementCostToNeighbour;
                        neighbour.hCost = GetDistance(neighbour, targetNode);
                        neighbour.parent = currentNode;
                        if (!openSet.Contains(neighbour))
                            openSet.Add(neighbour);
                    }
                }
            }
        }
        public void Stop()
        {
            StopAllCoroutines();
        }
        private int GetDistance(Node nodeA, Node nodeB)
        {
            int dstX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
            int dstY = Mathf.Abs(nodeA.gridY - nodeB.gridY);

            if (dstX > dstY)
                return 10 * dstY + 10 * (dstX - dstY);
            return 10 * dstX + 10 * (dstY - dstX);
        }
        private void RetracePath(Node startNode, Node targetNode)
        {
            List<Node> path = new List<Node>();
            Node currentNode = targetNode;

            while (currentNode != startNode)
            {
                path.Add(currentNode);
                currentNode = currentNode.parent;
            }
            path.Reverse();
            finalPath = path;
        }
    }
}
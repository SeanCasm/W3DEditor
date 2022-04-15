using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    public int gCost { get; set; }
    public int hCost { get; set; }
    public int fCost { get => hCost + gCost; }
    public int gridX { get; set; }
    public int gridY { get; set; }
    public Vector3 gridPosition { get => new Vector3(gridX + .5f, 0, gridY + .5f); }
    public bool isWalkable { get; set; }
    public Node parent { get; set; }
    public Node(int x, int y, bool walkable)
    {
        this.gridX = x;
        this.gridY = y;
        this.isWalkable = walkable;
    }
}
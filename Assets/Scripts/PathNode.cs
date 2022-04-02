using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathNode
{
    public int g { get; private set; }
    public int h { get; private set; }
    public int f { get; set; }
    public Vector3Int position { get; set; }
    public PathNode parent { get; private set; }
    public PathNode(int g, int h, Vector3Int position)
    {
        this.h = h;
        this.g = g;
        this.f = h + g;
        this.position = position;
    }

}

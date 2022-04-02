using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using WEditor;
public class LevelGlobalReferences : MonoBehaviour
{
    public static LevelGlobalReferences instance;
    public Tilemap levelTilemap;
    public const float levelTileSize = .64f;
    public int[,] grid { get; private set; }
    public Vector3Int playerTilemapPosition
    {
        get => levelTilemap.WorldToCell(PlayerGlobalReference.instance.position);
    }
    void Start()
    {
        instance = this;
    }
    
    public Vector3Int TilemapPosition(Vector3 position)
    {
        return levelTilemap.WorldToCell(position);
    }
    public bool ComparePositionAndPlayer(Vector3Int position)
    {
        return position.x == playerTilemapPosition.x
        &&
        position.y == playerTilemapPosition.y;
    }
}

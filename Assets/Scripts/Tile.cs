using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;



public enum TileType
{
    Empty,
    Node,
    Wall
}

public enum ColorType
{
    None,
    White,
    Red,
    Yellow,
    Blue,
    Green,
    Purple,
    Orange
}

public enum NodeSpriteType
{
    None,
    NotConnected,
    North,
    East,
    South,
    West
}

public enum PathSpriteType
{
    None,
    Horizontal,
    Vertical,
    WestToNorth,
    NorthToEast,
    EastToSouth,
    SouthToWest
}




public class Tile : MonoBehaviour
{
    public Vector2Int Position;

    public TileType Type;

    public ColorType Color;

    public PathSpriteType pathSprite;
    public GameObject NormalLine;
    public GameObject WhiteLine;

    public List<Tile> ConnectedTiles;

    private void Awake()
    {
        if (Type == TileType.Node)
        {
            ConnectedTiles = new List<Tile>();
        }
    }





}

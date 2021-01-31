using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell
{
    public Vector3Int Position { get { return _position; } }
    private Vector3Int _position;
    public TileType Type { get { return _type; } }
    private TileType _type;

    public Cell Left { get { return _left; } }
    private Cell _left;
    public Cell TopLeft { get { return _topLeft; } }
    private Cell _topLeft;
    public Cell TopRight { get { return _topRight; } }
    private Cell _topRight;
    public Cell Right { get { return _right; } }
    private Cell _right;
    public Cell BottomRight { get { return _bottomRight; } }
    private Cell _bottomRight;
    public Cell BottomLeft { get { return _bottomLeft; } }
    private Cell _bottomLeft;

    public Cell(Vector3Int positionInBoard, UnityEngine.Tilemaps.Tile associatedTileInTilemap)
    {
        _position = positionInBoard;
        _type = new TileType(associatedTileInTilemap);
    }

    public bool IsOccupied()
    {
        return false;
    }

    public Cell GetLeft()
    {
        return null;
    }

    public void SetNeighbors(Cell left, Cell topLeft, Cell topRight, Cell right, Cell bottomRight, Cell bottomLeft)
    {
        _left = left;
        _topLeft = topLeft;
        _topRight = topRight;
        _right = right;
        _bottomRight = bottomRight;
        _bottomLeft = bottomLeft;
    }

    public override string ToString()
    {
        return $"Cell at {Position} has neighbours Left {Left?.Position} TopLeft {(TopLeft?.Position)} TopRight {(TopRight?.Position)} Right {(Right?.Position)} BottomRight {(BottomRight?.Position)} BottomLeft {(BottomLeft?.Position)}";
    }
}

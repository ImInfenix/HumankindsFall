﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell
{
    public Vector3Int TileMapPosition { get { return _tileMapPosition; } }
    private Vector3Int _tileMapPosition;
    public Vector3 WorldPosition { get { return _worldPosition; } }
    private Vector3 _worldPosition;
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

    public Cell(Vector3Int positionInBoard, Vector3 worldPosition, UnityEngine.Tilemaps.Tile associatedTileInTilemap)
    {
        _tileMapPosition = positionInBoard;
        _worldPosition = worldPosition;
        _type = new TileType(associatedTileInTilemap);
    }

    public bool IsOccupied()
    {
        return false;
    }

    public Cell[] GetAllNeighbours()
    {
        Cell[] neighbours = { Left, TopLeft, TopRight, Right, BottomRight, BottomLeft    };
        return neighbours;
    }

    public void SetNeighbours(Cell left, Cell topLeft, Cell topRight, Cell right, Cell bottomRight, Cell bottomLeft)
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
        return $"Cell tilemap position: {TileMapPosition}, world position: {WorldPosition}, neighbours: Left {Left?.TileMapPosition} TopLeft {(TopLeft?.TileMapPosition)} TopRight {(TopRight?.TileMapPosition)} Right {(Right?.TileMapPosition)} BottomRight {(BottomRight?.TileMapPosition)} BottomLeft {(BottomLeft?.TileMapPosition)}\nProperties: {Type}";
    }
}
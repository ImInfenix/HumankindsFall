﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Board : MonoBehaviour
{
    public static Board CurrentBoard { get; private set; }

    [SerializeField]
    private Tilemap tilemap, placementTilemap;
    public Vector2Int tileToInspect;

    private Cell[,] board;

    public Vector2Int Size { get { return new Vector2Int(board.GetLength(0), board.GetLength(1)); } }
    public int SizeX { get { return board.GetLength(0); } }
    public int SizeY { get { return board.GetLength(1); } }

    private Vector2Int boardOrigin;

    [SerializeField] GameObject Unit;
    private List<Cell> allyCellsList = new List<Cell>();

    private BoundsInt bounds;

    void Awake()
    {
        CurrentBoard = this;
        Initialize(tilemap);

        GenerateUnits(placementTilemap);
    }

    public void Initialize(Tilemap tilemap)
    {
        tilemap.CompressBounds();
        bounds = tilemap.cellBounds;
        Vector3Int[] tilesPositions = GetTilesPositions(tilemap);

        if (tilesPositions.Length == 0)
        {
            Debug.LogError($"The tilemap {tilemap.name} has no tile inside of it !");
            return;
        }

        PutTilesInBoard(tilemap, tilesPositions);

        LinkAllTiles();

        Vector3Int firstCellPositionInTilemap = board[0, 0].TileMapPosition;
        boardOrigin = Vector2Int.zero - new Vector2Int(firstCellPositionInTilemap.x, firstCellPositionInTilemap.y);
    }

    public void GenerateUnits(Tilemap tilemap)
    {
        //go through all tiles of the tilemap
        TileBase[] tiles = tilemap.GetTilesBlock(bounds);

        for (int x = 0; x < bounds.size.x; x++)
        {
            for (int y = 0; y < bounds.size.y; y++)
            {
                TileBase tile = tiles[x + y * bounds.size.x];
                if (tile != null)
                {
                    Vector3Int tilePosition = new Vector3Int(x - Mathf.FloorToInt(SizeX / 2), y - Mathf.FloorToInt(SizeY / 2), 0);
                    Sprite tileSprite = tilemap.GetSprite(tilePosition);

                    //if it's an enemy tile, spawn a random enemy
                    if (tileSprite.name == "EnemyTileSprite")
                    {
                        Unit unit = Instantiate(Unit, transform).GetComponent<Unit>();
                        unit.setBoard(this);
                        unit.occupyNewCell(GetCell(tilePosition));
                        unit.updatePosition();
                        unit.tag = "UnitEnemy";
                    }

                    //if it's an ally tile, add it to the list of authorized tiles
                    else if (tileSprite.name == "AllyTileSprite")
                    {
                        allyCellsList.Add(GetCell(tilePosition));   
                    }
                }
            }
        }
    }

    public Cell GetCell(Vector3Int tilemapPosition)
    {
        try
        {
            Vector2Int positionInBoard = new Vector2Int(tilemapPosition.x, tilemapPosition.y) + boardOrigin;
            return board[positionInBoard.x, positionInBoard.y];
        }
        catch (System.IndexOutOfRangeException)
        {
            return null;
        }
    }

    public Cell GetCellOffset(Vector3Int tilemapPosition)
    {
        try
        {
            Vector2Int positionInBoard = new Vector2Int(tilemapPosition.x - Mathf.FloorToInt(SizeX / 2), tilemapPosition.y - Mathf.FloorToInt(SizeY / 2)) + boardOrigin;
            return board[positionInBoard.x, positionInBoard.y];
        }
        catch (System.IndexOutOfRangeException)
        {
            return null;
        }
    }

    private Vector3Int[] GetTilesPositions(Tilemap tilemap)
    {
        Vector3Int tileMapSize = tilemap.size;
        Vector3Int[] tilesPositionsInTilemap = new Vector3Int[tileMapSize.x * tileMapSize.y];
        var it = tilemap.cellBounds.allPositionsWithin;

        int currentTile = 0;
        while (it.MoveNext())
        {
            tilesPositionsInTilemap[currentTile] = it.Current;
            currentTile++;
        }

        return tilesPositionsInTilemap;
    }

    private void PutTilesInBoard(Tilemap tilemap, Vector3Int[] tilesPositions)
    {
        Vector3Int boardSize = tilemap.size;
        board = new Cell[boardSize.x, boardSize.y];

        for (int j = 0; j < boardSize.y; j++)
        {
            for (int i = 0; i < boardSize.x; i++)
            {
                int currentIndex = j * boardSize.x + i;
                Vector3Int currentTilePosition = tilesPositions[currentIndex];
                Tile associatedTileInTilemap = (Tile)tilemap.GetTile(currentTilePosition);
                board[i, j] = new Cell(currentTilePosition, tilemap.CellToWorld(currentTilePosition), associatedTileInTilemap, Size, this);
            }
        }
    }

    private void LinkAllTiles()
    {
        for (int j = 0; j < SizeY; j++)
        {
            Cell firstCellOfLine = board[0, j];
            int yPosition = firstCellOfLine.TileMapPosition.y;
            int yOffset = Mathf.Abs(yPosition % 2);

            for (int i = 0; i < SizeX; i++)
            {
                Cell left = i <= 0 ? null : board[i - 1, j];
                Cell topLeft = (i < 0 + 1 - yOffset || j >= SizeY - 1) ? null : board[i - 1 + yOffset, j + 1];
                Cell topRight = (i > SizeX - 1 - yOffset || j >= SizeY - 1) ? null : board[i + yOffset, j + 1];
                Cell right = i >= SizeX - 1 ? null : board[i + 1, j];
                Cell bottomRight = (i > SizeX - 1 - yOffset || j <= 0) ? null : board[i + yOffset, j - 1];
                Cell bottomLeft = (i < 0 + 1 - yOffset || j <= 0) ? null : board[i - 1 + yOffset, j - 1];
                board[i, j].SetNeighbours(left, topLeft, topRight, right, bottomRight, bottomLeft);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (Application.isPlaying)
        {
            Cell currentCell = GetCell(new Vector3Int(tileToInspect.x, tileToInspect.y, 0));
            Gizmos.color = Color.red;
            if (currentCell == null)
                return;

            Gizmos.DrawSphere(currentCell.WorldPosition, .2f);
            Gizmos.color = Color.cyan;

            foreach (Cell c in currentCell.GetAllNeighbours())
                if (c != null)
                    Gizmos.DrawSphere(c.WorldPosition, .2f);
        }
    }

    public void SetTileColour(Color colour, Vector3Int position)
    {
        tilemap.SetTileFlags(position, TileFlags.None);

        tilemap.SetColor(position, colour);
    }
    
    public void StartSetColorForSeconds(List<Cell> cells)
    {
        foreach (Cell cell in cells)
        {
            StartCoroutine(cell.SetColorForSeconds(new Color(1, 0.5f, 0.5f), 0.2f));
        }
    }

    public Tilemap GetTilemap()
    {
        return tilemap;
    }

    public List<Cell> GetAuthorizedAllyCells()
    {
        return allyCellsList;
    }

    public void HidePlacementTilemap()
    {
        placementTilemap.GetComponent<TilemapRenderer>().enabled = false;
    }

    private void OnDestroy()
    {
        if (CurrentBoard == this)
            CurrentBoard = null;
    }

    public bool isOccupiedNeighbour(Cell c)
    {
        Cell[] neighbourList = c.GetAllNeighbours();
        foreach(Cell cell in neighbourList)
        {
            if(cell != null)
            {
                if (cell.GetIsOccupied())
                    return true;
            }
        }
        return false;
    }

    public void ShowAllyTiles()
    {
        foreach (Cell cell in allyCellsList)
        {
            SetTileColour(new Color(1, 1, 1, 0.25f), cell.TileMapPosition);
        }
    }

    public void HideAllyTiles()
    {
        foreach (Cell cell in allyCellsList)
        {
            SetTileColour(new Color(1, 1, 1, 1), cell.TileMapPosition);
        }
    }
}

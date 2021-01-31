using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Board : MonoBehaviour
{
    [SerializeField]
    private Tilemap tilemap;

    private Cell[,] board;
    private bool isFirstCellExtremity;

    public Vector2Int Size { get { return new Vector2Int(board.GetLength(0), board.GetLength(1)); } }
    public int SizeX { get { return board.GetLength(0); } }
    public int SizeY { get { return board.GetLength(1); } }

    void Awake()
    {
        Initialize(tilemap);
    }

    public void Initialize(Tilemap tilemap)
    {
        tilemap.CompressBounds();
        Vector3Int[] tilesPositions = GetTilesPositions(tilemap);
        PutTilesInBoard(tilemap, tilesPositions);

        isFirstCellExtremity = tilemap.CellToLocal(tilesPositions[0]).x < tilemap.CellToLocal(tilesPositions[tilemap.size.x]).x;
        Debug.Log(isFirstCellExtremity);
        //Debug.Log(tilesPositions[0] + " - " + tilemap.CellToLocal(tilesPositions[0]));
        //Debug.Log(tilesPositions[tilemap.size.x] + " - " + tilemap.CellToLocal(tilesPositions[tilemap.size.x]));

        LinkAllTiles();

        for (int j = 0; j < SizeY; j++)
        {
            for (int i = 0; i < SizeX; i++)
            {
                Debug.Log(board[i,j]);
            }
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
                board[i, j] = new Cell(currentTilePosition, associatedTileInTilemap);
            }
        }
    }

    private void LinkAllTiles()
    {
        for (int j = 0; j < SizeY; j++)
        {
            for (int i = 0; i < SizeX; i++)
            {
                Cell left = i <= 0 ? null : board[i - 1, j];
                Cell topLeft = (i <= 0 || j >= SizeY - 1) ? null : board[i - 1, j + 1];
                Cell topRight = (i >= SizeX - 1 || j >= SizeY - 1) ? null : board[i + 1, j + 1];
                Cell right = i >= SizeX - 1 ? null : board[i + 1, j];
                Cell bottomRight = (i >= SizeX - 1 || j <= 0) ? null : board[i + 1, j - 1];
                Cell bottomLeft = (i <= 0 || j <= 0) ? null : board[i - 1, j - 1];
                board[i, j].SetNeighbors(left, topLeft, topRight, right, bottomRight, bottomLeft);
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Board : MonoBehaviour
{
    [SerializeField]
    private Tilemap tilemap;

    private Tile[,] board;

    // Start is called before the first frame update
    void Start()
    {
        TileBase[] tiles = tilemap.GetTilesBlock(tilemap.cellBounds);
        BoundsInt bounds = tilemap.cellBounds;
        Vector2Int size = new Vector2Int(bounds.size.x, bounds.size.y);
        Debug.Log(size);
        int i = 0;
        Tile[,] boardFromTiles = new Tile[size.x, size.y];
        MinMaxInt minMaxX = new MinMaxInt();
        MinMaxInt minMaxY = new MinMaxInt();
        foreach (Tile tile in tiles)
        {
            int x = i % size.x;
            int y = i / size.x;
            boardFromTiles[x, y] = tile;
            if (tile != null)
            {
                minMaxX.Evaluate(x);
                minMaxY.Evaluate(y);
            }
            i++;
        }

        board = new Tile[minMaxX.Count, minMaxY.Count];

        for(i = 0; i < size.x; i++)
        {
            for (int j = 0; j < size.y; j++)
            {
                //Ajouter les tiles
            }
        }
    }
}

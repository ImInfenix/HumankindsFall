using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum obstacleType
{
    Tree,
    Rock
}

public class Obstacle : MonoBehaviour
{
    private Cell[] topNeighbourCell;
    private bool transparent = false;

    [Header("TYPE")]
    public obstacleType type;

    [Header("POSITION")]
    private Board board;
    [SerializeField] private Vector3 worldPosition;
    [SerializeField] private Vector3Int currentPosition;
    [SerializeField] private Cell currentCell = null;

    [Header("PADDING")]
    public float paddingBot;
    public float paddingLeft;


    // Start is called before the first frame update
    void Start()
    {
        board = (Board)FindObjectOfType(typeof(Board));
        Vector3Int tileCoordinate = board.GetTilemap().WorldToCell(transform.position);
        Cell newCell = board.GetCell(tileCoordinate);
        currentCell = newCell;
        gameObject.SetActive(true);
        updatePosition();
        currentCell.SetIsObstacle(true);
        topNeighbourCell = currentCell.GetTopNeighbours();
    }

    // Update is called once per frame
    void Update()
    {
        if((topNeighbourCell[0] != null && topNeighbourCell[0].GetIsOccupied() == true) || (topNeighbourCell[1] != null && topNeighbourCell[1].GetIsOccupied() == true))
        {
            transparent = true;
        }
        else
        {
            transparent = false;
        }

        if(type == obstacleType.Tree)
            updateColor();
    }

    private void updatePosition()
    {
        currentPosition = currentCell.TileMapPosition;
        worldPosition = new Vector3(currentCell.WorldPosition.x+ paddingLeft, currentCell.WorldPosition.y + paddingBot, 0);
        transform.position = worldPosition;
    }  
    
    private void updateColor()
    {
        if(transparent == true)
        {
            GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 100f / 255f);
        }
        else
        {
            GetComponent<SpriteRenderer>().color = Color.white;
        }
    }
}

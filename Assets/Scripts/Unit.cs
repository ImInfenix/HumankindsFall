using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Unit : MonoBehaviour
{
    public enum Race
    {
        Orc,
        Humans
    }

    public enum Class
    {
        Warrior,
        Mage
    }

    public Race race;
    public Class clas;
    // public Object attack;

    protected int maxLife;
    protected int life;
    protected int maxMana;
    protected int mana;
    protected int armor;
    protected int moveSpeed;
    protected float attackSpeed;
    protected int damage;

    public Board board;
    private Vector3 worldPosition;
    public Vector3Int currentPosition;
    public Cell currentCell;

    private Transform thisTransform;


    // Start is called before the first frame update
    void Start()
    {
        thisTransform = transform;
        setPosition(board.GetCell(new Vector3Int(-3, -2, 0)));
        List<Cell> path = createPath(board.GetCell(new Vector3Int(1, 1, 0)));
        
        StartCoroutine(MoveOneTile(path));
    }

    IEnumerator MoveOneTile(List<Cell> path)
    {
        foreach (Cell cell in path)
        {
            yield return new WaitForSeconds(1);
            setPosition(cell);
        }
    }

    void setPosition(Cell cell)
    {
        currentCell = cell;
        currentPosition = currentCell.TileMapPosition;
        worldPosition = new Vector3(currentCell.WorldPosition.x, currentCell.WorldPosition.y, 0);
        transform.position = worldPosition;
    }

    protected List<Cell> createPath(Cell targetCell)
    {
        //On crée notre tableau local
        (Cell thisCell, bool isMarked, bool isExplorationList, float cost, Cell previousCell)[,] localBoard = new (Cell, bool, bool, float, Cell)[board.SizeX, board.SizeX];

        //On crée notre front d'exploration
        List<(int x, int y)> explorationList = new List<(int x, int y)>();

        //On initilalise notre tableau local
        for (int i = 0; i < board.SizeX; i++)
        {
            for (int j = 0; j < board.SizeX; j++)
            {
                localBoard[i, j].thisCell = board.GetCellOffset(new Vector3Int(i, j, 0));
                localBoard[i, j].isMarked = false;
                localBoard[i, j].cost = Mathf.Infinity;
                localBoard[i, j].previousCell = null;
            }
        }

        //On ajoute la cell ou ce trouve actuelle notre personnage au front d'exploration
        localBoard[currentCell.TileMapPositionOffset().x, currentCell.TileMapPositionOffset().y].cost = 0;
        explorationList.Add((currentCell.TileMapPositionOffset().x, currentCell.TileMapPositionOffset().y));
        localBoard[currentCell.TileMapPositionOffset().x, currentCell.TileMapPositionOffset().y].isExplorationList = true;


        //(Cell thisCell, bool isMarked, bool isExplorationList, float cost, Cell previousCell) truc = explorationList[0];
        //truc.cost = (float)52;
        //print("Le cost truc : " + truc.cost);
        //print("Le cost explorationList : " + explorationList[0].cost);
        //explorationList[0].isMarked = true;

        //
        bool continueSearch = true;
        int compte_prov = 0;
        do
        {
            compte_prov += 1;
            //Initialisation
            (int x, int y) localClosestCell = explorationList[0];
            float closestCost = localBoard[localClosestCell.x, localClosestCell.y].cost;
            int closestIndex = 0;

            print("La liste à l'étape " + compte_prov);
            foreach((int x, int y) localCellProv in explorationList)
            {
                print("(x,y) : " + localBoard[localCellProv.x, localCellProv.y].thisCell.TileMapPositionOffset().x + "," + localBoard[localCellProv.x, localCellProv.y].thisCell.TileMapPositionOffset().y);
            }

            //Search for the closest cell in the list
            for (int i = 1; i < explorationList.Count; i++)
            {
                if (localBoard[explorationList[i].x, explorationList[i].y].cost < closestCost)
                {
                    localClosestCell = explorationList[i];
                    closestCost = localBoard[explorationList[i].x, explorationList[i].y].cost;
                    closestIndex = i;
                }
            }

            //On marque le sommet
            localBoard[localClosestCell.x, localClosestCell.y].isMarked = true;
            explorationList.RemoveAt(closestIndex);

            print("La case marquer : " + localBoard[localClosestCell.x, localClosestCell.y].thisCell.TileMapPositionOffset().x + "," + localBoard[localClosestCell.x, localClosestCell.y].thisCell.TileMapPositionOffset().y);

            if (localBoard[localClosestCell.x, localClosestCell.y].thisCell == targetCell)
            {
                List<Cell> arrayCellPath = new List<Cell>();
                continueSearch = false;
                print("C'est fini");
                do
                {
                    print("postion (x,y) : " + localBoard[localClosestCell.x, localClosestCell.y].thisCell.TileMapPositionOffset().x + "," + localBoard[localClosestCell.x, localClosestCell.y].thisCell.TileMapPositionOffset().y);
                    arrayCellPath.Add(localBoard[localClosestCell.x, localClosestCell.y].thisCell);
                    localClosestCell = (localBoard[localClosestCell.x, localClosestCell.y].previousCell.TileMapPositionOffset().x, localBoard[localClosestCell.x, localClosestCell.y].previousCell.TileMapPositionOffset().y);
                }
                while (localBoard[localClosestCell.x, localClosestCell.y].previousCell != null);
                return arrayCellPath;
            }

            //Add all neighbours of the closest cell to the explorationList
            Cell[] arrayCell = localBoard[localClosestCell.x, localClosestCell.y].thisCell.GetAllNeighbours();

            print("Flague");
            ///foreach (Cell cellNeighbour in arrayCell)
            //{
            for(int i = 0; i < arrayCell.Length; i++)
            {
                Cell cellNeighbour = arrayCell[i];
                //print("erreur sur la case: " + cellNeighbour.TileMapPositionOffset().x + "," + cellNeighbour.TileMapPositionOffset().y);
                if (cellNeighbour != null && !localBoard[cellNeighbour.TileMapPositionOffset().x, cellNeighbour.TileMapPositionOffset().y].isMarked)
                {
                    if (localBoard[cellNeighbour.TileMapPositionOffset().x, cellNeighbour.TileMapPositionOffset().y].cost > localBoard[localClosestCell.x, localClosestCell.y].cost + 1)
                    {
                        localBoard[cellNeighbour.TileMapPositionOffset().x, cellNeighbour.TileMapPositionOffset().y].cost = localBoard[localClosestCell.x, localClosestCell.y].cost + 1;
                        localBoard[cellNeighbour.TileMapPositionOffset().x, cellNeighbour.TileMapPositionOffset().y].previousCell = localBoard[localClosestCell.x, localClosestCell.y].thisCell;
                    }

                    if (!localBoard[cellNeighbour.TileMapPositionOffset().x, cellNeighbour.TileMapPositionOffset().y].isExplorationList)
                    {
                        explorationList.Add((cellNeighbour.TileMapPositionOffset().x, cellNeighbour.TileMapPositionOffset().y));
                        localBoard[cellNeighbour.TileMapPositionOffset().x, cellNeighbour.TileMapPositionOffset().y].isExplorationList = true;
                    }
                }
            }
        }
        while (continueSearch && compte_prov < 100);
        print("compte_prov : " + compte_prov);
        return null;
    }
}

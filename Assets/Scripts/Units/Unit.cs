using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class Unit : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    public RaceStat raceStats;
    public ClassStat classStat;

    private Transform thisTransform;

    private Race race;
    private Class clas;

    private int maxLife;
    private int life;
    private int maxMana;
    private int mana;
    private int armor;
    private int moveSpeed;
    private float attackSpeed;
    private int damage;
    [SerializeField] private int range;

    public Board board;
    private Vector3 worldPosition;
    public Vector3Int currentPosition;
    public Cell currentCell;


    private List<Cell> path;

    //debug test positions
    public Vector3Int initialPos;
    private Vector3Int targetPos;
    private int targetDistance;
    private bool hasTarget;
    private bool pathCreated;
    private bool canAttack;


    // Start is called before the first frame update
    void Start()
    {
        race = raceStats.race;
        clas = classStat.clas;

        maxLife = raceStats.maxLife + classStat.maxLife;
        life = maxLife;
        maxMana = raceStats.maxMana + classStat.maxMana;
        mana = raceStats.mana + classStat.mana;
        armor = raceStats.armor + classStat.armor;
        moveSpeed = raceStats.moveSpeed + classStat.moveSpeed;
        attackSpeed = raceStats.attackSpeed + classStat.attackSpeed;
        damage = raceStats.damage + classStat.damage;
        range = classStat.range;

        /*
        print("longueur : " + board.SizeX);
        print("hauteur : " + board.SizeY);
        print("offset x : " + Mathf.FloorToInt(board.SizeX/2));
        print("offset y : " + Mathf.FloorToInt(board.SizeY/2));
        */

        range = 1;
        canAttack = true;
        hasTarget = false;

        thisTransform = transform;
        setPosition(board.GetCell(new Vector3Int(initialPos.x, initialPos.y, 0)));
        pathCreated = false;
    }

    private void Update()
    {
        //if the unit is not following a path yet and has a target cell different from their current cell
        if (!pathCreated /*&& hasTarget*/ && (targetPos.x != currentPosition.x || targetPos.y != currentPosition.y))
        {
            //get the path
            //path = createPath(board.GetCell(new Vector3Int(targetPos.x, targetPos.y, 0)));

            //if the target is not yet in range of attack
            if (targetDistance > range)
            {
                //--- DEBUG DISPLAY PATH ---
                string cellsPath = "";
                foreach (Cell cell in path)
                {
                    cellsPath += cell.TileMapPosition + " ";
                }
                print("path : " + cellsPath);
                print("targetDistance : " + targetDistance);
                print("range : " + range);

                //get the target to follow
                pathCreated = true;

                //start the coroutine to move to the next cell of the path
                StartCoroutine(MoveToCell(path[0]));
            }

            else
            {
                if (canAttack)
                    StartCoroutine(AttackTarget());
            }

            //check if a better target can be selected
            findTarget();
        }
    }

    //follow a path represented by a list of cells to cross
    IEnumerator MoveFollowingPath(List<Cell> path)
    {
        foreach (Cell cell in path)
        {
            yield return new WaitForSeconds(0.6f);
            setPosition(cell);
            //print(cell.TileMapPositionOffset());
            pathCreated = false;
        }
    }

    IEnumerator MoveToCell(Cell cell)
    {
        yield return new WaitForSeconds(0.6f);
        setPosition(cell);
        //print(cell.TileMapPositionOffset());
        pathCreated = false;
    }

    IEnumerator AttackTarget()
    {
        canAttack = false;
        print("attacking target at position " + targetPos);
        yield return new WaitForSeconds(1f);
        canAttack = true;
    }

    //set the position of the unit in a cell
    public void setPosition(Cell cell)
    {
        currentCell = cell;
        currentPosition = currentCell.TileMapPosition;
        worldPosition = new Vector3(currentCell.WorldPosition.x, currentCell.WorldPosition.y, 0);
        transform.position = worldPosition;
    }

    public Vector3Int getPosition()
    {
        return (currentPosition);
    }

    public Cell getCell()
    {
        return currentCell;
    }

    //create a list of cells which defines the path to follow to move toward a target cell
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

        //print("local board x :" + localBoard.Length);

        (int x, int y) currentCellPosition = (currentCell.TileMapPositionOffset().x, currentCell.TileMapPositionOffset().y);

        //On ajoute la cell où se trouve actuelle notre personnage au front d'exploration avec un coût de 0
        localBoard[currentCellPosition.x, currentCellPosition.y].cost = 0;
        explorationList.Add((currentCellPosition.x, currentCellPosition.y));
        localBoard[currentCellPosition.x, currentCellPosition.y].isExplorationList = true;


        //(Cell thisCell, bool isMarked, bool isExplorationList, float cost, Cell previousCell) truc = explorationList[0];
        //truc.cost = (float)52;
        //print("Le cost truc : " + truc.cost);
        //print("Le cost explorationList : " + explorationList[0].cost);
        //explorationList[0].isMarked = true;

        //determines the path to the target cell by explorating neighbour cells of a cell in the exploration list
        bool continueSearch = true;
        int debugSafetyCount = 0;
        do
        {
            debugSafetyCount += 1;

            if (explorationList.Count > 0)
            {

                //Initialisation
                (int x, int y) localClosestCell = explorationList[0];
                float closestCost = localBoard[localClosestCell.x, localClosestCell.y].cost;
                int closestIndex = 0;


                /*print("La liste à l'étape " + debugSafetyCount);
                foreach((int x, int y) localCellProv in explorationList)
                {
                    print("(x,y) : " + localBoard[localCellProv.x, localCellProv.y].thisCell.TileMapPosition.x + "," + localBoard[localCellProv.x, localCellProv.y].thisCell.TileMapPosition.y);
                }*/

                //Search for the closest cell in the exploration list
                for (int i = 1; i < explorationList.Count; i++)
                {
                    if (localBoard[explorationList[i].x, explorationList[i].y].cost < closestCost)
                    {
                        localClosestCell = explorationList[i];
                        closestCost = localBoard[explorationList[i].x, explorationList[i].y].cost;
                        closestIndex = i;
                    }
                }

                //Mark the selected node and remove it from exploration list
                localBoard[localClosestCell.x, localClosestCell.y].isMarked = true;
                explorationList.RemoveAt(closestIndex);

                //print("La case marquée : " + localBoard[localClosestCell.x, localClosestCell.y].thisCell.TileMapPosition.x + "," + localBoard[localClosestCell.x, localClosestCell.y].thisCell.TileMapPosition.y);

                //if the cell we are explorating is the target cell, return the path to this cell
                if (localBoard[localClosestCell.x, localClosestCell.y].thisCell == targetCell)
                {
                    List<Cell> arrayCellPath = new List<Cell>();
                    continueSearch = false;
                    //print("C'est fini");
                    do
                    {
                        //add the previous cell of the path to the list, then go to this cell
                        //print("postion (x,y) : " + localBoard[localClosestCell.x, localClosestCell.y].thisCell.TileMapPosition.x + "," + localBoard[localClosestCell.x, localClosestCell.y].thisCell.TileMapPosition.y);
                        arrayCellPath.Add(localBoard[localClosestCell.x, localClosestCell.y].thisCell);
                        localClosestCell = (localBoard[localClosestCell.x, localClosestCell.y].previousCell.TileMapPositionOffset().x, localBoard[localClosestCell.x, localClosestCell.y].previousCell.TileMapPositionOffset().y);
                    }
                    while (localBoard[localClosestCell.x, localClosestCell.y].previousCell != null); //while there are previous cells existing in the path
                                                                                                     //reverse to start from the first cell
                    arrayCellPath.Reverse();
                    return arrayCellPath;
                }

                //Get all the neighbours of the cell we are explorating
                Cell[] arrayCell = localBoard[localClosestCell.x, localClosestCell.y].thisCell.GetAllNeighbours();

                //print("Flag");

                //go through all the neighbours of the cell
                for (int i = 0; i < arrayCell.Length; i++)
                {
                    Cell cellNeighbour = arrayCell[i];
                    //if the neighbour cell exists and is not already marked
                    if (cellNeighbour != null && !localBoard[cellNeighbour.TileMapPositionOffset().x, cellNeighbour.TileMapPositionOffset().y].isMarked)
                    {
                        //print("voisin : " + cellNeighbour.TileMapPosition);
                        (int x, int y) cellNeighbourPosition = (cellNeighbour.TileMapPositionOffset().x, cellNeighbour.TileMapPositionOffset().y);

                        //if the cost of the neighbour is bigger than the cost of this cell + 1,
                        //it means that there is a shorter path than the one the neighbour knows which
                        if (localBoard[cellNeighbourPosition.x, cellNeighbourPosition.y].cost > localBoard[localClosestCell.x, localClosestCell.y].cost + 1)
                        {
                            localBoard[cellNeighbourPosition.x, cellNeighbourPosition.y].cost = localBoard[localClosestCell.x, localClosestCell.y].cost + 1;
                            localBoard[cellNeighbourPosition.x, cellNeighbourPosition.y].previousCell = localBoard[localClosestCell.x, localClosestCell.y].thisCell;
                        }

                        if (!localBoard[cellNeighbourPosition.x, cellNeighbourPosition.y].isExplorationList)
                        {
                            explorationList.Add((cellNeighbourPosition.x, cellNeighbourPosition.y));
                            localBoard[cellNeighbourPosition.x, cellNeighbourPosition.y].isExplorationList = true;
                        }
                    }
                }
            }
        }
        while (continueSearch && debugSafetyCount < 100); //while we haven't found the path to the target cell
        //print("debugSafetyCount : " + debugSafetyCount);
        return null;

    public void OnDrag(PointerEventData eventData)
    {
        throw new System.NotImplementedException();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        throw new System.NotImplementedException();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        throw new System.NotImplementedException();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        throw new System.NotImplementedException();
    }

    //find a target unit and get the position target
    private int findTarget()
    {
        string targetTag = "UnitAlly";

        //if the unit is an ally unit
        if (CompareTag("UnitAlly"))
            //it should target enemy units
            targetTag = "UnitEnemy";

        //get the list of possible targets, e.g. units the opposing team
        GameObject[] targetList = GameObject.FindGameObjectsWithTag(targetTag);

        //if there is at least one opponent alive
        if (targetList.Length > 0)
        {
            //get the distance toward the current target
            int currentTargetDistance;

            //if the unit has no target unit, the current target cell is irrelevant so we set a high distance
            if (!hasTarget)
                currentTargetDistance = 1000;
            else
                currentTargetDistance = createPath(board.GetCell(new Vector3Int(targetPos.x, targetPos.y, 0))).Count;

            //print("target list length : " + targetList.Length);

            //check every possible target in the list
            foreach (GameObject possibleTarget in targetList)
            {
                Cell possibleTargetCell = possibleTarget.GetComponent<Unit>().getCell();
                int possibleTargetDistance = createPath(possibleTargetCell).Count;

                //if there is an opponent closer than the current target
                if (possibleTargetDistance < currentTargetDistance)
                {
                    //change the target position with the new opponents's position
                    targetPos = possibleTarget.GetComponent<Unit>().getPosition();
                    hasTarget = true;
                }
            }
        }

        path = createPath(board.GetCell(new Vector3Int(targetPos.x, targetPos.y, 0)));
        targetDistance = path.Count - 1;
        return targetDistance;
    }
}

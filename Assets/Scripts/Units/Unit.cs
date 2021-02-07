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

    private static int infVal = 1000;

    private int maxLife;
    [SerializeField] private int currentLife;
    private int maxMana;
    private int mana;
    private int armor;
    private float moveSpeed;
    private float attackSpeed;
    private int damage;
    private int range;

    public Board board;
    private Vector3 worldPosition;
    public Vector3Int currentPosition;
    public Cell currentCell = null;
    private List<Cell> path = null;

    public Vector3Int initialPos;
    private Vector3Int targetPos;
    private Unit targetUnit = null;
    private int targetDistance = infVal;
    private bool hasTarget = false;
    private bool isActing = false;
    string targetTag = "UnitAlly";


    // Start is called before the first frame update
    void Start()
    {
        race = raceStats.race;
        clas = classStat.clas;

        maxLife = raceStats.maxLife + classStat.maxLife;
        currentLife = maxLife;
        maxMana = raceStats.maxMana + classStat.maxMana;
        mana = raceStats.mana + classStat.mana;
        armor = raceStats.armor + classStat.armor;
        moveSpeed = raceStats.moveSpeed + classStat.moveSpeed;
        attackSpeed = raceStats.attackSpeed + classStat.attackSpeed;
        damage = raceStats.damage + classStat.damage;
        range = classStat.range;


        //canAttack = true;
        hasTarget = false;
        isActing = false;

        thisTransform = transform;
        setPosition(board.GetCell(new Vector3Int(initialPos.x, initialPos.y, 0)));

        //if the unit is an ally unit
        if (CompareTag("UnitAlly"))
            //it should target enemy units
            targetTag = "UnitEnemy";

    }

    private void Update()
    {
        //findTarget();
        checkDeath();

        /*if (!hasTarget)
        {
            findTarget();
        }*/

        //if the unit is not following a path yet and has a target cell different from their current cell
        if (!isActing)
        {
            //if the target is not yet in range of attack
            if (targetDistance > range && targetUnit != null && path != null)
            {
                //start the coroutine to move to the next cell of the path
                StartCoroutine(MoveToCell(path[0]));
            }
            else if(targetDistance <= range && targetUnit != null && path != null)
            {
                //start the coroutine to attack the target
                StartCoroutine(AttackTarget());
            }

            //check if a better target can be selected
            findTarget();
        }
    }

    //follow a path represented by a list of cells to cross
    IEnumerator MoveFollowingPath(List<Cell> path)
    {
        isActing = true;
        foreach (Cell cell in path)
        {
            yield return new WaitForSeconds(moveSpeed);
            setPosition(cell);
            //print(cell.TileMapPositionOffset());
        }
        isActing = false;
    }

    IEnumerator MoveToCell(Cell cell)
    {
        if (cell.GetIsOccupied() == false)
        {
            isActing = true;
            setPosition(cell);
            yield return new WaitForSeconds(moveSpeed);
            //print(cell.TileMapPositionOffset());
            isActing = false;
        }
    }

    IEnumerator AttackTarget()
    {
        isActing = true;
        targetUnit.takeDamage(damage);

        yield return new WaitForSeconds(attackSpeed);
        isActing = false;
    }

    //set the position of the unit in a cell
    public void setPosition(Cell cell)
    {
        if (currentCell != null)
        {
            currentCell.SetIsOccupied(false);
        }
        currentCell = cell;
        //board[][]
        currentCell.SetIsOccupied(true);
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

        List<Cell> arrayCellPath = new List<Cell>();

        if (targetCell == currentCell)
        {
            return arrayCellPath;
        }

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

                //if the cell we are explorating is the target cell, return the path to this cell
                if (localBoard[localClosestCell.x, localClosestCell.y].thisCell == targetCell)
                {
                    continueSearch = false;

                    do
                    {
                        //add the previous cell of the path to the list, then go to this cell
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

                //go through all the neighbours of the cell
                for (int i = 0; i < arrayCell.Length; i++)
                {
                    Cell cellNeighbour = arrayCell[i];
                    //if the neighbour cell exists and is not already marked
                    if (cellNeighbour != null && !localBoard[cellNeighbour.TileMapPositionOffset().x, cellNeighbour.TileMapPositionOffset().y].isMarked)
                    {
                        (int x, int y) cellNeighbourPosition = (cellNeighbour.TileMapPositionOffset().x, cellNeighbour.TileMapPositionOffset().y);

                        //if the cost of the neighbour is bigger than the cost of this cell + 1,
                        //it means that there is a shorter path than the one the neighbour knows which
                        if (localBoard[cellNeighbourPosition.x, cellNeighbourPosition.y].cost > localBoard[localClosestCell.x, localClosestCell.y].cost + 1)
                        {
                            localBoard[cellNeighbourPosition.x, cellNeighbourPosition.y].cost = localBoard[localClosestCell.x, localClosestCell.y].cost + 1;
                            localBoard[cellNeighbourPosition.x, cellNeighbourPosition.y].previousCell = localBoard[localClosestCell.x, localClosestCell.y].thisCell;
                        }

                        if (!localBoard[cellNeighbourPosition.x, cellNeighbourPosition.y].isExplorationList && (cellNeighbour.GetIsOccupied() == false || cellNeighbour == targetCell))
                        {
                            explorationList.Add((cellNeighbourPosition.x, cellNeighbourPosition.y));
                            localBoard[cellNeighbourPosition.x, cellNeighbourPosition.y].isExplorationList = true;
                        }
                    }
                }
            }
        }
        while (continueSearch && debugSafetyCount < 100); //while we haven't found the path to the target cell

        return null;
    }


    //find a target unit and get the position target
    private int findTarget()
    {
        targetDistance = infVal;
        targetUnit = null;
        hasTarget = false;
        path = null;

        //get the list of possible targets, i.e. units the opposing team
        GameObject[] targetList = GameObject.FindGameObjectsWithTag(targetTag);

        //if there is at least one opponent alive
        if (targetList.Length > 0)
        {
            //check every possible target in the list
            foreach (GameObject possibleTarget in targetList)
            {
                Cell possibleTargetCell = possibleTarget.GetComponent<Unit>().getCell();

                List<Cell> pathProv = createPath(possibleTargetCell);
                if (pathProv != null)
                {
                    int possibleTargetDistance = pathProv.Count;

                    //if there is an opponent closer than the current target
                    if (possibleTargetDistance < targetDistance)
                    {
                        //change the target position with the new opponents's position
                        targetUnit = possibleTarget.GetComponent<Unit>();
                        targetPos = targetUnit.getPosition();
                        path = pathProv;
                        hasTarget = true;
                        targetDistance = path.Count;
                    }
                }
            }
        }

        return targetDistance;
    }

    private void checkDeath()
    {
        if(currentLife <= 0)
        {
            currentCell.SetIsOccupied(false);
            Destroy(this.gameObject);
        }
    }

    public void takeDamage(int damage)
    {
        currentLife -= damage;
    }

    private void OnDrawGizmosSelected()
    {
        if (Application.isPlaying)
        {
            Gizmos.color = Color.red;
            if (targetUnit == null || currentCell == null || hasTarget == false)
                return;

            Gizmos.DrawLine(transform.position, targetUnit.transform.position);
        }
    }
    
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
}

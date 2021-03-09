using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PathfindingTool
{
    public static int infVal = 1000;

    //find a target unit and get the position target
    public static List<Cell> findTarget(Board board, Cell currentCell, string targetTag)
    {
        int targetDistance = infVal;
        //Unit targetUnit = null;
        List<Cell> path = null;

        //get the list of possible targets, i.e. units the opposing team
        GameObject[] targetList = GameObject.FindGameObjectsWithTag(targetTag);

        //if there is at least one opponent alive
        if (targetList.Length > 0)
        {
            //check every possible target in the list
            foreach (GameObject possibleTarget in targetList)
            {
                Cell possibleTargetCell = possibleTarget.GetComponent<Unit>().getCell();

                List<Cell> pathProv = PathfindingTool.createPath(board, currentCell, possibleTargetCell);
                if (pathProv != null)
                {
                    int possibleTargetDistance = pathProv.Count;

                    //if there is an opponent closer than the current target
                    if (possibleTargetDistance < targetDistance)
                    {
                        //change the target position with the new opponents's position
                        path = pathProv;
                        targetDistance = path.Count;
                    }
                }
            }
        }

        return path;
    }

    //create a list of cells which defines the path to follow to move toward a target cell
    public static List<Cell> createPath(Board board, Cell currentCell, Cell targetCell)
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

        (int x, int y) currentCellPosition = (currentCell.TileMapPositionOffset().x, currentCell.TileMapPositionOffset().y);

        //On ajoute la cell où se trouve actuelle notre personnage au front d'exploration avec un coût de 0
        localBoard[currentCellPosition.x, currentCellPosition.y].cost = 0;
        explorationList.Add((currentCellPosition.x, currentCellPosition.y));
        localBoard[currentCellPosition.x, currentCellPosition.y].isExplorationList = true;

        //determines the path to the target cell by explorating neighbour cells of a cell in the exploration list
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

                        if (!localBoard[cellNeighbourPosition.x, cellNeighbourPosition.y].isExplorationList && (cellNeighbour.GetIsOccupied() == false || cellNeighbour == targetCell) && !localBoard[cellNeighbourPosition.x, cellNeighbourPosition.y].isMarked)
                        {
                            explorationList.Add((cellNeighbourPosition.x, cellNeighbourPosition.y));
                            localBoard[cellNeighbourPosition.x, cellNeighbourPosition.y].isExplorationList = true;
                        }
                    }
                }
            }
        }
        while (explorationList.Count > 0); //while we haven't found the path to the target cell

        return null;
    }

    public static List<Cell> createPathTarget(Unit unit)
    {
        List<Cell> arrayCellPath = new List<Cell>();

        if (PathfindingTool.unitsInRadius(unit.currentCell, unit.Range, unit.getTargetTag()).Count > 0)
        {
            return arrayCellPath;
        }

        //On crée notre tableau local
        (Cell thisCell, bool isMarked, bool isExplorationList, float cost, Cell previousCell)[,] localBoard = new (Cell, bool, bool, float, Cell)[unit.board.SizeX, unit.board.SizeX];

        //On crée notre front d'exploration
        List<(int x, int y)> explorationList = new List<(int x, int y)>();

        //On initilalise notre tableau local
        for (int i = 0; i < unit.board.SizeX; i++)
        {
            for (int j = 0; j < unit.board.SizeX; j++)
            {
                localBoard[i, j].thisCell = unit.board.GetCellOffset(new Vector3Int(i, j, 0));
                localBoard[i, j].isMarked = false;
                localBoard[i, j].cost = Mathf.Infinity;
                localBoard[i, j].previousCell = null;
            }
        }

        (int x, int y) currentCellPosition = (unit.currentCell.TileMapPositionOffset().x, unit.currentCell.TileMapPositionOffset().y);

        //On ajoute la cell où se trouve actuelle notre personnage au front d'exploration avec un coût de 0
        localBoard[currentCellPosition.x, currentCellPosition.y].cost = 0;
        explorationList.Add((currentCellPosition.x, currentCellPosition.y));
        localBoard[currentCellPosition.x, currentCellPosition.y].isExplorationList = true;

        //determines the path to the target cell by explorating neighbour cells of a cell in the exploration list
        do
        {

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
                List<Unit> listAvailableTarget = PathfindingTool.unitsInRadius(localBoard[localClosestCell.x, localClosestCell.y].thisCell, unit.Range, unit.getTargetTag());
                
                if (listAvailableTarget.Count > 0)
                {
                    unit.setTargetUnit(listAvailableTarget[0]);
                    
                    do
                    {
                        //add the previous cell of the path to the list, then go to this cell
                        arrayCellPath.Add(localBoard[localClosestCell.x, localClosestCell.y].thisCell);
                        localClosestCell = (localBoard[localClosestCell.x, localClosestCell.y].previousCell.TileMapPositionOffset().x, localBoard[localClosestCell.x, localClosestCell.y].previousCell.TileMapPositionOffset().y);
                    }
                    while (localBoard[localClosestCell.x, localClosestCell.y].previousCell != null); //while there are previous cells existing in the path
                                                                                                     //reverse to start from the first cell
                    arrayCellPath.Reverse();

                    //Debug.Log(arrayCellPath.Count);

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

                        if (!localBoard[cellNeighbourPosition.x, cellNeighbourPosition.y].isExplorationList && (cellNeighbour.GetIsOccupied() == false /*|| cellNeighbour == targetCell*/) && !localBoard[cellNeighbourPosition.x, cellNeighbourPosition.y].isMarked)
                        {
                            explorationList.Add((cellNeighbourPosition.x, cellNeighbourPosition.y));
                            localBoard[cellNeighbourPosition.x, cellNeighbourPosition.y].isExplorationList = true;
                        }
                    }
                }
            }
        }
        while (explorationList.Count > 0); //while we haven't found the path to the target cell

        return null;
    }

    public static List<Unit> unitsInRadius(Cell centerCell, int radius, string tagUnit)
    {
        List<Unit> listUnit = new List<Unit>();


        List<Cell> frontExploration = new List<Cell>();

        List<Cell> banList = new List<Cell>();

        frontExploration.Add(centerCell);

        List<Cell> newFrontExploration = new List<Cell>();

        for (int i = 0; i <= radius; i++)
        {
            foreach (Cell cellProv in frontExploration)
            {
                //cellProv.SetRed();
                if (cellProv.GetCurrentUnit() != null && cellProv.GetCurrentUnit().CompareTag(tagUnit))
                {
                    //Debug.Log("cellProv : " + cellProv.TileMapPosition);
                    listUnit.Add(cellProv.GetCurrentUnit());
                }
            }

            if (i < radius)
            {
                foreach (Cell cellProv in frontExploration)
                {
                    Cell[] neighbourCells = cellProv.GetAllNeighbours();

                    foreach (Cell neighbourCell in neighbourCells)
                    {
                        if (!newFrontExploration.Contains(neighbourCell) && !frontExploration.Contains(neighbourCell) && !banList.Contains(neighbourCell) && neighbourCell != null)
                        {
                            newFrontExploration.Add(neighbourCell);
                        }
                    }
                }

                banList.Clear();

                foreach (Cell cell in frontExploration)
                {
                    banList.Add(cell);
                }

                frontExploration.Clear();

                foreach(Cell cell in newFrontExploration)
                {
                    frontExploration.Add(cell);
                }

                newFrontExploration.Clear();
            }
        }

        return listUnit;
    }

    public static List<Cell> cellsInRadius(Cell centerCell, int radius)
    {
        List<Cell> listCells = new List<Cell>();

        List<Cell> frontExploration = new List<Cell>();

        List<Cell> banList = new List<Cell>();

        frontExploration.Add(centerCell);

        List<Cell> newFrontExploration = new List<Cell>();

        for (int i = 0; i <= radius; i++)
        {
            foreach (Cell cellProv in frontExploration)
            {
                //cellProv.SetRed();
                //Debug.Log("cellProv : " + cellProv.TileMapPosition);
                listCells.Add(cellProv);
            }

            if (i < radius)
            {
                foreach (Cell cellProv in frontExploration)
                {
                    Cell[] neighbourCells = cellProv.GetAllNeighbours();

                    foreach (Cell neighbourCell in neighbourCells)
                    {
                        if (!newFrontExploration.Contains(neighbourCell) && !frontExploration.Contains(neighbourCell) && !banList.Contains(neighbourCell) && neighbourCell != null)
                        {
                            newFrontExploration.Add(neighbourCell);
                        }
                    }
                }

                banList.Clear();

                foreach (Cell cell in frontExploration)
                {
                    banList.Add(cell);
                }

                frontExploration.Clear();

                foreach (Cell cell in newFrontExploration)
                {
                    frontExploration.Add(cell);
                }

                newFrontExploration.Clear();
            }
        }

        return listCells;
    }
}
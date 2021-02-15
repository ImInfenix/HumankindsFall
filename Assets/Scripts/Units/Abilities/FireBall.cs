using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : Ability
{
    private void Start()
    {
        castStaminaThreshold = 3;
    }
    override public void castAbility()
    {
        Debug.Log("Coucou !");
        unit.setIsAbilityActivated(true);

        List<Cell> listCells = PathfindingTool.cellsInRadius(unit.currentCell, 4);

        int maxTargetTouch = 0;
        Cell bestTargetCell = null;
        List<Unit> listUnitsTouch = new List<Unit>();

        foreach (Cell cell in listCells)
        {
            List<Unit> listUnitsTouchProv = PathfindingTool.unitsInRadius(cell, 6, unit.getTargetTag());
            int maxTargetTouchProv = listUnitsTouchProv.Count;

            if (maxTargetTouchProv > maxTargetTouch)
            {
                bestTargetCell = cell;
                maxTargetTouch = maxTargetTouchProv;
                listUnitsTouch = listUnitsTouchProv;
            }
        }

        print("maxTargetTouch : " + maxTargetTouch);

        if (bestTargetCell != null)
        {
            print("flagsdfqsf");
            print("listUnitsTouch : " + listUnitsTouch.Count);
            foreach (Unit unit in listUnitsTouch)
            {
                print("mort !");
                unit.takeDamage(1000);
            }

            //unit.setIsAbilityActivated(false);
        }
        unit.setIsAbilityActivated(false);
    }
}

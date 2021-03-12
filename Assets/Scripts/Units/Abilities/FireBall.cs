using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : Ability
{
    [SerializeField] private GameObject projectileGameObject;

    private int basicRange;

    private void Start()
    {
        castStaminaThreshold = 8;
        castRange = 4;
        areaOfEffect = 1;
        power = 35;

        projectileGameObject = Resources.Load("Ability Prefabs/Fireball") as GameObject;
    }
    override public void castAbility()
    {
        unit.setIsAbilityActivated(true);

        basicRange = unit.getRange();
        unit.setRange(castRange);

        List<Cell> listCells = PathfindingTool.cellsInRadius(unit.currentCell, castRange);

        int maxTargetTouch = 0;
        Cell bestTargetCell = null;
        List<Unit> listUnitsTouch = new List<Unit>();

        foreach (Cell cell in listCells)
        {
            List<Unit> listUnitsTouchProv = PathfindingTool.unitsInRadius(cell, areaOfEffect, unit.getTargetTag());
            int maxTargetTouchProv = listUnitsTouchProv.Count;

            if (maxTargetTouchProv > maxTargetTouch)
            {
                bestTargetCell = cell;
                maxTargetTouch = maxTargetTouchProv;
                listUnitsTouch = listUnitsTouchProv;
            }
        }

        //print("maxTargetTouch : " + maxTargetTouch);

        if (bestTargetCell != null)
        {
            List<Cell> listCellsTouched = PathfindingTool.cellsInRadius(bestTargetCell, areaOfEffect);
            StartCoroutine(ProjectileAnimation(bestTargetCell, listCellsTouched));
            unit.setRange(basicRange);
        }
        unit.setIsAbilityActivated(false);
    }
    
    IEnumerator ProjectileAnimation(Cell targetCell, List<Cell> listCells)
    {
        Vector3 startPosition = transform.position;
        GameObject projectile = Instantiate(projectileGameObject, startPosition, Quaternion.identity, transform);

        //set the speed of the animation (distance at each iteration of while loop)
        float speed = 0.06f;

        //set the maximum number of refresh of the projectile animation
        float maxRefresh = 1 / speed;

        //time to wait between every movement
        float refreshRate = 0.01f;

        int numberOfRefresh = 0;

        //get the distance between start position and target position
        Vector3 distance = targetCell.WorldPosition - projectile.transform.position;

        //set the correct angle
        float angle = Mathf.Atan2(distance.y, distance.x) * Mathf.Rad2Deg;
        projectile.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        //while there is still a target, the projectile is not arrived at the target position and the max number of refresh is not reached
        while (targetCell != null && projectile.transform.position != targetCell.WorldPosition && numberOfRefresh <= maxRefresh)
        {
            projectile.transform.position += distance * speed;
            numberOfRefresh++;
            yield return new WaitForSeconds(refreshRate);
        }

        Destroy(projectile);

        List<Unit> listUnitsTouch = PathfindingTool.unitsInRadius(targetCell, areaOfEffect, unit.getTargetTag());

        foreach (Unit unit in listUnitsTouch)
        {
            unit.takeDamage(power);
        }

        //color all hit tiles in red for a short duration, then set the color back to normal
        GameObject.Find("Board").GetComponent<Board>().StartSetColorForSeconds(listCells);
    }
}

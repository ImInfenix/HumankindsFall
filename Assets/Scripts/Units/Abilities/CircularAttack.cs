using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircularAttack : Ability
{
    private GameObject animationGameObject;

    private void Awake()
    {
        castStaminaThreshold = 800;
        castRange = 0;
        areaOfEffect = 1;
        BasePower = 35;

        animationGameObject = Resources.Load("Ability Prefabs/CircularAttack") as GameObject;
    }
    override public void castAbility()
    {
        base.castAbility();

        List<Unit> listUnitsHitProv = PathfindingTool.unitsInRadius(unit.currentCell, areaOfEffect, unit.getTargetTag());

        Instantiate(animationGameObject, transform.position, Quaternion.identity, transform);

        foreach (Unit unit in listUnitsHitProv)
            unit.takeDamage(currentPower);
    }
}

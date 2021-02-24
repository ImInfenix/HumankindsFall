using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircularAttack : Ability
{
    private GameObject animationGameObject;

    private void Start()
    {
        castStaminaThreshold = 6;
        castRange = 0;
        areaOfEffect = 1;
        power = 45;

        animationGameObject = Resources.Load("Ability Prefabs/CircularAttack") as GameObject;
    }
    override public void castAbility()
    {
        unit.setIsAbilityActivated(true);

        List<Unit> listUnitsHitProv = PathfindingTool.unitsInRadius(unit.currentCell, areaOfEffect, unit.getTargetTag());

        Instantiate(animationGameObject, transform.position, Quaternion.identity, transform);

        foreach (Unit unit in listUnitsHitProv)
            unit.takeDamage(power);


        unit.setIsAbilityActivated(false);
    }
}

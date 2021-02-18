using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircularAttack : Ability
{

    private void Start()
    {
        castStaminaThreshold = 6;
        castRange = 0;
        areaOfEffect = 1;
        power = 45;
    }
    override public void castAbility()
    {
        unit.setIsAbilityActivated(true);

        List<Unit> listUnitsHealProv = PathfindingTool.unitsInRadius(unit.currentCell, areaOfEffect, unit.getTargetTag());

        foreach (Unit unit in listUnitsHealProv)
            unit.takeDamage(power);


        unit.setIsAbilityActivated(false);
    }
}

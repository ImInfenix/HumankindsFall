using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealZone : Ability
{

    private void Start()
    {
        castStaminaThreshold = 5;
        castRange = 0;
        areaOfEffect = 2;
        power = 30;
    }
    override public void castAbility()
    {
        unit.setIsAbilityActivated(true);

        List<Unit> listUnitsHealProv = PathfindingTool.unitsInRadius(unit.currentCell, areaOfEffect, unit.tag);

        foreach (Unit unit in listUnitsHealProv)
            unit.heal(power);

        unit.setIsAbilityActivated(false);
    }
}
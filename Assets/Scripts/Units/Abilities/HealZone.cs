using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealZone : Ability
{
    private void Awake()
    {
        castStaminaThreshold = 500;
        castRange = 0;
        areaOfEffect = 2;
        BasePower = 30;
        soundEffect = Resources.Load("SoundEffects/magic-glitter-shot") as AudioClip;
    }
    override public void castAbility()
    {
        base.castAbility();

        List<Unit> listUnitsHealProv = PathfindingTool.unitsInRadius(unit.currentCell, areaOfEffect, unit.tag);

        playSound();

        foreach (Unit unit in listUnitsHealProv)
        {
            if (unit.classStat.clas != Class.Healer)
                unit.heal(currentPower);
        }
    }
}
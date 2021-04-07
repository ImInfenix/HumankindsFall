using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealZone : Ability
{
    private GameObject animationGameObject;

    private void Awake()
    {
        castStaminaThreshold = 500;
        castRange = 0;
        areaOfEffect = 2;
        BasePower = 30;
        soundEffect = Resources.Load("SoundEffects/magic-glitter-shot") as AudioClip;

        animationGameObject = Resources.Load("Ability Prefabs/HealZone") as GameObject;
    }
    override public void castAbility()
    {
        base.castAbility();

        List<Unit> listUnitsHealProv = PathfindingTool.unitsInRadius(unit.currentCell, areaOfEffect, unit.tag);

        playSound();
        Instantiate(animationGameObject, transform.position, Quaternion.identity, transform);

        foreach (Unit unit in listUnitsHealProv)
        {
            if (unit.classStat.clas != Class.Healer)
                unit.heal(currentPower);
        }
    }
}

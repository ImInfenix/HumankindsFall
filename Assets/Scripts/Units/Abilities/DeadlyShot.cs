using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadlyShot : Ability
{
    [SerializeField] private GameObject projectileGameObject;

    private void Start()
    {
        castStaminaThreshold = 7;
        power = 45;
    }
    override public void castAbility()
    {
        unit.setIsAbilityActivated(true);

        GameObject[] listEnemyUnits = GameObject.FindGameObjectsWithTag(unit.getTargetTag());

        if (listEnemyUnits.Length > 0)
        {
            Unit targetUnit = listEnemyUnits[0].GetComponent<Unit>();
            int minLifeProv = targetUnit.getCurrentLife();
            

            foreach (GameObject unit in listEnemyUnits)
            {
                if(unit.GetComponent<Unit>().getCurrentLife() < minLifeProv)
                {
                    minLifeProv = unit.GetComponent<Unit>().getCurrentLife();
                    targetUnit = unit.GetComponent<Unit>();
                }
            }

            targetUnit.takeDamage(power);
        }

        unit.setIsAbilityActivated(false);
    }
}
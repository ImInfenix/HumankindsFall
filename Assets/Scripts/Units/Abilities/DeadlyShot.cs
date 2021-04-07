using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadlyShot : Ability
{
    [SerializeField] private GameObject projectileGameObject;

    private void Awake()
    {
        castStaminaThreshold = 400;
        BasePower = 30;

        projectileGameObject = Resources.Load("Ability Prefabs/DeadlyShot") as GameObject;
    }
    override public void castAbility()
    {
        base.castAbility();

        GameObject[] listEnemyUnits = GameObject.FindGameObjectsWithTag(unit.getTargetTag());

        if (listEnemyUnits.Length > 0)
        {
            Unit targetUnit = listEnemyUnits[0].GetComponent<Unit>();
            float minLifeProv = targetUnit.CurrentLife;


            foreach (GameObject unit in listEnemyUnits)
            {
                if(unit.GetComponent<Unit>().CurrentLife < minLifeProv)
                {
                    minLifeProv = unit.GetComponent<Unit>().CurrentLife;
                    targetUnit = unit.GetComponent<Unit>();
                }
            }

            StartCoroutine(ProjectileAnimation(targetUnit, projectileGameObject));
        }
    }

    
}

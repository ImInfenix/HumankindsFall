using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonousDecoction : Ability
{
    private void Awake()
    {
        BasePower = 1;
        duration = 10;
        castStaminaThreshold = 650;
        soundEffect = Resources.Load("SoundEffects/dagger-woosh") as AudioClip;
    }
    public override void castAbility()
    {
        base.castAbility();

        GameObject[] listEnemyUnits = GameObject.FindGameObjectsWithTag(unit.getTargetTag());

        if (listEnemyUnits.Length > 0)
        {
            Unit targetUnit = listEnemyUnits[0].GetComponent<Unit>();
            float maxLifeProv = targetUnit.CurrentLife;


            foreach (GameObject unit in listEnemyUnits)
            {
                if (!unit.GetComponent<Unit>().Poisonned && unit.GetComponent<Unit>().CurrentLife > maxLifeProv)
                {
                    maxLifeProv = unit.GetComponent<Unit>().CurrentLife;
                    targetUnit = unit.GetComponent<Unit>();
                }
            }

            playSound();
            targetUnit.activatePoison(currentPower * duration, unit.poisonDamage * currentPower);
        }
    }
}

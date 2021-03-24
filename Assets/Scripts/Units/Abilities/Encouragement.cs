using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Encouragement : Ability
{
    List<(Unit boostedUnit, float incrementAttackSpeed, float incrementDamage)> boostedUnits;

    private void Awake()
    {
        castStaminaThreshold = 800;
        areaOfEffect = 1;
        BasePower = 20;
        duration = 5;
        boostedUnits = new List<(Unit, float, float)>();
    }

    public override void castAbility()
    {
        base.castAbility();

        List<Unit> unitsInRange = PathfindingTool.unitsInRadius(unit.currentCell, areaOfEffect, unit.tag);
        unitsInRange.Remove(unit);

        StartCoroutine(BoostUnit(unitsInRange));
    }

    IEnumerator BoostUnit(List<Unit> units)
    {
        RemoveUnitsBoost();

        foreach (Unit unit in units)
        {
            print("boost");
            float baseAttackSpeed = unit.AttackSpeed;
            float newAttackSpeed = unit.AttackSpeed * (1 + currentPower/100);
            unit.AttackSpeed = newAttackSpeed;
            float attackSpeedIncrement = newAttackSpeed - baseAttackSpeed;

            float baseDamage = unit.Damage;
            float newDamage = unit.Damage * (1 + currentPower / 200);
            unit.Damage = newAttackSpeed;
            float damageIncrement = newDamage - baseDamage;

            unit.Status.setEncouraged(true);
            boostedUnits.Add((unit, attackSpeedIncrement, damageIncrement));
        }

        yield return new WaitForSeconds(duration);

        RemoveUnitsBoost();
    }

    private void RemoveUnitsBoost()
    {
        foreach (var unit in boostedUnits)
        {
            if (unit.boostedUnit != null)
            {
                unit.boostedUnit.AttackSpeed -= unit.incrementAttackSpeed;
                unit.boostedUnit.Damage -= unit.incrementDamage;
                unit.boostedUnit.Status.setEncouraged(false);
            }
        }

        boostedUnits.Clear();
    }

    private void OnDestroy()
    {
        RemoveUnitsBoost();
    }
}

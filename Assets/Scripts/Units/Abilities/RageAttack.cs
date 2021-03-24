using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RageAttack : Ability
{
    private GameObject animationGameObject;

    private void Awake()
    {
        castStaminaThreshold = 400;
        BasePower = 30;
        castRange = 1;

        animationGameObject = Resources.Load("Ability Prefabs/RageAttack") as GameObject;
    }

    public override void castAbility()
    {
        base.castAbility();

        List<Unit> targets = PathfindingTool.unitsInRadius(unit.currentCell, castRange, unit.getTargetTag());

        if (targets.Count > 0)
        {
            float damageTakenPourcentage = 1 - (unit.CurrentLife / unit.MaxLife);
            currentPower += currentPower * damageTakenPourcentage;

            targets[0].takeDamage(currentPower);

            Instantiate(animationGameObject, transform.position, Quaternion.identity, transform);
        }
    }
}

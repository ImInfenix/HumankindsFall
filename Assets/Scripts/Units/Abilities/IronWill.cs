using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IronWill : Ability
{
    int duration;

    private void Awake()
    {
        castStaminaThreshold = 700;
        BasePower = 30;
        duration = 5;
    }

    public override void castAbility()
    {
        base.castAbility();

        if (canGenerateStamina)
            StartCoroutine(BoostArmor());
    }

    IEnumerator BoostArmor()
    {
        canGenerateStamina = false;
        unit.setIsAbilityActivated(true);
        unit.Status.setShieldUp(true);

        float baseArmor = unit.Armor;
        float boostedArmor = baseArmor * (1 + BasePower / 100);
        unit.Armor = boostedArmor;

        float armorAugmentation = boostedArmor - baseArmor;

        yield return new WaitForSeconds(duration);

        unit.Armor -= armorAugmentation;

        unit.Status.setShieldUp(false);
        unit.setIsAbilityActivated(false);
        canGenerateStamina = true;
    }
}

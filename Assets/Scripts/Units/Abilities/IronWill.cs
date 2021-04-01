using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IronWill : Ability
{
    private void Awake()
    {
        castStaminaThreshold = 700;
        BasePower = 30;
        duration = 5;

        soundEffect = Resources.Load("SoundEffects/armor-up-modified") as AudioClip;
    }

    public override void castAbility()
    {
        base.castAbility();

        if (canGenerateStamina)
            StartCoroutine(BoostArmor());
    }

    IEnumerator BoostArmor()
    {
        playSound();
        canGenerateStamina = false;
        unit.Status.setShieldUp(true);

        float baseArmor = unit.Armor;
        float boostedArmor = baseArmor * (1 + BasePower / 100);
        unit.Armor = boostedArmor;

        float armorAugmentation = boostedArmor - baseArmor;

        yield return new WaitForSeconds(duration);

        unit.Armor -= armorAugmentation;

        unit.Status.setShieldUp(false);
        canGenerateStamina = true;
    }
}

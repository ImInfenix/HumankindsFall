using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IronWill : Ability
{
    private GameObject animationGameObject;

    private void Awake()
    {
        castStaminaThreshold = 700;
        BasePower = 30;
        duration = 5;

        animationGameObject = Resources.Load("Ability Prefabs/IronWill") as GameObject;
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
        unit.Status.setShieldUp(true);

        float baseArmor = unit.Armor;
        float boostedArmor = baseArmor * (1 + BasePower / 100);
        unit.Armor = boostedArmor;

        float armorAugmentation = boostedArmor - baseArmor;

        Instantiate(animationGameObject, transform.position, Quaternion.identity, transform);

        yield return new WaitForSeconds(duration);

        unit.Armor -= armorAugmentation;

        unit.Status.setShieldUp(false);
        canGenerateStamina = true;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowSpellGem : Gem
{
    public override void AbilityGemEffect()
    {
        StartCoroutine(Invisibility());
    }

    public override void AttackGemEffect()
    {

    }

    public override void InitGemEffect()
    {

    }

    public override void InitializeDescription()
    {
        gemDescription = "Après avoir utilisé sa capacité, l'unité devient invisible pendant 2 secondes.";
    }

    public override void InitializeName()
    {
        gemName = "Gemme sort des ombres";
    }

    private IEnumerator Invisibility()
    {
        unit.startInvisibility();
        float baseIncrementStamina = unit.IncrementStamina;
        unit.IncrementStamina = 0;

        yield return new WaitForSeconds(2);

        unit.IncrementStamina = baseIncrementStamina;
        unit.stopInvisibility();
    }
}

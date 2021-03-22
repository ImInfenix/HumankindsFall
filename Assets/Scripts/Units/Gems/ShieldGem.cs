using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldGem : Gem
{
    public override void AbilityGemEffect()
    {

    }

    public override void AttackGemEffect()
    {

    }

    public override void InitGemEffect()
    {
        unit.Armor *= 1.15f;
    }

    public override void InitializeDescription()
    {
        gemName = "Gemme bouclier";
    }

    public override void InitializeName()
    {
        gemDescription = "Augmente l'armure de 15%.";
    }
}

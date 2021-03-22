using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PurityGem : Gem
{
    public override void AbilityGemEffect()
    {

    }

    public override void AttackGemEffect()
    {

    }

    public override void InitGemEffect()
    {
        unit.Ability.BasePower *= 1.2f;
    }

    public override void InitializeDescription()
    {
        gemDescription = "Augmente la puissance de la capacité de 20%.";
    }

    public override void InitializeName()
    {
        gemName = "Gemme de pureté magique";
    }
}

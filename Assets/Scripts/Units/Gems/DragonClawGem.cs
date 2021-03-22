using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonClawGem : Gem
{
    public override void AbilityGemEffect()
    {

    }

    public override void AttackGemEffect()
    {

    }

    public override void InitGemEffect()
    {
        unit.Damage *= 1.10f;
    }

    public override void InitializeDescription()
    {
        gemDescription = "Augmente les dégats d'attaque de 10%.";
    }

    public override void InitializeName()
    {
        gemName = "Gemme griffe de dragon";
    }
}

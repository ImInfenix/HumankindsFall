using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VampireGem : Gem
{
    public override void AbilityGemEffect()
    {

    }

    public override void AttackGemEffect()
    {
        unit.heal(0.2f * unit.Damage);
    }

    public override void InitGemEffect()
    {

    }

    public override float InitGemEffect(float statToModify)
    {
        return statToModify;
    }

    public override void InitializeDescription()
    {
        gemDescription = "À chaque attaque, l'unité se soigne de 20% de sa valeur d'attaque.";
    }

    public override void InitializeName()
    {
        gemName = "Gemme du vampire";
    }

    public override void InitializeStatModified()
    {

    }
}

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
        unit.heal(Mathf.RoundToInt(0.2f * unit.Damage));
    }

    public override void InitGemEffect()
    {
        
    }

    void Awake()
    {
        gemName = "Gemme du vampire";
        gemDescription = "À chaque attaque, l'unité se soigne de 20% de sa valeur d'attaque.";
    }
}

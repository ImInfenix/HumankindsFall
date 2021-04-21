using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RubyGem : Gem
{
    public override void InitializeStatModified()
    {
        statModified = StatModified.Health;
    }

    public override void AbilityGemEffect()
    {
        
    }

    public override void AttackGemEffect()
    {
        
    }

    public override void InitGemEffect()
    {
        unit.MaxLife += 20;
        unit.MaxLife = Mathf.RoundToInt(unit.MaxLife * 1.2f);
    }

    public override float InitGemEffect(float statToModify)
    {
        return Mathf.RoundToInt((statToModify + 20) * 1.2f);
    }

    public override void InitializeDescription()
    {
        gemDescription = "Augmente les PV max de 20, puis de 20%.";
    }

    public override void InitializeName()
    {
        gemName = "Gemme de rubis";
    }
}

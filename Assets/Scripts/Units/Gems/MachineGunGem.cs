using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineGunGem : Gem
{
    public override void InitializeStatModified()
    {
        statModified = StatModified.AttackSpeed;
    }

    public override void AbilityGemEffect()
    {
        
    }

    public override void AttackGemEffect()
    {
        
    }

    public override void InitGemEffect()
    {
        unit.AttackSpeed *= 1.20f;
    }

    public override float InitGemEffect(float statToModify)
    {
        return statToModify * 1.20f;
    }

    public override void InitializeDescription()
    {
        gemDescription = "Augmente la vitesse d'attaque de 20% (PAN PAN PAN).";
    }

    public override void InitializeName()
    {
        gemName = "Gemme Machine Gun";
    }
}

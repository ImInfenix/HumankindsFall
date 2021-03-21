using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineGunGem : Gem
{
    public override void AbilityGemEffect()
    {
        
    }

    public override void AttackGemEffect()
    {
        
    }

    public override void InitGemEffect()
    {
        unit.AttackSpeed *= 1.25f;
    }

    void Awake()
    {
        gemName = "Gemme Machine Gun";
        gemDescription = "Augmente la vitesse d'attaque de 25% (PAN PAN PAN).";
    }
}

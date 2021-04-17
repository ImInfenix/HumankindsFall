using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwiftnessGem : Gem
{
    public override void InitializeStatModified()
    {
        statModified = StatModified.MovementSpeed;
    }

    public override void AbilityGemEffect()
    {

    }

    public override void AttackGemEffect()
    {

    }

    public override void InitGemEffect()
    {
        unit.MoveSpeed *= 1.25f;
    }

    public override float InitGemEffect(float statToModify)
    {
        return statToModify * 1.25f;
    }

    public override void InitializeDescription()
    {
        gemDescription = "Augmente la vitesse de déplacement de 25%.";
    }

    public override void InitializeName()
    {
        gemName = "Gemme de célérité";
    }
}

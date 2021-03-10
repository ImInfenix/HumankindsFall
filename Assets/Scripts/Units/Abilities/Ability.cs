using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class Ability : MonoBehaviour
{
    protected Unit unit;
    protected float currentStamina;

    [SerializeField] protected int castStaminaThreshold;
    protected int castRange;
    protected int areaOfEffect;
    protected int basePower;
    protected int currentPower;

    public void setUnit(Unit unit)
    {
        this.unit = unit;
        unit.getHealthbar().SetStamina(currentStamina, castStaminaThreshold);
    }

    public void updateAbility(float incrementStamina)
    {
        currentStamina += incrementStamina;
        if (currentStamina >= castStaminaThreshold && !unit.getIsAbilityActivated())
        {
            currentStamina -= castStaminaThreshold;
            castAbility();
        }

        unit.getHealthbar().SetStamina(currentStamina, castStaminaThreshold);
    }

    virtual public void castAbility()
    {
        updateCurrentPower();
    }

    private void updateCurrentPower()
    {
        currentPower = basePower + unit.Power;
    }
    
    public void mageSynergy(int lvl)
    {
        if (lvl == 1)
        {
            currentStamina = castStaminaThreshold / 4;
        }
        if (lvl == 2)
        {
            currentStamina = castStaminaThreshold / 2;
        }
        unit.getHealthbar().SetStamina(currentStamina, castStaminaThreshold);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class Ability : MonoBehaviour
{
    [SerializeField] protected int castStaminaThreshold;
    protected int currentStamina;
    protected Unit unit;

    public void setUnit(Unit unit)
    {
        this.unit = unit;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //
    public void updateAbility(int incrementStamina)
    {
        currentStamina += incrementStamina;
        if (currentStamina >= castStaminaThreshold && !unit.getIsAbilityActivated())
        {
            currentStamina -= castStaminaThreshold;
            castAbility();
        }
    }

    abstract public void castAbility();
}
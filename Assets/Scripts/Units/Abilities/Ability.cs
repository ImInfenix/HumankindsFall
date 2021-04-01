using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class Ability : MonoBehaviour
{
    protected Unit unit;
    [SerializeField] protected float currentStamina;

    [SerializeField] protected int castStaminaThreshold;
    protected int castRange;
    protected int areaOfEffect;
    protected float duration;
    private float basePower;
    protected float currentPower;
    protected bool canGenerateStamina = true;

    protected AudioClip soundEffect;

    public float BasePower { get => basePower; set => basePower = value; }

    public void setUnit(Unit unit)
    {
        this.unit = unit;
        unit.getHealthbar().SetStamina(currentStamina, castStaminaThreshold);
    }

    public void updateAbility(float incrementStamina)
    {
        if (canGenerateStamina)
        {
            currentStamina += incrementStamina;
            if (currentStamina >= castStaminaThreshold && !unit.getIsAbilityActivated())
            {
                currentStamina -= castStaminaThreshold;
                castAbility();
            }

            unit.getHealthbar().SetStamina(currentStamina, castStaminaThreshold);
        }
    }

    virtual public void castAbility()
    {
        updateCurrentPower();
        unit.ApplyAbilityGemsEffects();
    }

    private void updateCurrentPower()
    {
        currentPower = BasePower * unit.Power;
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

    protected void playSound()
    {
        if (soundEffect != null)
        {
            AudioManager.PlayEffect(soundEffect);
        }
    }
}

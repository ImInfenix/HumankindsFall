using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class Gem : MonoBehaviour
{
    protected Unit unit;
    protected string gemName;
    protected string gemDescription;

    public enum StatModified
    {
        None,
        Damage,
        Health,
        Armor,
        Power,
        AttackSpeed,
        MovementSpeed
    }

    protected StatModified statModified = StatModified.None;

    public StatModified GetStatModified()
    {
        return statModified;
    }

    public void setUnit(Unit unit)
    {
        this.unit = unit;
    }

    abstract public void InitGemEffect();
    abstract public void AttackGemEffect();
    abstract public void AbilityGemEffect();
    abstract public void InitializeName();
    abstract public void InitializeDescription();
    abstract public void InitializeStatModified();
    abstract public float InitGemEffect(float statToModify);

    public override string ToString()
    {
        return gemName + " : " + gemDescription;
    }

    private void Awake()
    {
        InitializeName();
        InitializeDescription();
        InitializeStatModified();
    }
}

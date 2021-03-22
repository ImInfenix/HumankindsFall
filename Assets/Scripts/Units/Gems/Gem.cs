﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class Gem : MonoBehaviour
{
    protected Unit unit;
    protected string gemName;
    protected string gemDescription;

    public void setUnit(Unit unit)
    {
        this.unit = unit;
    }

    abstract public void InitGemEffect();
    abstract public void AttackGemEffect();
    abstract public void AbilityGemEffect();
    abstract public void InitializeName();
    abstract public void InitializeDescription();

    public override string ToString()
    {
        return gemName + " : " + gemDescription;
    }

    private void Awake()
    {
        InitializeName();
        InitializeDescription();
    }
}

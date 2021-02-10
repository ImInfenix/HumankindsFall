﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Class
{
    Warrior,
    Mage
}

[CreateAssetMenu(fileName = "New UnitClassStat", menuName = "Class Stat Units")]
public class ClassStat : ScriptableObject
{

    public Class clas;

    public int maxLife;
    public int maxMana;
    public int mana;
    public int armor;
    public int moveSpeed;
    public float attackSpeed;
    public int damage;
    public int range;
}
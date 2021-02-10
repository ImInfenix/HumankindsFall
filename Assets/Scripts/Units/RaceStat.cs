﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Race
{
    Orc,
    Humans
}

[CreateAssetMenu(fileName = "New UnitRaceStat", menuName = "Race Stat Units")]
public class RaceStat : ScriptableObject
{
    public Race race;

    // protected Object attack; A voir comment on implémente les attaques

    public int maxLife;
    public int maxMana;
    public int mana;
    public int armor;
    public int moveSpeed;
    public float attackSpeed;
    public int damage;
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Race
{
    Orc,
    Human,
    Skeleton
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
    public float moveSpeed;
    public float attackSpeed;
    public int damage;
    public string[] unitNames;
    public Sprite unitSprite;
}

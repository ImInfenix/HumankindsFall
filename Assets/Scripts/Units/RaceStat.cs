using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Race
{
    Orc,
    Human,
    Skeleton,
    Octopus,
    Elemental,
    Giant,
}

[CreateAssetMenu(fileName = "New UnitRaceStat", menuName = "Race Stat Units")]
public class RaceStat : ScriptableObject
{
    public Race race;

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

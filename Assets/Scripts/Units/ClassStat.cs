using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Class
{
    Warrior,
    Mage,
    Tank,
    Bowman,
    Healer,
    Support,
    Berserker,
    Assassin,
    Soldier,
    DemonKing
}

[CreateAssetMenu(fileName = "New UnitClassStat", menuName = "Class Stat Units")]
public class ClassStat : ScriptableObject
{
    public Class clas;

    public int maxLife;
    public int armor;
    public float moveSpeed;
    public float attackSpeed;
    public int damage;
    public int range;
    public int incrementStamina;
    public GameObject projectile;
    public string[] abilities;
    public Sprite classIconSprite;
}

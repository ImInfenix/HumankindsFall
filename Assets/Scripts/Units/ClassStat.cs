using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Class
{
    Warrior,
    Mage,
    Bowman,
    Healer
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
    public GameObject projectile;
    public string[] abilities;
}

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
    Ratman,
    Demon
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
    public Sprite warriorSprite;
    public Sprite mageSprite;
    public Sprite tankSprite;
    public Sprite bowmanSprite;
    public Sprite healerSprite;
    public Sprite supportSprite;
    public Sprite berserkerSprite;
    public Sprite assassinSprite;
    public Sprite soldierSprite;

    public Sprite getSprite(Class c)
    {
        switch(c)
        {
            case (Class.Warrior):
                return warriorSprite;
                break;
            case (Class.Mage):
                return mageSprite;
                break;
            case (Class.Tank):
                return tankSprite;
                break;
            case (Class.Bowman):
                return bowmanSprite;
                break;
            case (Class.Healer):
                return healerSprite;
                break;
            case (Class.Support):
                return supportSprite;
            case (Class.Berserker):
                return berserkerSprite;
                break;
            case (Class.Assassin):
                return assassinSprite;
                break;
            case (Class.Soldier):
                return soldierSprite;
                break;
            default:
                return unitSprite;
                break;
        }
    }
}

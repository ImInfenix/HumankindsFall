using System;
using UnityEngine;

public class UnitDescription
{
    public static uint currentId;

    private readonly Sprite sprite;
    private readonly RaceStat unitRace;
    private readonly ClassStat unitClass;
    private readonly string unitName;
    private readonly string abilityName;
    private readonly uint id;
    private uint experience;
    private uint level;
    private string[] gems;

    public UnitDescription(Unit unit)
    {
        sprite = unit.GetSprite();
        unitRace = unit.raceStats;
        unitClass = unit.classStat;
        abilityName = unit.GetAbilityName();
        unitName = unit.GetName();
        id = unit.id;
        level = unit.Level;
        gems = unit.GetGems();
    }

    /// <summary>
    /// Creates a new UnitDescription and attached a new id to it
    /// </summary>
    /// <param name="name"></param>
    /// <param name="unitRace"></param>
    /// <param name="unitClass"></param>
    /// <param name="abilityName"></param>
    public UnitDescription(string name, RaceStat unitRace, ClassStat unitClass, string abilityName, string unitTag, uint? id = null, uint experience = 0, uint level = 1, string[] gems = null)
    {
        unitName = name;
        this.unitRace = unitRace;
        this.unitClass = unitClass;
        this.abilityName = abilityName;
        sprite = unitRace.getSprite(unitClass.clas); 
        if (id == null)
            this.id = GetNewId();
        else this.id = (uint) id;
        this.experience = experience;
        this.gems = gems;
        this.level = level;
    }

    public string GetUnitName()
    {
        return unitName;
    }

    public Sprite GetSprite()
    {
        return sprite;
    }

    public RaceStat GetRace()
    {
        return unitRace;
    }

    public ClassStat GetClass()
    {
        return unitClass;
    }

    public string GetAbilityName()
    {
        return abilityName;
    }

    public uint GetId()
    {
        return id;
    }

    private static uint GetNewId()
    {
        uint res = currentId;
        currentId++;
        return res;
    }

    public bool IsOfSameTypeThan(UnitDescription otherUnit)
    {
        return unitRace == otherUnit.unitRace && unitClass == otherUnit.unitClass && abilityName == otherUnit.abilityName;
    }

    public uint GetExperience()
    {
        return experience;
    }

    public void EarnExperience(uint experienceAmount)
    {
        experience += experienceAmount;

        if (experience >= 10)
        {
            experience -= 10;
            IncreaseLevel();
        }
    }

    public uint GetLevel()
    {
        return level;
    }

    public void IncreaseLevel()
    {
        level++;
    }

    public string[] GetGems()
    {
        return gems;
    }

    public void AddGem(string gemName)
    {
        if (gems != null)
        {
            string[] newGems = new string[gems.Length + 1];

            for (int i = 0; i < gems.Length; i++)
            {
                newGems[i] = gems[i];
            }

            newGems[gems.Length] = gemName;
            gems = newGems;
        }

        else
            gems = new string[1] { gemName };
    }

    public void SetGems(string[] gems)
    {
        this.gems = gems;
    }
}

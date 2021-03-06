﻿using System;
using UnityEngine;

public class UnitDescription
{
    private static uint currentId = 0;

    private readonly Sprite sprite;
    private readonly RaceStat unitRace;
    private readonly ClassStat unitClass;
    private readonly string unitName;
    private readonly string abilityName;
    private readonly uint id;
    private uint experience;

    public UnitDescription(Unit unit)
    {
        sprite = unit.GetSprite();
        unitRace = unit.raceStats;
        unitClass = unit.classStat;
        abilityName = unit.GetAbilityName();
        unitName = unit.GetName();
        id = unit.id;
        experience = 0;
    }

    /// <summary>
    /// Creates a new UnitDescription and attached a new id to it
    /// </summary>
    /// <param name="name"></param>
    /// <param name="unitRace"></param>
    /// <param name="unitClass"></param>
    /// <param name="abilityName"></param>
    public UnitDescription(string name, RaceStat unitRace, ClassStat unitClass, string abilityName, string unitTag)
    {
        unitName = name;
        this.unitRace = unitRace;
        this.unitClass = unitClass;
        this.abilityName = abilityName;
        sprite = unitRace.unitSprite;
        id = GetNewId();
        experience = 0;
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
    }
}

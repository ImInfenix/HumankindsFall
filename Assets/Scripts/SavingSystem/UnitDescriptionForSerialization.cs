using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class UnitDescriptionForSerialization
{
    public string name;
    public string unitRace;
    public string unitClass;
    public string abilityName;
    public uint id;
    public uint experience;
    public string[] gems;

    private UnitDescriptionForSerialization(UnitDescription unit)
    {
        name = unit.GetUnitName();
        unitRace = unit.GetRace().race.ToString();
        unitClass = unit.GetClass().clas.ToString();
        abilityName = unit.GetAbilityName();
        id = unit.GetId();
        experience = unit.GetExperience();
        gems = unit.GetGems();
    }

    public static UnitDescriptionForSerialization GetSerializableVersionOf(UnitDescription unit)
    {
        return new UnitDescriptionForSerialization(unit);
    }

    public UnitDescription ToDescription()
    {
        Enum.TryParse(unitRace, out Race race);
        RaceStat raceStatFound = null;
        foreach(RaceStat raceStat in UnitGenerator.GetAllRaces())
        {
            if (raceStat.race == race)
                raceStatFound = raceStat;
        }

        Enum.TryParse(unitClass, out Class @class);
        ClassStat classStatFound = null;
        foreach (ClassStat classStat in UnitGenerator.GetAllClasses())
        {
            if (classStat.clas == @class)
                classStatFound = classStat;
        }

        return new UnitDescription(name, raceStatFound, classStatFound, abilityName, Unit.allyTag, id, experience, gems);
    }
}

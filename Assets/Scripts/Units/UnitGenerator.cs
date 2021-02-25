using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UnitGenerator
{
    public static UnitDescription GenerateUnit(string unitTag)
    {
        RaceStat unitRace = GetRandomRace(unitTag);
        ClassStat unitClass = GetRandomClass();
        string name = GetRandomName(unitRace);
        string abilityName = GetRandomAbilityName(unitClass);

        return new UnitDescription(name, unitRace, unitClass, abilityName, unitTag);
    }

    public static RaceStat GetRandomRace(string unitTag)
    {
        //Get all RaceStat ScriptableObject
        RaceStat[] races = Resources.LoadAll<RaceStat>("Stat Units/Race");

        RaceStat raceStats;
        if (unitTag == Unit.allyTag)
        {
            do
            {
                int randomRaceIndex = Random.Range(0, races.Length);
                raceStats = races[randomRaceIndex];
            }
            while (raceStats.race == Race.Human);

            return raceStats;
        }

        foreach (RaceStat raceStat in races)
        {
            if (raceStat.race == Race.Human)
            {
                return raceStat;
            }
        }

        Debug.LogError("GetRandomRace returned null");
        return null;
    }

    public static ClassStat GetRandomClass()
    {
        ClassStat[] classes = Resources.LoadAll<ClassStat>("Stat Units/Class");

        int randomClassIndex = Random.Range(0, classes.Length);
        return classes[randomClassIndex];
    }

    public static string GetRandomName(RaceStat raceStats)
    {
        string[] possibleNames = raceStats.unitNames;
        int randomNameIndex = Random.Range(0, possibleNames.Length);
        return possibleNames[randomNameIndex];
    }

    public static string GetRandomAbilityName(ClassStat classStat)
    {
        int randomAbilityIndex = Random.Range(0, classStat.abilities.Length);
        return classStat.abilities[randomAbilityIndex];
    }
}

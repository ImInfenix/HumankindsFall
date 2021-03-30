using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UnitGenerator
{
    private static ClassStat[] classes;
    private static RaceStat[] races;

    public static UnitDescription GenerateUnit(string unitTag)
    {
        RaceStat unitRace = GetRandomRace(unitTag);
        ClassStat unitClass = GetRandomClass(unitRace.race);
        string name = GetRandomName(unitRace);
        string abilityName = GetRandomAbilityName(unitClass);

        return new UnitDescription(name, unitRace, unitClass, abilityName, unitTag);
    }

    public static RaceStat GetRandomRace(string unitTag)
    {
        RaceStat[] races = GetAllRaces();

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

    public static ClassStat GetRandomClass(Race race)
    {
        ClassStat[] classes = GetAllClasses();

        int randomClassIndex;

        if (race != Race.Human && race != Race.Ratman)
        {
            do
            {
                randomClassIndex = Random.Range(0, classes.Length);
            } while (classes[randomClassIndex].clas == Class.Soldier || classes[randomClassIndex].clas == Class.DemonKing);
        
        }
        else if(race == Race.Ratman)
        {
            do
            {
                randomClassIndex = Random.Range(0, classes.Length);
            } while (classes[randomClassIndex].clas == Class.Soldier || classes[randomClassIndex].clas == Class.DemonKing || classes[randomClassIndex].clas == Class.Mage || classes[randomClassIndex].clas == Class.Bowman || classes[randomClassIndex].clas == Class.Healer || classes[randomClassIndex].clas == Class.Support);
        }
        else
        {
            do
            {
                randomClassIndex = Random.Range(0, classes.Length);
            } while (classes[randomClassIndex].clas == Class.DemonKing);
        }
            
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
        if (classStat.abilities.Length > 0)
        {
            int randomAbilityIndex = Random.Range(0, classStat.abilities.Length);
            return classStat.abilities[randomAbilityIndex];
        }

        else
            return null;
    }

    public static ClassStat[] GetAllClasses()
    {
        if(classes == null)
        {
            classes = Resources.LoadAll<ClassStat>("Stat Units/Class");
        }

        return classes;
    }

    public static RaceStat[] GetAllRaces()
    {
        if(races == null)
        {
            races = Resources.LoadAll<RaceStat>("Stat Units/Race");
        }

        return races;
    }
}

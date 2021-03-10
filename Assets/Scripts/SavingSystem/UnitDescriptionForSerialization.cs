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

    private UnitDescriptionForSerialization(UnitDescription unit)
    {
        name = unit.GetUnitName();
        unitRace = unit.GetRace().race.ToString();
        unitClass = unit.GetClass().clas.ToString();
        abilityName = unit.GetAbilityName();
        id = unit.GetId();
        experience = unit.GetExperience();
    }

    public static UnitDescriptionForSerialization GetSerializableVersionOf(UnitDescription unit)
    {
        return new UnitDescriptionForSerialization(unit);
    }
}

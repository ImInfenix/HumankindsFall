using UnityEngine;

public class UnitDescription
{
    private readonly Sprite sprite;
    private readonly RaceStat unitRace;
    private readonly ClassStat unitClass;
    private readonly string unitName;
    private readonly string abilityName;

    public UnitDescription(Unit unit)
    {
        sprite = unit.GetSprite();
        unitRace = unit.raceStats;
        unitClass = unit.classStat;
        abilityName = unit.GetAbilityName();
        unitName = unit.GetName();
    }

    public UnitDescription(string name, RaceStat unitRace, ClassStat unitClass, string abilityName)
    {
        unitName = name;
        this.unitRace = unitRace;
        this.unitClass = unitClass;
        this.abilityName = abilityName;
        sprite = unitRace.unitSprite;
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
}

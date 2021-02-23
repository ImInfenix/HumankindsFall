using UnityEngine;

public class UnitDescription
{
    Sprite spriteRenderer;
    RaceStat unitRace;
    ClassStat unitClass;
    string abilityName;

    public UnitDescription(
    Sprite spriteRenderer, RaceStat unitRace, ClassStat unitClass, string abilityName)
    {
        this.spriteRenderer = spriteRenderer;
        this.unitRace = unitRace;
        this.unitClass = unitClass;
        this.abilityName = abilityName;
    }

    public Sprite GetSprite()
    {
        return spriteRenderer;
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

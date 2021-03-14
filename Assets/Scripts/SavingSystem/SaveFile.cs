using System;
using System.Collections.Generic;

[Serializable]
public class SaveFile
{
    public int fileVersion = 0;
    public int walletAmount;
    public uint unitGeneratorId;
    public List<UnitDescriptionForSerialization> units;

    public void SetUnitsToSave(List<UnitDescription> unitsToSave)
    {
        units = new List<UnitDescriptionForSerialization>();

        foreach (UnitDescription unit in unitsToSave)
            units.Add(UnitDescriptionForSerialization.GetSerializableVersionOf(unit));
    }

    public List<UnitDescription> GetAllUnits()
    {
        List<UnitDescription> units = new List<UnitDescription>();

        foreach(UnitDescriptionForSerialization unitSerialized in this.units)
            units.Add(unitSerialized.ToDescription());

        return units;
    }
}

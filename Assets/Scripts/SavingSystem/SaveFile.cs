using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SaveFile
{
    public int walletAmount;
    public List<UnitDescriptionForSerialization> units;

    public void SetUnitsToSave(List<UnitDescription> unitsToSave)
    {
        units = new List<UnitDescriptionForSerialization>();

        foreach (UnitDescription unit in unitsToSave)
            units.Add(UnitDescriptionForSerialization.GetSerializableVersionOf(unit));
    }
}

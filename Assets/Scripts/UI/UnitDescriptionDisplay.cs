using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UnitDescriptionDisplay : MonoBehaviour
{
    [SerializeField]
    private TMP_Text UnitName;
    [SerializeField]
    private TMP_Text UnitStats;

    private InventorySlot actualSlot;


    public void Awake()
    {
        UnitName.text = "";
        UnitStats.text = "";
        actualSlot = null;
    }

    public void changeActualSlot(InventorySlot slot)
    {
        if (actualSlot != null)
        {
            actualSlot.Unselect();
        }
        actualSlot = slot;
        slot.Select();
    }

    public void unselectActualSlot()
    {
        if (actualSlot != null)
        {
            actualSlot.Unselect();
            actualSlot = null;
            UnitName.text = "";
            UnitStats.text = "";
        }
    }

    public void SetUnitName(string name)
    {
        UnitName.text = name;
    }

    public void SetUnitStats(string stats)
    {
        UnitStats.text = stats;
    }
}

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

    public void ChangeActualSlot(InventorySlot slot)
    {
        if (actualSlot != null)
        {
            actualSlot.Unselect();
        }
        actualSlot = slot;
        slot.Select();
        UpdateDescription();

        Player.instance.Inventory.Show();
    }

    public void UnselectActualSlot()
    {
        if (actualSlot == null)
            return;

        actualSlot.Unselect();
        actualSlot = null;
        UpdateDescription();

        Player.instance.Inventory.Hide();
    }

    public void SetUnitName(string name)
    {
        UnitName.text = name;
    }

    public void SetUnitStats(string stats)
    {
        UnitStats.text = stats;
    }

    public void UpdateDescription()
    {
        if (actualSlot == null)
        {
            UnitName.text = "";
            UnitStats.text = "";
            return;
        }

        UnitDescription currentDescription = actualSlot.GetCurrentUnitDescription();

        SetUnitName(currentDescription.GetUnitName());
        ClassStat classe = currentDescription.GetClass();
        RaceStat race = currentDescription.GetRace();

        int maxLife = classe.maxLife + race.maxLife;
        int maxMana = classe.maxMana + race.maxMana;
        int armor = classe.armor + race.armor;
        float atakSpeed = classe.attackSpeed + race.attackSpeed;

        string stats =
            "Class : " + classe.name + "\n" +
            "Race : " + race.name + "\n" +
            "PV : " + maxLife + "\n" +
            "Mana : " + maxMana + "\n" +
            "Armor : " + armor + "\n" +
            "Attack Speed : " + atakSpeed;
        ;
        SetUnitStats(stats);
    }
}

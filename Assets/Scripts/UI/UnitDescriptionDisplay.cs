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
    [SerializeField]
    private TMP_Text UnitExperience;

    private InventorySlot actualSlot;

    [HideInInspector]
    public ShopSystem shopSystem;

    public void Awake()
    {
        actualSlot = null;
        UpdateDescription();
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

        if(shopSystem != null)
        {
            if (actualSlot == null)
                shopSystem.SetShopToNoneMode();
            else if (actualSlot.GetSlotType() == InventorySlot.SlotType.Inventory)
                shopSystem.SetShopToSellMode();
            else 
                shopSystem.SetShopToBuyMode();
        }
    }

    public void UnselectActualSlot()
    {
        if (actualSlot == null)
            return;

        actualSlot.Unselect();
        actualSlot = null;
        UpdateDescription();

        Player.instance.Inventory.Hide();

        shopSystem?.SetShopToNoneMode();
    }

    public void SetUnitName(string name)
    {
        UnitName.text = name;
    }

    public void SetUnitStats(string stats)
    {
        UnitStats.text = stats;
    }

    public void SetUnitExperience(uint amount)
    {
        if (actualSlot.GetSlotType() == InventorySlot.SlotType.Inventory)
        {
            UnitExperience.text = $"XP: {amount}";
            return;
        }

        UnitExperience.text = "";
    }

    public void UpdateDescription()
    {
        if (actualSlot == null)
        {
            UnitName.text = "";
            UnitStats.text = "";
            UnitExperience.text = "";
            return;
        }

        UnitDescription currentDescription = actualSlot.GetCurrentUnitDescription();

        SetUnitName(currentDescription.GetUnitName());
        SetUnitExperience(currentDescription.GetExperience());
        ClassStat classe = currentDescription.GetClass();
        RaceStat race = currentDescription.GetRace();

        int maxLife = classe.maxLife + race.maxLife;
        int maxMana = race.maxMana;
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

    public InventorySlot.SlotType GetSelectedSlotType()
    {
        if (actualSlot == null) return InventorySlot.SlotType.None;

        return actualSlot.GetSlotType();
    }

    public InventorySlot GetActualSlot()
    {
        return actualSlot;
    }
}

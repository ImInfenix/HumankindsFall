using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopSystem : MonoBehaviour
{
    [SerializeField]
    List<InventorySlot> rewardSlots;

    [SerializeField]
    private UnitDescriptionDisplay unitDescriptionDisplay;

    List<UnitDescription> unitsToSell;

    private void Awake()
    {
        unitsToSell = new List<UnitDescription>();
    }

    private void Start()
    {
        InitializeShop();
    }

    public void InitializeShop()
    {
        gameObject.SetActive(true);

        foreach (InventorySlot slot in rewardSlots)
        {
            UnitDescription newUnit = UnitGenerator.GenerateUnit(Unit.allyTag);
            slot.PutInSlot(newUnit);
            unitsToSell.Add(newUnit);
        }

        Player.instance.Inventory.inventoryUI.UpdateGUI();
    }

    public void ConfirmShopping()
    {
        if (unitDescriptionDisplay.GetSelectedSlotType() != InventorySlot.SlotType.Shop)
            return;

        if (!Player.instance.Wallet.Pay(20))
            return;


        Player.instance.Inventory.AddUnitInInventory(unitDescriptionDisplay.GetActualSlot().GetCurrentUnitDescription(), true);

        unitDescriptionDisplay.GetActualSlot().ClearSlot();
        unitDescriptionDisplay.UnselectActualSlot();

        SavingSystem.SaveData();
    }
}

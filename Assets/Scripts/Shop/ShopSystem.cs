using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopSystem : MonoBehaviour
{
    [SerializeField]
    List<InventorySlot> rewardSlots;

    [SerializeField]
    private UnitDescriptionDisplay unitDescriptionDisplay;

    private void Start()
    {
        InitializeShop();
    }

    public void InitializeShop()
    {
        gameObject.SetActive(true);

        foreach (InventorySlot slot in rewardSlots)
            slot.PutInSlot(UnitGenerator.GenerateUnit(Unit.allyTag));

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

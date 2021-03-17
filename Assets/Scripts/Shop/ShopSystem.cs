using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShopSystem : MonoBehaviour
{
    [SerializeField]
    List<InventorySlot> rewardSlots;

    [SerializeField]
    private UnitDescriptionDisplay unitDescriptionDisplay;

    [SerializeField]
    private TMP_Text shopButton;

    List<UnitDescription> unitsToSell;

    enum ShopMode { None, Buy, Sell }
    ShopMode shopMode;

    private void Awake()
    {
        unitsToSell = new List<UnitDescription>();
        SetShopToNoneMode();
        unitDescriptionDisplay.shopSystem = this;
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
        switch (shopMode)
        {
            case ShopMode.None:
                break;
            case ShopMode.Buy:
                Buy();
                break;
            case ShopMode.Sell:
                Sell();
                break;
        }
    }

    private void Sell()
    {
        if (unitDescriptionDisplay.GetSelectedSlotType() != InventorySlot.SlotType.Inventory)
            return;

        Player.instance.Inventory.RemoveFromInventory(unitDescriptionDisplay.GetActualSlot().GetCurrentUnitDescription());

        Player.instance.Wallet.Earn(5);

        SavingSystem.SaveData();
    }

    private void Buy()
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

    public void SetShopToBuyMode()
    {
        shopMode = ShopMode.Buy;
        shopButton.text = "Acheter";
        shopButton.transform.parent.gameObject.SetActive(true);
    }

    public void SetShopToSellMode()
    {
        shopMode = ShopMode.Sell;
        shopButton.text = "Vendre";
        shopButton.transform.parent.gameObject.SetActive(true);
    }

    public void SetShopToNoneMode()
    {
        shopMode = ShopMode.None;
        shopButton.transform.parent.gameObject.SetActive(false);
    }
}
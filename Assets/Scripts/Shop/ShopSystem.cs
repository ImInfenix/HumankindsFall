using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;
using UnityEngine.UI;

public class ShopSystem : MonoBehaviour
{
    [SerializeField]
    List<InventorySlot> rewardSlots;
    
    [SerializeField]
    List<GemSlot> gemSlots;

    [SerializeField]
    GemsInventory gemsInventory;

    [SerializeField]
    private UnitDescriptionDisplay unitDescriptionDisplay;

    [SerializeField]
    private TMP_Text shopButton;

    List<UnitDescription> unitsToSell;

    enum ShopMode { None, Buy, Sell }
    ShopMode shopMode;

    public GemsInventory GemsInventory { get => gemsInventory; }

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

        foreach (GemSlot gemSlot in gemSlots)
        {
            GameObject[] existingGems = Resources.LoadAll("Gems", typeof(GameObject)).Cast<GameObject>().ToArray();

            int randomGemIndex = Random.Range(0, existingGems.Length);
            GameObject newGem = Instantiate(existingGems[randomGemIndex], gemSlot.transform.position, Quaternion.identity, gemSlot.transform);
            gemSlot.Gem = newGem.GetComponent<Gem>();
            newGem.GetComponentInChildren<Image>().rectTransform.sizeDelta *= 3;
            GemUI gemUI = newGem.GetComponentInChildren<GemUI>();
            gemUI.DisableDrag();
            gemUI.GemSlot = gemSlot;
            gemUI.GemSlot.IsShop = true;
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
        bool isUnit = (unitDescriptionDisplay.GetSelectedSlotType() == InventorySlot.SlotType.Inventory);
        bool isGem = (GemSlot.selectedGemSlot != null);

        if (!isUnit && !isGem)
            return;

        if (isUnit && !isGem)
        {
            Player.instance.Inventory.RemoveFromInventory(unitDescriptionDisplay.GetActualSlot().GetCurrentUnitDescription());
        }

        else if (!isUnit && isGem)
        {
            Player.instance.Inventory.RemoveGem(GemSlot.selectedGemSlot.Gem);
            GemSlot.selectedGemSlot.UnselectSlot();
            GemsInventory.UpdateDisplay();
        }

        Player.instance.Wallet.Earn(5);

        SavingSystem.SaveData();
    }

    private void Buy()
    {
        bool isUnit = (unitDescriptionDisplay.GetSelectedSlotType() == InventorySlot.SlotType.Shop);
        bool isGem = (GemSlot.selectedGemSlot != null);

        if (!isUnit && !isGem)
            return;

        if (!Player.instance.Wallet.Pay(20))
            return;

        if (isUnit && !isGem)
        {
            Player.instance.Inventory.AddUnitInInventory(unitDescriptionDisplay.GetActualSlot().GetCurrentUnitDescription(), true);

            unitDescriptionDisplay.GetActualSlot().ClearSlot();
            unitDescriptionDisplay.UnselectActualSlot();
        }

        else if (!isUnit && isGem)
        {
            Player.instance.Inventory.AddGem(GemSlot.selectedGemSlot.Gem);
            GemsInventory.UpdateDisplay();
            Gem selectedGem = GemSlot.selectedGemSlot.Gem;
            GemSlot.selectedGemSlot.UnselectSlot();
            Destroy(selectedGem.gameObject);
        }

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
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;
using UnityEngine.UI;

public class ShopSystem : MonoBehaviour
{
    public static bool generateNewContent = true;
    private static bool gemsInitialized = false;

    private static GameObject[] existingGems;

    [SerializeField]
    List<InventorySlot> unitsToSellSlots;

    [SerializeField]
    List<GemSlot> gemSlots;

    [SerializeField]
    GemsInventory gemsInventory;

    [SerializeField]
    private UnitDescriptionDisplay unitDescriptionDisplay;

    [SerializeField]
    private TMP_Text shopButton;

    static List<UnitDescription> unitsToSell;
    static List<int> gemsToSell;

    enum ShopMode { None, Buy, Sell }
    ShopMode shopMode;

    public GemsInventory GemsInventory { get => gemsInventory; }

    private void Awake()
    {
        SetShopToNoneMode();
        unitDescriptionDisplay.shopSystem = this;

        if (!gemsInitialized)
        {
            existingGems = Resources.LoadAll("Gems", typeof(GameObject)).Cast<GameObject>().ToArray();
            gemsInitialized = true;
        }
    }

    private void Start()
    {
        InitializeShop();
    }

    public void InitializeShop()
    {
        gameObject.SetActive(true);

        if (generateNewContent)
        {
            unitsToSell = new List<UnitDescription>();
            for (int i = 0; i < unitsToSellSlots.Count; i++)
            {
                UnitDescription newUnit = UnitGenerator.GenerateUnit(Unit.allyTag);
                unitsToSell.Add(newUnit);
            }

            gemsToSell = new List<int>();
            for (int i = 0; i < gemSlots.Count; i++)
            {
                gemsToSell.Add(Random.Range(0, existingGems.Length));
            }

            generateNewContent = false;
        }

        {
            int i = 0;
            foreach (InventorySlot slot in unitsToSellSlots)
            {
                if (i >= unitsToSell.Count)
                    break;

                slot.PutInSlot(unitsToSell[i]);
                i++;
            }

            i = 0;
            foreach (GemSlot gemSlot in gemSlots)
            {
                if (i >= gemsToSell.Count)
                    break;

                GameObject newGem = Instantiate(existingGems[gemsToSell[i]], gemSlot.transform.position, Quaternion.identity, gemSlot.transform);
                i++;
                gemSlot.Gem = newGem.GetComponent<Gem>();
                newGem.GetComponentInChildren<Image>().rectTransform.sizeDelta *= 3;
                GemUI gemUI = newGem.GetComponentInChildren<GemUI>();
                gemUI.DisableDrag();
                gemUI.GemSlot = gemSlot;
                gemUI.GemSlot.IsShop = true;
            }
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
            UnitDescription soldUnit = unitDescriptionDisplay.GetActualSlot().GetCurrentUnitDescription();
            unitsToSell.Remove(soldUnit);
            Player.instance.Inventory.AddUnitInInventory(soldUnit, true);

            unitDescriptionDisplay.GetActualSlot().ClearSlot();
            unitDescriptionDisplay.UnselectActualSlot();
        }

        else if (!isUnit && isGem)
        {
            Gem soldGem = GemSlot.selectedGemSlot.Gem;

            int toDelete = -1;

            int i = 0;

            foreach (GameObject g in existingGems)
            {
                Gem testGem = g.GetComponent<Gem>();
                testGem.InitializeName();
                testGem.InitializeDescription();
                if(soldGem.ToString() == testGem.ToString())
                {
                    toDelete = i;
                    break;
                }

                i++;
            }

            gemsToSell.Remove(toDelete);

            Player.instance.Inventory.AddGem(soldGem);
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
        shopButton.text = "Acheter (20)";
        shopButton.transform.parent.gameObject.SetActive(true);
    }

    public void SetShopToSellMode()
    {
        shopMode = ShopMode.Sell;
        shopButton.text = "Vendre (5)";
        shopButton.transform.parent.gameObject.SetActive(true);
    }

    public void SetShopToNoneMode()
    {
        shopMode = ShopMode.None;
        shopButton.transform.parent.gameObject.SetActive(false);
    }
}
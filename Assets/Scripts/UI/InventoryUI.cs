using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    [SerializeField]
    private RectTransform UnitsSlots;
    [SerializeField]
    private RectTransform GemsSlots;
    [SerializeField]
    private UnitDescriptionDisplay CurrentUnitDescription;
    public RewardSystem rewardSystem;

    private Inventory inventory;

    [SerializeField]
    private List<InventorySlot> slots;

    public bool IsDisplayed { get; private set; }

    private void Awake()
    {
        inventory = Player.instance.Inventory;

        uint i = 0;
        foreach (InventorySlot slot in slots)
        {
            slot.id = i;
            i++;
        }
    }

    private void Start()
    {
        UpdateGUI();
        Hide();
    }

    public void PutInEmptySlot(UnitDescription unit)
    {
        foreach (InventorySlot slot in slots)
        {
            if (slot.Status == InventorySlot.SlotState.Empty)
            {
                slot.PutInSlot(unit);
                return;
            }
        }
    }

    public void UpdateGUI()
    {
        foreach (InventorySlot slot in slots)
            slot.ClearSlot();

        UnitDescription[] unitsInInventory = inventory.GetAllUnits();
        foreach (UnitDescription desc in unitsInInventory)
            PutInEmptySlot(desc);
    }

    public void ClearSlots()
    {
    }

    public void OnClick()
    {
        if (IsDisplayed)
        {
            Hide();
            return;
        }

        IsDisplayed = true;
        ShowGemsSlots();
    }

    public void Show()
    {
        IsDisplayed = true;

        ShowDescription();
        ShowSlots();
        ShowGemsSlots();
    }

    public void Hide()
    {
        IsDisplayed = false;

        HideDescription();

        if (GameManager.instance.gamestate != GameManager.GameState.Shopping)
            HideGemsSlots();

        switch (GameManager.instance.gamestate)
        {
            case GameManager.GameState.Placement:
                ShowSlots();
                break;
            case GameManager.GameState.Combat:
                HideSlots();
                break;
            case GameManager.GameState.Resolution:
                ShowSlots();
                break;
            case GameManager.GameState.Shopping:
                ShowSlots();
                break;
        }
    }

    public void ShowSlots()
    {
        UnitsSlots.gameObject.SetActive(true);
    }

    public void HideSlots()
    {
        UnitsSlots.gameObject.SetActive(false);
    }

    public void ShowDescription()
    {
        CurrentUnitDescription.gameObject.SetActive(true);
    }

    public void HideDescription()
    {
        CurrentUnitDescription.gameObject.SetActive(false);
        CurrentUnitDescription.UnselectActualSlot();
    }

    public void ShowGemsSlots()
    {
        if (GemsSlots != null)
        {
            GemsSlots.gameObject?.SetActive(true);
            if (SynergyHandler.instance != null)
                SynergyHandler.instance.gameObject.SetActive(false);
        }
    }

    public void HideGemsSlots()
    {
        if (GemsSlots != null)
        {
            GemsSlots.gameObject?.SetActive(false);
            if (SynergyHandler.instance != null)
                SynergyHandler.instance.gameObject.SetActive(true);
        }
    }
}

using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    private RectTransform UnitsSlots;
    private RectTransform CurrentUnitDescription;

    private Inventory inventory;

    [SerializeField]
    private List<InventorySlot> slots;

    public bool IsDisplayed { get; private set; }

    private void Awake()
    {
        inventory = FindObjectOfType<Inventory>();
        Transform inventorySubObject = transform.Find("Inventory");
        UnitsSlots = inventorySubObject.Find("UnitsSlots").GetComponent<RectTransform>();
        CurrentUnitDescription = inventorySubObject.Find("CurrentUnitDescription").GetComponent<RectTransform>();

        uint i = 0;
        foreach (InventorySlot slot in slots)
        {
            slot.id = i;
            i++;
        }
    }

    private void Start()
    {
        UnitDescription[] unitsInInventory = inventory.GetAllUnits();
        foreach (UnitDescription desc in unitsInInventory)
            PutInEmptySlot(desc);

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

    public void OnClick()
    {
        if (IsDisplayed)
        {
            Hide();
            return;
        }

        Show();
    }

    public void Show()
    {
        IsDisplayed = true;

        ShowDescription();
        ShowSlots();
    }

    public void Hide()
    {
        IsDisplayed = false;

        HideDescription();

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
    }
}

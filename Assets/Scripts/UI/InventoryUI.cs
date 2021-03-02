using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    private RectTransform UnitsSlots;
    private RectTransform CurrentUnitDescription;

    private Inventory inventory;

    private List<InventorySlot> slots;

    public bool isDisplayed { get; private set; }

    private void Awake()
    {
        inventory = FindObjectOfType<Inventory>();
        Transform inventorySubObject = transform.Find("Inventory");
        UnitsSlots = inventorySubObject.Find("UnitsSlots").GetComponent<RectTransform>();
        CurrentUnitDescription = inventorySubObject.Find("CurrentUnitDescription").GetComponent<RectTransform>();
        
        this.slots = new List<InventorySlot>();
        InventorySlot[] slots = FindObjectsOfType<InventorySlot>();
        foreach (InventorySlot slot in slots)
        {
            AddSlot(slot);
        }
    }

    public void PutInEmptySlot(UnitDescription unit)
    {
        foreach(InventorySlot slot in slots)
        {
            if(slot.Status == InventorySlot.SlotState.Empty)
            {
                slot.PutInSlot(unit);
                return;
            }
        }
    }

    private void Start()
    {
        UnitDescription[] unitsInInventory = inventory.GetAllUnits();
        foreach (UnitDescription desc in unitsInInventory)
            PutInEmptySlot(desc);

        Hide();
    }

    public void OnClick()
    {
        if (isDisplayed)
        {
            Hide();
            return;
        }

        Show();
    }

    public void Show()
    {
        isDisplayed = true;

        ShowDescription();
        ShowSlots();
    }

    public void Hide()
    {
        isDisplayed = false;

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

    public void AddSlot(InventorySlot slot)
    {
        slots.Add(slot);
    }
}

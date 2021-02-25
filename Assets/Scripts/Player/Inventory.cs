﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Player)), DisallowMultipleComponent]
public class Inventory : MonoBehaviour
{
    private Dictionary<uint, UnitDescription> unitsInInventory;

    public InventoryUI inventoryUI;

    private void Awake()
    {
        unitsInInventory = new Dictionary<uint, UnitDescription>();
    }

    private void Start()
    {
        for (int i = 0; i < 8; i++)
        {
            AddRandomUnit();
        }
    }

    public void FillFields()
    {
        inventoryUI = FindObjectOfType<InventoryUI>();
    }

    public void Hide()
    {
        inventoryUI.Hide();
    }

    public void Show()
    {
        inventoryUI.Show();
    }

    public void AddUnitInInventory(UnitDescription unit)
    {
        unitsInInventory.Add(unit.GetId(), unit);
        inventoryUI.PutInEmptySlot(unit);
    }

    public void RemoveFromInventory(UnitDescription unit)
    {
        unitsInInventory.Remove(unit.GetId());
    }

    //A retirer plus tard
    private void AddRandomUnit()
    {
        UnitGenerator.GenerateUnit(Unit.allyTag);
    }
}

﻿using System;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Player)), DisallowMultipleComponent]
public class Inventory : MonoBehaviour
{
    private Dictionary<uint, UnitDescription> unitsInInventory;

    public InventoryUI inventoryUI;

    [Header("Game start setup"), SerializeField]
    private uint startingUnitCount;

    private void Awake()
    {
        unitsInInventory = new Dictionary<uint, UnitDescription>();
    }

    private void Start()
    {
        for (int i = 0; i < startingUnitCount; i++)
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
    }

    public void RemoveFromInventory(UnitDescription unit)
    {
        unitsInInventory.Remove(unit.GetId());
    }

    //A retirer plus tard
    private void AddRandomUnit()
    {
        UnitDescription newUnit = null;
        bool createdUnit = false;
        while (!createdUnit) //Unsafe, must have at most as many slots than possible combinations
        {
            newUnit = UnitGenerator.GenerateUnit(Unit.allyTag);
            if (GetEquivalentUnit(newUnit) == null)
                createdUnit = true;
        }
        AddUnitInInventory(newUnit);
    }

    public UnitDescription[] GetAllUnits()
    {
        return unitsInInventory.Values.ToArray();
    }

    private UnitDescription GetEquivalentUnit(UnitDescription unitToFind)
    {
        foreach(UnitDescription unit in unitsInInventory.Values)
        {
            if (unit.IsOfSameTypeThan(unitToFind))
            {
                return unit;
            }
        }

        return null;
    }
}

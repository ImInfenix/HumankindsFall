using System;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Player)), DisallowMultipleComponent]
public class Inventory : MonoBehaviour
{
    private Dictionary<uint, UnitDescription> unitsInInventory;
    private List<Gem> gemsInInventory;

    public InventoryUI inventoryUI;

    [Header("Game start setup"), SerializeField]
    private uint startingUnitCount;

    private void Awake()
    {
        unitsInInventory = new Dictionary<uint, UnitDescription>();
        gemsInInventory = new List<Gem>();

        //TEST GEMS
        GenerateGems();
    }

    public void Initialize(List<UnitDescription> units = null)
    {
        if(units == null)
        {
            for (int i = 0; i < startingUnitCount; i++)
            {
                AddRandomUnit();
            }
            return;
        }

        foreach (UnitDescription unit in units)
            AddUnitInInventory(unit);
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

    public void AddUnitInInventory(UnitDescription unit, bool addToGUINow = false)
    {
        UnitDescription equivalentUnit = GetEquivalentUnit(unit);
        if(equivalentUnit == null)
        {
            unitsInInventory.Add(unit.GetId(), unit);

            if (addToGUINow)
                inventoryUI.PutInEmptySlot(unit);

            return;
        }

        equivalentUnit.EarnExperience(3);
    }

    public void RemoveFromInventory(UnitDescription unit)
    {
        unitsInInventory.Remove(unit.GetId());
        inventoryUI.UpdateGUI();
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

    public UnitDescription GetUnit(uint id)
    {
        return unitsInInventory[id];
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

    public List<Gem> GetAllGems()
    {
        return gemsInInventory;
    }

    public void AddGem(Gem gem)
    {
        gemsInInventory.Add(gem);
    }

    public void RemoveGem(Gem gem)
    {
        for (int i = 0; i < gemsInInventory.Count; i++)
        {
            if (gemsInInventory[i].GetType() == gem.GetType())
            {
                gemsInInventory.RemoveAt(i);
                break;
            }
        }
    }

    //TESTING FUNCTION
    //generate one gem of each gameObject in Resources/Gems
    public void GenerateGems()
    {
        GameObject[] gemsGameObjects = Resources.LoadAll("Gems", typeof(GameObject)).Cast<GameObject>().ToArray();
        foreach (GameObject gem in gemsGameObjects)
        {
            GameObject newGem = Instantiate(gem);
            gemsInInventory.Add(newGem.GetComponent<Gem>());
            Destroy(newGem);
        }
    }
}

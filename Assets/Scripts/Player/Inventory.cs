using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Player))]
public class Inventory : MonoBehaviour
{
    private InventoryUI inventoryUI;

    public void FillFields()
    {
        inventoryUI = FindObjectOfType<InventoryUI>();
    }

    public void UpdateGUI()
    {
        inventoryUI.Hide();
    }
}

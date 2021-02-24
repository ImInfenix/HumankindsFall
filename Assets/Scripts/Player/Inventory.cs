using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Player)), DisallowMultipleComponent]
public class Inventory : MonoBehaviour
{
    private InventoryUI inventoryUI;

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
}

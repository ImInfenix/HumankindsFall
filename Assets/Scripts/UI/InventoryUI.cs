using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    private Inventory inventory;

    private void Awake()
    {
        inventory = FindObjectOfType<Inventory>();
    }

    public void OnClick()
    {
        if(inventory.isDisplayed)
        {
            inventory.Hide();
            return;
        }

        inventory.Show();
    }
}

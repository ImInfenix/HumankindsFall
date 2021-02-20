using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    [SerializeField]
    private InventoryUI inventoryUI;
    [SerializeField]
    private Button inventoryButton;
    [SerializeField]
    private RectTransform inventoryGUI;

    public bool isDisplayed { get; private set; }

    public void FillFields()
    {
        inventoryUI = FindObjectOfType<InventoryUI>();
        inventoryButton = inventoryUI.transform.Find("InventoryOpenCloseButton").GetComponent<Button>();
        inventoryGUI = inventoryUI.transform.Find("Inventory").GetComponent<RectTransform>();

        Hide();
    }

    public void Show()
    {
        inventoryGUI.gameObject.SetActive(true);
        isDisplayed = true;
    }

    public void Hide()
    {
        inventoryGUI.gameObject.SetActive(false);
        isDisplayed = false;
    }
}

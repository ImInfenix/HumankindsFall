using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GemSlot : MonoBehaviour
{
    private Gem gem;
    private bool isUnitSlot = false;

    [SerializeField]
    private Image selectedImage;

    private UnitDescriptionDisplay unitDescriptionDisplay;

    public static GemSlot selectedGemSlot;
    private bool isSelected = false;
    private bool isShop = false;

    private ShopSystem shopSystem;

    public Gem Gem { get => gem; set => gem = value; }
    public bool IsUnitSlot { get => isUnitSlot; set => isUnitSlot = value; }
    public bool IsSelected { get => isSelected; set => isSelected = value; }
    public bool IsShop { get => isShop; set => isShop = value; }

    private void Awake()
    {
        unitDescriptionDisplay = GameObject.Find("CurrentUnitDescription").GetComponent<UnitDescriptionDisplay>();
        FindShopSystem();
    }

    public void FindShopSystem()
    {
        shopSystem = GameObject.Find("ShopSystem").GetComponent<ShopSystem>();
    }

    public void SelectSlot()
    {
        selectedImage.gameObject.SetActive(true);
        selectedGemSlot = this;
        IsSelected = true;

        if (unitDescriptionDisplay != null)
            unitDescriptionDisplay.UnselectActualSlot();

        if (shopSystem)
        {
            if (isShop)
                shopSystem.SetShopToBuyMode();
            else
                shopSystem.SetShopToSellMode();

            unitDescriptionDisplay.gameObject.SetActive(false);
        }        
    }

    public void UnselectSlot()
    {
        selectedImage.gameObject.SetActive(false);
        selectedGemSlot = null;
        IsSelected = false;
        shopSystem.SetShopToNoneMode();
    }
}

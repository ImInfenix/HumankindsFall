using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetGemsButton : MonoBehaviour
{
    [SerializeField] GameObject CurrentUnitDesc;
    [SerializeField] GameObject GemsInventory;

    public void ResetGems()
    {
        List<Gem> currentGems = CurrentUnitDesc.GetComponent<UnitDescriptionDisplay>().GetCurrentGems();

        if (currentGems.Count > 0)
        {
            foreach (Gem gem in currentGems)
            {
                Player.instance.Inventory.AddGem(gem);
                if (GemsInventory != null)
                {
                    GemsInventory.GetComponent<GemsInventory>().UpdateDisplay();
                }
            }
        }

        CurrentUnitDesc.GetComponent<UnitDescriptionDisplay>().ClearGems();
        CurrentUnitDesc.GetComponent<UnitDescriptionDisplay>().UpdateDescription();
    }
}

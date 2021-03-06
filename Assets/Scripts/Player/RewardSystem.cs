using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardSystem : MonoBehaviour
{
    [SerializeField]
    List<InventorySlot> rewardSlots;

    [SerializeField]
    private UnitDescriptionDisplay unitDescriptionDisplay;

    private void Awake()
    {
        gameObject.SetActive(false);
    }

    public void StartRewardPhase()
    {
        gameObject.SetActive(true);
        Player.instance.Wallet.Earn(10); // a terme, remplacer le 10 par niveau.getRecompense()
        foreach (InventorySlot slot in rewardSlots)
            slot.PutInSlot(UnitGenerator.GenerateUnit(Unit.allyTag));
    }

    public void ConfirmRewardPhase()
    {
        if (unitDescriptionDisplay.GetSelectedSlotType() != InventorySlot.SlotType.Shop)
            return;

        Player.instance.Inventory.AddUnitInInventory(unitDescriptionDisplay.GetActualSlot().GetCurrentUnitDescription());
        gameObject.SetActive(false);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardSystem : MonoBehaviour
{
    [SerializeField]
    List<InventorySlot> rewardSlots;

    [SerializeField]
    private UnitDescriptionDisplay unitDescriptionDisplay;

    private List<uint> unitsInCombatIds;

    private void Awake()
    {
        gameObject.SetActive(false);
    }

    public void RegisterCombatParticipants()
    {
        unitsInCombatIds = new List<uint>();
        foreach(Unit unit in FindObjectsOfType<Unit>())
        {
            if(unit.CompareTag(Unit.allyTag))
                unitsInCombatIds.Add(unit.id);
        }
    }

    public void StartRewardPhase()
    {
        gameObject.SetActive(true);
        Player.instance.Wallet.Earn(10); // a terme, remplacer le 10 par niveau.getRecompense()
        foreach (InventorySlot slot in rewardSlots)
            slot.PutInSlot(UnitGenerator.GenerateUnit(Unit.allyTag));
        foreach (uint id in unitsInCombatIds)
            Player.instance.Inventory.GetUnit(id).EarnExperience(1);

        Player.instance.Inventory.inventoryUI.UpdateGUI();
    }

    public void ConfirmRewardPhase()
    {
        if (unitDescriptionDisplay.GetSelectedSlotType() != InventorySlot.SlotType.Shop)
            return;

        Player.instance.Inventory.AddUnitInInventory(unitDescriptionDisplay.GetActualSlot().GetCurrentUnitDescription(), true);
        gameObject.SetActive(false);
        SavingSystem.SaveData();

        Player.instance.Inventory.Hide();
        ResolutionPhaseHandler.instance.ShowExitButton();
        ShopSystem.generateNewContent = true;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UnitDescriptionDisplay : MonoBehaviour
{
    [SerializeField]
    private TMP_Text UnitName;
    [SerializeField]
    private TMP_Text UnitStats;
    [SerializeField]
    private TMP_Text UnitExperience;
    [SerializeField]
    private GameObject GemSlot;
    [SerializeField]
    private GameObject UnitGemsZone;

    private UnitDescription currentDescription;
    private InventorySlot actualSlot;
    private List<GameObject> gemSlots;
    private List<Gem> currentGems;
    private List<GameObject> currentGemsDisplayed;

    public void Awake()
    {
        UnitName.text = "";
        UnitStats.text = "";
        actualSlot = null;
        gemSlots = new List<GameObject>();
        currentGems = new List<Gem>();
        currentGemsDisplayed = new List<GameObject>();
    }

    public void ChangeActualSlot(InventorySlot slot)
    {
        if (actualSlot != null)
        {
            actualSlot.Unselect();
        }
        actualSlot = slot;
        slot.Select();
        UpdateDescription();

        Player.instance.Inventory.Show();
    }

    public void UnselectActualSlot()
    {
        if (actualSlot == null)
            return;

        actualSlot.Unselect();
        actualSlot = null;
        UpdateDescription();

        Player.instance.Inventory.Hide();
    }

    public void SetUnitName(string name)
    {
        UnitName.text = name;
    }

    public void SetUnitStats(string stats)
    {
        UnitStats.text = stats;
    }

    public void SetUnitExperience(uint amount)
    {
        if (actualSlot.GetSlotType() == InventorySlot.SlotType.Inventory)
        {
            UnitExperience.text = $"XP: {amount}";
            return;
        }

        UnitExperience.text = "";
    }

    public void UpdateDescription()
    {
        if (actualSlot == null)
        {
            UnitName.text = "";
            UnitStats.text = "";
            UnitExperience.text = "";
            return;
        }

        currentDescription = actualSlot.GetCurrentUnitDescription();

        SetUnitName(currentDescription.GetUnitName());
        SetUnitExperience(currentDescription.GetExperience());
        ClassStat classe = currentDescription.GetClass();
        RaceStat race = currentDescription.GetRace();

        int maxLife = classe.maxLife + race.maxLife;
        int maxMana = race.maxMana;
        int armor = classe.armor + race.armor;
        float atakSpeed = classe.attackSpeed + race.attackSpeed;

        string stats =
            "Class : " + classe.name + "\n" +
            "Race : " + race.name + "\n" +
            "PV : " + maxLife + "\n" +
            "Mana : " + maxMana + "\n" +
            "Armor : " + armor + "\n" +
            "Attack Speed : " + atakSpeed;
        ;
        SetUnitStats(stats);

        string[] gems = currentDescription.GetGems();
        GenerateSlots(gems);
    }

    public void GenerateSlots(string[] unitGems)
    {
        //number of gems slots should be the unit level
        int numberOfGemsSlots = 8;

        float widthOffset = 0.8f;
        float heightOffset = -0.8f;

        int nbGemsSlotsPerLine = numberOfGemsSlots;
        if (numberOfGemsSlots > 5)
            nbGemsSlotsPerLine = 5;

        float baseX = 0 - (nbGemsSlotsPerLine - 1) * widthOffset / 2;
        float x = baseX;
        float y = 0.5f;

        //destroy previous displayed slots
        foreach (GameObject slot in gemSlots)
        {
            Destroy(slot);
        }

        gemSlots = new List<GameObject>();
        currentGems = new List<Gem>();

        //create gem slots
        for (int i = 1; i <= numberOfGemsSlots; i++)
        {
            //get RectTransform corners to start at the top left corner
            Vector3 position = new Vector3(x, y, 0) + UnitGemsZone.transform.position;

            x += widthOffset;

            GameObject slot = Instantiate(GemSlot, position, Quaternion.identity, UnitGemsZone.transform);
            slot.GetComponent<GemSlot>().IsUnitSlot = true;
            gemSlots.Add(slot);

            if (i != 0 && i % 5 == 0)
            {
                x = baseX;
                y += heightOffset;
            }
        }

        //put unit owned gems in slots
        if (unitGems != null)
        {
            for (int i = 0; i < unitGems.Length; i++)
            {
                GameObject gemGameObject = Resources.Load("Gems/" + unitGems[i]) as GameObject;
                Vector3 position = gemSlots[i].transform.position;
                GameObject newGem = Instantiate(gemGameObject, position, Quaternion.identity, gemSlots[i].transform);

                GemSlot currentGemSlot = gemSlots[i].GetComponent<GemSlot>();
                currentGemSlot.Gem = newGem.GetComponent<Gem>();

                newGem.transform.GetChild(0).GetComponent<GemUI>().GemSlot = currentGemSlot;

                currentGemsDisplayed.Add(newGem);
                currentGems.Add(currentGemSlot.Gem);
            }
        }
    }

    public InventorySlot.SlotType GetSelectedSlotType()
    {
        if (actualSlot == null) return InventorySlot.SlotType.None;

        return actualSlot.GetSlotType();
    }

    public InventorySlot GetActualSlot()
    {
        return actualSlot;
    }

    public List<Gem> GetCurrentGems()
    {
        return currentGems;
    }

    public void AddGem(GameObject gemGameObject)
    {
        Gem gem = gemGameObject.GetComponent<Gem>();
        currentGems.Add(gem);
        currentDescription.AddGem(gem.GetType().ToString());
        currentGemsDisplayed.Add(gemGameObject);
    }
    
    public void ClearGems()
    {
        currentGems.Clear();
        currentDescription.SetGems(null);

        if (currentGemsDisplayed.Count > 0)
        {
            foreach (GameObject gem in currentGemsDisplayed)
            {
                Destroy(gem);
            }
            currentGemsDisplayed.Clear();
        }
    }
}

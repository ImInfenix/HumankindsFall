using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Text;

public class UnitDescriptionDisplay : MonoBehaviour
{
    [SerializeField]
    private TMP_Text UnitName;
    [SerializeField]
    private TMP_Text UnitStats;
    [SerializeField]
    private TMP_Text UnitExperience;
    [SerializeField]
    private TMP_Text UnitLevel;
    [SerializeField]
    private GameObject GemSlot;
    [SerializeField]
    private GameObject UnitGemsZone;
    [SerializeField]
    private Button ResetGemsButton;

    private UnitDescription currentDescription;
    private InventorySlot actualSlot;
    private List<GameObject> gemSlots;
    private List<Gem> currentGems;
    private List<GameObject> currentGemsDisplayed;

    [HideInInspector]
    public ShopSystem shopSystem;

    private GameObject gemsUIGameObject;

    public void Awake()
    {
        actualSlot = null;
        gemSlots = new List<GameObject>();
        currentGems = new List<Gem>();
        currentGemsDisplayed = new List<GameObject>();
        UpdateDescription();

        gemsUIGameObject = GameObject.Find("GemsSlots");
    }

    public void ChangeActualSlot(InventorySlot slot)
    {
        if (actualSlot != null)
        {
            actualSlot.Unselect();
        }

        if(actualSlot == slot)
        {
            UnselectActualSlot();
            return;
        }

        actualSlot = slot;
        slot.Select();
        UpdateDescription();

        Player.instance.Inventory.Show();

        if (shopSystem != null)
        {
            if (actualSlot == null)
            {
                shopSystem.SetShopToNoneMode();
                gemsUIGameObject.SetActive(true);
            }
            else if (actualSlot.GetSlotType() == InventorySlot.SlotType.Inventory)
            {
                shopSystem.SetShopToSellMode();
                gemsUIGameObject.SetActive(false);
            }
            else
            {
                shopSystem.SetShopToBuyMode();
                gemsUIGameObject.SetActive(false);
            }
        }
    }

    public void UnselectActualSlot()
    {
        if (actualSlot == null)
            return;

        actualSlot.Unselect();
        actualSlot = null;
        UpdateDescription();
        gemsUIGameObject.SetActive(true);

        Player.instance.Inventory.Hide();

        shopSystem?.SetShopToNoneMode();
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

    public void SetUnitLevel(uint level)
    {
        if (actualSlot.GetSlotType() == InventorySlot.SlotType.Inventory)
        {
            UnitLevel.text = $"Level: {level}";
            return;
        }

        UnitLevel.text = "";
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
        SetUnitLevel(currentDescription.GetLevel());
        ClassStat classe = currentDescription.GetClass();
        RaceStat race = currentDescription.GetRace();

        //get ability name and add space before every capital letter
        string abilityName = currentDescription.GetAbilityName();
        StringBuilder abilityNameWithSpaces = new StringBuilder(abilityName.Length * 2);
        abilityNameWithSpaces.Append(abilityName[0]);
        for (int i = 1; i < abilityName.Length; i++)
        {
            if (char.IsUpper(abilityName[i]) && abilityName[i - 1] != ' ')
                abilityNameWithSpaces.Append(' ');
            abilityNameWithSpaces.Append(abilityName[i]);
        }


        string stats =
            "Class : " + classe.name + "\n\n" +
            "Race : " + race.name + "\n\n" +
            "Ability : " + abilityNameWithSpaces + "\n"
            ;
        SetUnitStats(stats);

        string[] gems = currentDescription.GetGems();


        //destroy previously displayed slots
        foreach (GameObject slot in gemSlots)
        {
            Destroy(slot);
        }

        gemSlots = new List<GameObject>();
        currentGems = new List<Gem>();

        if (ResetGemsButton)
        {
            if (GameManager.instance.gamestate == GameManager.GameState.Placement ||
                    (GameManager.instance.gamestate == GameManager.GameState.Shopping ||
                     GameManager.instance.gamestate == GameManager.GameState.Resolution) &&
                     actualSlot.GetSlotType() == InventorySlot.SlotType.Inventory)
            {
                GenerateSlots(gems);
                ResetGemsButton.gameObject.SetActive(true);
            }

            else
                ResetGemsButton.gameObject.SetActive(false);
        }
    }

    public void GenerateSlots(string[] unitGems)
    {
        //number of gems slots should be the unit level
        uint numberOfGemsSlots = currentDescription.GetLevel();

        float widthOffset = 0.8f;
        float heightOffset = -0.8f;

        uint nbGemsSlotsPerLine = numberOfGemsSlots;

        float y = 0.5f;

        if (numberOfGemsSlots > 5)
            nbGemsSlotsPerLine = 5;

        else
            y = 0;

        float baseX = 0 - (nbGemsSlotsPerLine - 1) * widthOffset / 2;
        float x = baseX;

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

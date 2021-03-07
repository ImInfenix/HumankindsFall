using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SynergyHandler : MonoBehaviour
{

    [SerializeField]
    //private TMP_Text synergyText;
    public static SynergyHandler instance;
    public Button ButtonSynergy;

    private List<RaceCount> rc = new List<RaceCount>();
    private List<ClassCount> cc = new List<ClassCount>();

    private Button orcButton;
    private Button skeletonButton;
    private Button octopusButton;
    private Button elementalButton;
    private Button giantButton;

    private Button mageButton;
    private Button warriorButton;
    private Button tankButton;
    private Button bowmanButton;
    private Button healerButton;
    private Button supportButton;
    private Button berserkerButton;
    private Button assassinButton;

    private RaceCount orcs = new RaceCount(Race.Orc, 0);
    private RaceCount skeletons = new RaceCount(Race.Skeleton, 0);
    private RaceCount octopus = new RaceCount(Race.Octopus, 0);
    private RaceCount elementals = new RaceCount(Race.Elemental, 0);
    private RaceCount giants = new RaceCount(Race.Giant, 0);

    private ClassCount mages = new ClassCount(Class.Mage, 0);
    private ClassCount warriors = new ClassCount(Class.Warrior, 0);
    private ClassCount tanks = new ClassCount(Class.Tank, 0);
    private ClassCount bowmans = new ClassCount(Class.Bowman, 0);
    private ClassCount healers = new ClassCount(Class.Healer, 0);
    private ClassCount supports = new ClassCount(Class.Support, 0);
    private ClassCount berserkers = new ClassCount(Class.Berserker, 0);
    private ClassCount assassins = new ClassCount(Class.Assassin, 0);

    private void Awake()
    {
        //Singleton creation
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        //synergyText = GetComponent<TextMeshProUGUI>();

        rc.Add(orcs);
        rc.Add(skeletons);
        rc.Add(octopus);
        rc.Add(elementals);
        rc.Add(giants);

        cc.Add(mages);
        cc.Add(warriors);
        cc.Add(tanks);
        cc.Add(bowmans);
        cc.Add(healers);
        cc.Add(supports);
        cc.Add(berserkers);
        cc.Add(assassins);

        orcButton = Instantiate(ButtonSynergy, GetComponent<RectTransform>());
        orcButton.GetComponentInChildren<TextMeshProUGUI>().text = " Orc : ";
        orcButton.GetComponent<SynergyButton>().setTooltipDef(orcs.getString());
        orcButton.gameObject.SetActive(false);

        skeletonButton = Instantiate(ButtonSynergy, GetComponent<RectTransform>());
        skeletonButton.GetComponentInChildren<TextMeshProUGUI>().text = " Skeleton : ";
        skeletonButton.GetComponent<SynergyButton>().setTooltipDef(skeletons.getString());
        skeletonButton.gameObject.SetActive(false);

        octopusButton = Instantiate(ButtonSynergy, GetComponent<RectTransform>());
        octopusButton.GetComponentInChildren<TextMeshProUGUI>().text = " Octopus : ";
        octopusButton.GetComponent<SynergyButton>().setTooltipDef(octopus.getString());
        octopusButton.gameObject.SetActive(false);

        elementalButton = Instantiate(ButtonSynergy, GetComponent<RectTransform>());
        elementalButton.GetComponentInChildren<TextMeshProUGUI>().text = " Elemental : ";
        elementalButton.GetComponent<SynergyButton>().setTooltipDef(elementals.getString());
        elementalButton.gameObject.SetActive(false);

        giantButton = Instantiate(ButtonSynergy, GetComponent<RectTransform>());
        giantButton.GetComponentInChildren<TextMeshProUGUI>().text = " Giant : ";
        giantButton.GetComponent<SynergyButton>().setTooltipDef(giants.getString());
        giantButton.gameObject.SetActive(false);


        mageButton = Instantiate(ButtonSynergy, GetComponent<RectTransform>());
        mageButton.GetComponentInChildren<TextMeshProUGUI>().text = " Mage : ";
        mageButton.GetComponent<SynergyButton>().setTooltipDef(mages.getString());
        mageButton.gameObject.SetActive(false);

        warriorButton = Instantiate(ButtonSynergy, GetComponent<RectTransform>());
        warriorButton.GetComponentInChildren<TextMeshProUGUI>().text = " Warrior : ";
        warriorButton.GetComponent<SynergyButton>().setTooltipDef(warriors.getString());
        warriorButton.gameObject.SetActive(false);

        tankButton = Instantiate(ButtonSynergy, GetComponent<RectTransform>());
        tankButton.GetComponentInChildren<TextMeshProUGUI>().text = " Tank : ";
        tankButton.GetComponent<SynergyButton>().setTooltipDef(tanks.getString());
        tankButton.gameObject.SetActive(false);

        bowmanButton = Instantiate(ButtonSynergy, GetComponent<RectTransform>());
        bowmanButton.GetComponentInChildren<TextMeshProUGUI>().text = " Bowman : ";
        bowmanButton.GetComponent<SynergyButton>().setTooltipDef(bowmans.getString());
        bowmanButton.gameObject.SetActive(false);

        healerButton = Instantiate(ButtonSynergy, GetComponent<RectTransform>());
        healerButton.GetComponentInChildren<TextMeshProUGUI>().text = " Healer : ";
        healerButton.GetComponent<SynergyButton>().setTooltipDef(healers.getString());
        healerButton.gameObject.SetActive(false);

        supportButton = Instantiate(ButtonSynergy, GetComponent<RectTransform>());
        supportButton.GetComponentInChildren<TextMeshProUGUI>().text = " Support : ";
        supportButton.GetComponent<SynergyButton>().setTooltipDef(supports.getString());
        supportButton.gameObject.SetActive(false);

        berserkerButton = Instantiate(ButtonSynergy, GetComponent<RectTransform>());
        berserkerButton.GetComponentInChildren<TextMeshProUGUI>().text = " Berserker : ";
        berserkerButton.GetComponent<SynergyButton>().setTooltipDef(berserkers.getString());
        berserkerButton.gameObject.SetActive(false);

        assassinButton = Instantiate(ButtonSynergy, GetComponent<RectTransform>());
        assassinButton.GetComponentInChildren<TextMeshProUGUI>().text = " Assassin : ";
        assassinButton.GetComponent<SynergyButton>().setTooltipDef(assassins.getString());
        assassinButton.gameObject.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public List<ClassCount> getClassList()
    {
        return cc;
    }

    public List<RaceCount> getRaceList()
    {
        return rc;
    }

    public void addUnit(Unit u)
    {
        if(u.tag == Unit.allyTag)
        {
            foreach (RaceCount r in rc)
            {
                if (r.getRace() == u.getRace())
                {
                    r.setNumber(r.getNumber() + 1);
                }
            }

            foreach (ClassCount c in cc)
            {
                if (c.getClass() == u.getClass())
                {
                    c.setNumber(c.getNumber() + 1);
                }
            }

            updateText();
        }   
    }

    public void removeUnit(Unit u)
    {
        if (u.tag == "UnitAlly")
        {
            foreach (RaceCount r in rc)
            {
                if (r.getRace() == u.getRace())
                    r.setNumber(r.getNumber() - 1);
            }

            foreach (ClassCount c in cc)
            {
                if (c.getClass() == u.getClass())
                    c.setNumber(c.getNumber() - 1);
            }

            updateText();
        }
    }

    private void updateText()
    {
        //synergyText.text = "";
        foreach (RaceCount r in rc)
        {
            if(r.getNumber() > 0)
            {
                switch (r.getRace())
                {
                    case Race.Orc:
                        if(orcButton)
                        {
                            orcButton.GetComponentInChildren<TextMeshProUGUI>().text = " Orc : " + r.getNumber();
                            orcButton.gameObject.SetActive(true);
                        }
                        break;

                    case Race.Skeleton:
                        if(skeletonButton)
                        {
                            skeletonButton.GetComponentInChildren<TextMeshProUGUI>().text = " Skeleton : " + r.getNumber();
                            skeletonButton.gameObject.SetActive(true);
                        }
                        
                        break;

                    case Race.Octopus:
                        if(octopusButton)
                        {
                            octopusButton.GetComponentInChildren<TextMeshProUGUI>().text = " Octopus : " + r.getNumber();
                            octopusButton.gameObject.SetActive(true);
                        }
                        
                        break;

                    case Race.Elemental:
                        if(elementalButton)
                        {
                            elementalButton.GetComponentInChildren<TextMeshProUGUI>().text = " Elemental : " + r.getNumber();
                            elementalButton.gameObject.SetActive(true);
                        }
                        
                        break;

                    case Race.Giant:
                        if(giantButton)
                        {
                            giantButton.GetComponentInChildren<TextMeshProUGUI>().text = " Giant : " + r.getNumber();
                            giantButton.gameObject.SetActive(true);
                        }
                        
                        break;
                        
                }
            }
            else
            {
                switch (r.getRace())
                {
                    case Race.Orc:
                        if(orcButton)
                            orcButton.gameObject.SetActive(false);
                        break;

                    case Race.Skeleton:
                        if(skeletonButton)
                            skeletonButton.gameObject.SetActive(false);
                        break;

                    case Race.Octopus:
                        if(octopusButton)
                            octopusButton.gameObject.SetActive(false);
                        break;

                    case Race.Elemental:
                        if(elementalButton)
                            elementalButton.gameObject.SetActive(false);
                        break;

                    case Race.Giant:
                        if(giantButton)
                            giantButton.gameObject.SetActive(false);
                        break;
                }
            }
                
        }

        foreach (ClassCount c in cc)
        {
            if (c.getNumber() > 0)
            {
                switch (c.getClass())
                {
                    case Class.Mage:
                        if(mageButton)
                        {
                            mageButton.GetComponentInChildren<TextMeshProUGUI>().text = " Mage : " + c.getNumber();
                            mageButton.gameObject.SetActive(true);
                        }
                       
                        break;

                    case Class.Warrior:
                        if(warriorButton)
                        {
                            warriorButton.GetComponentInChildren<TextMeshProUGUI>().text = " Warrior : " + c.getNumber();
                            warriorButton.gameObject.SetActive(true);
                        }
                        
                        break;

                    case Class.Tank:
                        if(tankButton)
                        {
                            tankButton.GetComponentInChildren<TextMeshProUGUI>().text = " Tank : " + c.getNumber();
                            tankButton.gameObject.SetActive(true);
                        }
                       
                        break;

                    case Class.Bowman:
                        if(bowmanButton)
                        {
                            bowmanButton.GetComponentInChildren<TextMeshProUGUI>().text = " Bowman : " + c.getNumber();
                            bowmanButton.gameObject.SetActive(true);
                        }
                        
                        break;

                    case Class.Healer:
                        if(healerButton)
                        {
                            healerButton.GetComponentInChildren<TextMeshProUGUI>().text = " Healer : " + c.getNumber();
                            healerButton.gameObject.SetActive(true);
                        }
                       
                        break;

                    case Class.Support:
                        if(supportButton)
                        {
                            supportButton.GetComponentInChildren<TextMeshProUGUI>().text = " Support : " + c.getNumber();
                            supportButton.gameObject.SetActive(true);
                        }
                        
                        break;

                    case Class.Berserker:
                        if(berserkerButton)
                        {
                            berserkerButton.GetComponentInChildren<TextMeshProUGUI>().text = " Berserker : " + c.getNumber();
                            berserkerButton.gameObject.SetActive(true);
                        }
                        
                        break;

                    case Class.Assassin:
                        if(assassinButton)
                        {
                            assassinButton.GetComponentInChildren<TextMeshProUGUI>().text = " Assassin : " + c.getNumber();
                            assassinButton.gameObject.SetActive(true);
                        }
                       
                        break;
                }
            }                                
            else
            {
                switch (c.getClass())
                {
                    case Class.Mage:
                        if(mageButton)
                            mageButton.gameObject.SetActive(false);
                        break;

                    case Class.Warrior:
                        if(warriorButton)
                            warriorButton.gameObject.SetActive(false);
                        break;

                    case Class.Tank:
                        if(tankButton)
                            tankButton.gameObject.SetActive(false);
                        break;

                    case Class.Bowman:
                        if(bowmanButton)
                            bowmanButton.gameObject.SetActive(false);
                        break;

                    case Class.Healer:
                        if(healerButton)
                            healerButton.gameObject.SetActive(false);
                        break;

                    case Class.Support:
                        if(supportButton)
                            supportButton.gameObject.SetActive(false);
                        break;

                    case Class.Berserker:
                        if(berserkerButton)
                            berserkerButton.gameObject.SetActive(false);
                        break;

                    case Class.Assassin:
                        if(assassinButton)
                            assassinButton.gameObject.SetActive(false);
                        break;
                }
            }
        }
    }
}

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
    private Button ratmanButton;
    private Button demonButton;

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
    private RaceCount ratmen = new RaceCount(Race.Ratman, 0);
    private RaceCount demons = new RaceCount(Race.Demon, 0);

    private ClassCount mages = new ClassCount(Class.Mage, 0);
    private ClassCount warriors = new ClassCount(Class.Warrior, 0);
    private ClassCount tanks = new ClassCount(Class.Tank, 0);
    private ClassCount bowmans = new ClassCount(Class.Bowman, 0);
    private ClassCount healers = new ClassCount(Class.Healer, 0);
    private ClassCount supports = new ClassCount(Class.Support, 0);
    private ClassCount berserkers = new ClassCount(Class.Berserker, 0);
    private ClassCount assassins = new ClassCount(Class.Assassin, 0);

    private Color colorLvlBase = new Color(1f, 1f, 1f, 163 / 255f);
    private Color colorLvlInt = new Color(210f / 255f, 127f / 255f, 25f / 255f, 163f / 255f);
    private Color colorLvlMax = new Color(1f, 0, 0, 163f / 255f);//Color.red;

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
        rc.Add(ratmen);
        rc.Add(demons);

        cc.Add(mages);
        cc.Add(warriors);
        cc.Add(tanks);
        cc.Add(bowmans);
        cc.Add(healers);
        cc.Add(supports);
        cc.Add(berserkers);
        cc.Add(assassins);

        orcButton = Instantiate(ButtonSynergy, GetComponent<RectTransform>());
        orcButton.GetComponent<SynergyButton>().setTooltipDef(orcs.getString());
        orcButton.gameObject.SetActive(false);

        skeletonButton = Instantiate(ButtonSynergy, GetComponent<RectTransform>());
        skeletonButton.GetComponent<SynergyButton>().setTooltipDef(skeletons.getString());
        skeletonButton.gameObject.SetActive(false);

        octopusButton = Instantiate(ButtonSynergy, GetComponent<RectTransform>());
        octopusButton.GetComponent<SynergyButton>().setTooltipDef(octopus.getString());
        octopusButton.gameObject.SetActive(false);

        elementalButton = Instantiate(ButtonSynergy, GetComponent<RectTransform>());
        elementalButton.GetComponent<SynergyButton>().setTooltipDef(elementals.getString());
        elementalButton.gameObject.SetActive(false);

        giantButton = Instantiate(ButtonSynergy, GetComponent<RectTransform>());
        giantButton.GetComponent<SynergyButton>().setTooltipDef(giants.getString());
        giantButton.gameObject.SetActive(false);

        ratmanButton = Instantiate(ButtonSynergy, GetComponent<RectTransform>());
        ratmanButton.GetComponent<SynergyButton>().setTooltipDef(ratmen.getString());
        ratmanButton.gameObject.SetActive(false);

        demonButton = Instantiate(ButtonSynergy, GetComponent<RectTransform>());
        demonButton.GetComponent<SynergyButton>().setTooltipDef(demons.getString());
        demonButton.gameObject.SetActive(false);


        mageButton = Instantiate(ButtonSynergy, GetComponent<RectTransform>());
        mageButton.GetComponent<SynergyButton>().setTooltipDef(mages.getString());
        mageButton.gameObject.SetActive(false);

        warriorButton = Instantiate(ButtonSynergy, GetComponent<RectTransform>());
        warriorButton.GetComponent<SynergyButton>().setTooltipDef(warriors.getString());
        warriorButton.gameObject.SetActive(false);

        tankButton = Instantiate(ButtonSynergy, GetComponent<RectTransform>());
        tankButton.GetComponent<SynergyButton>().setTooltipDef(tanks.getString());
        tankButton.gameObject.SetActive(false);

        bowmanButton = Instantiate(ButtonSynergy, GetComponent<RectTransform>());
        bowmanButton.GetComponent<SynergyButton>().setTooltipDef(bowmans.getString());
        bowmanButton.gameObject.SetActive(false);

        healerButton = Instantiate(ButtonSynergy, GetComponent<RectTransform>());
        healerButton.GetComponent<SynergyButton>().setTooltipDef(healers.getString());
        healerButton.gameObject.SetActive(false);

        supportButton = Instantiate(ButtonSynergy, GetComponent<RectTransform>());
        supportButton.GetComponent<SynergyButton>().setTooltipDef(supports.getString());
        supportButton.gameObject.SetActive(false);

        berserkerButton = Instantiate(ButtonSynergy, GetComponent<RectTransform>());
        berserkerButton.GetComponent<SynergyButton>().setTooltipDef(berserkers.getString());
        berserkerButton.gameObject.SetActive(false);

        assassinButton = Instantiate(ButtonSynergy, GetComponent<RectTransform>());
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

    public RaceCount getElementals()
    {
        return elementals;
    }

    public void addUnit(Unit u)
    {
        if(u.tag == Unit.allyTag)
        {
            foreach (RaceCount r in rc)
            {
                if (r.getRace() == u.getRace() && u.getClass() != Class.DemonKing)
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
                            if(r.getNumber()>=2)
                            {
                                orcButton.GetComponent<SynergyButton>().addOutline(colorLvlMax);
                                boldRace(orcs, orcButton, 1);
                            }
                            else
                            {                               
                                orcButton.GetComponent<SynergyButton>().hideOutline();
                                boldRace(orcs, orcButton, 0);
                            }
                        }
                        break;

                    case Race.Skeleton:
                        if(skeletonButton)
                        {
                            skeletonButton.GetComponentInChildren<TextMeshProUGUI>().text = " Skeleton : " + r.getNumber();
                            skeletonButton.gameObject.SetActive(true);
                            if (r.getNumber() >= 2)
                            {
                                skeletonButton.GetComponent<SynergyButton>().addOutline(colorLvlMax);
                                boldRace(skeletons, skeletonButton, 1);
                            }
                            else
                            {
                                skeletonButton.GetComponent<SynergyButton>().hideOutline();
                                boldRace(skeletons, skeletonButton, 0);
                            }
                        }
                        
                        break;

                    case Race.Octopus:
                        if(octopusButton)
                        {
                            octopusButton.GetComponentInChildren<TextMeshProUGUI>().text = " Octopus : " + r.getNumber();
                            octopusButton.gameObject.SetActive(true);
                            if (r.getNumber() >= 2)
                            {
                                octopusButton.GetComponent<SynergyButton>().addOutline(colorLvlMax);
                                boldRace(octopus, octopusButton, 1);
                            }
                            else
                            {
                                octopusButton.GetComponent<SynergyButton>().hideOutline();
                                boldRace(octopus, octopusButton, 0);
                            }
                        }
                        
                        break;

                    case Race.Elemental:
                        if(elementalButton)
                        {
                            elementalButton.GetComponentInChildren<TextMeshProUGUI>().text = " Elemental : " + r.getNumber();
                            elementalButton.gameObject.SetActive(true);
                            if (r.getNumber() >= 2)
                            {
                                elementalButton.GetComponent<SynergyButton>().addOutline(colorLvlMax);
                                boldRace(elementals, elementalButton, 1);
                            }
                            else
                            {
                                elementalButton.GetComponent<SynergyButton>().hideOutline();
                                boldRace(elementals, elementalButton, 0);
                            }
                        }
                        
                        break;

                    case Race.Giant:
                        if(giantButton)
                        {
                            giantButton.GetComponentInChildren<TextMeshProUGUI>().text = " Giant : " + r.getNumber();
                            giantButton.gameObject.SetActive(true);
                            if (r.getNumber() >= 2)
                            {
                                giantButton.GetComponent<SynergyButton>().addOutline(colorLvlMax);
                                boldRace(giants, giantButton, 1);
                            }
                            else
                            {
                                giantButton.GetComponent<SynergyButton>().hideOutline();
                                boldRace(giants, giantButton, 0);
                            }
                        }
                        break;

                    case Race.Ratman:
                        if (ratmanButton)
                        {
                            ratmanButton.GetComponentInChildren<TextMeshProUGUI>().text = " Ratman : " + r.getNumber();
                            ratmanButton.gameObject.SetActive(true);
                            if (r.getNumber() >= 2)
                            {
                                ratmanButton.GetComponent<SynergyButton>().addOutline(colorLvlMax);
                                boldRace(ratmen, ratmanButton, 1);
                            }
                            else
                            {
                                ratmanButton.GetComponent<SynergyButton>().hideOutline();
                                boldRace(ratmen, ratmanButton, 0);
                            }
                        }
                        break;

                    case Race.Demon:
                        if (demonButton)
                        {
                            demonButton.GetComponentInChildren<TextMeshProUGUI>().text = " Demon : " + r.getNumber();
                            demonButton.gameObject.SetActive(true);
                            if (r.getNumber() >= 2)
                            {
                                demonButton.GetComponent<SynergyButton>().addOutline(colorLvlMax);
                                boldRace(demons, demonButton, 1);
                            }
                            else
                            {
                                demonButton.GetComponent<SynergyButton>().hideOutline();
                                boldRace(demons, demonButton, 0);
                            }
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

                    case Race.Ratman:
                        if (giantButton)
                            ratmanButton.gameObject.SetActive(false);
                        break;

                    case Race.Demon:
                        if (giantButton)
                            demonButton.gameObject.SetActive(false);
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
                            if (c.getNumber() >= 2 && c.getNumber() < 4)
                            {
                                mageButton.GetComponent<SynergyButton>().addOutline(colorLvlBase);
                                boldClass(mages, mageButton, 1);
                            }
                            if(c.getNumber() >= 4)
                            {
                                mageButton.GetComponent<SynergyButton>().addOutline(colorLvlMax);
                                boldClass(mages, mageButton, 2);
                            }
                            if (c.getNumber() < 2)
                            {
                                mageButton.GetComponent<SynergyButton>().hideOutline();
                                boldClass(mages, mageButton, 0);
                            }
                        }
                       
                        break;

                    case Class.Warrior:
                        if(warriorButton)
                        {
                            warriorButton.GetComponentInChildren<TextMeshProUGUI>().text = " Warrior : " + c.getNumber();
                            warriorButton.gameObject.SetActive(true);
                            if (c.getNumber() >= 2 && c.getNumber() < 4)
                            {
                                warriorButton.GetComponent<SynergyButton>().addOutline(colorLvlBase);
                                boldClass(warriors, warriorButton, 1);
                            }
                            if(c.getNumber() >= 4)
                            {
                                warriorButton.GetComponent<SynergyButton>().addOutline(colorLvlMax);
                                boldClass(warriors, warriorButton, 2);
                            }
                            if (c.getNumber() < 2)
                            {
                                warriorButton.GetComponent<SynergyButton>().hideOutline();
                                boldClass(warriors, warriorButton, 0);
                            }
                        }
                        
                        break;

                    case Class.Tank:
                        if(tankButton)
                        {
                            tankButton.GetComponentInChildren<TextMeshProUGUI>().text = " Tank : " + c.getNumber();
                            tankButton.gameObject.SetActive(true);
                            if (c.getNumber() >= 2)
                            {
                                tankButton.GetComponent<SynergyButton>().addOutline(colorLvlMax);
                                boldClass(tanks, tankButton, 1);
                            }
                            else
                            {
                                tankButton.GetComponent<SynergyButton>().hideOutline();
                                boldClass(tanks, tankButton, 0);
                            }
                        }
                       
                        break;

                    case Class.Bowman:
                        if(bowmanButton)
                        {
                            bowmanButton.GetComponentInChildren<TextMeshProUGUI>().text = " Bowman : " + c.getNumber();
                            bowmanButton.gameObject.SetActive(true);
                            if (c.getNumber() >= 2)
                            {
                                bowmanButton.GetComponent<SynergyButton>().addOutline(colorLvlMax);
                                boldClass(bowmans, bowmanButton, 1);
                            }
                            else
                            {
                                bowmanButton.GetComponent<SynergyButton>().hideOutline();
                                boldClass(bowmans, bowmanButton, 0);
                            }
                        }
                        
                        break;

                    case Class.Healer:
                        if(healerButton)
                        {
                            healerButton.GetComponentInChildren<TextMeshProUGUI>().text = " Healer : " + c.getNumber();
                            healerButton.gameObject.SetActive(true);

                            if (c.getNumber() == 1)
                            {
                                healerButton.GetComponent<SynergyButton>().addOutline(colorLvlBase);
                                boldClass(healers, healerButton, 1);
                            }
                            if (c.getNumber() == 2)
                            {
                                healerButton.GetComponent<SynergyButton>().addOutline(colorLvlInt);
                                boldClass(healers, healerButton, 2);
                            }
                            if (c.getNumber() >= 3)
                            {
                                healerButton.GetComponent<SynergyButton>().addOutline(colorLvlMax);
                                boldClass(healers, healerButton, 3);
                            }
                            if (c.getNumber() < 1)
                            {
                                healerButton.GetComponent<SynergyButton>().hideOutline();
                                boldClass(healers, healerButton, 0);
                            }
                        }                       
                        break;

                    case Class.Support:
                        if(supportButton)
                        {
                            supportButton.GetComponentInChildren<TextMeshProUGUI>().text = " Support : " + c.getNumber();
                            supportButton.gameObject.SetActive(true);

                            if (c.getNumber() == 2)
                            {
                                supportButton.GetComponent<SynergyButton>().addOutline(colorLvlBase);
                                boldClass(supports, supportButton, 1);
                            }
                            if (c.getNumber() == 3)
                            {
                                supportButton.GetComponent<SynergyButton>().addOutline(colorLvlInt);
                                boldClass(supports, supportButton, 2);
                            }
                            if (c.getNumber() >= 4)
                            {
                                supportButton.GetComponent<SynergyButton>().addOutline(colorLvlMax);
                                boldClass(supports, supportButton, 3);
                            }
                            if (c.getNumber() < 2)
                            {
                                supportButton.GetComponent<SynergyButton>().hideOutline();
                                boldClass(supports, supportButton, 0);
                            }
                        }                        
                        break;

                    case Class.Berserker:
                        if(berserkerButton)
                        {
                            berserkerButton.GetComponentInChildren<TextMeshProUGUI>().text = " Berserker : " + c.getNumber();
                            berserkerButton.gameObject.SetActive(true);

                            if (c.getNumber() >= 2)
                            {
                                berserkerButton.GetComponent<SynergyButton>().addOutline(colorLvlMax);
                                boldClass(berserkers, berserkerButton, 1);
                            }
                            else
                            {
                                berserkerButton.GetComponent<SynergyButton>().hideOutline();
                                boldClass(berserkers, berserkerButton, 0);
                            }
                        }                        
                        break;

                    case Class.Assassin:
                        if(assassinButton)
                        {
                            assassinButton.GetComponentInChildren<TextMeshProUGUI>().text = " Assassin : " + c.getNumber();
                            assassinButton.gameObject.SetActive(true);

                            if (c.getNumber() >= 1)
                            {
                                assassinButton.GetComponent<SynergyButton>().addOutline(colorLvlMax);
                                boldClass(assassins, assassinButton, 1);
                            }
                            else
                            {
                                assassinButton.GetComponent<SynergyButton>().hideOutline();
                                boldClass(assassins, assassinButton, 0);
                            }
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

    public void HideSynergy()
    {
        orcButton.gameObject.SetActive(false);

        skeletonButton.gameObject.SetActive(false);

        octopusButton.gameObject.SetActive(false);

        elementalButton.gameObject.SetActive(false);

        giantButton.gameObject.SetActive(false);

        ratmanButton.gameObject.SetActive(false);

        demonButton.gameObject.SetActive(false);

        mageButton.gameObject.SetActive(false);

        warriorButton.gameObject.SetActive(false);

        tankButton.gameObject.SetActive(false);

        bowmanButton.gameObject.SetActive(false);

        healerButton.gameObject.SetActive(false);

        supportButton.gameObject.SetActive(false);

        berserkerButton.gameObject.SetActive(false);

        assassinButton.gameObject.SetActive(false);
    }

    private void boldRace(RaceCount raceC, Button b, int lvl)
    {
        raceC.initDefinition(lvl);
        b.GetComponent<SynergyButton>().setTooltipDef(raceC.getString());
    }

    private void boldClass(ClassCount classC, Button b, int lvl)
    {
        classC.initDefinition(lvl);
        b.GetComponent<SynergyButton>().setTooltipDef(classC.getString());
    }
}

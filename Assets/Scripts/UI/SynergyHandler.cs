using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SynergyHandler : MonoBehaviour
{

    [SerializeField]
    private TMP_Text synergyText;
    public static SynergyHandler instance;

    private List<RaceCount> rc = new List<RaceCount>();
    private List<ClassCount> cc = new List<ClassCount>();

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

        synergyText = GetComponent<TextMeshProUGUI>();
    }

    // Start is called before the first frame update
    void Start()
    {
        rc.Add(new RaceCount(Race.Orc, 0, " Orc : "));

        cc.Add(new ClassCount(Class.Mage, 0, " Mage : "));
        cc.Add(new ClassCount(Class.Warrior, 0, " Warrior : "));

        synergyText.gameObject.SetActive(true);
        synergyText.text = "\n";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void addUnit(Unit u)
    {
        if(u.tag == "UnitAlly")
        {
            foreach (RaceCount r in rc)
            {
                if (r.getRace() == u.getRace())
                    r.setNumber(r.getNumber() + 1);
            }

            foreach (ClassCount c in cc)
            {
                if (c.getClass() == u.getClass())
                    c.setNumber(c.getNumber() + 1);
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
        synergyText.text = "";
        foreach (RaceCount r in rc)
        {
            if(r.getNumber() > 0)
                synergyText.text += r.getString() + r.getNumber() + "\n";
        }

        foreach (ClassCount c in cc)
        {
            if(c.getNumber() > 0)
                synergyText.text += c.getString() + c.getNumber() + "\n";
        }
    }
}

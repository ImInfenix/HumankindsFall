using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellHandler : MonoBehaviour
{

    public static SpellHandler instance;

    [Header("Spell buttons")]
    public GameObject orcSpell;
    public GameObject skeletonSpell;
    public GameObject octopusSpell;
    public GameObject elementalSpell;
    public GameObject giantSpell;

    private List<RaceCount> rc = new List<RaceCount>();

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

        orcSpell = Instantiate(orcSpell, GetComponent<RectTransform>());
        orcSpell.SetActive(false);

        skeletonSpell = Instantiate(skeletonSpell, GetComponent<RectTransform>());
        skeletonSpell.SetActive(false);

        octopusSpell = Instantiate(octopusSpell, GetComponent<RectTransform>());
        octopusSpell.SetActive(false);

        elementalSpell = Instantiate(elementalSpell, GetComponent<RectTransform>());
        elementalSpell.SetActive(false);

        giantSpell = Instantiate(giantSpell, GetComponent<RectTransform>());
        giantSpell.SetActive(false);
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Activate spells
    public void ActivateRaceSynergy()
    {
        rc = SynergyHandler.instance.getRaceList();
        foreach(RaceCount race in rc)
        {
            if(race.getNumber()>=2)
            {
                switch (race.getRace())
                {
                    case (Race.Orc):
                        orcSpell.SetActive(true);
                        break;
                    case (Race.Skeleton):
                        skeletonSpell.SetActive(true);
                        break;
                    case (Race.Octopus):

                        break;
                    case (Race.Elemental):

                        break;
                    case (Race.Giant):

                        break;
                }
            }
            
        }
        
    }
}

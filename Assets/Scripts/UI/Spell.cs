using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Spell : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    private string definition;
    [Header ("Select Spell Race")]
    public Race race;

    // Start is called before the first frame update
    void Start()
    {
        switch(race)
        {
            case (Race.Orc):
                definition = "For 5 seconds, orc in spell area ignore enemy defense but they lose 10% accuracy";
                break;
            case (Race.Skeleton):
                definition = "Ennemis in spell area loose 10% moovespeed for 5 seconds";
                break;
            case (Race.Octopus):
                definition = "Stun the target for 5 seconds";
                break;
            case (Race.Elemental):
                definition = "Increase the target's attack by number of elemental on board";
                break;
            case (Race.Giant):
                definition = "Choose a giant unit, his next attack will deal 15% more damage and stun the enemy for 2 seconds";
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Tooltip.ShowTooltip_Static(definition);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Tooltip.HideTooltip_Static();
    }
}

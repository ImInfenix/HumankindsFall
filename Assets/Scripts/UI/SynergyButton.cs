using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SynergyButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    private string definition;
    public Outline outline;

    // Start is called before the first frame update
    void Start()
    {
        //outline = GetComponent<Outline>();
        outline.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setTooltipDef(string def)
    {
        definition = def;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Tooltip.ShowTooltip_Static(definition);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Tooltip.HideTooltip_Static();
    }

    public void addOutline(Color col)
    {
        outline.effectColor = col;
        outline.enabled = true;       
    }

    public void hideOutline()
    {
        if(outline.enabled == true)
            outline.enabled = false;
    }
}

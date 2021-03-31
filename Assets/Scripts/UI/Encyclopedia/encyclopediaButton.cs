using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class encyclopediaButton : MonoBehaviour, IPointerClickHandler
{

    public GameObject encyclopedia;
    private bool active;

    // Start is called before the first frame update
    void Start()
    {
        encyclopedia.SetActive(false);
        active = false;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(active == false)
        {
            encyclopedia.SetActive(true);
            active = true;
        }
        else
        {
            encyclopedia.SetActive(false);
            active = false;
        }
    }

}

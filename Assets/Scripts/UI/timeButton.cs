using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public enum buttonType
{
    PlayPause,
    Speed
}
public class timeButton : MonoBehaviour, IPointerClickHandler
{
    public buttonType type;
    public Sprite play;
    public Sprite pause;
   

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if(type == buttonType.PlayPause)
        {
            if (TimeButtonHandler.instance.time == 1 || TimeButtonHandler.instance.time == 2)
            {
                GetComponent<Image>().sprite = pause;
            }
            if (TimeButtonHandler.instance.time == 0)
            {
                GetComponent<Image>().sprite = play;
            }
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        switch(type)
        {
            case (buttonType.PlayPause):
                TimeButtonHandler.instance.PlayPause();
                break;

            case (buttonType.Speed):
                switch(TimeButtonHandler.instance.time)
                {
                    case (1):
                        Time.timeScale = 2;
                        TimeButtonHandler.instance.time = 2;
                        break;
                    case (2):
                        Time.timeScale = 1;
                        TimeButtonHandler.instance.time = 1;
                        break;
                    case (0):
                        Time.timeScale = 1;
                        TimeButtonHandler.instance.time = 1;
                        break;
                }
                break;
        }
        
    }    

}

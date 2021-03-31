using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TabGroup : MonoBehaviour
{
    public List<TabButton> tabButtonList;

    public Sprite tabIdle;
    public Sprite tabActive;

    public TabButton selectedTab;
    public List<GameObject> objectsToSwap;

    public void Subscribe(TabButton button)
    {
        if (tabButtonList == null)
        {
            tabButtonList = new List<TabButton>();
        }
        tabButtonList.Add(button);
    }


    public void OnTabSeleceted(TabButton button)
    {
        selectedTab = button;
        ResetTabs();
        button.background.sprite = tabActive;
        int index = button.transform.GetSiblingIndex();
        for(int i=0; i<objectsToSwap.Count; i++)
        {
            if(i == index)
            {
                objectsToSwap[i].SetActive(true);
            }
            else
            {
                objectsToSwap[i].SetActive(false);
            }
        }
    }

    private void ResetTabs()
    {
        foreach(TabButton button in tabButtonList)
        {
            button.background.sprite = tabIdle;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusHandler : MonoBehaviour
{
    public GameObject poison;
    public GameObject breakShield;
    public GameObject OrcUp;
    public GameObject shieldUp;
    public GameObject encouraged;

    private void Awake()
    {
        poison = Instantiate(poison, GetComponent<RectTransform>());
        poison.SetActive(false);

        breakShield = Instantiate(breakShield, GetComponent<RectTransform>());
        breakShield.SetActive(false);

        OrcUp = Instantiate(OrcUp, GetComponent<RectTransform>());
        OrcUp.SetActive(false);

        shieldUp = Instantiate(shieldUp, GetComponent<RectTransform>());
        shieldUp.SetActive(false);

        encouraged = Instantiate(encouraged, GetComponent<RectTransform>());
        encouraged.SetActive(false);
    }

    public void setPoison(bool b)
    {
        poison.SetActive(b);
    }

    public void setBreakShield(bool b)
    {
        breakShield.SetActive(b);
    }

    public void setOrcUp(bool b)
    {
        OrcUp.SetActive(b);
    }

    public void setShieldUp(bool b)
    {
        shieldUp.SetActive(b);
    }

    public void setEncouraged(bool b)
    {
        encouraged.SetActive(b);
    }
}

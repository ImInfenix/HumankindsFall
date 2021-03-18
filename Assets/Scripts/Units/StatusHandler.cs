using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusHandler : MonoBehaviour
{
    public GameObject poison;
    public GameObject breakShield;
    public GameObject OrcUp;

    private void Awake()
    {
        poison = Instantiate(poison, GetComponent<RectTransform>());
        poison.SetActive(false);

        breakShield = Instantiate(breakShield, GetComponent<RectTransform>());
        breakShield.SetActive(false);

        OrcUp = Instantiate(OrcUp, GetComponent<RectTransform>());
        OrcUp.SetActive(false);
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
}

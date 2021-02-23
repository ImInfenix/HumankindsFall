using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UnitDescriptionDisplay : MonoBehaviour
{
    [SerializeField]
    private TMP_Text UnitName;
    [SerializeField]
    private TMP_Text UnitStats;

    public void SetUnitName(string name)
    {
        UnitName.text = name;
    }
}

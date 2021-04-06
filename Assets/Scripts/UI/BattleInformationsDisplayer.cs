using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Text = TMPro.TMP_Text;

[ExecuteAlways]
public class BattleInformationsDisplayer : MonoBehaviour
{
    public Text textHolder;

    private void Awake()
    {
        textHolder = GetComponentInChildren<Text>();
        GetComponent<Canvas>().worldCamera = Camera.main;
    }

    private void Start()
    {
        PlacementTilemapReader infos = FindObjectOfType<PlacementTilemapReader>();
        if (infos == null)
            return;
        textHolder.text = FindObjectOfType<PlacementTilemapReader>().GetData().sceneName;
    }
}

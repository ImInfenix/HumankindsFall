﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ResolutionPhaseHandler : MonoBehaviour
{
    [SerializeField]
    private TMP_Text resolutionText;

    [SerializeField]
    private GameObject resolutionButton;

    public static ResolutionPhaseHandler instance;

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
    }

    // Start is called before the first frame update
    void Start()
    {
        Hide();
    }

    public void ChangeText(string newText)
    {
        resolutionText.text = newText;
    }

    public void ShowExitButton()
    {
        resolutionButton.SetActive(true);
    }

    /// <summary>
    /// Show the text of the resolution phase
    /// </summary>
    public void Show()
    {
        gameObject.SetActive(true);
    }
    
    /// <summary>
    /// Hide the text of the resolution phase
    /// </summary>
    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void RetryCombat()
    {
        GameManager.instance.EnterBattle(Marker.currentBattle);
    }

    public void FleeCombat()
    {
        Marker.currentBattle = null;
        SceneLoader.LoadMapScene();
    }
}

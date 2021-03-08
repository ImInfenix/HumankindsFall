﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Marker : MonoBehaviour
{
    [SerializeField]
    private List<Marker> Neighbours;
    private bool isAccessible;
    private bool isFinished;
    [SerializeField]
    private SpriteRenderer spriteRenderer;
    [SerializeField]
    private Sprite GreenFlag;
    [SerializeField]
    private Sprite RedFlag;
    [SerializeField]
    private GameObject Description;


    public void Awake()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = RedFlag;
    }
    public void FinishLevel()
    {
        spriteRenderer.sprite = GreenFlag;
    }

    public void Update()
    {
        if (Input.GetKeyDown("space"))
            FinishLevel();
    }

    public void OnMouseDown()
    {
        Description.SetActive(true);
    }
}

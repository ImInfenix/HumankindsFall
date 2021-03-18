﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Marker : MonoBehaviour
{
    public static List<string> finishedLevels;

    [SerializeField]
    private List<Marker> Neighbours;
    [SerializeField]
    private bool isAccessible;
    private bool isFinished;
    [SerializeField]
    private SpriteRenderer spriteRenderer;
    [SerializeField]
    private Sprite GreenFlag;
    [SerializeField]
    private Sprite RedFlag;
    [SerializeField]
    private LevelDescription levelDescription;
    [SerializeField]
    private Sprite sprite;
    [SerializeField]
    private TextAsset descriptionTextFile;

    private List<string> eachLine;

    public void Awake()
    {
        if (!isAccessible)
            gameObject.SetActive(false);
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = RedFlag;
        eachLine = new List<string>();
        string AllFile = descriptionTextFile.text;
        eachLine.AddRange(AllFile.Split("\n"[0]));
    }

    private void Start()
    {
        if (finishedLevels != null && finishedLevels.Contains(descriptionTextFile.name))
            FinishLevel();
    }

    public void FinishLevel()
    {
        spriteRenderer.sprite = GreenFlag;
        foreach (Marker marker in Neighbours)
        {
            marker.Unlock();
        }
    }

    public void Unlock()
    {
        if (!isAccessible)
            gameObject.SetActive(true);
    }

    public void OnMouseDown()
    {
        levelDescription.ChangeDescription(eachLine[1]);
        levelDescription.ChangeTitle(eachLine[0]);
        levelDescription.ChangeName(descriptionTextFile.name);
        levelDescription.ChangeImage(sprite);
        levelDescription.Show();
    }

    public static void Add(string level)
    {
        if (finishedLevels == null)
            finishedLevels = new List<string>();

        if (!finishedLevels.Contains(level))
            finishedLevels.Add(level);
    }
}
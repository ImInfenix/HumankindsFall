﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private List<Unit> units;
    private enum GameState { Placement, Combat, Resolution };
    private GameState gamestate;

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
        DontDestroyOnLoad(gameObject);

        //initialization
        units = new List<Unit>();
        gamestate = GameState.Placement;
    }

    /// <summary>
    /// Register all units in a list so we can update them only when we want to
    /// </summary>
    /// <param name="unit"></param>
    public void AddUnit(Unit unit)
    {
        units.Add(unit);
    }

    public void RemoveUnit(Unit unit)
    {
        units.Remove(unit);
    }

    public void ConfirmPlacement()
    {
        gamestate = GameState.Combat;
    }

    public void Update()
    {
        if (gamestate == GameState.Placement)
        {
            foreach (Unit unit in units)
            {
                unit.UpdateDragDrop();
            }
        }
        else if (gamestate == GameState.Combat)
        {
            foreach (Unit unit in units)
            {
                unit.UpdateUnit();
            }
        }
        else if (gamestate == GameState.Resolution)
        {

        }
        
    }
}
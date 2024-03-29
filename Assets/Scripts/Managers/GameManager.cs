﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SceneLoader))]
public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private List<Unit> units;
    public enum GameState { Placement, Combat, Resolution, Shopping, Map, Menu };
    public GameState gamestate { get; private set; }

    public GameState startingGameState = GameState.Placement;

    [SerializeField]
    private GameObject audioManagerPrefab;

    private void Awake()
    {
        //Singleton creation
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;

        DontDestroyOnLoad(gameObject);

        //initialization
        units = new List<Unit>();
        gamestate = startingGameState;

        OptionsMenu.Init();
        GetComponent<SceneLoader>().Initialize();

        Instantiate(audioManagerPrefab).GetComponent<AudioManager>().Initialize();
        GetComponentInChildren<PauseMenu>(true).Initialize();
    }

    /// <summary>
    /// Register all units in a list so we can update them only when we want to
    /// </summary>
    /// <param name="unit"></param>
    public void AddUnit(Unit unit)
    {
        units.Add(unit);
        SynergyHandler.instance.addUnit(unit);
    }

    public void RemoveUnit(Unit unit)
    {
        units.Remove(unit);
        if (unit.CurrentLife > 0)
            SynergyHandler.instance.removeUnit(unit);
    }

    public void ConfirmPlacement()
    {
        if (gamestate != GameState.Placement)
            return;

        Board.CurrentBoard.HidePlacementTilemap();
        gamestate = GameState.Combat;
        ActivateClassSynergy();
        SpellHandler.instance.ActivateRaceSynergy();
        Player.instance.Inventory.Hide();
        TimeButtonHandler.instance.ShowTimeButton();
        HealthbarHandler.ShowBars();
        Player.instance.Inventory.inventoryUI.rewardSystem.RegisterCombatParticipants();
    }

    public void EnterNewLevel()
    {
        Player.instance.InitiateForNewScene();
    }

    public void EnterMap()
    {
        gamestate = GameState.Map;
        SceneLoader.LoadMapScene();
    }

    public void EnterShop()
    {
        gamestate = GameState.Shopping;
        SceneLoader.LoadShopScene();
    }

    public void EnterBattle(string battleName)
    {
        gamestate = GameState.Placement;
        SceneLoader.LoadBattle(battleName);
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
            string resolution = units[0].gameObject.tag;
            bool tousEgaux = true;
            foreach (Unit unit in units)
            {
                unit.UpdateUnit();
                tousEgaux = tousEgaux && unit.CompareTag(resolution);

            }
            if (tousEgaux)
            {
                CombatResolution(resolution);
            }

        }
        else if (gamestate == GameState.Resolution)
        {

        }

        if (Input.GetKeyDown(KeyCode.Escape) && gamestate != GameState.Menu)
            PauseMenu.ChangeState();
    }

    private void CombatResolution(string resolution)
    {
        gamestate = GameState.Resolution;
        if (resolution == "UnitEnemy")
        {
            ResolutionPhaseHandler.instance.ChangeText("DEFAITE");
            ResolutionPhaseHandler.instance.Show();
        }
        if (resolution == "UnitAlly")
        {
            ResolutionPhaseHandler.instance.ChangeText("");
            Player.instance.Inventory.Hide();
            Player.instance.Inventory.inventoryUI.rewardSystem.StartRewardPhase();
        }

        SpellHandler.instance.HideSpells();
        SpellHandler.instance.gameObject.SetActive(false);
        Tooltip.HideTooltip_Static();
        HealthbarHandler.HideAll();
        TimeButtonHandler.instance.HideTimeButton();
        Player.instance.Inventory.Hide();
    }

    private void OnDestroy()
    {
        if (instance == this)
            instance = null;
    }

    private void ActivateClassSynergy()
    {
        List<ClassCount> cc = SynergyHandler.instance.getClassList();

        foreach (Unit unit in units)
        {
            if (unit.CompareTag("UnitAlly"))
                unit.ActivateClass(unit.getClass(), cc.Find(x => x.getClass() == unit.getClass()).getNumber());
        }
    }

    public Unit searchHealTarget()
    {
        Unit target = null;
        foreach (Unit unit in units)
        {
            if (unit.CompareTag("UnitAlly"))
            {
                target = unit;
                if (unit.CurrentLife <= target.CurrentLife && unit.CurrentLife > 0)
                    target = unit;
            }
        }
        return target;
    }

    public List<Unit> getUnit()
    {
        return units;
    }
}

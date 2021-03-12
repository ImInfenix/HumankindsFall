using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SceneLoader))]
public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private List<Unit> units;
    public enum GameState { Placement, Combat, Resolution };
    public GameState gamestate { get; private set; }

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
        gamestate = GameState.Placement;

        GetComponent<SceneLoader>().Initialize();
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

        if (unit.getCurrentLife() > 0)
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
        HealthbarHandler.ShowAll();
        Player.instance.Inventory.inventoryUI.rewardSystem.RegisterCombatParticipants();
    }

    public void EnterNewCombatLevel()
    {
        gamestate = GameState.Placement;
        Player.instance.InitiateForNewScene();
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
            Player.instance.Inventory.Hide();
            Player.instance.Inventory.inventoryUI.rewardSystem.StartRewardPhase();
        }

        HealthbarHandler.HideAll();
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
            if(unit.CompareTag("UnitAlly"))
                unit.ActivateClass(unit.getClass(), cc.Find(x => x.getClass() == unit.getClass()).getNumber());
        }
    }

   public Unit searchHealTarget()
    {
        Unit target = units[0];
        foreach(Unit unit in units)
        {
            if (unit.CompareTag("UnitAlly"))
            {
                if (unit.getCurrentLife() <= target.getCurrentLife() && unit.getCurrentLife() > 0)
                    target = unit;
            }
        }
        return target;
    }
}

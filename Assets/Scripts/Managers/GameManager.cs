using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private List<Unit> units;
    public enum GameState { Placement, Combat, Resolution };
    public GameState gamestate { get; private set; }

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
        Player.instance.Inventory.Hide();
        HealthbarHandler.ShowAll();
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
        }
        if (resolution == "UnitAlly")
        {
            ResolutionPhaseHandler.instance.ChangeText("VICTOIRE");
            Player.instance.Wallet.Earn(10); // a terme, remplacer le 10 par niveau.getRecompense()
            Player.instance.Inventory.Hide();
        }

        ResolutionPhaseHandler.instance.Show();
        HealthbarHandler.HideAll();
    }
}

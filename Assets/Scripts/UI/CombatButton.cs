using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatButton : MonoBehaviour
{
    public void RunCombat()
    {
        if (GameManager.instance.gamestate == GameManager.GameState.Placement)
        {
            GameManager.instance.ConfirmPlacement();
            gameObject.SetActive(false);
        }
    }

    public void ExitCombat()
    {
        if (GameManager.instance.gamestate == GameManager.GameState.Resolution)
        {
            Marker.Add(Marker.currentBattle);
            Marker.currentBattle = null;
            GameManager.instance.EnterMap();
        }
    }
}

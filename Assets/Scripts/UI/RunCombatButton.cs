using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunCombatButton : MonoBehaviour
{
    public void RunCombat()
    {
        if (GameManager.instance.gamestate == GameManager.GameState.Placement)
        {
            GameManager.instance.ConfirmPlacement();
            gameObject.SetActive(false);
        }
    }
}

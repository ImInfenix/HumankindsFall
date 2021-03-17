using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuNavigation : MonoBehaviour
{
    public GameObject continueButton;

    public void NewGame()
    {
        CheckForPlayerExistence();
        GameManager.instance.EnterMap();
    }

    public void ContinueGame()
    {
        CheckForPlayerExistence();
        SavingSystem.RetrieveDataFromDisk();
        GameManager.instance.EnterMap();
    }

    public void Leave()
    {
        Application.Quit();
        Debug.Log("Leaving application");
    }

    private void OnEnable()
    {
        if (!SavingSystem.GameSaveExists())
            continueButton.SetActive(false);
    }

    public void CheckForPlayerExistence()
    {
        if (Player.instance != null)
            Destroy(Player.instance.gameObject);
    }
}

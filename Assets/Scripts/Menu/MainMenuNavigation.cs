using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuNavigation : MonoBehaviour
{
    public GameObject continueButton;
    public GameObject newGameConfirm;
    public GameObject creditsContent;

    public void NewGame()
    {
        if(!SavingSystem.GameSaveExists())
        {
            NewGameConfirm();
            return;
        }

        newGameConfirm.SetActive(true);
    }

    public void CloseConfirm()
    {
        newGameConfirm.gameObject.SetActive(false);
    }

    public void NewGameConfirm()
    {
        CheckForPlayerExistence();
        SavingSystem.DeleteData();
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
        newGameConfirm.gameObject.SetActive(false);
    }

    public void CheckForPlayerExistence()
    {
        if (Player.instance != null)
            Destroy(Player.instance.gameObject);
    }

    public void ShowCredits()
    {
        creditsContent.SetActive(true);
    }

    public void HideCredits()
    {
        creditsContent.SetActive(false);
    }
}

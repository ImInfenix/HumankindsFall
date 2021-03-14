using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuNavigation : MonoBehaviour
{
    public void NewGame()
    {
        SceneLoader.LoadNextScene();
    }

    public void ContinueGame()
    {
        SavingSystem.RetrieveDataFromDisk();
        SceneLoader.LoadNextScene();
    }

    public void Leave()
    {
        Application.Quit();
        Debug.Log("Leaving application");
    }
}

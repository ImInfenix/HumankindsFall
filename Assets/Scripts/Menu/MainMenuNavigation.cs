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
        Debug.Log("Loading saved game");
    }

    public void Leave()
    {
        Application.Quit();
        Debug.Log("Leaving application");
    }
}

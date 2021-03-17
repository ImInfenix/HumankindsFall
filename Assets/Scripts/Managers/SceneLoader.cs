using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(GameManager))]
public class SceneLoader : MonoBehaviour
{


    private static SceneLoader instance;

    public void Initialize()
    {
        if (instance != null)
            Debug.LogError("An instance of scene loader already exists");

        SceneManager.sceneLoaded += OnSceneLoaded;
        instance = this;
    }

    private void OnSceneLoaded(Scene loadedScene, LoadSceneMode loadMode)
    {
        if (GameManager.instance.gamestate == GameManager.GameState.Placement)
            GameManager.instance.EnterNewCombatLevel();
    }

    public static void LoadMapScene()
    {
        SceneManager.LoadScene("Map", LoadSceneMode.Single);
    }
    
    public static void LoadNextScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public static void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void OnDestroy()
    {
        if (instance == this)
            instance = null;
    }
}

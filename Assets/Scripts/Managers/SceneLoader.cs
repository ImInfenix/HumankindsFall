using System;
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
        if (GameManager.instance.gamestate == GameManager.GameState.Placement || GameManager.instance.gamestate == GameManager.GameState.Shopping)
            GameManager.instance.EnterNewLevel();

        if(loadMode == LoadSceneMode.Single)
        {
            PickMusic(loadedScene.name);
        }
    }

    private void PickMusic(string newSceneName)
    {
        if (newSceneName == "Shop")
            AudioManager.PlayShopMusic();
        else if (newSceneName != "MainMenu" && newSceneName != "Map")
        {
            if (newSceneName == "Vertmont")
                AudioManager.PlayFinalBattleMusic();
            else
                AudioManager.PlayBattleMusic();
        }
        else
            AudioManager.PlayMainMusic();
    }

    public static void LoadMenu()
    {
        Destroy(GameManager.instance.gameObject);
        Destroy(Player.instance.gameObject);
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
    }

    public static void LoadMapScene()
    {
        SceneManager.LoadScene("Map", LoadSceneMode.Single);
    }

    public static void LoadShopScene()
    {
        SceneManager.LoadScene("Shop", LoadSceneMode.Single);
    }

    public static void LoadBattle(string battleName)
    {
        SceneManager.LoadScene(battleName, LoadSceneMode.Single);
    }

    public static void LoadNextScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    private void OnDestroy()
    {
        if (instance == this)
            instance = null;
    }
}

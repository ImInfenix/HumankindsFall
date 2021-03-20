using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static PauseMenu instance;
    public static bool isGamePaused { get; private set; }

    [SerializeField]
    private GameObject menuContent;
    [SerializeField]
    private OptionsMenu optionsMenu;

    public void Initialize()
    {
        SceneManager.sceneLoaded += OnNewSceneLoaded;

        instance = this;
    }

    private void OnNewSceneLoaded(Scene newScene, LoadSceneMode mode)
    {
        if (mode != LoadSceneMode.Single)
            return;

        HideMenu();
    }

    private void OnEnable()
    {
        GetComponent<Canvas>().worldCamera = Camera.main;       
    }

    public static void ChangeState()
    {
        bool newState = !instance.gameObject.activeSelf;

        if (newState)
            instance.ShowMenu();
        else
            instance.HideMenu();
    }

    public void ShowMenu()
    {
        instance.gameObject.SetActive(true);
        optionsMenu.Hide();
        isGamePaused = true;
        Time.timeScale = 0;
    }

    public void HideMenu()
    {
        instance.gameObject.SetActive(false);
        isGamePaused = false;
        Time.timeScale = 1;
    }

    public void ShowMainContent()
    {
        menuContent.gameObject.SetActive(true);
    }

    public void HideMainContent()
    {
        menuContent.gameObject.SetActive(false);
    }

    public void OptionsButton()
    {
        HideMainContent();
        optionsMenu.Show();
    }

    public void MenuButton()
    {
        HideMenu();
        SceneLoader.LoadMenu();
    }

    private void OnDestroy()
    {
        if (instance == this) instance = null;
        SceneManager.sceneLoaded -= OnNewSceneLoaded;
    }
}

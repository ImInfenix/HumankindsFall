using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static PauseMenu instance;

    public void Initialize()
    {
        SceneManager.sceneLoaded += OnNewSceneLoaded;

        instance = this;
    }

    private void OnNewSceneLoaded(Scene newScene, LoadSceneMode mode)
    {
        if (mode != LoadSceneMode.Single)
            return;

        gameObject?.SetActive(false);
    }

    private void OnEnable()
    {
        GetComponent<Canvas>().worldCamera = Camera.main;       
    }

    public static void ChangeState()
    {
        instance.gameObject.SetActive(!instance.gameObject.activeSelf);
    }

    private void OnDestroy()
    {
        if (instance == this) instance = null;
        SceneManager.sceneLoaded -= OnNewSceneLoaded;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
    [SerializeField]
    private PauseMenu pauseMenu;

    [SerializeField]
    private Slider musicSlider;

    [SerializeField]
    private Slider effectsSlider;

    private const string musicKey = "MusicVolume";
    private const string effectsKey = "EffectsVolume";

    public static float musicVolume = 1;
    public static float effectsVolume = 1;

    public static void Init()
    {
        if (PlayerPrefs.HasKey(musicKey))
            musicVolume = PlayerPrefs.GetFloat(musicKey);
        else musicVolume = 1;

        if (PlayerPrefs.HasKey(effectsKey))
            effectsVolume = PlayerPrefs.GetFloat(effectsKey);
        else effectsVolume = 1;
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
        pauseMenu?.ShowMainContent();
    }

    private void OnEnable()
    {
        musicSlider.value = musicVolume;
        effectsSlider.value = effectsVolume;
    }

    public void OnMusicVolumeChanged(float newValue)
    {
        musicVolume = newValue;
        AudioManager.SetMusicVolume(newValue);
    }

    public void OnEffectsVolumeChanged(float newValue)
    {
        effectsVolume = newValue;
    }

    private void OnDestroy()
    {
        PlayerPrefs.SetFloat(musicKey, musicVolume);
        PlayerPrefs.SetFloat(effectsKey, effectsVolume);
    }
}

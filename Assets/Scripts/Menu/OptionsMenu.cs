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

    public static float musicVolume = 1f;
    public static float effectsVolume = 1f;

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
}

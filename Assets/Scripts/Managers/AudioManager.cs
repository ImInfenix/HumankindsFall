using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private static AudioManager instance;

    [SerializeField]
    private AudioClip mainMusic;
    [SerializeField]
    private AudioClip shopMusic;

    private AudioSource audioSource;

    public void Initialize()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }    

        instance = this;

        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.loop = true;

        PlayMainMusic();

        DontDestroyOnLoad(gameObject);
    }

    public static void PlayMainMusic()
    {
        instance.PlayMusic(instance.mainMusic);
    }

    public static void PlayShopMusic()
    {
        instance.PlayMusic(instance.shopMusic);
    }

    private void PlayMusic(AudioClip music)
    {
        audioSource.clip = music;
        audioSource.volume = OptionsMenu.musicVolume;
        audioSource.Play();
    }

    //private void PlayEffect(AudioClip effect)
    //{
    //    audioSource.PlayOneShot(effect, OptionsMenu.effectsVolume);
    //}

    private void OnDestroy()
    {
        if (instance == this)
            instance = null;
    }
}

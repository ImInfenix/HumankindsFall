﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private static AudioManager instance;

    [SerializeField]
    private float smoothTransitionDelay = 1.0f;
    [SerializeField]
    private AnimationCurve transitionCurve;

    [SerializeField]
    private AudioClip mainMusic;
    [SerializeField]
    private AudioClip shopMusic;
    [SerializeField]
    private AudioClip battleMusic;
    [SerializeField]
    private AudioClip finalBattleMusic;

    private AudioSource musicAudioSource;
    private AudioSource effectsAudioSource;

    private List<AudioSource> sourcesBeingDeleted;

    public void Initialize()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }    

        instance = this;

        sourcesBeingDeleted = new List<AudioSource>();

        musicAudioSource = CreateNewSource(null);

        effectsAudioSource = CreateNewSource(null);
        musicAudioSource.loop = false;

        PlayMainMusic(true);

        DontDestroyOnLoad(gameObject);
    }

    public static void PlayMainMusic(bool doHardTransition = false)
    {
        instance.PlayMusic(instance.mainMusic, doHardTransition);
    }

    public static void PlayShopMusic(bool doHardTransition = false)
    {
        instance.PlayMusic(instance.shopMusic, doHardTransition);
    }

    public static void PlayBattleMusic(bool doHardTransition = false)
    {
        instance.PlayMusic(instance.battleMusic, doHardTransition);
    }

    public static void PlayFinalBattleMusic(bool doHardTransition = false)
    {
        instance.PlayMusic(instance.finalBattleMusic, doHardTransition);
    }

    private void PlayMusic(AudioClip music, bool doHardTransition = false)
    {
        if (musicAudioSource.clip == music)
            return;

        AudioSource oldSource = musicAudioSource;
        musicAudioSource = CreateNewSource(music);

        musicAudioSource.clip = music;
        musicAudioSource.Play();

        if (doHardTransition)
        {
            Destroy(oldSource);
            musicAudioSource.volume = OptionsMenu.musicVolume;
            return;
        }

        StopAllCoroutines();
        foreach (AudioSource source in sourcesBeingDeleted)
            Destroy(source);
        StartCoroutine(EndMusic(oldSource, smoothTransitionDelay));
        StartCoroutine(StartMusic(musicAudioSource, smoothTransitionDelay));
    }

    private AudioSource CreateNewSource(AudioClip clipToPlay)
    {
        AudioSource audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = clipToPlay;
        audioSource.loop = true;
        return audioSource;
    }

    public static void PlayEffect(AudioClip effect)
    {
        instance.effectsAudioSource.PlayOneShot(effect, OptionsMenu.effectsVolume);
    }

    private void OnDestroy()
    {
        if (instance == this)
            instance = null;
    }

    private IEnumerator EndMusic(AudioSource source, float smoothDelay)
    {
        float elapsedTime = 0;

        sourcesBeingDeleted.Add(source);

        while (elapsedTime < smoothDelay)
        {
            yield return null;
            elapsedTime += Time.deltaTime;
            source.volume = OptionsMenu.musicVolume * transitionCurve.Evaluate(1 - elapsedTime / smoothDelay);
        }

        sourcesBeingDeleted.Remove(source);
        Destroy(source);
    }

    private IEnumerator StartMusic(AudioSource source, float smoothDelay)
    {
        float elapsedTime = 0;

        while (elapsedTime < smoothDelay)
        {
            yield return null;
            elapsedTime += Time.deltaTime;
            source.volume = OptionsMenu.musicVolume * transitionCurve.Evaluate(elapsedTime / smoothDelay);
        }
    }

    public static void SetMusicVolume(float volume)
    {
        instance.musicAudioSource.volume = volume;
    }
}

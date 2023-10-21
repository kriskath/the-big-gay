using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private static AudioManager _instance;
    public static AudioManager Instance { get { return _instance; } }
    
    [Header("Music")]
    [SerializeField] private AudioSource bgmAudioSource;
    [SerializeField] private AudioClip gameMainThemeMusic;
    [SerializeField] private AudioClip gameTravelingMusic;
    [SerializeField] private AudioClip gameBattleMusic;
    
    [Header("SFX")]
    [SerializeField] private AudioSource sfxAudioSource;

    //UI
    [SerializeField] private AudioClip onButtonHovered;
    [SerializeField] private AudioClip onButtonClicked;
    
    //Minigame
    [SerializeField] private AudioClip startGameSFX;
    [SerializeField] private AudioClip rightSFX;
    [SerializeField] private AudioClip wrongSFX;
    [SerializeField] private AudioClip toBeHitSFX;
    [SerializeField] private AudioClip toHitSFX;
    [SerializeField] private AudioClip waitingSFX;


    public void PlayMainMenuTheme()
    {
        bgmAudioSource.Stop();
        bgmAudioSource.clip = gameMainThemeMusic;
        bgmAudioSource.Play();
    }
    public void PlayTravelingTheme()
    {
        bgmAudioSource.Stop();
        bgmAudioSource.clip = gameTravelingMusic;
        bgmAudioSource.Play();
    }
    public void PlayBattleTheme()
    {
        bgmAudioSource.Stop();
        bgmAudioSource.clip = gameBattleMusic;
        bgmAudioSource.Play();
    }

    public void PlayStartGameSFX()
    {
        bgmAudioSource.Stop();
        bgmAudioSource.clip = startGameSFX;
        bgmAudioSource.Play();
    }
    public void PlayRightSFX()
    {
        bgmAudioSource.Stop();
        bgmAudioSource.clip = rightSFX;
        bgmAudioSource.Play();
    }
    public void PlayWrongSFX()
    {
        bgmAudioSource.Stop();
        bgmAudioSource.clip = wrongSFX;
        bgmAudioSource.Play();
    }
    public void PlayToBeHitSFX()
    {
        bgmAudioSource.Stop();
        bgmAudioSource.clip = toBeHitSFX;
        bgmAudioSource.Play();
    }
    public void PlayToHitSFX()
    {
        bgmAudioSource.Stop();
        bgmAudioSource.clip = toHitSFX;
        bgmAudioSource.Play();
    }
    public void PlayWaitingSFX()
    {
        bgmAudioSource.Stop();
        bgmAudioSource.clip = waitingSFX;
        bgmAudioSource.Play();
    }
}

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

    [Header("Voices")]
    [SerializeField] private AudioSource voxAudioSource;
    [SerializeField] private AudioClip characterSpeaking;

    private void Awake()
	{
		if (_instance != null && _instance != this)
		{
			Destroy(gameObject);
		}
		else
		{
			_instance = this;
			
			//Make persistent
			DontDestroyOnLoad(gameObject);
		}
	}
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
        sfxAudioSource.Stop();
        sfxAudioSource.clip = startGameSFX;
        sfxAudioSource.Play();
    }
    public void PlayRightSFX()
    {
        sfxAudioSource.Stop();
        sfxAudioSource.clip = rightSFX;
        sfxAudioSource.Play();
    }
    public void PlayWrongSFX()
    {
        sfxAudioSource.Stop();
        sfxAudioSource.clip = wrongSFX;
        sfxAudioSource.Play();
    }
    public void PlayToBeHitSFX()
    {
        sfxAudioSource.Stop();
        sfxAudioSource.clip = toBeHitSFX;
        sfxAudioSource.Play();
    }
    public void PlayToHitSFX()
    {
        sfxAudioSource.Stop();
        sfxAudioSource.clip = toHitSFX;
        sfxAudioSource.Play();
    }
    public void PlayWaitingSFX()
    {
        sfxAudioSource.Stop();
        sfxAudioSource.clip = waitingSFX;
        sfxAudioSource.Play();
    }
    public void PlayHoverSFX()
    {
        sfxAudioSource.Stop();
        sfxAudioSource.clip = onButtonHovered;
        sfxAudioSource.Play();
    }
        public void PlaySelectSFX()
    {
        sfxAudioSource.Stop();
        sfxAudioSource.clip = onButtonClicked;
        sfxAudioSource.Play();
    }

     public void PlayCharacterSpeaking (float pitch)
    {
        voxAudioSource.pitch = pitch;
        voxAudioSource.Stop();
        voxAudioSource.clip = characterSpeaking;
        voxAudioSource.Play();
    }

    public void StopCharacterSpeaking(){
        voxAudioSource.Stop();
    }
}

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
        if (bgmAudioSource.clip == gameMainThemeMusic)
        {
            return;
        }
        
        bgmAudioSource.Stop();
        bgmAudioSource.clip = gameMainThemeMusic;
        bgmAudioSource.Play();
    }
    public void PlayTravelingTheme()
    {
        if (bgmAudioSource.clip == gameTravelingMusic)
        {
            return;
        }
        
        bgmAudioSource.Stop();
        bgmAudioSource.clip = gameTravelingMusic;
        bgmAudioSource.Play();
    }
    public void PlayBattleTheme()
    {
        if (bgmAudioSource.clip == gameBattleMusic)
        {
            return;
        }
        
        bgmAudioSource.Stop();
        bgmAudioSource.clip = gameBattleMusic;
        bgmAudioSource.Play();
    }

    public void PlayStartGameSFX()
{
    bgmAudioSource.volume -= 10f;

    sfxAudioSource.Stop();
    sfxAudioSource.clip = startGameSFX;
    sfxAudioSource.Play();

    bgmAudioSource.volume += 10f;
}

public void PlayRightSFX()
{
    bgmAudioSource.volume -= 10f;

    sfxAudioSource.Stop();
    sfxAudioSource.clip = rightSFX;
    sfxAudioSource.Play();

    bgmAudioSource.volume += 10f;
}

public void PlayWrongSFX()
{
    bgmAudioSource.volume -= 10f;

    sfxAudioSource.Stop();
    sfxAudioSource.clip = wrongSFX;
    sfxAudioSource.Play();

    bgmAudioSource.volume += 10f;
}

public void PlayToBeHitSFX()
{
    bgmAudioSource.volume -= 10f;

    sfxAudioSource.Stop();
    sfxAudioSource.clip = toBeHitSFX;
    sfxAudioSource.Play();

    bgmAudioSource.volume += 10f;
}

public void PlayToHitSFX()
{
    bgmAudioSource.volume -= 10f;

    sfxAudioSource.Stop();
    sfxAudioSource.clip = toHitSFX;
    sfxAudioSource.Play();

    bgmAudioSource.volume += 10f;
}

public void PlayWaitingSFX()
{
    bgmAudioSource.volume -= 10f;

    sfxAudioSource.Stop();
    sfxAudioSource.clip = waitingSFX;
    sfxAudioSource.Play();

    bgmAudioSource.volume += 10f;
}

public void PlayHoverSFX()
{
    bgmAudioSource.volume -= 10f;

    sfxAudioSource.Stop();
    sfxAudioSource.clip = onButtonHovered;
    sfxAudioSource.Play();

    bgmAudioSource.volume += 10f;
}

public void PlaySelectSFX()
{
    bgmAudioSource.volume -= 10f;

    sfxAudioSource.Stop();
    sfxAudioSource.clip = onButtonClicked;
    sfxAudioSource.Play();

    bgmAudioSource.volume += 10f;
}

public void PlayCharacterSpeaking(float pitch)
{
    voxAudioSource.pitch = pitch;
    voxAudioSource.volume -= 10f;

    voxAudioSource.Stop();
    voxAudioSource.clip = characterSpeaking;
    voxAudioSource.Play();

    bgmAudioSource.volume += 10f;
}


    public void StopCharacterSpeaking(){
        voxAudioSource.Stop();
    }

    public void StopBGMusic(){
        bgmAudioSource.Stop();
    }

}

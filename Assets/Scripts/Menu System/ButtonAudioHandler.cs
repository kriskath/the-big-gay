using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonAudioHandler : MonoBehaviour
{
    
    public void PlayButtonClickedSound()
    {
        AudioManager.Instance.PlaySelectSFX();
    }

    public void PlayButtonHoveredSound()
    {
        AudioManager.Instance.PlayHoverSFX();
    }

    public void PlayButtonStartGameSound()
    {
        AudioManager.Instance.PlayStartGameSFX();
    }
}

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

namespace MenuAsset
{
    /*
     * Code by Kristopher Kath
     */


    public class SettingsMenu : MonoBehaviour
    {
        [Tooltip("The first button selected to hover when navigating to Main Menu.")]
        [SerializeField] private GameObject menuFirstButton;
        [Tooltip("The Audio Mixer to modify volume from.")]
        [SerializeField] private AudioMixer audioMixer;

        [Header("Dropdowns | Choose only one.")]
        [Tooltip("Resolution Dropdown from TextMeshPro.")]
        [SerializeField] private TMPro.TMP_Dropdown TMProResolutionDropdown;
        [Tooltip("Default Resolution Dropdown.")]
        [SerializeField] private Dropdown defaultResolutionDropdown;

        [Header("Pause Menu")] 
        [SerializeField]
        private GameObject PauseMenuGroup;
        
        private Resolution[] resolutions;
        private int currentResolutionIndex;
        
        private void Start()
        {
            resolutions = Screen.resolutions;

            //Handles resolutions for default dropdown
            if (defaultResolutionDropdown != null)
            {
                defaultResolutionDropdown.ClearOptions();

                defaultResolutionDropdown.AddOptions(ResolutionsFunction());
                defaultResolutionDropdown.value = currentResolutionIndex;
                defaultResolutionDropdown.RefreshShownValue();
            }
            //Handles resolutions for TextMeshPro dropdown
            else if (TMProResolutionDropdown != null)
            {
                TMProResolutionDropdown.ClearOptions();

                TMProResolutionDropdown.AddOptions(ResolutionsFunction());
                TMProResolutionDropdown.value = currentResolutionIndex;
                TMProResolutionDropdown.RefreshShownValue();
            }
        }

        private void Update()
        {
            if (PauseMenuGroup && Input.GetKeyDown(KeyCode.Escape))
            {
                SetMenuActiveState();
            }
        }

        public void SetMenuActiveState()
        {
            if (GameManager.Instance.GetMainMenuSceneBuildIndex == SceneManager.GetActiveScene().buildIndex)
                return;
            PauseMenuGroup.SetActive(!PauseMenuGroup.activeSelf);
        }

        //Gets list of resolutions as a string and returns
        //Also sets default resolution
        private List<string> ResolutionsFunction()
        {
            //fill options list with strings of resolutions
            List<string> options = new List<string>();
            for (int i = 0; i < resolutions.Length; i++)
            {
                string option = resolutions[i].width + " x " + resolutions[i].height;
                options.Add(option);

                //finds current running resolution to set as default
                if (resolutions[i].width == Screen.currentResolution.width &&
                    resolutions[i].height == Screen.currentResolution.height)
                {
                    currentResolutionIndex = i;
                }
            }
            return options;
        }

        public void QuitGame()
        {
            Debug.Log("Quit Game");
            Application.Quit();
        }
        
        public void SetResolution(int resolutionIndex)
        {
            Resolution res = resolutions[resolutionIndex];
            Screen.SetResolution(res.width, res.height, Screen.fullScreen);
        }

        public void SetMainVolume(float volume)
        {
            if (!audioMixer)
            {
                Debug.LogError("No assigned audio mixer, unable to set volume.");
                return;
            }

            audioMixer.SetFloat("MasterVolume", (volume == 0f ? -80f : Mathf.Log10(volume)) * 20); //represents slider value to log base 10 and mult by factor of 20
        }

        public void SetSfxVolume(float volume)
        {
            if (!audioMixer)
            {
                Debug.LogError("No assigned audio mixer, unable to set volume.");
                return;
            }
            
            audioMixer.SetFloat("SFXVolume", (volume == 0f ? -80f : Mathf.Log10(volume)) * 20); //represents slider value to log base 10 and mult by factor of 20
        }
        
        public void SetMusicVolume(float volume)
        {
            if (!audioMixer)
            {
                Debug.LogError("No assigned audio mixer, unable to set volume.");
                return;
            }

            audioMixer.SetFloat("MusicVolume", (volume == 0f ? -80f : Mathf.Log10(volume)) * 20); //represents slider value to log base 10 and mult by factor of 20
        }

        public void SetQuality(int qualityIndex)
        {
            QualitySettings.SetQualityLevel(qualityIndex);
        }

        public void SetFullscreen(bool isFullscreen)
        {
            Screen.fullScreen = isFullscreen;
        }

        public void ExitOptions()
        {
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(menuFirstButton);
        }
    }
}
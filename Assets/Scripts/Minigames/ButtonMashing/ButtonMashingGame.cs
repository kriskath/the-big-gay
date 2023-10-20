using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using System; 
using System.Collections;
public class ButtonMashingGame : MonoBehaviour, IMiniGame
{

    // Meter stuff
    public float fillRate = 0.001f; // Rate at which the meter fills per button press.
    public float maxMeterValue = 1.0f; // Maximum value for the meter.
    private float currentMeterValue = 0.0f;
    public Slider meterSlider;

    // Timer Stuff
    public float gameDuration = 5.0f;
    private float timeLeft;

    // States
    private bool isGameActive = false;
    public event Action MiniGameCompleted; 
    public event Action MiniGameFailed;   
 
    public void StartMiniGame()
    {
        
        meterSlider = FindObjectOfType<Slider>();
        if (meterSlider != null)
        {
            meterSlider.gameObject.SetActive(true);
            meterSlider.value = 0;
        }
        isGameActive = true;
        currentMeterValue = 0.0f;
        timeLeft = gameDuration;
        StartCoroutine(Countdown());
    }


     private IEnumerator Countdown()
    {
        while (timeLeft > 0)
        {
            timeLeft -= Time.deltaTime;
            // Update UI here

            yield return null;
        }

        // When the timer reaches zero, stop the game.
        if (isGameActive) FailMiniGame();
        
    }

    private void DeactivateGame()
    {
        isGameActive = false;
        StopCoroutine(Countdown());
        if (meterSlider != null)
        {
            meterSlider.gameObject.SetActive(false);
        }
    }

    public void StopMiniGame()
    {
        DeactivateGame();
        MiniGameCompleted?.Invoke();
    }
    public void FailMiniGame()
    {
        DeactivateGame();
        MiniGameFailed?.Invoke();
    }

    
    private void Update()
    {
        if (isGameActive)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                //Debug.Log("[Button Masher] space bar pressed ");
                // Increase the meter value with each button press.
                currentMeterValue += fillRate;


                // Update the slider value based on the currentMeterValue.
                if (meterSlider != null)
                {
                    meterSlider.value = currentMeterValue / maxMeterValue;
                }

                // Check if the meter is full.
                if (currentMeterValue >= maxMeterValue)
                {
                    // Mini-game completed.
                    isGameActive = false;
                    Debug.Log("[Button Masher] Game Completed!");
                    StopMiniGame();
                }
            }
        }
    }
}
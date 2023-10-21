using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using System; 
using System.Collections;
using TMPro;

public class ButtonMashingGame : MonoBehaviour, IMiniGame
{

    // Meter stuff
    public float fillRate = 0.1f; // Rate at which the meter fills per button press.
    public float maxMeterValue = 1.0f; // Maximum value for the meter.
    private float currentMeterValue = 0.0f;
    public float smoothTransitionSpeed = 5.0f;
    public Slider meterSlider;
    public GameObject minigameUI;

    // Timer Stuff
    public float gameDuration = 5.0f;
    private float timeLeft;
    public TextMeshProUGUI timerText; 

    // States
    private bool isGameActive = false;
    public event Action MiniGameCompleted; 
    public event Action MiniGameFailed;   
 
    public void StartMiniGame()
    {
        
        if (minigameUI != null)
        {
            minigameUI.gameObject.SetActive(true);
            meterSlider.value = 0;
        }
        isGameActive = true;
        currentMeterValue = 0.0f;
        timeLeft = gameDuration;
        StopCoroutine(Countdown());
        StartCoroutine(Countdown());
    }


     private IEnumerator Countdown()
    {
        while (timeLeft > 0)
        {
            timeLeft -= Time.deltaTime;
            UpdateTimerText();

            yield return null;
        }

        // When the timer reaches zero, stop the game.
        if (isGameActive) FailMiniGame();
        
    }

    private void DeactivateGame()
    {
        StopCoroutine(Countdown());
        //introducing these for a better transition??
        StartCoroutine(Sleep());
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

    
    IEnumerator Sleep()
    {
        yield return new WaitForSeconds(0.3f);

        if (minigameUI != null)
        {
            minigameUI.gameObject.SetActive(false);
        }
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
                StartCoroutine(UpdateSliderSmoothly());

                // Check if the meter is full.
                if (currentMeterValue >= maxMeterValue)
                {
                    // Mini-game completed.
                    isGameActive = false;
                    Debug.Log(currentMeterValue);
                    Debug.Log("[Button Masher] Game Completed!");
                    StopMiniGame();
                }
            }
        }
    }

    private IEnumerator UpdateSliderSmoothly()
    {
        if (meterSlider != null)
        {
            float targetValue = currentMeterValue / maxMeterValue;
            float duration = 0.1f; // Duration over which the slider fills up
            float elapsed = 0f;
            float startValue = meterSlider.value;

            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                meterSlider.value = Mathf.Lerp(startValue, targetValue, elapsed / duration);
                yield return null;
            }
            meterSlider.value = targetValue; // Ensure the slider reaches the target value
        }
    }
    private void UpdateTimerText()
    {
        if (timerText != null && isGameActive)
        {
        // Format the timeLeft to display seconds:miliseconds
        float seconds = timeLeft;
        float milliseconds = (timeLeft % 1) * 1000;
        timerText.text = string.Format("{0:00}.{1:000}", Mathf.FloorToInt(seconds), Mathf.FloorToInt(milliseconds));
        }
    }
}
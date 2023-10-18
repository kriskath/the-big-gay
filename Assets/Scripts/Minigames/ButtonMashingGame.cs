using UnityEngine;
using UnityEngine.Events;
using System; 
public class ButtonMashingGame : MonoBehaviour, IMiniGame
{
    public float fillRate = 0.1f; // Rate at which the meter fills per button press.
    public float maxMeterValue = 1.0f; // Maximum value for the meter.

    private float currentMeterValue = 0.0f;
    private bool isGameActive = false;
    public event Action MiniGameCompleted;

    public void StartMiniGame()
    {
        currentMeterValue = 0.0f;
        isGameActive = true;
    }

    public void StopMiniGame()
    {
        isGameActive = false;
        MiniGameCompleted?.Invoke();
    }

    private void Update()
    {
        if (isGameActive)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Debug.Log("[Button Masher] space bar pressed ");
                // Increase the meter value with each button press.
                currentMeterValue += fillRate;

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
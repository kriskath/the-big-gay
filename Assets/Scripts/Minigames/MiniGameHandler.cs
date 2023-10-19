using UnityEngine;


public class MiniGameHandler : MonoBehaviour
{
    public MiniGameType initialMiniGame;
    private IMiniGame currentMiniGame;
    // Public variables for minigame prefabs.
    public ButtonMashingGame buttonMashingGame;


    private void Start()
    {
        // Can probably change this
        SwitchToMiniGame(initialMiniGame);
    }

    public void SwitchToMiniGame(MiniGameType gameType)
    {
        // Disable the current mini-game (if any).
        if (currentMiniGame != null)
        {
            currentMiniGame.StopMiniGame();
        }

        // Determine which prefab to use based on the MiniGameType.
        switch (gameType)
        {
            case MiniGameType.ButtonMashing:
                currentMiniGame = buttonMashingGame;
                break;
            case MiniGameType.NoGame:
                break;
            default:
                Debug.LogWarning("[MiniGameHandler] Minigame type not recognized.");
                break;
        }

        if (currentMiniGame != null)
        {
            currentMiniGame.MiniGameCompleted += OnMiniGameCompleted;
            currentMiniGame.MiniGameFailed += OnMiniGameFailed;
            currentMiniGame.StartMiniGame();
        }

    }

    private void OnMiniGameCompleted()
    {
        Debug.Log("Completed Game");
        currentMiniGame.MiniGameCompleted -= OnMiniGameCompleted;
    }

    private void OnMiniGameFailed()
    {
        Debug.Log("Game Failed");
        currentMiniGame.MiniGameCompleted -= OnMiniGameCompleted;
        currentMiniGame.MiniGameFailed -= OnMiniGameFailed;
    }

}
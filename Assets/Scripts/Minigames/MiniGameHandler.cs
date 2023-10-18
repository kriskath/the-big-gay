using UnityEngine;


public class MiniGameHandler : MonoBehaviour
{
    public MiniGameType initialMiniGame;
    private IMiniGame currentMiniGame;
    private GameObject miniGamePrefab;

    // Public variables for minigame prefabs.
    public GameObject buttonMashingGamePrefab;


    private void Start()
    {
        // Can probably change this
        SwitchToMiniGame(initialMiniGame);
    }

    public void SwitchToMiniGame(MiniGameType gameType)
    {
        GameObject miniGamePrefab = null;

        // Determine which prefab to use based on the MiniGameType.
        switch (gameType)
        {
            case MiniGameType.ButtonMashing:
                miniGamePrefab = buttonMashingGamePrefab;
                break;
            case MiniGameType.NoGame:
                break;
            default:
                Debug.LogWarning("[MiniGameHandler] Minigame type not recognized.");
                break;
        }
        if (miniGamePrefab != null)
        {
            // Instantiate the selected mini-game prefab.
            GameObject miniGameInstance = Instantiate(miniGamePrefab);

            // Get the IMiniGame script from the instantiated GameObject.
            currentMiniGame = miniGameInstance.GetComponent<IMiniGame>();

            if (currentMiniGame != null)
            {
                // subscribe to their event
                currentMiniGame.MiniGameCompleted += OnMiniGameCompleted;
                currentMiniGame.StartMiniGame();
            }
            else
            {
                Debug.LogWarning("[MiniGameHandler] IMiniGame script not found on the instantiated GameObject.");
            }
        }
        else
        {
            Debug.LogWarning("[MiniGameHandler] Minigame prefab not assigned.");
        }
    }

    private void OnMiniGameCompleted()
    {
        Debug.Log("Completed Game");
        // just need to figure out a better way to destroy
        // if (miniGamePrefab != null) Destroy (miniGamePrefab);
        initialMiniGame = MiniGameType.NoGame;
    }

}
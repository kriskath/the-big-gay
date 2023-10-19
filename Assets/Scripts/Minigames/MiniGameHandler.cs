using UnityEngine;
using DialogueEditor;

public class MiniGameHandler : MonoBehaviour
{
    public NPCConversation battleConversation;

    private IMiniGame currentMiniGame;

    // Public variables for minigame handlers.
    public ButtonMashingGame buttonMashingGame;

    private void Start()
    {
        ConversationManager.Instance.StartConversation(battleConversation);
    }

    // Unfortunately have to make these so the dialogue system calls them
    public void StartButtonMasher (){
        SwitchToMiniGame(MiniGameType.ButtonMashing);
    }

    public void SwitchToMiniGame(MiniGameType gameType)
    {
        // Disable the current mini-game (if any).
        if (currentMiniGame != null)
        {
            currentMiniGame.StopMiniGame();
        }

        // Determine which game to load based on the MiniGameType.
        switch (gameType)
        {
            case MiniGameType.ButtonMashing:
                currentMiniGame = buttonMashingGame;
                break;
            case MiniGameType.NoGame:
                currentMiniGame = null;
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
        currentMiniGame.MiniGameFailed -= OnMiniGameFailed;
        ContiniueFight();
    }

    private void OnMiniGameFailed()
    {
        Debug.Log("Game Failed");
        // Decrease morale here
        currentMiniGame.MiniGameCompleted -= OnMiniGameCompleted;
        currentMiniGame.MiniGameFailed -= OnMiniGameFailed;
        ContiniueFight();
    }

    private void ContiniueFight(){
        ConversationManager.Instance.StartConversation(battleConversation);
    }

}
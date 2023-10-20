using UnityEngine;
using DialogueEditor;


public class MiniGameHandler : MonoBehaviour
{
    private IMiniGame currentMiniGame;

    // Public variables for minigame handlers.
    public ButtonMashingGame buttonMashingGame;

    public BattleUI battleUI;

    public NPCConversation battleStart;
    public NPCConversation battleNoFight;

    private void Start(){
        battleUI.NPCSpeak(battleStart);
    }

    private void OnEnable()
    {
        BattleUI.OnSwitchGame += SwitchToMiniGame; 
    }

    private void OnDisable()
    {
        BattleUI.OnSwitchGame -= SwitchToMiniGame; 
    }


    // Unfortunately have to make these so the dialogue system calls them
    //public void StartButtonMasher (){
   //     SwitchToMiniGame(MiniGameType.ButtonMashing);
    //}    

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
            case MiniGameType.Fight:
                battleUI.AddCompanionButton();
                battleUI.NPCSpeak(battleNoFight);
                battleUI.UpdateHP(-10);
                currentMiniGame = null;
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
            battleUI.HideDialogueBubbles();
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
        ContinueFight();
    }


  
    private void OnMiniGameFailed()
    {
        Debug.Log("Game Failed");
        // Decrease morale here
        currentMiniGame.MiniGameCompleted -= OnMiniGameCompleted;
        currentMiniGame.MiniGameFailed -= OnMiniGameFailed;
        battleUI.UpdateHP(-10);
        ContinueFight();
    }

    private void ContinueFight(){
        currentMiniGame = null;
        battleUI.NPCSpeak(battleStart);
    }

 

}
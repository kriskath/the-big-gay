using UnityEngine;
using DialogueEditor;


public class MiniGameHandler : MonoBehaviour
{
    private IMiniGame currentMiniGame;

    // Public variables for minigame handlers.
    public ButtonMashingGame buttonMashingGame;

    public BattleUI battleUI;

    public NPCConversation battleOnStart;
    public NPCConversation battleOnWinMinigame;
    public NPCConversation battleOnMiss;
    public NPCConversation battleOnFight;
    public NPCConversation battleOnWin;
    public NPCConversation battleOnGameOver;
    public NPCConversation battleOnContinue;
    public NPCConversation battleOnDefend;
    public NPCConversation battleOnRun;
    
    private int tries = 3;
    private int phases = 3;

    private void Start(){
        battleUI.NPCSpeak(battleOnStart);
    }

    private void OnEnable()
    {
        BattleUI.OnSwitchGame += SwitchToMiniGame; 
    }

    private void OnDisable()
    {
        BattleUI.OnSwitchGame -= SwitchToMiniGame; 
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
                battleUI.NPCSpeak(battleOnFight); // need to change this and get one for starting the game
                currentMiniGame = buttonMashingGame;
                break;
            case MiniGameType.Precision:
                battleUI.NPCSpeak(battleOnFight);
                currentMiniGame = buttonMashingGame; // need to change this and get one for starting the game
                break;
            case MiniGameType.Fight:
                battleUI.NPCSpeak(battleOnFight);
                battleUI.UpdateHP(-10);
                currentMiniGame = null;
                break;
            case MiniGameType.Defend:
                battleUI.NPCSpeak(battleOnDefend);
                currentMiniGame = null;
                break;
            case MiniGameType.Run:
                battleUI.NPCSpeak(battleOnRun);
                currentMiniGame = null;
                break;
            case MiniGameType.NoGame:
                currentMiniGame = null;
                break;
            default:
                Debug.LogWarning("[MiniGameHandler] Minigame type not recognized.");
                break;
        }

        if (tries > 0){
            tries--;
        }
        if (tries == 0){
            battleUI.AddCompanionButton();
        }

        if (currentMiniGame != null)
        {
            battleUI.inGame = true;
            battleUI.HideDialogueBubbles();
            currentMiniGame.MiniGameCompleted += OnMiniGameCompleted;
            currentMiniGame.MiniGameFailed += OnMiniGameFailed;
            currentMiniGame.StartMiniGame();
        }
    }

   
    private void OnMiniGameCompleted()
    {
        phases --;
        if (phases == 0){

            return;
        }
        battleUI.NPCSpeak(battleOnWinMinigame);
        Debug.Log("Completed Game");
        currentMiniGame.MiniGameCompleted -= OnMiniGameCompleted;
        currentMiniGame.MiniGameFailed -= OnMiniGameFailed;
        ContinueFight();
    }


  
    private void OnMiniGameFailed()
    {
        battleUI.NPCSpeak(battleOnMiss);
        Debug.Log("Game Failed");
        // Decrease morale here
        currentMiniGame.MiniGameCompleted -= OnMiniGameCompleted;
        currentMiniGame.MiniGameFailed -= OnMiniGameFailed;
        battleUI.UpdateHP(-10);
        ContinueFight();
    }

    private void ContinueFight(){
        currentMiniGame = null;
        battleUI.inGame = false;
        battleUI.NPCSpeak(battleOnContinue);
    }

 

}
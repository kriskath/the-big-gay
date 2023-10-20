using UnityEngine;
using DialogueEditor;
using UnityEngine.UI;

public class MiniGameHandler : MonoBehaviour
{
    public NPCConversation battleStart;
    public NPCConversation battleNoFight;

    private IMiniGame currentMiniGame;

    // Public variables for minigame handlers.
    public ButtonMashingGame buttonMashingGame;

    public BattleButtons[] buttons;

    private int menuIndex = 0;
    private int menuIndexMax = 1; // might want to change this for special cases like asgore

    public GameObject battleUI;

    public float hp = 100;

    private void Start()
    {
        NPCSpeak(battleStart);
    }

    // Unfortunately have to make these so the dialogue system calls them
    public void StartButtonMasher (){
        SwitchToMiniGame(MiniGameType.ButtonMashing);
    }

    private void NPCSpeak(NPCConversation convo){
        BattleMManager.Instance.StartConversation(convo);

    }

    public void Update (){
        if (BattleMManager.Instance.IsIdle()){

            if (Input.GetKeyDown("z") || Input.GetKeyDown("enter")) 
            {

                SwitchToMiniGame(buttons[menuIndex].gameType);
            }

            if (Input.GetKeyDown("left") || Input.GetKeyDown("a")) 
                {
                    menuIndex--;
                } 
                else if (Input.GetKeyDown("right") || Input.GetKeyDown("d")) 
                {
                    menuIndex++;
                }

                //Cross over menu checks
                if (menuIndex < 0) 
                {
                    menuIndex = menuIndexMax;
                }
                if (menuIndex > menuIndexMax) 
                {
                    menuIndex = 0;
                }
                UpdateMenu();
        }

        if (hp <= 0){
            Debug.Log("[HP gone] you failed.");
        }
    }


     void UpdateMenu()
    {
        for(int i = 0; i <= menuIndexMax; i++) 
        {
            GameObject currentButton = buttons[i].instance;

            //if selected battle button
            if (menuIndex == i) 
            {
                currentButton.GetComponent<Image>().sprite = buttons[i].spriteActive;
                
            } 
            else 
            {
                currentButton.GetComponent<Image>().sprite = buttons[i].spriteInactive;
            }
        }
        
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
                battleUI.SetActive(false);
                currentMiniGame = buttonMashingGame;
                break;
            case MiniGameType.Fight:
                NPCSpeak(battleNoFight);
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
        hp -= 10;
        ContiniueFight();
    }

    private void ContiniueFight(){
        battleUI.SetActive(true);
        NPCSpeak(battleStart);
    }

}
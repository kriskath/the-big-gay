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
    private int menuIndexMax = 2; // might want to change this for special cases like asgore

    //battle UI stuff
    public GameObject[] dialogueBubbles;

    // HP
    public Slider hpSlider;
    public float hp = 100;
    private bool gameOver = false;


    private void OnEnable()
    {
        BattleMManager.OnNameChecked += HandleNameChecked;
    }

    private void OnDisable()
    {
        BattleMManager.OnNameChecked -= HandleNameChecked;
    }

    private void Start()
    {
        hpSlider.value = hp;
        NPCSpeak(battleStart);
    }

    // Unfortunately have to make these so the dialogue system calls them
    public void StartButtonMasher (){
        SwitchToMiniGame(MiniGameType.ButtonMashing);
    }

    private void NPCSpeak(NPCConversation convo){
        BattleMManager.Instance.StartConversation(convo);
    }

  public void Update()
    {
        if (gameOver == false){
            HandleInput();
            UpdateMenu();
        }

        if (hp <= 0)
        {
            gameOver = true;
            Debug.Log("[HP gone] you failed.");
        }
    }

    void HandleInput()
    {
        if (!BattleMManager.Instance.IsIdle() || currentMiniGame != null)
            return;

        if (Input.GetKeyDown("e") || Input.GetKeyDown("enter"))
        {
            SwitchToMiniGame(buttons[menuIndex].gameType);
        }

        if (Input.GetKeyDown("left") || Input.GetKeyDown("a"))
        {
            menuIndex = (menuIndex + 1) % (menuIndexMax + 1);
        }
        else if (Input.GetKeyDown("right") || Input.GetKeyDown("d"))
        {
            menuIndex = (menuIndex - 1 + menuIndexMax + 1) % (menuIndexMax + 1);
        }
    }

    void UpdateMenu()
    {
        if (currentMiniGame != null || !BattleMManager.Instance.IsIdle())
        {
            foreach (var button in buttons)
                button.instance.GetComponent<Image>().sprite = button.spriteInactive;
            return;
        }
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
                currentMiniGame = buttonMashingGame;
                break;
            case MiniGameType.Fight:
                NPCSpeak(battleNoFight);
                hp -= 10;
                currentMiniGame = null;
                break;
            default:
                Debug.LogWarning("[MiniGameHandler] Minigame type not recognized.");
                break;
        }

        if (currentMiniGame != null)
        {
            HideDialogueBubbles();
            currentMiniGame.MiniGameCompleted += OnMiniGameCompleted;
            currentMiniGame.MiniGameFailed += OnMiniGameFailed;
            currentMiniGame.StartMiniGame();
        }
    }

    private void HideDialogueBubbles(){
        foreach (GameObject bubble in dialogueBubbles)
        {
            bubble.SetActive(false);
        }
    }


    private void HandleNameChecked(string name)
    {
        if (name == "hero")
        {
            // Toggle on the bubble for the hero
            dialogueBubbles[0].SetActive(true);
            dialogueBubbles[1].SetActive(false);
        }
        else
        {
            // Toggle on the bubble for the other guy
            dialogueBubbles[0].SetActive(false);
            dialogueBubbles[1].SetActive(true);
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
        hp -= 10;
        hpSlider.value = hp;
        ContinueFight();
    }

    private void ContinueFight(){
        currentMiniGame = null;
        NPCSpeak(battleStart);
    }

}
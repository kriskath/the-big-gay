using System; 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DialogueEditor;

public class BattleUI : MonoBehaviour
{
        // Group: Dialogue UI
    [SerializeField] 
    private GameObject[] dialogueBubbles;

    // Group: Health UI
    [SerializeField] 
    private Slider hpSlider;
    [SerializeField] 
    private float hp = 100;
    [SerializeField] 
    private bool gameOver = false;

    // Group: Battle Buttons
    [SerializeField] 
    private List<BattleButtons> buttons = new List<BattleButtons>();
    [SerializeField] 
    private BattleButtons companionAction;
    [SerializeField] 
    private int menuIndex = 0;
    [SerializeField] 
    private int menuIndexMax = 2;

    // Group: Events
    [SerializeField] 
    public static event Action<MiniGameType> OnSwitchGame;

    // Group: Game State
    [SerializeField] 
    public bool inGame = false;
    public GameObject gameManager;

    // Group: Audio
    [SerializeField] 
    public AudioManager audioManager;
    [SerializeField] 
    public float companionPitch;

    private void OnEnable()
    {
        BattleMManager.OnNameChecked += HandleNameChecked;
    }

    private void Start()
    {
        if (AudioManager.Instance == null){
            gameManager.SetActive(true);
            audioManager = gameManager.GetComponent<AudioManager>();
        }
        else audioManager = AudioManager.Instance;
        audioManager.PlayBattleTheme();
        hpSlider.value = hp;
    }

    private void OnDisable()
    {
        BattleMManager.OnNameChecked -= HandleNameChecked;
    }
    public void Update()
    {

        if (BattleMManager.Instance.IsIdle()){
            audioManager.StopCharacterSpeaking();
        }
        if (gameOver == false){
            HandleInput();
            UpdateMenu();
        }

        if (hp <= 0)
        {
            gameOver = true;
            OnSwitchGame?.Invoke(MiniGameType.NoGame);
            Debug.Log("[HP gone] you failed.");
        }
    }
    private void HandleInput()
    {
        //current minigame also
        if (inGame || !BattleMManager.Instance.IsIdle())
            return;

        if (Input.GetKeyDown("e") || Input.GetKeyDown("enter"))
        {
            audioManager.PlaySelectSFX();
            OnSwitchGame?.Invoke((buttons[menuIndex].gameType));
        }

        if (Input.GetKeyDown("left") || Input.GetKeyDown("a"))
        {
            audioManager.PlayHoverSFX();
            menuIndex = (menuIndex + 1) % (menuIndexMax + 1);
        }
        else if (Input.GetKeyDown("right") || Input.GetKeyDown("d"))
        {
            audioManager.PlayHoverSFX();
            menuIndex = (menuIndex - 1 + menuIndexMax + 1) % (menuIndexMax + 1);
        }
    }

    //current minigame also 
    void UpdateMenu()
    {
        if (inGame || !BattleMManager.Instance.IsIdle())
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
    public void HideDialogueBubbles(){
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
            audioManager.PlayCharacterSpeaking(1f);
        }
        else
        {
            // Toggle on the bubble for the other guy
            audioManager.PlayCharacterSpeaking(companionPitch);
            dialogueBubbles[0].SetActive(false);
            dialogueBubbles[1].SetActive(true);
            
        }
    }
    
    public void NPCSpeak(NPCConversation convo)
    {
        BattleMManager.Instance.StartConversation(convo);
    }

    public void AddCompanionButton()
    {
        companionAction.instance.GetComponent<Image>().material = null;
        buttons.Add(companionAction);
        menuIndexMax++;
    }

    public void UpdateHP (float dhp)
    {
        hp += dhp;
        hpSlider.value = hp;
    }
    
}

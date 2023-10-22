using System; 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DialogueEditor;
using TMPro;

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
    private Color originalColor;
    private float hpDepletionSpeed = 10.0f;

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
        originalColor = hpSlider.fillRect.GetComponent<Image>().color;
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
            audioManager.StopBGMusic();
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
        audioManager.PlayStartGameSFX();
        companionAction.instance.GetComponent<Image>().material = null;
        buttons.Add(companionAction);
        menuIndexMax++;
    }

    public void UpdateHP (float dhp)
    {
        StartCoroutine(ChangeColorAndDepleteHP(dhp));
    }

    private IEnumerator ChangeColorAndDepleteHP(float dhp)
    {
        hpSlider.fillRect.GetComponent<Image>().color = Color.red;

        float targetHP = hp + dhp;
        float elapsedTime = 0f;

        while (hp > targetHP)
        {
            elapsedTime += Time.deltaTime;
            float newHP = Mathf.Lerp(hp, targetHP, elapsedTime * hpDepletionSpeed);
            hp = newHP;
            hpSlider.value = hp;

            yield return null;
        }

        // Ensure the HP and slider values are set to the target.
        hp = targetHP;
        hpSlider.value = hp;

        hpSlider.fillRect.GetComponent<Image>().color = originalColor;
    }

    public void DragUpYourLife(){
        GameObject companionIcon = GameObject.Find("CompanionIcon");
        GameObject wubbDragPortrait = GameObject.Find("dragportrait");
        TextMeshProUGUI wubbsName = GameObject.Find("CompanionName").GetComponent<TextMeshProUGUI>();

        if (companionIcon != null && wubbDragPortrait != null)
        {
            Image companionImage = companionIcon.GetComponent<Image>();
            SpriteRenderer wubbSpriteRenderer = wubbDragPortrait.GetComponent<SpriteRenderer>();

            if (companionImage != null && wubbSpriteRenderer != null)
            {
                companionImage.sprite = wubbSpriteRenderer.sprite;
                GameObject.Find("wubbdrag").transform.localScale = new Vector3(1f, 1f, 1f);
                wubbsName.text = "BINCHY DOLL";
                wubbsName.fontSize = 19;
            }
            else
            {
                Debug.LogWarning("Image or SpriteRenderer component not found.");
            }
        }
        else
        {
            Debug.LogWarning("GameObject not found.");
        }
    }
    
}

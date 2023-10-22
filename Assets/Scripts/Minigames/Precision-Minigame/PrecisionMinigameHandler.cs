using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Events;
using System;
using System.Collections;
using TMPro;

public class PrecisionMinigameHandler : MonoBehaviour, IMiniGame
{
    //IMiniGame
    public event Action MiniGameCompleted; 
    public event Action MiniGameFailed; 
    //End IMiniGame
    
    [Header("General")]
    [SerializeField] [Tooltip("The GameObject containing this script and UI its elements.")]
    private GameObject minigameUI;
    [SerializeField] 
    private SpriteRenderer sliceAreaRenderer;
    [SerializeField] 
    private SpriteRenderer playerSlicerRenderer;
    [SerializeField] 
    private SpriteRenderer hitRegionRenderer;
    [SerializeField] 
    public TextMeshProUGUI triesText; 

    [FormerlySerializedAs("percisionSprite")]
    [Header("Slicer UI Assets")] 
    [SerializeField]
    private Sprite precisionSprite;
    [SerializeField]
    private Sprite perfectSprite;
    [SerializeField]
    private Sprite missedSprite;
    
    [Header("Configurables")] 
    [SerializeField]
    private float timeToWait = 3f;
    [Space]
    [SerializeField] [Range(0f, 20f)] 
    private float slicerMovementSpeed = 10f;
    [SerializeField] [Range(-10f, 10f)] 
    private float slicerXConfig = 0f;
    [SerializeField] [Range(-10f, 10f)] 
    private float slicerYConfig = 0f;
    [Space]
    [SerializeField] [Range(-10f, 10f)] 
    private float sliceAreaXConfig = 0f;
    [SerializeField] [Range(-10f, 10f)] 
    private float sliceAreaYConfig = 0f;
    
    
    //Area
    private Vector2 slicerStartPos;
    private Vector2 sliceAreaMin;
    private Vector2 sliceAreaMax;
    private Vector2 sliceAreaCenter;
    
    //Slicer
    private bool bGoRight = true;
    private float playerSlicerHalfLen;
    
    //HitRegion
    private float hitRegionHalfLen;

    private bool bPaused = true;
    private int tries = 3; //todo add ui for this?

    void Update()
    {
        if (bPaused) return;
        UpdateSlicerLocation();

        CheckPlayerInput();
    }

    private void CheckPlayerInput()
    {
        if (Input.GetKeyDown("e"))
        {
            
            //if within hit area
            float slicerPosX = playerSlicerRenderer.transform.position.x, hitRegionPosX = hitRegionRenderer.transform.position.x;
            if (slicerPosX > hitRegionPosX - hitRegionHalfLen && slicerPosX < hitRegionPosX + hitRegionHalfLen)
            {
                StopMiniGame();
            }
            else
            {
                if (tries > 1){
                    tries--;
                    triesText.text = "Tries: " + tries;
                    StartCoroutine(FailedCountdown());                
                }
                else 
                    FailMiniGame();
            }
        }
    }
    
    private void UpdateSlicerLocation()
    {
        
        //if we extend passed area, switch dir
        if (playerSlicerRenderer.transform.position.x + playerSlicerHalfLen > sliceAreaMax.x)
        {
            bGoRight = false;
        }
        else if (playerSlicerRenderer.transform.position.x - playerSlicerHalfLen < sliceAreaMin.x)
        {
            bGoRight = true;
        }
        
        int slicerDirectionMod = (bGoRight) ? 1 : -1 ;
        playerSlicerRenderer.transform.Translate(slicerDirectionMod * Time.deltaTime * slicerMovementSpeed, 0, 0);
    }
    
    
    //Resets minigame when game is started
    private void ResetMiniGame(bool bResetSlicer = true, bool bRandomizeHitRegion = true)
    {
        //Set start pos
        if (bResetSlicer)
        {
            playerSlicerRenderer.transform.position = new Vector3(slicerStartPos.x + playerSlicerHalfLen + slicerXConfig, sliceAreaCenter.y + slicerYConfig, 0);
        }
        
        //Set random pos
        if (bRandomizeHitRegion)
        {
            hitRegionRenderer.transform.position = new Vector3(UnityEngine.Random.Range(sliceAreaMin.x + hitRegionHalfLen, sliceAreaMax.x - hitRegionHalfLen + sliceAreaXConfig) , sliceAreaCenter.y + sliceAreaYConfig, 0);
        }
    } 

#region IMiniGame Interface

    public void StartMiniGame()
    {
        Debug.Log("[Precision minigame] started");
        //UI should be hidden, so unhide it
        minigameUI.SetActive(true);
        tries = 3;
        triesText.text = "Tries: " + tries;
        //Initialize data and start game
        //SliceArea
        sliceAreaMin = sliceAreaRenderer.bounds.min;
        sliceAreaMax = sliceAreaRenderer.bounds.max;
        sliceAreaCenter = sliceAreaRenderer.bounds.center;
        
        //Slicer
        slicerStartPos = sliceAreaMin;
        playerSlicerHalfLen = playerSlicerRenderer.bounds.extents.x;
        
        //HitRegion
        hitRegionHalfLen = hitRegionRenderer.bounds.extents.x;

        //Start countdown
        StartCoroutine(StartupCountdown());
    }

    private IEnumerator StartupCountdown()
    {
        bPaused = true;
        
        ResetMiniGame();
        
        float time = 0f;
        while (time < timeToWait)
        {
            time += Time.deltaTime;
            yield return null;
        }
        
        bPaused = false;
    }

    public void StopMiniGame()
    {
        MiniGameCompleted?.Invoke();
        StartCoroutine(FinishedGameCountdown());
    }

    private IEnumerator FinishedGameCountdown()
    {
        bPaused = true;
        
        //ToDo: Player success audio clip from audio manager
        playerSlicerRenderer.sprite = perfectSprite;
        
        float time = 0f;
        while (time < 0.3)
        {
            time += Time.deltaTime;
            yield return null;
        }

        playerSlicerRenderer.sprite = precisionSprite;
        minigameUI.SetActive(false);
    }
    
 
    public void FailMiniGame()
    {
        MiniGameFailed?.Invoke();
        
        //Play failed stuff, then retry
        StartCoroutine(FinishedGameCountdown());
    }
    
    private IEnumerator FailedCountdown()
    {
        bPaused = true;
        
        //ToDo: call failed audio from audio manager
        playerSlicerRenderer.sprite = missedSprite;
        
        float time = 0f;
        while (time < 0.3)
        {
            time += Time.deltaTime;
            yield return null;
        }
        
        playerSlicerRenderer.sprite = precisionSprite;
        
        bPaused = false;
        ResetMiniGame(false);
    }

#endregion
    
}

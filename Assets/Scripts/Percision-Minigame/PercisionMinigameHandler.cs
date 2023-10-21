using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Events;

public class PercisionMinigameHandler : MonoBehaviour
{
    public UnityEvent onPlayerSuccessfulHit;
    public UnityEvent onPlayerMissedHit;
    
    [Header("General")]
    [SerializeField] 
    private GameObject sliceArea;
    [SerializeField] 
    private GameObject playerSlicer;
    [SerializeField] 
    private GameObject hitRegion;
    
    [Header("Configurables")]
    [SerializeField] [Range(0f, 20f)] 
    private float slicerMovementSpeed = 10f;
    
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
    
    private void OnEnable()
    {
        ResetMiniGame();
    }
    
    // We want to have a game object moving that goes from 
    void Start()
    {
        //SliceArea
        SpriteRenderer spriteRenderer = sliceArea.GetComponent<SpriteRenderer>();
        sliceAreaMin = spriteRenderer.bounds.min;
        sliceAreaMax = spriteRenderer.bounds.max;
        sliceAreaCenter = spriteRenderer.bounds.center;
        
        //Slicer
        slicerStartPos = sliceAreaMin;
        playerSlicerHalfLen = playerSlicer.GetComponent<SpriteRenderer>().bounds.extents.x;
        
        //HitRegion
        hitRegionHalfLen = hitRegion.GetComponent<SpriteRenderer>().bounds.extents.x;
        
        ResetMiniGame();
    }

    void Update()
    {
        UpdateSlicerLocation();

        CheckPlayerInput();
    }

    private void CheckPlayerInput()
    {
        if (Input.GetKeyDown("space"))
        {
            //if within hit area
            float slicerPosX = playerSlicer.transform.position.x, hitRegionPosX = hitRegion.transform.position.x;
            if (slicerPosX > hitRegionPosX - hitRegionHalfLen && slicerPosX < hitRegionPosX + hitRegionHalfLen)
            {
                OnSuccessfulHit();
            }
            else
            {
                OnMissedHit();
            }
        }
    }
    
    private void UpdateSlicerLocation()
    {
        //if we extend passed area, switch dir
        if (playerSlicer.transform.position.x + playerSlicerHalfLen > sliceAreaMax.x)
        {
            bGoRight = false;
        }
        else if (playerSlicer.transform.position.x - playerSlicerHalfLen < sliceAreaMin.x)
        {
            bGoRight = true;
        }
        
        int slicerDirectionMod = (bGoRight) ? 1 : -1 ;
        playerSlicer.transform.Translate(slicerDirectionMod * Time.deltaTime * slicerMovementSpeed, 0, 0);
    }
    
    
    //Resets minigame when game is started
    private void ResetMiniGame(bool bResetSlicer = true, bool bRandomizeHitRegion = true)
    {
        //Set start pos
        if (bResetSlicer)
        {
            playerSlicer.transform.position = new Vector3(slicerStartPos.x + playerSlicerHalfLen, sliceAreaCenter.y, 0);
        }
        
        //Set random pos
        if (bRandomizeHitRegion)
        {
            hitRegion.transform.position = new Vector3(Random.Range(sliceAreaMin.x + hitRegionHalfLen, sliceAreaMax.x - hitRegionHalfLen) , sliceAreaCenter.y , 0);
        }
    } 
    
    #region GameEvents
    private void OnSuccessfulHit()
    {
        print("Successful hit! :3");
        onPlayerSuccessfulHit?.Invoke();
    }

    private void OnMissedHit()
    {
        print("Whomp whomp... missed hit. :(");
        onPlayerMissedHit?.Invoke();
        ResetMiniGame(false);
    }
    #endregion    

}

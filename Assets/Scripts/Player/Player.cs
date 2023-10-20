using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SpriteRenderer))]
public class Player : MonoBehaviour 
{

    [SerializeField] private float interactionDistance = 2.0f;
    [SerializeField] private LayerMask interactableLayer;     // Need to set a layer in Edit -> Project Settings -> Tags and Layers
    [SerializeField] [Range(0f, 20f )] private float speed = 1f;
    
    public Animator animator;
    private Rigidbody2D rb;
    private SpriteRenderer sprite;

    public bool CanMove => _canMove;
    private bool _canMove = true;
    
    public bool CanInteract => _canInteract;
    private bool _canInteract = true;
    
    private float vertical;
    private float horizontal;
	
    // Use this for initialization
	void Start () 
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
    }
	
	// Update is called once per frame
	void Update () 
    {
        animator.SetFloat("Speed", Mathf.Abs(horizontal));

        if (CanMove)
        {
            vertical = Input.GetAxisRaw("Vertical");
            horizontal = Input.GetAxisRaw("Horizontal");
            UpdateMovement();
            
            HandleAnimation();
            
            if (CanInteract && Input.GetKeyDown(KeyCode.E))
            {
                CheckForNPCs();
            }  
        }
    }

    private void HandleAnimation()
    {
        switch ((int)horizontal) {
            case (<0):
                animator.SetBool("WalkLeft", true);
                animator.SetBool("WalkRight", false);
                break;
            case (>0):
                animator.SetBool("WalkRight", true);
                animator.SetBool("WalkLeft", false);
                break;
            default:
                animator.SetBool("WalkLeft", false);
                animator.SetBool("WalkRight", false);
                break;
        }
        
    }

    //Updates player movement and animations
    private void UpdateMovement()
    {
        //clamp movement
        Vector2 movement = new Vector2(horizontal, vertical);
        if (movement.magnitude > 1)
        {
            movement = movement.normalized;
        }
        rb.velocity = movement * speed;
    }

    public void StopMoving()
    {
        horizontal = 0;
        vertical = 0;
        SetCanMove(false);
    }

    void CheckForNPCs()
    {
    // Detect all colliders within a box centered at the player's position with dimensions defined by 'interactionDistance' and filter by the 'interactableLayer'.
        Collider2D[] colliders = Physics2D.OverlapBoxAll(transform.position, new Vector2(interactionDistance, interactionDistance), 0f, interactableLayer);
        foreach (Collider2D collider in colliders)
        {
            // Check if the collider belongs to an interactable character.
            NPCInteract npcInteraction = collider.GetComponent<NPCInteract>();

            if (npcInteraction != null)
            {
                npcInteraction.StartConversation(rb.transform);
            }
        }
    }

    public void StartConversation()
    {
        SetCanInteract(false);
        SetCanMove(false);
        StopMoving(); 
    }

    public void EndConversation()
    {
        SetCanInteract(true);
        SetCanMove(true);
    }

    public void SetCanMove(bool bCanMove)
    {
        _canMove = bCanMove;
    }
    
    public void SetCanInteract(bool bCanInteract)
    {
        _canInteract = bCanInteract;
    }

}

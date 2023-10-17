using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    

    public float interactionDistance = 2.0f;
    public LayerMask interactableLayer;     // Need to set a layer in Edit -> Project Settings -> Tags and Layers
    private Rigidbody2D playerBody;

    private bool inDialogue = false;

	// Use this for initialization
	void Start () {
        playerBody = GetComponent<Rigidbody2D>();
    }
	
	// Update is called once per frame
	void Update () {

        // basic movement to test
        if (!inDialogue){
            Movement();
            if (Input.GetKeyDown(KeyCode.E))
            {
               CheckForNPCs();
            }
        }
    }

    void Movement(){
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        playerBody.velocity = new Vector2(horizontal, vertical);

    }
    void CheckForNPCs(){
    // Detect all colliders within a box centered at the player's position with dimensions defined by 'interactionDistance' and filter by the 'interactableLayer'.
        Collider2D[] colliders = Physics2D.OverlapBoxAll(transform.position, new Vector2(interactionDistance, interactionDistance), 0f, interactableLayer);
        foreach (Collider2D collider in colliders)
        {
            // Check if the collider belongs to an interactable character.
            NPCInteract npcInteraction = collider.GetComponent<NPCInteract>();

            if (npcInteraction != null)
            {
                npcInteraction.StartConversation(playerBody.transform);
            }
        }
    }
    public void StartConversation()
    {
        inDialogue = true;
        playerBody.velocity = new Vector2(0, 0);
    }
    public void EndConversation()
    {
        inDialogue = false;
    }
  
}

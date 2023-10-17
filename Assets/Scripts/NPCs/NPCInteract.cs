using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DialogueEditor;

public class NPCInteract : MonoBehaviour
{
    public NPCConversation conversation;

    public void StartConversation(Transform playerTransform)
    {
        if (!ConversationManager.Instance.IsConversationActive){
            Vector3 direction = (playerTransform.position - transform.position).normalized;
            direction.y = 0; // Keep the character upright in the 2D world (if needed).
            transform.right = direction; // Make the character face the player.

            ConversationManager.Instance.StartConversation(conversation);
        }
      
    }

}


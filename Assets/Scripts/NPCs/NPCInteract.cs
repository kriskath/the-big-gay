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
            ConversationManager.Instance.StartConversation(conversation);
        }
      
    }
}


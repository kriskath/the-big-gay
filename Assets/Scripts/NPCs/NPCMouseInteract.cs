using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DialogueEditor;

public class NPCMouseInteract : MonoBehaviour
{
    public NPCConversation conversation;

    public void OnMouseOver(){
        if (Input.GetMouseButtonDown(0)){
            ConversationManager.Instance.StartConversation(conversation);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DialogueEditor;

public class Wizard : MonoBehaviour
{
    public NPCConversation wizardConversation;

    public void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0)){
            ConversationManager.Instance.StartConversation(wizardConversation);
        }
    }

}

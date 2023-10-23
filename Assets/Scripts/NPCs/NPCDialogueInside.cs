using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DialogueEditor;

public class NPCDialogueInside : MonoBehaviour
{
    public NPCConversation preFight;
    public NPCConversation postFight;

    void Start()
    {
        // Todo: add check from game manager if won
        ConversationManager.Instance.StartConversation(preFight);
  
    }
}

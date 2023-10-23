using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DialogueEditor;

public class NPCPlayerInteract : MonoBehaviour
{
   [SerializeField] private Player player;

   void OnEnable(){
        ConversationManager.OnConversationStarted += player.StartConversation;
        ConversationManager.OnConversationEnded += player.EndConversation;
    }

    void OnDisable(){
        ConversationManager.OnConversationStarted -= player.StartConversation;
        ConversationManager.OnConversationEnded -= player.EndConversation;
    }
}

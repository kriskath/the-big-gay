using UnityEngine;
using DialogueEditor;

public class DialogueHandler : MonoBehaviour
{
    private Player player;

    private void OnEnable(){
        player = FindObjectOfType<Player>(); 
        ConversationManager.OnConversationStarted += player.StartConversation;
        ConversationManager.OnConversationEnded += player.EndConversation;
    }
    private void OnDisable(){
        ConversationManager.OnConversationStarted -= player.StartConversation;
        ConversationManager.OnConversationEnded -= player.EndConversation;
    }
}
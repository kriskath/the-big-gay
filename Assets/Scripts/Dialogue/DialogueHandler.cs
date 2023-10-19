using System;
using UnityEngine;
using DialogueEditor;

public class DialogueHandler : MonoBehaviour
{
    [SerializeField] private Player player;

    private void Start()
    {
        FindPlayer();
    }

    private void OnEnable()
    {
        FindPlayer();
        ConversationManager.OnConversationStarted += player.StartConversation;
        ConversationManager.OnConversationEnded += player.EndConversation;
    }
    private void OnDisable(){
        ConversationManager.OnConversationStarted -= player.StartConversation;
        ConversationManager.OnConversationEnded -= player.EndConversation;
    }

    private void FindPlayer()
    {
        if (!player)
        {
            player = FindObjectOfType<Player>(); 
        }
    }
    
}
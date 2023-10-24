using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DialogueEditor;

public enum Place
{
    Dragbar,
    Bakery,
    Overworld,
    Other
}
public class NPCDialogueInside : MonoBehaviour
{
    public NPCConversation preFight;
    public NPCConversation postFight;

    public Place place;

    void Start()
    {
        switch (place){
            case (Place.Dragbar):
                if (GameManager.Instance.IsDragFinished()){
                    ConversationManager.Instance.StartConversation(postFight);
                    }
                else ConversationManager.Instance.StartConversation(preFight);
                break;
            case (Place.Bakery):
                if (GameManager.Instance.IsBakeryFinished()){
                    ConversationManager.Instance.StartConversation(postFight);
                    }
                else ConversationManager.Instance.StartConversation(preFight);
                break;
            case (Place.Overworld):
                if (GameManager.Instance.IsBakeryFinished() || GameManager.Instance.IsDragFinished()){
                    return;
                    }
                else ConversationManager.Instance.StartConversation(preFight);
                break;
            case (Place.Other):
                ConversationManager.Instance.StartConversation(preFight);
                break;

        }

        
  
    }
}

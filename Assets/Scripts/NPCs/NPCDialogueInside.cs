using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DialogueEditor;

public class NPCDialogueInside : MonoBehaviour
{
    public NPCConversation preFight;
    public NPCConversation postFightFail;
    public NPCConversation postFightWin;
    // Start is called before the first frame update
    void Start()
    {
        ConversationManager.Instance.StartConversation(preFight);
  
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

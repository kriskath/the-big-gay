using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueFinishedTransitionHandler : MonoBehaviour
{
    public void LoadAfterBakeryBattle()
    {
        GameManager.Instance.SetBakeryFinished(true);
        GameManager.Instance.LoadBakeryScene();
    }

    public void LoadBakeryBattle()
    {
        GameManager.Instance.LoadBakeryBattleScene();
    }
    
    public void LoadAfterDragBarBattle()
    {
        GameManager.Instance.SetBakeryFinished(true);
        GameManager.Instance.LoadDragBarScene();
    }
    
    public void LoadDragBarBattle()
    {
        GameManager.Instance.LoadDragBarBattleScene();
    }

    public void LoadOverworld()
    {
        if ( GameManager.Instance.IsBakeryAndDragFinished())
        {
            GameManager.Instance.LoadOutroScene();
        }
        else
        {
            GameManager.Instance.LoadTownScene();
        }
    }
}

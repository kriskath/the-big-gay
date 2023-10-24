using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueFinishedTransitionHandler : MonoBehaviour
{
    public void LoadAfterBakeryBattle()
    {
        GameManager.Instance.LoadBakeryScene();
    }

    public void LoadBakeryBattle()
    {
        GameManager.Instance.LoadBakeryBattleScene();
    }
    
    public void LoadAfterDragBarBattle()
    {
        GameManager.Instance.LoadDragBarScene();
    }
    
    public void LoadDragBarBattle()
    {
        GameManager.Instance.LoadDragBarBattleScene();
    }

    public void LoadOverworld()
    {
        GameManager.Instance.LoadTownScene();
    }
}

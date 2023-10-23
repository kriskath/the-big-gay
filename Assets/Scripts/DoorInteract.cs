using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorInteract : MonoBehaviour
{
    [SerializeField] private bool bCanTraverse = false;
    [SerializeField] private TraversalType traversalType;

    enum TraversalType
    {
        None,
        Overworld,
        Bakery,
        DragBar,
        Count
    }
    
    public void Interact()
    {
        if (bCanTraverse)
        {
            switch (traversalType)
            {
                case TraversalType.Overworld:
                    GameManager.Instance.LoadTownScene();
                    return;    
                case TraversalType.Bakery:
                    GameManager.Instance.LoadBakeryScene();
                    return;
                case TraversalType.DragBar:
                    GameManager.Instance.LoadDragBarScene();
                    return;
                default:
                    return;
            }
        }
        
        //ToDo: Display text info
    }
}

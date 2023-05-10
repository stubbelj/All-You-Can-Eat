using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : MonoBehaviour
{
    public string itemName = "blank";
    public GameManager gameManager;

    public virtual void Activate(){}

    protected virtual void Deactivate(){}

    public virtual void ChangeItem(Item newItem){
        if (newItem != null) {
            itemName = newItem.itemName;
        } else {
            itemName = "blank";
        }
    }

    public virtual void ChangeItem(string newItem){
        itemName = newItem;
    }

    void Start() {
        gameManager = GameManager.inst;
    }

    
}

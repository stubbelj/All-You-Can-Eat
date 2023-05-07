using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : MonoBehaviour
{
    public string itemName = "blank";

    public virtual void Activate(){}

    protected virtual void Deactivate(){}

    public virtual void ChangeItem(Item newItem){
        itemName = newItem.itemName;
    }

    public virtual void ChangeItem(string newItem){
        itemName = newItem;
    }
}

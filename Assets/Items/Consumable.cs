using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Consumable : Item
{
    public void Init(string newItemName) {
        itemName = newItemName;
    }

    public override void Activate() {
        switch(itemName) {
            case "tomatoSoup":
                //Player.ApplyEffect();
                //unimplemented, should modify player status
                break;
        }
    }
}

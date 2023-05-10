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
                print("drank tomato soup");
                gameManager.player.ApplyEffect(MSmod : 1000);
                //unimplemented, should modify player status
                break;
            case "creamMushroomSoup":
                print("drank mushroom soup");
                //Player.ApplyEffect();
                //unimplemented, should modify player status
                break;
            case "carrotSoup":
                print("drank carrot soup");
                //Player.ApplyEffect();
                //unimplemented, should modify player status
                break;
            case "twoEggs":
                print("ate two eggs");
                //Player.ApplyEffect();
                //unimplemented, should modify player status
                break;
            case "stirFryVeggies":
                print("ate stir fry");
                //Player.ApplyEffect();
                //unimplemented, should modify player status
                break;
        }
    }
}

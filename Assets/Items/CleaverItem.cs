using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CleaverItem : Item
{
    public override void Activate(){
        gameManager.player.currWeapon.Activate();
    }
}

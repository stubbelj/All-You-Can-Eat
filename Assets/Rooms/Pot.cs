using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pot : Interactable
{
    GameObject craftingUI;

    public override void Activate() {
            //print("actually activating");
            activated = true;
            craftingUI.SetActive(true);
            if (!gameManager.player.inventoryMenu.activeSelf) {
                gameManager.player.ToggleInventory();
            }
    }

    public override void Deactivate() {
            //print("deactivating");
            activated = false;
            craftingUI.SetActive(false);
            gameManager.player.ToggleInventory();
    }

    public override void AbstractStart() {
        craftingUI = GameObject.Find("Canvas").transform.Find("UI").Find("CraftingUI").gameObject;
    }
}

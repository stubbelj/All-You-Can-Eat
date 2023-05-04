using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pot : Interactable
{
    GameObject craftingUI;

    public override void Activate() {
        craftingUI.SetActive(true);
        if (!gameManager.player.inventoryMenu.activeSelf) {
            gameManager.player.EscapePress();
        }
    }

    public override void Deactivate() {
        craftingUI.SetActive(false);
        gameManager.player.EscapePress();
    }

    public override void AbstractStart() {
        craftingUI = GameObject.Find("Canvas").transform.Find("UI").Find("CraftingUI").gameObject;
    }
}

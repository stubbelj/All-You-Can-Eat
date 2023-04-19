using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    public Inventory inventory;
    public int[] coords;
    public string item = "blank";
    public GameObject itemSpriteContainer;
    GameManager gameManager;

    void Awake() {
        gameManager = GameManager.inst;
    }

    public void ChangeItem(string newItemName) {
        if (newItemName == "") {
            newItemName = "blank";
        }
        item = newItemName;
        itemSpriteContainer.GetComponent<Image>().sprite = gameManager.itemSprites[gameManager.itemIndices[newItemName]];
    }
}

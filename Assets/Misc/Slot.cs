using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    public Inventory inventory;
    public int[] coords;
    public Item item;
    public GameObject itemSpriteContainer;
    public GameManager gameManager;
    public string type = "none";
    public bool draggable = true;
    List<string> weaponList = new List<string>{
        "tomatoClub", "slingPeas", "cleaver"
    };
    List<string> itemList = new List<string>{
        "tomatoClub", "slingPeas", "cleaver"
    };

    void Awake() {
        gameManager = GameManager.inst;
    }

    void Start() {
        gameManager = GameManager.inst;
        item = GetComponent<Item>();
    }

    public void ChangeItem(Item newItem) {
        if (newItem != null) {
            if (weaponList.Contains(newItem.itemName)) {
                type = "weapon";
            }
            itemSpriteContainer.GetComponent<Image>().sprite = gameManager.itemSprites[gameManager.itemIndices[newItem.itemName]];
        } else {
            itemSpriteContainer.GetComponent<Image>().sprite = gameManager.itemSprites[gameManager.itemIndices["blank"]];
        }
        item.ChangeItem(newItem);
    }

    public void ChangeItem(string newItem) {
        if (newItem != "") {
            if (weaponList.Contains(newItem)) {
                type = "weapon";
            }
            itemSpriteContainer.GetComponent<Image>().sprite = gameManager.itemSprites[gameManager.itemIndices[newItem]];
        } else {
            itemSpriteContainer.GetComponent<Image>().sprite = gameManager.itemSprites[gameManager.itemIndices["blank"]];
        }
        item.ChangeItem(newItem);
    }
}

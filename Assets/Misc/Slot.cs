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

    void Awake() {
        gameManager = GameManager.inst;
        item = transform.Find("Item").GetComponent<Item>();
    }

    void Start() {
        gameManager = GameManager.inst;
    }

    public void ChangeItem(Item newItem) {
        if (newItem != null) {
            if (newItem.itemName == "cleaver") {
                type = "weapon";
            }
            itemSpriteContainer.GetComponent<Image>().sprite = gameManager.itemSprites[gameManager.itemIndices[newItem.itemName]];
        } else {
            itemSpriteContainer.GetComponent<Image>().sprite = gameManager.itemSprites[gameManager.itemIndices["blank"]];
        }
        if (newItem == null) {
            Destroy(item);
            item = gameObject.AddComponent<BlankItem>();
        } else if (newItem.itemName == "cleaver") {
            Destroy(item);
            item = gameObject.AddComponent<CleaverItem>();
        } else {
            Destroy(item);
            item = gameObject.AddComponent<Consumable>();
        }
        item.ChangeItem(newItem);
    }

    public void ChangeItem(string newItem) {
        if (newItem != "") {
            if (newItem == "cleaver") {
                type = "weapon";
            }
            itemSpriteContainer.GetComponent<Image>().sprite = gameManager.itemSprites[gameManager.itemIndices[newItem]];
        } else {
            itemSpriteContainer.GetComponent<Image>().sprite = gameManager.itemSprites[gameManager.itemIndices["blank"]];
        }
        if (newItem == "cleaver") {
            Destroy(item);
            item = gameObject.AddComponent<CleaverItem>();
        } else if (newItem == "blank") {
            Destroy(item);
            item = gameObject.AddComponent<BlankItem>();
        } else {
            Destroy(item);
            item = gameObject.AddComponent<Consumable>();
        }
        item.ChangeItem(newItem);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActiveHotbar : MonoBehaviour
{
    public Inventory inventory;
    public Player player;
    public List<Slot> slots = new List<Slot>();
    int curr = 0;
    int prev;
    // Start is called before the first frame update
    void Start()
    {
        foreach (Slot slot in slots) {
            Color tempColor = slot.GetComponent<Image>().color;
            tempColor.a = 0.5f;
            slot.GetComponent<Image>().color = tempColor;
            tempColor = slot.itemSpriteContainer.GetComponent<Image>().color;
            tempColor.a = 0.5f;
            slot.itemSpriteContainer.GetComponent<Image>().color = tempColor;
        }

        //StartCoroutine(LateStart());
    }

    public IEnumerator LateStart() {
        yield return new WaitForSeconds(0.1f);
        UpdateInventory();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.mouseScrollDelta.y != 0) {
            ScrollActiveHotbar(Input.mouseScrollDelta.y);
        }

        //print(slots[0].GetType());
    }

    void ScrollActiveHotbar(float dir) {
        dir = -dir;
        prev = curr;
        while(true) {
            //iterate until the next item in hotbar or return to prev
            if (dir == 1) {
                if (curr == slots.Count - 1) {
                    curr = 0;
                } else {
                    curr += 1;
                }
            } else if (dir == -1) {
                if (curr == 0) {
                    curr = slots.Count - 1;
                } else {
                    curr -= 1;
                }
            }

            if (slots[curr].item != "blank") {
                break;
            }
            if (curr == prev) {
                break;
            }
        }
        Color tempColor = slots[prev].itemSpriteContainer.GetComponent<Image>().color;
        tempColor.a = 0.5f;
        slots[prev].itemSpriteContainer.GetComponent<Image>().color = tempColor;

        tempColor = slots[curr].itemSpriteContainer.GetComponent<Image>().color;
        tempColor.a = 1f;
        slots[curr].itemSpriteContainer.GetComponent<Image>().color = tempColor;

        if(slots[curr].type == "weapon") {
            player.ChangeWeapon(slots[curr].item);
        }
    }

    public void UpdateInventory() {
        for (int i = 0; i < inventory.inventoryWidth; i++) {
            //iterate through hotbar slots, update activeHotbar
            Slot currSlot = inventory.slots[i][inventory.inventoryHeight - 1];
            slots[i].item = currSlot.item;
            slots[i].type = currSlot.type;
            slots[i].itemSpriteContainer.GetComponent<Image>().sprite = currSlot.itemSpriteContainer.GetComponent<Image>().sprite;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public GameObject slotPrefab;
    public GameObject mainInventory;
    public GameObject hotbar;
    public GameObject bg;
    public GameObject debugSlot;
    
    Camera cam;
    GameManager gameManager;
    public int inventoryWidth = 4;
    public int inventoryHeight = 3;
    Slot activeDrag;
    bool dragging = false;
    public List<List<Slot>> slots = new List<List<Slot>>();
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.inst;
        cam = Camera.main;

        for (int i = 0; i < inventoryWidth; i++) {
            slots.Add(new List<Slot>());
            for (int j = 0; j < inventoryHeight; j++) {
                slots[i].Add(null);
            }
        }

        for (int i = 0; i < inventoryWidth; i++) {
            //generate main inventory slots
            for (int j = 0; j < inventoryHeight - 1; j++) {
                Vector2 spawnLocation = new Vector3((bg.GetComponent<RectTransform>().rect.width / (2 * inventoryWidth + 5f)) * (i - 1.5f), (bg.GetComponent<RectTransform>().rect.height / (inventoryHeight + 4)) * (2.5f + 0.75f * (j - 1)), 0);
                GameObject newSlot = GameObject.Instantiate(slotPrefab, spawnLocation, Quaternion.identity);
                newSlot.transform.SetParent(mainInventory.transform);
                newSlot.GetComponent<Slot>().coords = new int[]{i, j};
                newSlot.GetComponent<Slot>().inventory = this;
                slots[i][j] = newSlot.GetComponent<Slot>();
            }
        }

        for (int i = 0; i < inventoryWidth; i++) {
            //generate hotbar slots
            for (int j = 0; j < 1; j++) {
                Vector2 spawnLocation = new Vector3((bg.GetComponent<RectTransform>().rect.width / (2 * inventoryWidth + 5f)) * (i - 1.5f), (bg.GetComponent<RectTransform>().rect.height / (inventoryHeight + 4)) * (2.5f + 0.75f * (j - 2.05f)), 0);
                GameObject newSlot = GameObject.Instantiate(slotPrefab, spawnLocation, Quaternion.identity);
                newSlot.transform.SetParent(hotbar.transform);
                newSlot.GetComponent<Slot>().coords = new int[]{i, j};
                newSlot.GetComponent<Slot>().inventory = this;
                slots[i][inventoryHeight - 1] = newSlot.GetComponent<Slot>();
            }
        }
        
        AddItem("tomato");
    }

    // Update is called once per frame
    void Update()
    {
        if (!dragging && Input.GetMouseButtonDown(0)) {
            BeginDrag();
        } else if (dragging && Input.GetMouseButtonUp(0)) {
            EndDrag(false);
        }

    }

    void AddItem(string itemName) {
        for (int i = 0; i < inventoryWidth; i++) {
            if (slots[i][inventoryHeight - 1].item == "blank"){
                slots[i][inventoryHeight - 1].ChangeItem(itemName);
                return;
            }
        }
    }

    public void BeginDrag() {
        Vector2 hoverCoords = new Vector2(cam.ScreenToWorldPoint(Input.mousePosition).x, cam.ScreenToWorldPoint(Input.mousePosition).y);
        Slot hoverSlot = SlotFromCoords(hoverCoords);
        if (hoverSlot != null) {
            dragging = true;
            activeDrag = hoverSlot;
            gameManager.ChangeReticle(activeDrag.item);
        }
    }

    public void EndDrag(bool term) {
        dragging = false;
        if (!term) {
            Vector2 hoverCoords = new Vector2(cam.ScreenToWorldPoint(Input.mousePosition).x, cam.ScreenToWorldPoint(Input.mousePosition).y);
            Slot hoverSlot = SlotFromCoords(hoverCoords);
            if (hoverSlot != null) {
                string tempHoverSlotItem = hoverSlot.item;
                hoverSlot.ChangeItem(activeDrag.item);
                activeDrag.ChangeItem(tempHoverSlotItem);
            }
        }
        activeDrag = null;
        Sprite nullSprite = null;
        gameManager.ChangeReticle(nullSprite);
    }

    Slot SlotFromCoords(Vector2 point) {
        foreach (List<Slot> slotList in slots) {
            foreach (Slot slot in slotList) {
                if (InsideRectTransform(point, slot)) {
                    return slot;
                }
            }
        }
        return null;
    }
    
    public bool InsideRectTransform(Vector2 point, Slot slot, float errorMargin = 0) {
        //checks if a point falls within the bounds of a rectransform
        if (point.x > slot.gameObject.transform.position.x - slot.gameObject.GetComponent<RectTransform>().rect.width / 2 + errorMargin) {
            if ((point.x < slot.gameObject.transform.position.x + slot.gameObject.GetComponent<RectTransform>().rect.width / 2 - errorMargin)) {
                if ((point.y > slot.gameObject.transform.position.y - slot.gameObject.GetComponent<RectTransform>().rect.height / 2 + errorMargin)) {
                    if ((point.y < slot.gameObject.transform.position.y + slot.gameObject.GetComponent<RectTransform>().rect.height / 2 - errorMargin)) {
                        return true;
                    }
                }
            }
        }
        return false;
    }

}

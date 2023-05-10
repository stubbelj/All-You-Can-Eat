using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    public bool GUIVisible = true;
    public List<List<Slot>> slots = new List<List<Slot>>();
    // Start is called before the first frame update
    Dictionary<(string, string), string> craftingDict = new Dictionary<(string, string), string>{
        {("tomato", "tomato"), "tomatoClub"}
    };
    void Awake()
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
                float spawnLocationX = i * ((bg.GetComponent<RectTransform>().rect.width / (inventoryWidth + 2f)) + (bg.GetComponent<RectTransform>().rect.width / 16)) - (bg.GetComponent<RectTransform>().rect.width / 2) + (bg.GetComponent<RectTransform>().rect.width / 6);                      
                float spawnLocationY = -j * ((bg.GetComponent<RectTransform>().rect.height / (inventoryWidth + 2f)) + (bg.GetComponent<RectTransform>().rect.height / 8)) + (bg.GetComponent<RectTransform>().rect.height / 2) - (bg.GetComponent<RectTransform>().rect.height / 6);
                Vector3 spawnLocation = new Vector3(spawnLocationX, spawnLocationY, 0);
                GameObject newSlot = GameObject.Instantiate(slotPrefab, spawnLocation, Quaternion.identity);
                newSlot.transform.SetParent(mainInventory.transform, false);
                newSlot.GetComponent<Slot>().coords = new int[]{i, j};
                newSlot.GetComponent<Slot>().inventory = this;
                slots[i][j] = newSlot.GetComponent<Slot>();
            }
        }

        for (int i = 0; i < inventoryWidth; i++) {
            //generate hotbar slots
            for (int j = 0; j < 1; j++) {
                float spawnLocationX = i * ((bg.GetComponent<RectTransform>().rect.width / (inventoryWidth + 2f)) + (bg.GetComponent<RectTransform>().rect.width / 16)) - (bg.GetComponent<RectTransform>().rect.width / 2) + (bg.GetComponent<RectTransform>().rect.width / 6);                      
                float spawnLocationY = -j * ((bg.GetComponent<RectTransform>().rect.height / (inventoryWidth + 2f)) + (bg.GetComponent<RectTransform>().rect.height / 8)) + (bg.GetComponent<RectTransform>().rect.height / 2) - (bg.GetComponent<RectTransform>().rect.height / 2);
                Vector3 spawnLocation = new Vector3(spawnLocationX, spawnLocationY, 0);
                GameObject newSlot = GameObject.Instantiate(slotPrefab, spawnLocation, Quaternion.identity);
                newSlot.transform.SetParent(hotbar.transform, false);
                newSlot.GetComponent<Slot>().coords = new int[]{i, j};
                newSlot.GetComponent<Slot>().inventory = this;
                slots[i][inventoryHeight - 1] = newSlot.GetComponent<Slot>();
            }
        }
        gameManager.player.activeHotbar.GetComponent<ActiveHotbar>().UpdateInventory();
        gameManager.player.currItem = slots[0][inventoryHeight - 1].item;
        SetGUI(false);
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

    public void AddItem(string itemName) {
        for (int i = 0; i < inventoryWidth; i++) {
            if (slots[i][inventoryHeight - 1].item.itemName == "blank"){
                slots[i][inventoryHeight - 1].ChangeItem(itemName);
                gameManager.player.activeHotbar.GetComponent<ActiveHotbar>().UpdateInventory();
                return;
            }
        }
        for (int j = 0; j < inventoryHeight - 1; j++) {
            for (int i = 0; i < inventoryWidth; i++) {
                if (slots[i][j].item.itemName == "blank"){
                    slots[i][j].ChangeItem(itemName);
                    gameManager.player.activeHotbar.GetComponent<ActiveHotbar>().UpdateInventory();
                    return;
                }
            }
        }
    }

    public void BeginDrag() {
        Vector2 hoverCoords = new Vector2(cam.ScreenToWorldPoint(Input.mousePosition).x, cam.ScreenToWorldPoint(Input.mousePosition).y);
        Slot hoverSlot = SlotFromCoords(hoverCoords);
        if (hoverSlot != null && hoverSlot.draggable) {
            dragging = true;
            activeDrag = hoverSlot;
            gameManager.ChangeReticle(activeDrag.item.itemName);
        }
    }

    public void EndDrag(bool dragOverride) {
        dragging = false;
        if (!dragOverride) {
            Vector2 hoverCoords = new Vector2(cam.ScreenToWorldPoint(Input.mousePosition).x, cam.ScreenToWorldPoint(Input.mousePosition).y);
            //print(slots[0][1].transform.position);
            Slot hoverSlot = SlotFromCoords(hoverCoords);
            if (hoverSlot != null && hoverSlot.draggable) {
                if (hoverSlot.item.itemName == "blank") {
                    hoverSlot.ChangeItem(activeDrag.item);
                    activeDrag.ChangeItem("blank");
                } else {
                    string tempHoverSlotItemName = hoverSlot.item.itemName;
                    hoverSlot.ChangeItem(activeDrag.item);
                    activeDrag.ChangeItem(tempHoverSlotItemName);
                }
            }
        }
        activeDrag = null;
        Sprite nullSprite = null;
        gameManager.ChangeReticle(nullSprite);
    }

    public void SetGUI(bool isVisible) {
        if (isVisible) {
            GUIVisible = true;
            GetComponent<Image>().enabled = true;
            transform.Find("BG").gameObject.GetComponent<Image>().enabled = true;
            foreach (Transform tran in transform.Find("MainInventory")) {
                tran.gameObject.GetComponent<Image>().enabled = true;
                tran.Find("Item").gameObject.GetComponent<Image>().enabled = true;
            }
            transform.Find("Hotbar").GetComponent<Image>().enabled = true;
            foreach (Transform tran in transform.Find("Hotbar")) {
                tran.gameObject.GetComponent<Image>().enabled = true;
                tran.Find("Item").gameObject.GetComponent<Image>().enabled = true;
            }
        } else {
            GUIVisible = false;
            GetComponent<Image>().enabled = false;
            transform.Find("BG").gameObject.GetComponent<Image>().enabled = false;
            foreach (Transform tran in transform.Find("MainInventory")) {
                tran.gameObject.GetComponent<Image>().enabled = false;
                tran.Find("Item").gameObject.GetComponent<Image>().enabled = false;
            }
            transform.Find("Hotbar").GetComponent<Image>().enabled = false;
            foreach (Transform tran in transform.Find("Hotbar")) {
                tran.gameObject.GetComponent<Image>().enabled = false;
                tran.Find("Item").gameObject.GetComponent<Image>().enabled = false;
            }
        }
        
    }

    Slot SlotFromCoords(Vector2 point) {
        foreach (List<Slot> slotList in slots) {
            foreach (Slot slot in slotList) {
                if (InsideRectTransform(point, slot)) {
                    //print(slot.coords[0] + ", " + slot.coords[1]);
                    return slot;
                }
            }
        }
        return null;
    }
    
    public bool InsideRectTransform(Vector2 point, Slot slot, float errorMargin = 0) {
        //checks if a point falls within the bounds of a rectransform
        float slotWidth = 30f;
        float slotHeight = 30f;
        //why is this correct? the rect transform width is 100 so it makes no sense


        if (point.x > slot.gameObject.transform.position.x - slotWidth / 2 + errorMargin) {
            if ((point.x < slot.gameObject.transform.position.x + slotWidth / 2 - errorMargin)) {
                if ((point.y > slot.gameObject.transform.position.y - slotHeight / 2 + errorMargin)) {
                    if ((point.y < slot.gameObject.transform.position.y + slotHeight / 2 - errorMargin)) {
                        return true;
                    }
                }
            }
        }
        return false;
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : RoomObject
{
    GameManager gameManager;
    GameObject interactPopup;
    float interactRange = 200f;
    bool activateable = false;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.inst;
        interactPopup = transform.Find("InteractPopup").gameObject;
    }

    void Update() {
        if (Mathf.Abs((gameManager.player.transform.position - transform.position).magnitude) <= interactRange) {
            interactPopup.SetActive(true);
            activateable = true;
        } else {
            interactPopup.SetActive(false);
            activateable = false;
        }

        if (Input.GetKey("e") && activateable) {
            Activate();
        }
    }

    public abstract void Activate();
}

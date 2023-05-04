using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : RoomObject
{
    public GameManager gameManager;
    public GameObject interactPopup;
    float interactRange = 200f;
    bool activateable = false;
    bool activated = false;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.inst;
        interactPopup = transform.Find("InteractPopup").gameObject;
        AbstractStart();
    }

    void Update() {
        if (Mathf.Abs((gameManager.player.transform.position - transform.position).magnitude) <= interactRange) {
            interactPopup.SetActive(true);
            activateable = true;
        } else {
            interactPopup.SetActive(false);
            activateable = false;
            if (activated == true) {
                Deactivate();
            }
        }

        if (Input.GetKey("e") && activateable) {
            Activate();
            activated = true;
        }
    }

    public abstract void Activate();

    public abstract void Deactivate();

    public abstract void AbstractStart();
}

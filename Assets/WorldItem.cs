using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldItem : MonoBehaviour
{
    public string itemName;

    void OnCollisionEnter2D(Collision2D col) {
        if (col.gameObject.tag == "Player") {
            col.gameObject.GetComponent<Player>().inventory.AddItem(itemName);
            Destroy(gameObject);
        }
    }
}

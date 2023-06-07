using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldItem : MonoBehaviour
{
    public string itemName;
    float timer = 1f;

    void Update() {
        if (timer > 0) {
            timer -= Time.deltaTime;
        }
    }

    void OnCollisionEnter2D(Collision2D col) {
        if (col.gameObject.tag == "Player" && timer > 0) {
            col.gameObject.GetComponent<Player>().inventory.AddItem(itemName);
            Destroy(gameObject);
        }
    }
}

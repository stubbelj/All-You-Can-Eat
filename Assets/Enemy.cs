using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{   
    GameManager gameManager;
    Player player;

    int contactDamage = 1;
    float moveSpeed = 1f;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.inst;
        player = gameManager.player;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 playerVec = (player.gameObject.transform.position - transform.position).normalized;
        //create unit vector pointing towards player
        transform.position += playerVec * moveSpeed * Time.deltaTime;
    }

    void OnCollisionEnter2D (Collision2D col) {
        //handle collisions
        if (col.gameObject.tag == "Player") {
            player.TakeDamage(contactDamage);
        }
    }
}

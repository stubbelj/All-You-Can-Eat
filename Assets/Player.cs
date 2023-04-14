using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Player : MonoBehaviour
{
    public TMP_Text healthText;

    float moveSpeed = 2f;
    int health = 5;

    // Start is called before the first frame update
    void Start()
    {
        healthText.text = health.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey("w")) {
            //if the player is currently pressing down 'w'
            transform.position += Vector3.up * moveSpeed * Time.deltaTime;
            // add to position a vector (0, 1, 0) * moveSpeed variable * time passed since last frame update
            // Time.deltaTime is important for things happening in Update() because frames are inconsistently spaced apart
        }
        if (Input.GetKey("a")) {
            transform.position += Vector3.left * moveSpeed * Time.deltaTime;
        }
        if (Input.GetKey("s")) {
            transform.position += Vector3.down * moveSpeed * Time.deltaTime;
        }
        if (Input.GetKey("d")) {
            transform.position += Vector3.right * moveSpeed * Time.deltaTime;
        }
    }

    void OnCollisionEnter2D (Collision2D col) {
        //handle collisions
        //calls to TakeDamage should happen on the damage source's script so that information can be easily passed about the type of damage
    }

    public void TakeDamage(int damage) {
        //handle damage
        health -= damage;
        if (health <= 0) {
            Die();
        }
        healthText.text = health.ToString();
    }

    void Die() {
        print("You died!");
    }
}

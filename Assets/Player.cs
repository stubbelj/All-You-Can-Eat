using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Player : MonoBehaviour
{
    public TMP_Text healthText;
    public Weapon currWeapon;

    float moveSpeed = 1000f;
    float maxSpeed = 150f;
    int health = 5;

    public Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        healthText.text = health.ToString();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey("w")) {
            //if the player is currently pressing down 'w'
            rb.velocity += Vector2.up * moveSpeed * Time.deltaTime;
            // add to position a vector (0, 1, 0) * moveSpeed variable * time passed since last frame update
            // Time.deltaTime is important for things happening in Update() because frames are inconsistently spaced apart
        }
        if (Input.GetKey("a")) {
            rb.velocity += Vector2.left * moveSpeed * Time.deltaTime;
        }
        if (Input.GetKey("s")) {
            rb.velocity += Vector2.down * moveSpeed * Time.deltaTime;
        }
        if (Input.GetKey("d")) {
            rb.velocity += Vector2.right * moveSpeed * Time.deltaTime;
        }

        rb.velocity = Vector2.ClampMagnitude(rb.velocity, maxSpeed);

        if(Input.GetKey("space")) {
            StartCoroutine(currWeapon.Attack());
        }
    }

    void OnCollisionEnter2D (Collision2D col) {
        //handle collisions
        //calls to TakeDamage should happen on the damage source's script so that information can be easily passed about the damage
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
        //dead
    }
}

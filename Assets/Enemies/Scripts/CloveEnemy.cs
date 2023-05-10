using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloveEnemy : Enemy
{
    int contactDamage = 1;
    float moveSpeed = 1000f;
    //having a variable like moveSpeed is useful for tweaking enemy behaviour!
    float maxSpeed = 100f;
    int health = 1;

    void Start()
    {
        gameManager = GameManager.inst;
        player = gameManager.player;
        anim = gameObject.GetComponent<Animator>();
        rb = gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 dirVec = player.gameObject.transform.position - transform.position;
        if (dirVec.magnitude > 5f) {
            rb.velocity += dirVec.normalized * moveSpeed * Time.deltaTime;
            rb.velocity = Vector2.ClampMagnitude(rb.velocity, maxSpeed);
        } else {
            rb.velocity = Vector2.zero;
        }
    }

    void OnCollisionEnter2D (Collision2D col) {
        if (col.gameObject.tag == "Player") {
            player.TakeDamage(contactDamage);
        }
    }

    public override void TakeDamage(int delta) {
        health -= delta;
        if (health <= 0) {
            Destroy(gameObject);
        }
    }
}

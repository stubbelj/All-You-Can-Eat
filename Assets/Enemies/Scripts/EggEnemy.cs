using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EggEnemy : Enemy
{
    public GameObject yolkPrefab;

    int contactDamage = 1;
    float moveSpeed = 1000f;
    float maxSpeed = 70f;
    int health = 5;
    bool jumping = false;
    Vector3 targetPos;

    void Start()
    {
        gameManager = GameManager.inst;
        player = gameManager.player;
        anim = gameObject.GetComponent<Animator>();
        rb = gameObject.GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (!jumping) {
            Vector2 dirVec = player.gameObject.transform.position - transform.position;
            if (dirVec.magnitude > 150f) {
                rb.velocity += dirVec.normalized * moveSpeed * Time.deltaTime;
                rb.velocity = Vector2.ClampMagnitude(rb.velocity, maxSpeed);
            } else {
                targetPos = player.gameObject.transform.position;
                jumping = true;
            }
        } else {
                Vector2 dirVec = targetPos - transform.position;
                rb.velocity += dirVec.normalized * moveSpeed * 5 * Time.deltaTime;
                rb.velocity = Vector2.ClampMagnitude(rb.velocity, maxSpeed * 5);
                if (Mathf.Abs((targetPos - transform.position).magnitude) < 5) {
                    Die();
                }
        }
    }

    void Die() {
        GameObject.Instantiate(yolkPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    void OnCollisionEnter2D (Collision2D col) {
        if (col.gameObject.tag == "Player") {
            player.TakeDamage(contactDamage);
            Die();
        }
    }

    public override void TakeDamage(int delta) {
        health -= delta;
        if (health <= 0) {
            Die();
        }
    }
}

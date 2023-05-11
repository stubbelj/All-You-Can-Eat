using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YolkEnemy : Enemy
{
    public GameObject eggWorldItem;

    int contactDamage = 1;
    float moveSpeed = 100f;
    float maxSpeed = 10f;
    int health = 1;

    // Start is called before the first frame update
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
            if (dirVec.x > 0) {
                GetComponent<SpriteRenderer>().flipX = false;
            } else {
                GetComponent<SpriteRenderer>().flipX = true;
            }
            rb.velocity += dirVec.normalized * moveSpeed * Time.deltaTime;
            rb.velocity = Vector2.ClampMagnitude(rb.velocity, maxSpeed);
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
            GameObject.Instantiate(eggWorldItem, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}

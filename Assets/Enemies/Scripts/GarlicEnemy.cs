using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GarlicEnemy : Enemy
{
    public List<Sprite> peaSprites = new List<Sprite>();
    public List<GameObject> clove = new List<GameObject>();
    public GameObject ClovePrefab;

    float moveSpeed = 100f;
    float maxSpeed = 50f;
    int health = 3;

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
        //create vector pointing towards player
        if (dirVec.magnitude > 5f) {
            if (dirVec.x > 0) {
                GetComponent<SpriteRenderer>().flipX = false;
            } else {
                GetComponent<SpriteRenderer>().flipX = true;
            }
            //do not move if you are pretty much on top of player to avoid jitter
            rb.velocity += dirVec.normalized * moveSpeed * Time.deltaTime;
            //Time.deltaTime is important because frames (NOT the same as render frames, like what you mean when you say "I'm running this at 60fps") happen inconsistently
            //Time.deltaTime is the amount of time since the last frame was processed. use this anytime you need something to happen smoothly, like movement
            //also common to use this along with Lerping(Linear Interpolation)
            //dirVec is normalized so that direction is preserved, but movement speed does not change based on distance to the player
            rb.velocity = Vector2.ClampMagnitude(rb.velocity, maxSpeed);
        }
    }


    void SpawnClove() {
        for (int i = 0; i < 6; i++)
        {
        clove.Add(GameObject.Instantiate(ClovePrefab, transform.position, Quaternion.identity));
        //Adds cloves into clove list 6 times
    }
        Destroy(gameObject);
    }
    
    public override void TakeDamage(int delta) {
        //reduce the health of this enemy
        health -= delta;
        if (health <= 0) {
            SpawnClove();
        }
    }
}

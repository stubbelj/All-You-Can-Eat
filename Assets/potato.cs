using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class potato : Enemy
{
    public GameObject potatoWorldItem;

    int contactDamage = 1;
    float moveSpeed = 1000f;
    //having a variable like moveSpeed is useful for tweaking enemy behaviour!
    float maxSpeed = 100f;
    int health = 6;
    //squishy lil guy

    // Start is called before the first frame update
    void Start()
    {
        //Start() is a good place to grab variables, references, etc. that can't be assigned in variable declaration. For example, player cannot be assigned until Start() because gameManager isn't
        //assigned yet, because assigning gameManager has to happen after gameManager.inst is created, which happens inside the Start() function of GameManager.cs
        gameManager = GameManager.inst;
        //there is always ONLY ONE GameManger object, which stores important values for other scripts. in cases like this it is called a "singleton" and GameManager.inst is a reference to that
        //object. this is great because having only ONE object means that all scripts will have the same value for something like gameManager.currentFloor, whereas if you made a new GameManager
        //for each script you would have to update every script manually every time you changed the value of currentFloor.
        player = gameManager.player;
        //for example, here the gameManager has a reference to the player!
        anim = gameObject.GetComponent<Animator>();
        //syntax to get the Animator component attached to this game object
        rb = gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        //Update() is where all the 'behaviour' of a script goes - once this gets really dense, it helps to turn it into a big list of if statements and function calls like:
        //ChasePlayer()
        //if (minionCount < 3) { SpawnMinions(); }
        //if (state == "buff") { BuffMinions(); }

        Vector2 dirVec = player.gameObject.transform.position - transform.position;

        if (Mathf.Abs(dirVec.magnitude) < 100)
        {
            if (dirVec.magnitude > 5f) 
            {
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
                //ChangeAnimationState("tempTomatoWalk");
                //play the walking animation if nothing else is playing
            }
        }


        //create vector pointing towards player
        
    }

    void OnCollisionEnter2D (Collision2D col) {
        //gets called every time a Collider2D component on another gameObject runs into the Collider2D on this object.
        //Collision2D col stores several things, most importantly the game object that the other collider is attached to
        if (col.GetContact(0).collider.gameObject.tag == "Player") {
            //tags are just a string that you can use to label objects, and are mostly so that you can assign editor stuff like "enemies don't collide with enemies"
            player.TakeDamage(contactDamage);
            //when this enemy collides with the player, deal contact damage to them!
        }
    }

    public override void TakeDamage(int delta) {
        //reduce the health of this enemy
        health -= delta;
        if (health <= 0) {
            GameObject.Instantiate(potatoWorldItem, transform.position, Quaternion.identity);
            Destroy(gameObject);
            //destroys the gameObject this script is attached to, which will destroy the script as well
        }
    }


    // change potato sprite from buried to walking if player detected

}

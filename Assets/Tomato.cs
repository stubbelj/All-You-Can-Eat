using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tomato : MonoBehaviour
{   
    //MonoBehaviour is a class you need to inherit from in order to use a lot of unity game object stuff. for example, print(gameObject) will print the object this script is attached
    //to because MonoBehaviour.gameObject stores that reference for you
    GameManager gameManager;
    //this is the GameManager class I made to store globally useful or game-function related variables and functions, like a reference to the player or functions to initialize the level
    Player player;
    //this is the player - we will get the reference for it from the GameManager in Start()

    int contactDamage = 1;
    float moveSpeed = 1f;
    //having a variable like moveSpeed is useful for tweaking enemy behaviour!

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
    }

    // Update is called once per frame
    void Update()
    {
        //Update() is where all the 'behaviour' of a script goes - once this gets really dense, it helps to turn it into a big list of if statements and function calls like:
        //ChasePlayer()
        //if (minionCount < 3) { SpawnMinions(); }
        //if (state == "buff") { BuffMinions(); }

        Vector3 dirVec = (player.gameObject.transform.position - transform.position).normalized;
        //create unit vector (vector of magnitude 1) pointing towards player
        transform.position += dirVec * moveSpeed * Time.deltaTime;
        //Time.deltaTime is important because frames (NOT the same as render frames, like what you mean when you say "I'm running this at 60fps") happen inconsistently
        //Time.deltaTime is the amount of time since the last frame was processed. use this anytime you need something to happen smoothly, like movement
        //also common to use this along with Lerping(Linear Interpolation)
    }

    void OnCollisionEnter2D (Collision2D col) {
        //gets called every time a Collider2D component on another gameObject runs into the Collider2D on this object.
        //Collision2D col stores several things, most importantly the game object that the other collider is attached to
        if (col.gameObject.tag == "Player") {
            //tags are just a string that you can use to label objects, and are mostly so that you can assign editor stuff like "enemies don't collide with enemies"
            player.TakeDamage(contactDamage);
            //when this enemy collides with the player, deal contact damage to them!
        }
    }
}

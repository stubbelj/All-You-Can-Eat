using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    public GameManager gameManager;
    //this is the GameManager class I made to store globally useful or game-function related variables and functions, like a reference to the player or functions to initialize the level
    public Player player;
    //this is the player - we will get the reference for it from the GameManager in Start()
    public Rigidbody2D rb;
    //the rigidbody component, which is a component used for physics operations like collision, velocity or mass
    public Animator anim;
    //this is an animator component on this object, which runs animation
    string currAnimState;
    //holds the current animation state
    

    public bool AnimatorIsPlaying() {
        //check if animation is playing
        return anim.GetCurrentAnimatorStateInfo(0).length > anim.GetCurrentAnimatorStateInfo(0).normalizedTime;
    }

    public bool AnimatorIsPlaying(string stateName) {
        //check if a specific animation is playing
        return AnimatorIsPlaying() && anim.GetCurrentAnimatorStateInfo(0).IsName(stateName);
        //checks if an anim is even playing, then gets the name of the anim and checks if it is the same as stateName
    }

    public void ChangeAnimationState(string newState) {
        //play an animation!
        if (currAnimState == newState) return;
        //if the current animation is already playing, don't interrupt it and start the animation over.
        //if you want to cleanly loop an animation, you can set it as a loopable animation in the animation controller.
        anim.Play(newState);
        //tell the animation controller to play the animation
        currAnimState = newState;
        //keep track of currently playing animation
    }
    
    public abstract void TakeDamage(int delta);
    /*abstract functions are ones that are INTENDED to be overridden - all enemies define a public override void TakeDamage(int delta)
    this is so that you can call GetComponent<Enemy>().TakeDamage() and have it call the enemy subclass, ex. tomato or potato
     it's kind of like if you did:
    TakeDamage(int delta) {
        //subClass.TakeDamage(delta);
    }
    if you're familiar with method overriding in other languages, this is the same except c# requires the abstract/override keywords
    */

}

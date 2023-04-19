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
        //makes use of properties of the Animator component
    }

    public bool AnimatorIsPlaying(string stateName) {
        //check if a specific animation is playing
        return AnimatorIsPlaying() && anim.GetCurrentAnimatorStateInfo(0).IsName(stateName);
        //checks if an anim is even playing, then gets the name of the anim and checks with the argument
    }

    public void ChangeAnimationState(string newState) {
        //play an animation!
        if (currAnimState == newState) return;
        anim.Play(newState);
        currAnimState = newState;
    }
    
    public abstract void TakeDamage(int delta);
}

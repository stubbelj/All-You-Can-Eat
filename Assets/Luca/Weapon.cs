using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    //this is an abstract parent class that exists so calls can be made to weapons that inherit it for stuff like Attack()

    public float attackCooldown;
    public SpriteRenderer sr;
    public bool attacking;
    public Animator anim;
    public string currAnimState;
    public int weaponDamage = 1;

    public bool AnimatorIsPlaying() {
        return anim.GetCurrentAnimatorStateInfo(0).length > anim.GetCurrentAnimatorStateInfo(0).normalizedTime;
    }

    public bool AnimatorIsPlaying(string stateName) {
        return AnimatorIsPlaying() && anim.GetCurrentAnimatorStateInfo(0).IsName(stateName);
    }

    public void ChangeAnimationState(string newState) {
        if (currAnimState == newState) return;
        anim.Play(newState);
        currAnimState = newState;
    }

    public abstract IEnumerator Attack();
}

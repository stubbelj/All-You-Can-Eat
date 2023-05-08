using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : Item
{
    //this is an abstract parent class that exists so calls can be made to weapons that inherit it for stuff like Attack()
    public bool attacking;

    protected float attackCooldown;
    protected SpriteRenderer sr;
    protected string currAnimState;
    protected int weaponDamage = 1;
    protected GameManager gameManager;
    protected Animator anim;

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

    public override void Activate() {
        StartCoroutine(Attack());
    }

    public abstract IEnumerator Attack();
}

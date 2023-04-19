using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TomatoClub : Weapon
{
    public Collider2D[] colliders = new Collider2D[3];
    Animator anim;
    string currAnimState;
    int weaponDamage = 1;
    
    void Start() {
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
    }

    void Update() {
        if (attackCooldown > 0) {attackCooldown -= Time.deltaTime; }
        if (attackCooldown < 0) {attackCooldown = 0; }
    }

    public override IEnumerator Attack() {
        if (attackCooldown == 0) {
            attacking = true;
            attackCooldown = 0.5f;
            ChangeAnimationState("tempTomatoClubAttack");
            //colliders[0].enabled = true;
            yield return new WaitForSeconds(0.1f);
            //colliders[0].enabled = false;
            //doesn't feel right to deal damage instantly at the beginning of the animation, so the hitboxes are disabled for the first animation frame
            colliders[1].enabled = true;
            yield return new WaitForSeconds(0.1f);
            colliders[1].enabled = false;
            colliders[2].enabled = true;
            yield return new WaitForSeconds(0.1f);
            colliders[2].enabled = false;
            ChangeAnimationState("tempTomatoClubIdle");
            attacking = false;
        }
        yield return null;
    }

    void OnCollisionEnter2D (Collision2D col) {
        if (col.gameObject.tag == "Enemy") {
            col.gameObject.GetComponent<Enemy>().TakeDamage(weaponDamage);
        }
    }

    bool AnimatorIsPlaying() {
        return anim.GetCurrentAnimatorStateInfo(0).length > anim.GetCurrentAnimatorStateInfo(0).normalizedTime;
    }

    bool AnimatorIsPlaying(string stateName) {
        return AnimatorIsPlaying() && anim.GetCurrentAnimatorStateInfo(0).IsName(stateName);
    }

    void ChangeAnimationState(string newState) {
        if (currAnimState == newState) return;
        anim.Play(newState);
        currAnimState = newState;
    }
}

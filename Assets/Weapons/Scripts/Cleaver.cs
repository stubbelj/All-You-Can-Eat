using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cleaver : Weapon
{

    public Collider2D[] colliders = new Collider2D[3];
    
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
            anim.speed = attackSpeedMod;
            attackCooldown = 0.5f * attackSpeedMod;
            ChangeAnimationState("cleaverAttack");
            //colliders[0].enabled = true;
            yield return new WaitForSeconds(0.1f * attackSpeedMod);
            //colliders[0].enabled = false;
            //doesn't feel right to deal damage instantly at the beginning of the animation, so the hitboxes are disabled for the first animation frame
            colliders[1].enabled = true;
            yield return new WaitForSeconds(0.1f * attackSpeedMod);
            colliders[1].enabled = false;
            colliders[2].enabled = true;
            yield return new WaitForSeconds(0.1f * attackSpeedMod);
            colliders[2].enabled = false;
            ChangeAnimationState("cleaverIdle");
            attacking = false;
        }
        yield return null;
    }

    void OnCollisionEnter2D (Collision2D col) {
        if (col.gameObject.tag == "Enemy") {
            col.gameObject.GetComponent<Enemy>().TakeDamage(weaponDamage + attackDamageMod);
        }
    }
}

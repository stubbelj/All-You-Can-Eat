using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlingPeas : Weapon
{
    public GameObject peaProjectilePrefab;
    int projectileSpeed = 100;
    public Camera cam;
    // Start is called before the first frame update
    void Start() {
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update() {
        if (attackCooldown > 0) {attackCooldown -= Time.deltaTime; }
        if (attackCooldown < 0) {attackCooldown = 0; }
    }

    public override IEnumerator Attack() {
        if (attackCooldown == 0) {
            attacking = true;
            attackCooldown = 0.5f;
            ChangeAnimationState("slingPeasAttack");
            yield return new WaitForSeconds(0.2f);
            GameObject newPeaProjectile = GameObject.Instantiate(peaProjectilePrefab, transform.position, Quaternion.identity);
            newPeaProjectile.GetComponent<Rigidbody2D>().velocity = (new Vector2(cam.ScreenToWorldPoint(Input.mousePosition).x, cam.ScreenToWorldPoint(Input.mousePosition).y) - (Vector2)transform.position).normalized * projectileSpeed;
            ChangeAnimationState("slingPeasIdle");
            attacking = false;
        }
        yield return null;
    }
}

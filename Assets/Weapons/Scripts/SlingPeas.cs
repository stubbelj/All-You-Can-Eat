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
        gameManager = GameManager.inst;
    }

    // Update is called once per frame
    void Update() {
        if (attackCooldown > 0) {attackCooldown -= Time.deltaTime; }
        if (attackCooldown < 0) {attackCooldown = 0; }
    }

    public override IEnumerator Attack() {
        if (attackCooldown == 0) {
            attacking = true;
            Vector3 pos = gameManager.player.transform.position;
            if (pos.y > pos.x && pos.y > -pos.x) {
                //make the player flip to the direction they're shooting
                gameManager.player.ChangeOrientation("up");
            } else if (pos.y < pos.x && pos.y < -pos.x) {
                //make the player flip to the direction they're shooting
                gameManager.player.ChangeOrientation("down");
            } else if(pos.y < pos.x && pos.y > -pos.x) {
                //make the player flip to the direction they're shooting
                gameManager.player.ChangeOrientation("left");
            } else if (pos.y > pos.x && pos.y < -pos.x) {
                //make the player flip to the direction they're shooting
                gameManager.player.ChangeOrientation("right");
            }
            attackCooldown = 0.5f;
            ChangeAnimationState("slingPeasAttack");
            yield return new WaitForSeconds(0.2f);
            GameObject newPeaProjectile = GameObject.Instantiate(peaProjectilePrefab, transform.position, Quaternion.identity);
            newPeaProjectile.GetComponent<Rigidbody2D>().velocity = (new Vector2(cam.ScreenToWorldPoint(Input.mousePosition).x, cam.ScreenToWorldPoint(Input.mousePosition).y) - (Vector2)transform.position).normalized * projectileSpeed;
            ChangeAnimationState("slingPeasIdle");
            yield return new WaitForSeconds(0.2f);
            //makes the player NOT rotate their character sprite for a little bit to make the attack anim seem more realistic
            attacking = false;
        }
        yield return null;
    }
}

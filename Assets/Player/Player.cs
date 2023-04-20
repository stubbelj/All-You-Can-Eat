using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Player : MonoBehaviour
{
    public Inventory inventory;
    public TMP_Text healthText;
    public Weapon currWeapon;
    public GameObject inventoryMenu;
    public GameObject activeHotbar;
    
    GameManager gameManager;
    float moveSpeed = 1000f;
    float maxSpeed = 150f;
    int health = 5;
    float pauseDelay = 0.1f;
    SpriteRenderer sr;

    public Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {   
        gameManager = GameManager.inst;
        healthText.text = health.ToString();
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey("w")) {
            //if the player is currently pressing down 'w'
            rb.velocity += Vector2.up * moveSpeed * Time.deltaTime;
            // add to position a vector (0, 1, 0) * moveSpeed variable * time passed since last frame update
            // Time.deltaTime is important for things happening in Update() because frames are inconsistently spaced apart
        }
        if (Input.GetKey("a")) {
            rb.velocity += Vector2.left * moveSpeed * Time.deltaTime;
            if (!sr.flipX && !currWeapon.attacking && pauseDelay == 0) {
                sr.flipX = true;
                currWeapon.transform.position -= new Vector3(2 * Mathf.Abs(currWeapon.transform.localPosition.x), 0, 0);
                currWeapon.sr.flipX = true;
            }
        }
        if (Input.GetKey("s")) {
            rb.velocity += Vector2.down * moveSpeed * Time.deltaTime;
        }
        if (Input.GetKey("d")) {
            rb.velocity += Vector2.right * moveSpeed * Time.deltaTime;
            if (sr.flipX && !currWeapon.attacking && pauseDelay == 0) {
                sr.flipX = false;
                currWeapon.transform.position += new Vector3(2 * Mathf.Abs(currWeapon.transform.localPosition.x), 0, 0);
                currWeapon.sr.flipX = false;
            }
        }

        rb.velocity = Vector2.ClampMagnitude(rb.velocity, maxSpeed);

        if(Input.GetKey("space")) {
            StartCoroutine(currWeapon.Attack());
        }

        if (Input.GetKey("escape") && pauseDelay == 0) {
            pauseDelay = 0.1f;
            if (Time.timeScale == 1) {
                Time.timeScale = 0;
            } else {
                Time.timeScale = 1;
                gameManager.EndDrag(true);
            }
            ToggleInventory();
        }

        pauseDelay -= Time.unscaledDeltaTime;
        pauseDelay = Mathf.Clamp(pauseDelay, 0, 0.1f);

    }

    void OnCollisionEnter2D (Collision2D col) {
        //handle collisions
        //calls to TakeDamage should happen on the damage source's script so that information can be easily passed about the damage
    }

    public void TakeDamage(int damage) {
        //handle damage
        health -= damage;
        if (health <= 0) {
            Die();
        }
        healthText.text = health.ToString();
    }

    void Die() {
        //dead
    }

    void ToggleInventory() {
        if (!inventoryMenu.activeSelf) {
            activeHotbar.SetActive(false);
        } else {
            activeHotbar.SetActive(true);
            activeHotbar.GetComponent<ActiveHotbar>().UpdateInventory();
        }
        inventoryMenu.SetActive(!inventoryMenu.activeSelf);
    }
}

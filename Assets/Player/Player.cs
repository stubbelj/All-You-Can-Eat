using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Player : MonoBehaviour
{
    public Inventory inventory;
    public TMP_Text healthText;
    public Item currItem;
    public Weapon currWeapon;
    public GameObject inventoryMenu;
    public GameObject activeHotbar;
    public Rigidbody2D rb;
    public GameObject weapons;
    
    GameManager gameManager;
    float moveSpeed = 1000f;
    float maxSpeed = 150f;
    int health = 5;
    float pauseDelay = 0.1f;
    SpriteRenderer sr;
    // Start is called before the first frame update
    void Start()
    {   
        gameManager = GameManager.inst;
        healthText.text = health.ToString();
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        weapons = transform.Find("Weapons").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey("w")) {
            //if the player is currently pressing down 'w'
            rb.velocity += Vector2.up * moveSpeed * Time.deltaTime;
            // add to position a vector (0, 1, 0) * moveSpeed variable * time passed since last frame update
            // Time.deltaTime is important for things happening in Update() because frames are inconsistently spaced apart
            if (!currWeapon.attacking && pauseDelay == 0) {
                ChangeOrientation("up");
            }
        }
        if (Input.GetKey("a")) {
            rb.velocity += Vector2.left * moveSpeed * Time.deltaTime;
            if (!currWeapon.attacking && pauseDelay == 0) {
                ChangeOrientation("left");
            }
        }
        if (Input.GetKey("s")) {
            rb.velocity += Vector2.down * moveSpeed * Time.deltaTime;
            if (!currWeapon.attacking && pauseDelay == 0) {
                ChangeOrientation("down");
            }
        }
        if (Input.GetKey("d")) {
            rb.velocity += Vector2.right * moveSpeed * Time.deltaTime;
            if (!currWeapon.attacking && pauseDelay == 0) {
                ChangeOrientation("right");
            }
        }

        rb.velocity = Vector2.ClampMagnitude(rb.velocity, maxSpeed);

        if(Input.GetKey("space")) {
            currItem.Activate();
            //StartCoroutine(currWeapon.Attack());
        }

        if (Input.GetKey("tab")) {
            ToggleInventory();
        }

        if (Input.GetKey("escape") && pauseDelay == 0) {
            EscapePress();
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
    
    public void EscapePress() {
        pauseDelay = 0.1f;
        if (Time.timeScale == 1) {
            Time.timeScale = 0;
        } else {
            Time.timeScale = 1;
            gameManager.EndDrag(true);
        }
    }

    bool inventoryToggleDelay = false;
    
    public void ToggleInventory() {
        if (!inventoryToggleDelay) {
            Delay(ref inventoryToggleDelay);
            if (!inventoryMenu.activeSelf) {
                inventoryMenu.SetActive(true);
                activeHotbar.SetActive(false);
            } else {
                inventoryMenu.SetActive(false);
                activeHotbar.SetActive(true);
                activeHotbar.GetComponent<ActiveHotbar>().UpdateInventory();
                if (gameManager.craftingUI.activeSelf) {
                    gameManager.craftingUI.SetActive(false);
                }
            }
        }
    }

    public void Delay(ref bool flag, float waitTime = 0.1f) {
        IEnumerator DelayItr(bool flag, float itrWaitTime = 0.1f) {
            flag = true;
            yield return new WaitForSeconds(itrWaitTime);
            flag = false;
        }

        if (waitTime != 0.5f) {
            StartCoroutine(DelayItr(flag, itrWaitTime : waitTime));
        } else {
            StartCoroutine(DelayItr(flag));
        }
    }

    

    public void ChangeWeapon(string weaponName) {
        currWeapon.gameObject.SetActive(false);
        currWeapon = weapons.transform.Find(weaponName).gameObject.GetComponent<Weapon>();
        currWeapon.gameObject.SetActive(true);
    }

    public void ChangeOrientation(string dir) {
        //dir is up, down, left, right
        switch (dir) {
            case "up":
                //implement when new sprites get added
                break;
            case "down":
                //implement when new sprites get added
                break;
            case "left":
                if (!sr.flipX) {
                    sr.flipX = true;
                    weapons.transform.localScale = new Vector3(-weapons.transform.localScale.x, weapons.transform.localScale.y, 0);
                }
                break;
            case "right":
                if (sr.flipX) {
                    sr.flipX = false;
                    weapons.transform.localScale = new Vector3(-weapons.transform.localScale.x, weapons.transform.localScale.y, 0);
                }
                break;
        }
    }
}

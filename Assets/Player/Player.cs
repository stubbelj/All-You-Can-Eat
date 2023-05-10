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
    public GameObject heartPrefab;
    public Hearts hearts;
    
    GameManager gameManager;
    float moveSpeed = 3000f;
    float moveSpeedMod = 1;
    float maxSpeed = 300f;
    public int maxHP = 5;
    public int currHP = 5;
    float pauseDelay = 0.1f;
    SpriteRenderer sr;
    // Start is called before the first frame update
    void Start()
    {   
        gameManager = GameManager.inst;
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        weapons = transform.Find("Weapons").gameObject;
        currWeapon = weapons.transform.Find("cleaver").GetComponent<Weapon>();
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKey("w")) {
            //if the player is currently pressing down 'w'
            rb.velocity += Vector2.up * moveSpeed * moveSpeedMod * Time.deltaTime;
            // add to position a vector (0, 1, 0) * moveSpeed variable * time passed since last frame update
            // Time.deltaTime is important for things happening in Update() because frames are inconsistently spaced apart
            if (!currWeapon.attacking && pauseDelay == 0) {
                ChangeOrientation("up");
            }
        }
        if (Input.GetKey("a")) {
            rb.velocity += Vector2.left * moveSpeed * moveSpeedMod * Time.deltaTime;
            if (!currWeapon.attacking && pauseDelay == 0) {
                ChangeOrientation("left");
            }
        }
        if (Input.GetKey("s")) {
            rb.velocity += Vector2.down * moveSpeed * moveSpeedMod * Time.deltaTime;
            if (!currWeapon.attacking && pauseDelay == 0) {
                ChangeOrientation("down");
            }
        }
        if (Input.GetKey("d")) {
            rb.velocity += Vector2.right * moveSpeed * moveSpeedMod * Time.deltaTime;
            if (!currWeapon.attacking && pauseDelay == 0) {
                ChangeOrientation("right");
            }
        }

        rb.velocity = Vector2.ClampMagnitude(rb.velocity, maxSpeed);

        if(Input.GetKey("space")) {
            if (itemActivateable) {
                ItemActivateDelay();
                currItem.Activate();
            }
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
        currHP -= damage;
        currHP = Mathf.Clamp(currHP, 0, maxHP);
        if (currHP <= 0) {
            Die();
        }
        hearts.UpdateHearts();
    }

    void Die() {
        //dead
    }
    
    public void EscapePress() {
        /*
        pauseDelay = 0.1f;
        if (Time.timeScale == 1) {
            Time.timeScale = 0;
        } else {
            Time.timeScale = 1;
            gameManager.EndDrag(true);
        }*/
    }

    bool inventoryToggleable = true;
    bool itemActivateable = true;
    
    public void ToggleInventory() {
        if (inventoryToggleable) {
            InventoryToggleDelay();
            if (!inventoryMenu.GetComponent<Inventory>().GUIVisible) {
                inventoryMenu.GetComponent<Inventory>().SetGUI(true);
                activeHotbar.GetComponent<ActiveHotbar>().SetGUI(false);
            } else {
                inventoryMenu.GetComponent<Inventory>().SetGUI(false);
                activeHotbar.GetComponent<ActiveHotbar>().SetGUI(true);
                activeHotbar.GetComponent<ActiveHotbar>().UpdateInventory();
                if (gameManager.craftingUI.activeSelf) {
                    gameManager.craftingUI.SetActive(false);
                }
                gameManager.EndDrag(true);
            }
        }
    }

    public void InventoryToggleDelay() {
        IEnumerator InventoryToggleDelayItr() {
            yield return new WaitForSeconds(0.2f);
            inventoryToggleable = true;
        }
        inventoryToggleable = false;
        StartCoroutine(InventoryToggleDelayItr());
    }

    public void ItemActivateDelay() {
        IEnumerator ItemActivateDelayItr() {
            yield return new WaitForSeconds(0.2f);
            itemActivateable = true;
        }
        itemActivateable = false;
        StartCoroutine(ItemActivateDelayItr());
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

    public void ApplyEffect(int ADmod = 0, float ASmod = 0, int MSmod = 0, int hpMod = 0) {
        currWeapon.attackDamageMod += ADmod;
        currWeapon.attackSpeedMod += ASmod;
        moveSpeedMod += MSmod;
        maxSpeed += MSmod;
        maxHP += hpMod;
        TakeDamage(-hpMod);
    }


}

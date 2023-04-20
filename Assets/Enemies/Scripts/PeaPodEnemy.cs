using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeaPodEnemy : Enemy
{
    public List<Sprite> peaSprites = new List<Sprite>();
    public List<GameObject> peas = new List<GameObject>();
    public GameObject peaPrefab;

    float moveSpeed = 100f;
    float maxSpeed = 50f;
    int health = 3;

    float peaSpawnCooldown = 1.5f;
    bool regenPea = false;
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (peas[0].GetComponent<SpriteRenderer>().sprite == null && !regenPea) {
            StartCoroutine(RegenPea(0));
        } else if (peaSpawnCooldown == 0) {
            StartCoroutine(SpawnPea());
        }

        peaSpawnCooldown -= Time.deltaTime;
        peaSpawnCooldown = Mathf.Clamp(peaSpawnCooldown, 0, 5f);

        Vector2 dirVec = player.gameObject.transform.position - transform.position;
        //create vector pointing towards player
        if (dirVec.magnitude < 150f) {
            //run away if near player
            rb.velocity += -dirVec.normalized * moveSpeed * Time.deltaTime;
            rb.velocity = Vector2.ClampMagnitude(rb.velocity, maxSpeed);
        } else {
            rb.velocity = Vector2.zero;
        }
    }

    IEnumerator RegenPea(int index) {
        regenPea = true;
        yield return new WaitForSeconds(3f);
        peas[0].GetComponent<SpriteRenderer>().sprite = peaSprites[0];
        peas[1].GetComponent<SpriteRenderer>().sprite = peaSprites[1];
        peas[2].GetComponent<SpriteRenderer>().sprite = peaSprites[2];
        yield return null;
        regenPea = false;
    }

    IEnumerator SpawnPea() {
        peaSpawnCooldown = 1.5f;
        yield return new WaitForSeconds(0.5f);
        GameObject activePea = peas[0];
        if (peas[2].GetComponent<SpriteRenderer>().sprite != null) {
            activePea = peas[2];
        } else if (peas[1].GetComponent<SpriteRenderer>().sprite != null) {
            activePea = peas[1];
        }
        //print(activePea.GetComponent<SpriteRenderer>().sprite);
        activePea.GetComponent<SpriteRenderer>().sprite = null;
        //print(activePea.GetComponent<SpriteRenderer>().sprite);
        GameObject.Instantiate(peaPrefab, activePea.transform.position, Quaternion.identity);
        yield return null;
    }
    
    public override void TakeDamage(int delta) {
        //reduce the health of this enemy
        health -= delta;
        if (health <= 0) {
            Destroy(gameObject);
            //destroys the gameObject this script is attached to, which will destroy the script as well
        }
    }
}

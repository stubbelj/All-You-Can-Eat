using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeaProjectile : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Expire());
    }

    IEnumerator Expire() {
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }

    void OnCollisionEnter2D (Collision2D col) {
        if (col.gameObject.tag == "Enemy") {
            col.gameObject.GetComponent<Enemy>().TakeDamage(1);
            Destroy(gameObject);
        }
    }
}

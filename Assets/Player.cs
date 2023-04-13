using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    float moveSpeed = 2f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey("w")) {
            transform.position = transform.position + Vector3.up * moveSpeed * Time.deltaTime;
        }
        if (Input.GetKey("a")) {
            transform.position = transform.position + Vector3.left * moveSpeed * Time.deltaTime;
        }
        if (Input.GetKey("s")) {
            transform.position = transform.position + Vector3.down * moveSpeed * Time.deltaTime;
        }
        if (Input.GetKey("d")) {
            transform.position = transform.position + Vector3.right * moveSpeed * Time.deltaTime;
        }
    }
}

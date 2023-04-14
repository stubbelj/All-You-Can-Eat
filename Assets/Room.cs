using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    GameManager gameManager;
    System.Random r;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.inst;
        r = gameManager.r;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    Room Init(string type = null) {
        List<string> roomTypes = new List<string>{
            "empty",
            "oneEnemy"
        };
        if (type == null) {
            type = roomTypes[r.Next(0, roomTypes.Count)];
        }
        switch(type) {
            case "empty":
                //init walls/floor
                break;
            case "oneEnemy":
                //init walls/floor
                //init enemy
                break;
        }
    }
}

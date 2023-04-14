using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager inst = null;

    public Player player;

    System.Random r = new System.Random();

    void Awake() {
        if (inst == null) {
            inst = this;
        } else {
            Destroy(gameObject);
        }

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /*void InitLevel(int level) {
        Dictionary<int, int> roomQuantities = new Dictionary<int, int>() {
            //
        }
    }*/
}

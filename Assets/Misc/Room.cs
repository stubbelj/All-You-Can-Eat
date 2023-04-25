using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Room : MonoBehaviour
{
    GameManager gameManager;
    public System.Random r;

    public float[] bounds;
    //xmin, xmax, ymin, ymax
    [HideInInspector]
    public float width = 140f;
    [HideInInspector]
    public float height = 140f;
    //width, height
    public List<Transform> spawnPoints = new List<Transform>();
    public List<Transform> doorways = new List<Transform>();
    public List<GameObject> doors = new List<GameObject>();

    string state = "inactive";
    //inactive, active, complete

    void Awake()
    {
        bounds = new float[]{transform.position.x - width / 2, transform.position.x + width / 2, transform.position.y - width / 2, transform.position.y +  width / 2};
    }

    void Start() {
        StartCoroutine(LateStart());
    }

    IEnumerator LateStart() {
        yield return new WaitForSeconds(0.1f);
        gameManager = GameManager.inst;
        r = gameManager.r;
    }

    void Update()
    {
        
    }

    public void DoorwayTrigger(GameObject doorway) {
        if (state == "inactive") {
            state = "active";
            Activate();
        }
    }

    public float[] GetBounds() {
        bounds = new float[]{transform.position.x - width / 2, transform.position.x + width / 2, transform.position.y - width / 2, transform.position.y +  width / 2};
        return bounds;
    }

    public abstract void Activate();

    public abstract void Deactivate();
}
